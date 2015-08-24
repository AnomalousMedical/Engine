using Engine;
using Engine.ObjectManagement;
using Engine.Threads;
using OgrePlugin;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin.VirtualTexture
{
    public class VirtualTextureManager : IDisposable
    {
        enum Phase
        {
            RenderFeedback,
            CopyFeedbackToStaging,
            CopyStagingToMemory,
            AnalyzeFeedback,
            Waiting,
            InitialLoad,
            Reset
        }

        public const String ResourceGroup = "VirtualTextureGroup";

        private FeedbackBuffer opaqueFeedbackBuffer;
        private FeedbackBuffer transparentFeedbackBuffer;
        private int frameCount = 0;
        private int updateBufferFrame = 2;
        private Phase phase = Phase.InitialLoad; //All indirection textures will be created requesting their lowest mip levels, this will force us to load those as quickly as possible
        private int texelsPerPage;
        private float pageSizeLog2;
        private int padding;
        private CompressedTextureSupport textureFormat;
        private IntSize2 physicalTextureSize;
        private int maxSyncPerFrame = int.MaxValue;

        private TextureLoader textureLoader;

        private Dictionary<String, PhysicalTexture> physicalTextures = new Dictionary<string, PhysicalTexture>();
        private Dictionary<String, IndirectionTexture> indirectionTextures = new Dictionary<string, IndirectionTexture>();
        private Dictionary<int, IndirectionTexture> indirectionTexturesById = new Dictionary<int, IndirectionTexture>();
        private List<IndirectionTexture> activeIndirectionTextures = new List<IndirectionTexture>();
        private List<IndirectionTexture> retiredIndirectionTextures = new List<IndirectionTexture>();
        private List<IndirectionTexture> newIndirectionTextures = new List<IndirectionTexture>();
        private ConcurrentStack<StagingBufferSet> waitingGpuSyncs = new ConcurrentStack<StagingBufferSet>();

        public VirtualTextureManager(int numPhysicalTextures, IntSize2 physicalTextureSize, int texelsPerPage, CompressedTextureSupport textureFormat, int stagingBufferCount, IntSize2 feedbackBufferSize, ulong maxCacheSizeBytes)
        {
            this.physicalTextureSize = physicalTextureSize;
            this.texelsPerPage = texelsPerPage;
            this.pageSizeLog2 = (float)Math.Log(texelsPerPage, 2.0);
            if (!OgreResourceGroupManager.getInstance().resourceGroupExists(VirtualTextureManager.ResourceGroup))
            {
                OgreResourceGroupManager.getInstance().createResourceGroup(VirtualTextureManager.ResourceGroup);
            }

            switch (textureFormat)
            {
                case CompressedTextureSupport.DXT_BC4_BC5:
                case CompressedTextureSupport.DXT:
                    padding = 4;
                    break;
                default:
                    padding = 1;
                    break;
            }
            this.textureFormat = textureFormat;

            opaqueFeedbackBuffer = new FeedbackBuffer(this, feedbackBufferSize, 0, 0x1);
            transparentFeedbackBuffer = new FeedbackBuffer(this, feedbackBufferSize, 1, 0x2);
            textureLoader = new TextureLoader(this, physicalTextureSize, texelsPerPage, padding, stagingBufferCount, numPhysicalTextures, 6, maxCacheSizeBytes);

            MipSampleBias = -3.0f;
        }

        public void Dispose()
        {
            textureLoader.Dispose();
            opaqueFeedbackBuffer.Dispose();
            transparentFeedbackBuffer.Dispose();

            foreach (var physicalTexture in physicalTextures.Values)
            {
                physicalTexture.Dispose();
            }

            //Dispose all new textures
            lock (newIndirectionTextures)
            {
                foreach (var newTex in newIndirectionTextures)
                {
                    newTex.Dispose();
                }
            }

            //Be sure to dispose all retired textures
            lock (retiredIndirectionTextures)
            {
                foreach (var indirectionTex in retiredIndirectionTextures)
                {
                    indirectionTex.Dispose();
                }
            }

            foreach (var indirectionTexture in indirectionTextures.Values)
            {
                indirectionTexture.Dispose();
            }
        }

        public PhysicalTexture createPhysicalTexture(String name, PixelFormatUsageHint pixelFormatUsageHint)
        {
            PhysicalTexture pt = new PhysicalTexture(name, physicalTextureSize, this, texelsPerPage, textureFormat, pixelFormatUsageHint);
            physicalTextures.Add(name, pt);
            textureLoader.addedPhysicalTexture(pt);
            return pt;
        }

        public void destroyFeedbackBufferCamera(SimScene scene)
        {
            opaqueFeedbackBuffer.destroyCamera(scene);
            transparentFeedbackBuffer.destroyCamera(scene);
        }

        public void createFeedbackBufferCamera(SimScene scene, FeedbackCameraPositioner cameraPositioner)
        {
            opaqueFeedbackBuffer.createCamera(scene, cameraPositioner);
            transparentFeedbackBuffer.createCamera(scene, cameraPositioner);
        }

        public void update()
        {
            if (frameCount == 0)
            {
                switch (phase)
                {
                    case Phase.RenderFeedback:
                        PerformanceMonitor.start("FeedbackBuffer Render");
                        opaqueFeedbackBuffer.update();
                        transparentFeedbackBuffer.update();
                        PerformanceMonitor.stop("FeedbackBuffer Render");
                        phase = Phase.CopyFeedbackToStaging;
                        break;
                    case Phase.CopyFeedbackToStaging:
                        PerformanceMonitor.start("FeedbackBuffer blit to staging");
                        opaqueFeedbackBuffer.blitToStaging();
                        transparentFeedbackBuffer.blitToStaging();
                        PerformanceMonitor.stop("FeedbackBuffer blit to staging");
                        phase = Phase.CopyStagingToMemory;
                        break;
                    case Phase.CopyStagingToMemory:
                        PerformanceMonitor.start("FeedbackBuffer blit staging to memory");
                        opaqueFeedbackBuffer.blitStagingToMemory();
                        transparentFeedbackBuffer.blitStagingToMemory();
                        PerformanceMonitor.stop("FeedbackBuffer blit staging to memory");
                        phase = Phase.AnalyzeFeedback;
                        break;
                    case Phase.AnalyzeFeedback:
                        phase = Phase.Waiting;
                        ThreadManager.RunInBackground(() =>
                        {
                            PerformanceMonitor.start("FeedbackBuffer Analyze");
                            opaqueFeedbackBuffer.analyzeBuffer();
                            transparentFeedbackBuffer.analyzeBuffer();
                            PerformanceMonitor.stop("FeedbackBuffer Analyze");

                            IEnumerable<IndirectionTexture> currentRetiringTextures = null;
                            lock (retiredIndirectionTextures)
                            {
                                if (retiredIndirectionTextures.Count > 0)
                                {
                                    currentRetiringTextures = retiredIndirectionTextures.ToList();
                                    retiredIndirectionTextures.Clear();
                                }
                            }

                            //Begin indirection texture retirement
                            if (currentRetiringTextures != null)
                            {
                                foreach (var indirectionTex in currentRetiringTextures)
                                {
                                    indirectionTex.beginRetirement();
                                }
                            }

                            PerformanceMonitor.start("Finish Page Update");
                            foreach (var indirectionTex in activeIndirectionTextures)
                            {
                                indirectionTex.finishPageUpdate();
                            }
                            PerformanceMonitor.stop("Finish Page Update");

                            PerformanceMonitor.start("Update Texture Loader");
                            if (currentRetiringTextures != null)
                            {
                                textureLoader.updatePagesFromRequests(() =>
                                {
                                    //Finish indirection texture retirement, this is called as the texture load loop resumes
                                    textureLoader.clearCache(); //Pretty brute force, can we remove just the cached textures for the removed indirection textures?
                                    foreach (var indirectionTex in currentRetiringTextures)
                                    {
                                        indirectionTexturesById.Remove(indirectionTex.Id);
                                        activeIndirectionTextures.Remove(indirectionTex);
                                        ThreadManager.invoke(indirectionTex.Dispose);
                                    }

                                    //Activate new textures
                                    activateNewIndirectionTextures();
                                },
                                currentRetiringTextures.Select(i => i.Id));

                            }
                            else
                            {
                                if (newIndirectionTextures.Count > 0)
                                {
                                    textureLoader.updatePagesFromRequests(activateNewIndirectionTextures);
                                }
                                else
                                {
                                    textureLoader.updatePagesFromRequests();
                                }
                            }
                            PerformanceMonitor.stop("Update Texture Loader");

                            phase = Phase.RenderFeedback;
                        });
                        break;
                    case Phase.InitialLoad:
                        phase = Phase.Waiting;
                        ThreadManager.RunInBackground(() =>
                        {
                            activateNewIndirectionTextures();

                            foreach (var indirectionTex in activeIndirectionTextures)
                            {
                                indirectionTex.finishPageUpdate();
                            }

                            textureLoader.updatePagesFromRequests();

                            phase = Phase.RenderFeedback;
                        });
                        break;
                    case Phase.Reset:
                        phase = Phase.Waiting;
                        ThreadManager.RunInBackground(() =>
                        {
                            foreach (var indirectionTex in activeIndirectionTextures)
                            {
                                indirectionTex.finishPageUpdate();
                            }
                            textureLoader.updatePagesFromRequests();
                            textureLoader.clearCache();

                            phase = Phase.RenderFeedback;
                        });
                        break;
                }
            }
            frameCount = (frameCount + 1) % updateBufferFrame;
            uploadStagingToGpu();
        }

        private void activateNewIndirectionTextures()
        {
            lock (newIndirectionTextures)
            {
                foreach (var indirectionTex in newIndirectionTextures)
                {
                    indirectionTexturesById.Add(indirectionTex.Id, indirectionTex);
                    activeIndirectionTextures.Add(indirectionTex);
                }
                newIndirectionTextures.Clear();
            }
        }

        public void reset()
        {
            phase = Phase.Reset;
        }

        public void destroyIndirectionTexture(String materialSetKey)
        {
            IndirectionTexture indirectionTex;
            if (indirectionTextures.TryGetValue(materialSetKey, out indirectionTex))
            {
                destroyIndirectionTexture(indirectionTex);
            }
        }

        public void destroyIndirectionTexture(IndirectionTexture indirectionTex)
        {
            lock (newIndirectionTextures)
            {
                //Make sure we aren't waiting to be activated as a new texture
                newIndirectionTextures.Remove(indirectionTex);
            }
            lock (retiredIndirectionTextures)
            {
                retiredIndirectionTextures.Add(indirectionTex);
                indirectionTextures.Remove(indirectionTex.MaterialSetKey);
            }
        }

        internal PhysicalTexture getPhysicalTexture(string name)
        {
            return physicalTextures[name];
        }

        internal bool getIndirectionTexture(int id, out IndirectionTexture tex)
        {
            return indirectionTexturesById.TryGetValue(id, out tex);
        }

        public IEnumerable<string> TextureNames
        {
            get
            {
                foreach (var item in physicalTextures.Values)
                {
                    yield return item.TextureName;
                }
                if (opaqueFeedbackBuffer.TextureName != null)
                {
                    yield return opaqueFeedbackBuffer.TextureName;
                }
                if (transparentFeedbackBuffer.TextureName != null)
                {
                    yield return transparentFeedbackBuffer.TextureName;
                }
                foreach (var item in activeIndirectionTextures)
                {
                    yield return item.TextureName;
                }
            }
        }

        internal TextureLoader TextureLoader
        {
            get
            {
                return textureLoader;
            }
        }

        public int TexelsPerPage
        {
            get
            {
                return texelsPerPage;
            }
        }

        /// <summary>
        /// This controls how many staging buffers are uploaded per frame. This way if you have lots
        /// of memory but slow upload you can still allocate a lot of staging buffers but throttle their
        /// upload with this property.
        /// </summary>
        public int MaxStagingUploadPerFrame
        {
            get
            {
                return maxSyncPerFrame;
            }
            set
            {
                maxSyncPerFrame = value;
            }
        }

        /// <summary>
        /// The bias to apply to mip maps when sampling the scene in the indirection texture.
        /// </summary>
        public float MipSampleBias { get; set; }

        /// <summary>
        /// The visibility mask for the opaque feedback buffer.
        /// </summary>
        public uint OpaqueFeedbackBufferVisibilityMask
        {
            get
            {
                return opaqueFeedbackBuffer.VisibilityMask;
            }
            set
            {
                opaqueFeedbackBuffer.VisibilityMask = value;
            }
        }

        /// <summary>
        /// The visibility mask for the transparent feedback buffer. 
        /// </summary>
        public uint TransparentFeedbackBufferVisibilityMask
        {
            get
            {
                return transparentFeedbackBuffer.VisibilityMask;
            }
            set
            {
                transparentFeedbackBuffer.VisibilityMask = value;
            }
        }

        internal Vector2 PhysicalSizeRecrip
        {
            get
            {
                float textelRatio = texelsPerPage + padding * 2;
                return new Vector2(1.0f / (physicalTextureSize.Width / textelRatio), 1.0f / (physicalTextureSize.Height / textelRatio));
            }
        }

        /// <summary>
        /// Since the dx11 and opengl drivers treat these textures
        /// slightly differently we must base the usage off the current render system.
        /// This property takes care of that.
        /// </summary>
        internal TextureUsage RendersystemSpecificTextureUsage
        {
            get
            {
                if (OgreInterface.Instance.ChosenRenderSystem == RenderSystemType.D3D11)
                {
                    return TextureUsage.TU_STATIC_WRITE_ONLY;
                }
                return TextureUsage.TU_DYNAMIC_WRITE_ONLY;
            }
        }

        internal void syncToGpu(StagingBufferSet stagingBufferSet)
        {
            waitingGpuSyncs.Push(stagingBufferSet);
        }

        private void uploadStagingToGpu()
        {
            if (waitingGpuSyncs.Count > 0)
            {
                PerformanceMonitor.start("Virtual Texture Staging Texture Upload");
                StagingBufferSet current;
                for (int i = 0; i < maxSyncPerFrame && waitingGpuSyncs.TryPop(out current); ++i)
                {
                    current.uploadTexturesToGpu();
                    textureLoader.returnStagingBuffer(current);
                }
                PerformanceMonitor.stop("Virtual Texture Staging Texture Upload");
            }
        }

        //New System
        /// <summary>
        /// Create or retrieve an indirection texture, will return true if the texture was just created. Useful
        /// for filling out other info on the texture if needed.
        /// </summary>
        /// <param name="materialSetKey">The key to use to search for this texture.</param>
        /// <param name="textureSize">The size of the virtual texture this indirection texture needs to remap.</param>
        /// <param name="indirectionTex">An out variable for the results.</param>
        /// <returns>True if the texture was just created, false if not.</returns>
        public bool createOrRetrieveIndirectionTexture(String materialSetKey, IntSize2 textureSize, bool keepHighestMip, out IndirectionTexture indirectionTex)
        {
            if (!indirectionTextures.TryGetValue(materialSetKey, out indirectionTex))
            {
                indirectionTex = new IndirectionTexture(materialSetKey, textureSize, texelsPerPage, this, keepHighestMip);
                indirectionTextures.Add(indirectionTex.MaterialSetKey, indirectionTex);
                lock (newIndirectionTextures)
                {
                    newIndirectionTextures.Add(indirectionTex);
                }
                return true;
            }
            return false;
        }

        public void setupVirtualTextureFragmentParams(GpuProgramParametersSharedPtr gpuParams, IndirectionTexture indirectionTexture)
        {
            if (gpuParams.Value.hasNamedConstant("physicalSizeRecip"))
            {
                gpuParams.Value.setNamedConstant("physicalSizeRecip", PhysicalSizeRecrip);
                var realSize = indirectionTexture.RealTextureSize;
                gpuParams.Value.setNamedConstant("mipBiasSize", new Vector2(realSize.Width, realSize.Height));
                gpuParams.Value.setNamedConstant("pagePaddingScale", TextureLoader.PagePaddingScale);
                gpuParams.Value.setNamedConstant("pagePaddingOffset", TextureLoader.PagePaddingOffset);
                if (gpuParams.Value.hasNamedConstant("pageSizeLog2"))
                {
                    gpuParams.Value.setNamedConstant("pageSizeLog2", pageSizeLog2);
                }
            }
            else
            {
                Logging.Log.Error("physicalSizeRecip varaible missing");
            }
        }
    }
}
