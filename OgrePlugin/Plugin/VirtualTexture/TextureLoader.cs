using Engine;
using Engine.Threads;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OgrePlugin.VirtualTexture
{
    class TextureLoader : IDisposable
    {
        private Object syncObject = new object();

        private HashSet<VTexPage> addedPages;
        private List<VTexPage> removedPages;

        private List<PTexPage> physicalPageQueue; //FIFO queue for used pages, allows us to reuse pages if they are requested again quickly and keep track of what parts of the physical texture we can use
        private Dictionary<VTexPage, PTexPage> physicalPagePool;
        private Dictionary<VTexPage, PTexPage> usedPhysicalPages;

        private VirtualTextureManager virtualTextureManager;
        private int maxPages;
        private int textelsPerPage;
        private int padding;
        private int padding2;
        private int textelsPerPhysicalPage;
        private bool cancelBackgroundLoad = false;

        private List<StagingBufferSet> stagingBufferSets;
        private Task<bool>[] copyTostagingImageTasks;
        private ManualResetEventSlim stagingBufferWaitEvent = new ManualResetEventSlim(true);

        private TextureCache textureCache;

        private Task loadingTask;
        private bool stopLoading = false;
        private List<VTexPage> pagesToLoad = new List<VTexPage>();

        public TextureLoader(VirtualTextureManager virtualTextureManager, IntSize2 physicalTextureSize, int textelsPerPage, int padding, int stagingBufferCount, int stagingImageCapacity, int maxMipCount, UInt64 maxCacheSizeBytes, bool texturesArePagedOnDisk)
        {
            textureCache = new TextureCache(maxCacheSizeBytes, texturesArePagedOnDisk);
            this.virtualTextureManager = virtualTextureManager;
            IntSize2 pageTableSize = physicalTextureSize / textelsPerPage;
            this.maxPages = pageTableSize.Width * pageTableSize.Height;
            this.textelsPerPage = textelsPerPage;
            this.padding = padding;
            this.padding2 = padding * 2;
            this.textelsPerPhysicalPage = textelsPerPage + padding2;

            addedPages = new HashSet<VTexPage>();
            removedPages = new List<VTexPage>(10);
            pagesToLoad = new List<VTexPage>(10);

            stagingBufferSets = new List<StagingBufferSet>(stagingBufferCount);
            for (int i = 0; i < stagingBufferCount; ++i)
            {
                stagingBufferSets.Add(new StagingBufferSet(stagingImageCapacity, maxMipCount));
            }
            copyTostagingImageTasks = new Task<bool>[stagingImageCapacity];

            float scale = (float)textelsPerPage / textelsPerPhysicalPage;
            PagePaddingScale = new Vector2(scale, scale);

            scale = (float)padding / textelsPerPhysicalPage;
            PagePaddingOffset = new Vector2(scale, scale);

            //Build pool
            int x = 0;
            int y = 0;
            int pageX = 0;
            int pageY = 0;
            physicalPageQueue = new List<PTexPage>(maxPages);
            physicalPagePool = new Dictionary<VTexPage, PTexPage>();
            usedPhysicalPages = new Dictionary<VTexPage, PTexPage>();
            for(int i = 0 ; i < maxPages; ++i)
            {
                physicalPageQueue.Add(new PTexPage(x, y, pageX, pageY));
                x += textelsPerPhysicalPage;
                ++pageX;
                if (x + textelsPerPhysicalPage >= physicalTextureSize.Width)
                {
                    x = 0;
                    y += textelsPerPhysicalPage;
                    pageX = 0;
                    ++pageY;
                    if (y + textelsPerPhysicalPage > physicalTextureSize.Height)
                    {
                        break;
                    }
                }
            }
        }

        public void Dispose()
        {
            lock (syncObject)
            {
                cancelBackgroundLoad = true;
                textureCache.Dispose();
                foreach(var stagingBuffer in stagingBufferSets)
                {
                    stagingBuffer.Dispose();
                }
                stagingBufferWaitEvent.Dispose();
            }
        }

        public void addedPhysicalTexture(PhysicalTexture physicalTexture)
        {
            foreach(var buffer in stagingBufferSets)
            {
                buffer.addedPhysicalTexture(physicalTexture, textelsPerPhysicalPage);
            }
        }

        public void addRequestedPage(VTexPage page)
        {
            addedPages.Add(page);
        }

        public void removeRequestedPage(VTexPage page)
        {
            addedPages.Remove(page);
            removedPages.Add(page);
        }

        /// <summary>
        /// Update the pages in the texture loader that should be loaded. Note that if you are retiring indirection textures
        /// you can pass an enumerator to their ids so their removal will not be pooled.
        /// </summary>
        /// <param name="loopResumingCallback">An action that is fired just before the loop restarts to load images.</param>
        /// <param name="retiringIndirectionTextureIds">An enumerator over indirection texture ids that should not have removal pooled since they are being retired. Null means ignore this (default).</param>
        public void updatePagesFromRequests(Action loopResumingCallback = null, IEnumerable<byte> retiringIndirectionTextureIds = null)
        {
            //If there are no added pages or changes to indirection textures, there is nothing to do with this call, just return
            if (addedPages.Count == 0 && removedPages.Count == 0 && loopResumingCallback == null && retiringIndirectionTextureIds == null)
            {
                return;
            }

            //Finish any current image loading and wait until that is complete
            if (loadingTask != null)
            {
                stopLoading = true;
                loadingTask.Wait();
            }

            //Careful with order, we want to make sure the loadingtask is done before getting a lock again since that function locks for its duration.
            lock (syncObject)
            {
                //We have a lock now, are we still ok to load?
                if (cancelBackgroundLoad)
                {
                    throw new CancelThreadException();
                }
            }

            //If we have pages to retire, for extra safety wait until all outstanding staging buffers are finished uploading
            if (retiringIndirectionTextureIds != null)
            {
                foreach (var stagingBuffer in stagingBufferSets)
                {
                    stagingBuffer.waitForGpuUpload();
                }
            }

            //Reset
            stopLoading = false;

            //Remove pages
            foreach (var page in removedPages)
            {
                PTexPage pTexPage;
                if (usedPhysicalPages.TryGetValue(page, out pTexPage))
                {
                    //See if this page is part of a retired texture, if not pool the page
                    if (retiringIndirectionTextureIds == null || !retiringIndirectionTextureIds.Contains(pTexPage.VirtualTexturePage.indirectionTexId))
                    {
                        physicalPagePool.Add(page, pTexPage);
                    }
                    else
                    {
                        pTexPage.VirtualTexturePage = null;
                    }
                    physicalPageQueue.Add(pTexPage);
                    usedPhysicalPages.Remove(page);
                }
                else
                {
                    pagesToLoad.Remove(page);
                }
            }

            //Clear out any other physical page pool entries
            if(retiringIndirectionTextureIds != null)
            {
                List<PTexPage> currentPagePool = physicalPagePool.Values.ToList();
                foreach (var pooledPage in currentPagePool)
                {
                    if (retiringIndirectionTextureIds.Contains(pooledPage.VirtualTexturePage.indirectionTexId))
                    {
                        physicalPagePool.Remove(pooledPage.VirtualTexturePage);
                        pooledPage.VirtualTexturePage = null;
                    }
                }
            }

            //Process pages into loaded and not loaded
            foreach(var addedPage in addedPages)
            {
                PTexPage pTexPage;
                //Does the virtual texture already contain the page?
                if (physicalPagePool.TryGetValue(addedPage, out pTexPage))
                {
                    //Already contained, just update the queue and pool, this requires no texture updates
                    physicalPageQueue.Remove(pTexPage);
                    physicalPagePool.Remove(addedPage);
                    usedPhysicalPages.Add(addedPage, pTexPage);
                }
                else if (!usedPhysicalPages.ContainsKey(addedPage) && !pagesToLoad.Contains(addedPage)) //Add all new pages that are not already used, could potentailly be slow, can second check be replaced by a hash map
                {
                    pagesToLoad.Add(addedPage);
                }
            }

            if (loopResumingCallback != null)
            {
                loopResumingCallback.Invoke();
            }

            //Start loading task again
            pagesToLoad.Sort((v1, v2) => v1.GetHashCode() - v2.GetHashCode());
            loadingTask = Task.Run(() =>
                {
                    lock (syncObject)
                    {
                        PerformanceMonitor.start("updatePagesFromRequests processing pages");
                        for (int i = pagesToLoad.Count - 1; i > -1; --i) //Process backwards, try to avoid as many collection element shifts as possible, this is sorted so the desired read order is reversed in actual memory
                        {
                            if (cancelBackgroundLoad) //Reaquired lock, are we still active
                            {
                                throw new CancelThreadException();
                            }
                            if (stopLoading)
                            {
                                break;
                            }
                            System.Threading.Monitor.Exit(syncObject);
                            stagingBufferWaitEvent.Wait(); //Make sure we actually can dequeue a staging buffer
                            StagingBufferSet stagingBuffers;
                            lock (stagingBufferSets)
                            {
                                stagingBuffers = this.stagingBufferSets[0];
                                stagingBuffers.reset();
                                stagingBufferSets.RemoveAt(0);
                                if(stagingBufferSets.Count == 0)
                                {
                                    //We have no more staging buffers, force next iteration to wait until the last one has been returned.
                                    stagingBufferWaitEvent.Reset();
                                }
                            }
                            System.Threading.Monitor.Enter(syncObject);
                            if (loadPage(pagesToLoad[i], stagingBuffers))
                            {
                                virtualTextureManager.syncToGpu(stagingBuffers);
                            }
                            else
                            {
                                returnStagingBuffer(stagingBuffers);
                            }
                            pagesToLoad.RemoveAt(i);
                        }
                        PerformanceMonitor.stop("updatePagesFromRequests processing pages");
                    }
                });

            //Reset added and removed pages
            addedPages.Clear();
            removedPages.Clear();
        }

        public void returnStagingBuffer(StagingBufferSet stagingBuffers)
        {
            lock (stagingBufferSets)
            {
                stagingBuffers.returnedToPool();
                stagingBufferSets.Add(stagingBuffers);
                if (stagingBufferSets.Count > 0)
                {
                    stagingBufferWaitEvent.Set();
                }
            }
        }

        private bool loadPage(VTexPage page, StagingBufferSet stagingBuffers)
        {
            bool added = false;
            try
            {
                //First see if we still have that page in our virtual texture pool, possible optimization to sort these to the front of the list
                if (physicalPageQueue.Count > 0) //Do we have pages available
                {
                    PTexPage pTexPage = physicalPageQueue[0]; //The physical page candidate, do not modify before usedPhysicalPages if statement below
                    if (loadImages(page, pTexPage, stagingBuffers))
                    {
                        //Alert old texture of removal if there was one, Do not modify pTexPage above this if block, we need the old data
                        IndirectionTexture oldIndirectionTexture = null;
                        if (pTexPage.VirtualTexturePage != null)
                        {
                            if (virtualTextureManager.getIndirectionTexture(pTexPage.VirtualTexturePage.indirectionTexId, out oldIndirectionTexture))
                            {
                                oldIndirectionTexture.removePhysicalPage(pTexPage);
                            }

                            physicalPagePool.Remove(pTexPage.VirtualTexturePage); //Be sure to remove the page from the pool if it was used previously
                        }

                        physicalPageQueue.RemoveAt(0);
                        pTexPage.VirtualTexturePage = page;
                        usedPhysicalPages.Add(page, pTexPage);

                        //Add to new indirection texture
                        IndirectionTexture newIndirectionTex;
                        if (virtualTextureManager.getIndirectionTexture(page.indirectionTexId, out newIndirectionTex))
                        {
                            newIndirectionTex.addPhysicalPage(pTexPage);
                            stagingBuffers.setIndirectionTextures(oldIndirectionTexture, newIndirectionTex);
                        }
                        added = true;
                    }
                }
            }
            catch(Exception ex)
            {
                Logging.Log.Debug("{0} loading page {1}. Message: {2}", ex.GetType().Name, page.ToString(), ex.Message);
            }
            return added;
        }

        internal void clearCache()
        {
            textureCache.clear();
        }

        internal Vector2 PagePaddingScale { get; private set; }

        internal Vector2 PagePaddingOffset { get; private set; }

        /// <summary>
        /// Load the given image. Note that pTexPage is constant for the duration of this function call
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pTexPage"></param>
        /// <returns></returns>
        private bool loadImages(VTexPage page, PTexPage pTexPage, StagingBufferSet stagingBuffers)
        {
            int stagingImageIndex = 0;
            bool usedPhysicalPage = false;
            IndirectionTexture indirectionTexture;
            if (virtualTextureManager.getIndirectionTexture(page.indirectionTexId, out indirectionTexture))
            {
                //Fire off image loading and blitting tasks
                foreach (var textureUnit in indirectionTexture.OriginalTextures)
                {
                    copyTostagingImageTasks[stagingImageIndex] = fireCopyToStaging(page, stagingBuffers, indirectionTexture, textureUnit);
                    ++stagingImageIndex;
                }
                //Wait for results
                for (int i = 0; i < stagingImageIndex; ++i)
                {
                    copyTostagingImageTasks[i].Wait();
                    if (copyTostagingImageTasks[i].Result)
                    {
                        usedPhysicalPage = true;
                    }
                }

                //Single threaded
                //foreach (var textureUnit in indirectionTexture.OriginalTextures)
                //{
                //    if (copyToStaging(page, stagingBuffers, indirectionTexture, textureUnit))
                //    {
                //        usedPhysicalPage = true;
                //    }
                //}

                //Update staging buffer info
                stagingBuffers.Dest = new IntRect(pTexPage.x, pTexPage.y, textelsPerPhysicalPage, textelsPerPhysicalPage);
            }
            return usedPhysicalPage;
        }

        private Task<bool> fireCopyToStaging(VTexPage page, StagingBufferSet buffers, IndirectionTexture indirectionTexture, OriginalTextureInfo textureUnit)
        {
            return Task.Run(() => copyToStaging(page, buffers, indirectionTexture, textureUnit));
        }

        private bool copyToStaging(VTexPage page, StagingBufferSet buffers, IndirectionTexture indirectionTexture, OriginalTextureInfo textureUnit)
        {
            bool usedPhysicalPage = false;

            try
            {
                if (page.mip >= textureUnit.MipOffset)
                {
                    //Load or grab from cache
                    using (var pageHandle = textureCache.getImage(page, indirectionTexture, textureUnit, textelsPerPage, padding, padding2))
                    {
                        buffers.setPhysicalPage(pageHandle.PixelBox, virtualTextureManager.getPhysicalTexture(textureUnit.TextureUnit), padding);
                        usedPhysicalPage = true;
                    }
                }
                else
                {
                    Logging.Log.Warning("Unable to load mip map level {0} for texture {1}", page.mip - textureUnit.MipOffset, textureUnit.TextureFileName);
                }
            }
            catch(Exception ex)
            {
                Logging.Log.Error("{0} loading image {1}. Message: {2}", ex.GetType().Name, textureUnit.TextureFileName, ex.Message);
            }
            return usedPhysicalPage;
        }
    }
}
