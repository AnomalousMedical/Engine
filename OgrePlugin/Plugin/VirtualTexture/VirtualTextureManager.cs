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
            Reset,
            Delay, //Delay is different from waiting, in waiting we are waiting for a specific event to finish in delay we are causing a delay to occur
        }

        public const String ResourceGroup = "VirtualTextureGroup";

        private FeedbackBuffer opaqueFeedbackBuffer;
        private FeedbackBuffer transparentFeedbackBuffer;
        private Phase phase = Phase.InitialLoad; //All indirection textures will be created requesting their lowest mip levels, this will force us to load those as quickly as possible
        private int texelsPerPage;
        private float pageSizeLog2;
        private int padding;
        private CompressedTextureSupport textureFormat;
        private IntSize2 physicalTextureSize;
        private int maxSyncPerFrame = int.MaxValue;
        private int currentFeedbackDelayCount = 0;
        private int maxFeedbackDelay = 10;
        private int lastNumTexturesUploaded = 0;

        private bool autoAjustMipLevel = true;
        private bool updateMipSampleBias = true;
        private float lastMipSampleBias = -3.0f;
        private float mipSampleBias = -3.0f;
        private float minMipBias = -3.0f;
        private float maxMipBias = 10.0f;

        private TextureLoader textureLoader;
        private TexturePtr testTexture;

        private Dictionary<String, PhysicalTexture> physicalTextures = new Dictionary<string, PhysicalTexture>();
        private Dictionary<String, IndirectionTexture> indirectionTextures = new Dictionary<string, IndirectionTexture>();
        private Dictionary<int, IndirectionTexture> indirectionTexturesById = new Dictionary<int, IndirectionTexture>();
        private List<IndirectionTexture> activeIndirectionTextures = new List<IndirectionTexture>();
        private List<IndirectionTexture> retiredIndirectionTextures = new List<IndirectionTexture>();
        private List<IndirectionTexture> newIndirectionTextures = new List<IndirectionTexture>();
        private ConcurrentQueue<StagingBufferSet> waitingGpuSyncs = new ConcurrentQueue<StagingBufferSet>();
        private Phase onSuspendedPhase = Phase.Delay;
        private bool wantsReset = false;
        private bool active = true;

        //Texture upload data
        private HashSet<byte> uploadedIndirectionTextures;
        private List<StagingBufferSet> toUploadList;
        private GpuSharedParametersPtr sharedFeedbackParameters;

        public VirtualTextureManager(int numPhysicalTextures, IntSize2 physicalTextureSize, int texelsPerPage, int largestRealTextureSize, CompressedTextureSupport textureFormat, int stagingBufferCount, IntSize2 feedbackBufferSize, ulong maxCacheSizeBytes, bool texturesArePagedOnDisk)
        {
            uploadedIndirectionTextures = new HashSet<byte>();
            toUploadList = new List<StagingBufferSet>(stagingBufferCount);

            this.physicalTextureSize = physicalTextureSize;
            this.texelsPerPage = texelsPerPage;
            this.pageSizeLog2 = (float)Math.Log(texelsPerPage, 2.0);
            if (!OgreResourceGroupManager.getInstance().resourceGroupExists(VirtualTextureManager.ResourceGroup))
            {
                OgreResourceGroupManager.getInstance().createResourceGroup(VirtualTextureManager.ResourceGroup);
            }

            //Determine actual runtime texture formats by just creating a simple one quickly.
            testTexture = TextureManager.getInstance().createManual("TestTexture__VTRESERVED", VirtualTextureManager.ResourceGroup, TextureType.TEX_TYPE_2D, 1, 1, 1, 0, PixelFormat.PF_A8R8G8B8, TextureUsage.TU_STATIC, null, false, 0);
            IndirectionTexture.BufferFormat = testTexture.Value.Format;

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
            int highestMip = 0;
            for (highestMip = 0; largestRealTextureSize >> highestMip >= texelsPerPage; ++highestMip) { }
            textureLoader = new TextureLoader(this, physicalTextureSize, texelsPerPage, padding, stagingBufferCount, numPhysicalTextures, highestMip, maxCacheSizeBytes, texturesArePagedOnDisk);

            sharedFeedbackParameters = GpuProgramManager.Instance.createSharedParameters("__VirtualTexturingFeedbackSharedParams");
            sharedFeedbackParameters.Value.addNamedConstant("mipSampleBias", GpuConstantType.GCT_FLOAT1);

            PerformanceMonitor.addValueProvider("NumTexturePageUploads", () => lastNumTexturesUploaded.ToString());
        }

        public void Dispose()
        {
            TextureManager.getInstance().remove(testTexture);
            testTexture.Dispose();

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

            sharedFeedbackParameters.Dispose();
        }

        public PhysicalTexture createPhysicalTexture(String name, PixelFormat pixelFormat)
        {
            PhysicalTexture pt;
            switch (OgreInterface.Instance.GpuVendor)
            {
                case GPUVendor.GPU_ARM:
                    pt = new BufferedPhysicalTexture(name, physicalTextureSize, this, texelsPerPage, pixelFormat);
                    break;
                default:
                    pt = new DirectPhysicalTexture(name, physicalTextureSize, this, texelsPerPage, pixelFormat);
                    break;
            }

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

        public void suspend()
        {
            if (active)
            {
                active = false;
                foreach (var tex in physicalTextures.Values)
                {
                    tex.suspendTexture(testTexture.Value.Name);
                }
                onSuspendedPhase = phase;
                phase = Phase.Waiting;
                textureLoader.clearCache();
            }
        }

        public void resume()
        {
            if (!active)
            {
                active = true;
                foreach (var tex in physicalTextures.Values)
                {
                    tex.restoreTexture();
                }
                wantsReset = true;
                phase = onSuspendedPhase;
            }
        }

        /// <summary>
        /// Tick the virtual texture manager, this will always attempt to do some useful work either
        /// updating the feedback buffers, blitting the feedback buffer to main memory, updating the 
        /// texture load thread or updating the textures on the gpu. This function should be safe
        /// to call at any time after the VirtualTextureManager is setup. So if for example you wanted
        /// to update textures during your splash screen, call this function and it will attempt to do 
        /// something useful and will start rendering feedback buffers as soon as the camera is created.
        /// </summary>
        public void update()
        {
            switch (phase)
            {
                case Phase.RenderFeedback:
                    if (opaqueFeedbackBuffer.AbleToRender)
                    {
                        if (updateMipSampleBias)
                        {
                            sharedFeedbackParameters.Value.setNamedConstant("mipSampleBias", mipSampleBias);
                            updateMipSampleBias = false;
                        }

                        PerformanceMonitor.start("FeedbackBuffer Render");
                        opaqueFeedbackBuffer.update();
                        transparentFeedbackBuffer.update();
                        PerformanceMonitor.stop("FeedbackBuffer Render");
                        phase = Phase.CopyFeedbackToStaging;
                    }
                    else
                    {
                        //Since we can't render change phase to analyze feedback and then do an immediate update to do that case now.
                        //This makes the lowest mips load if they are forced to load even if we haven't rendered a feedback buffer yet.
                        phase = Phase.AnalyzeFeedback;
                        update();
                    }
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
                        try
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

                            foreach (var indirectionTex in activeIndirectionTextures)
                            {
                                indirectionTex.finishPageUpdate();
                            }

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
                        }
                        finally
                        {
                            //Adust the mip bias
                            if (AutoAdjustMipLevel)
                            {
                                if (TextureLoader.Overprevisioned)
                                {
                                    mipSampleBias += 1;
                                    if (mipSampleBias > maxMipBias)
                                    {
                                        mipSampleBias = maxMipBias;
                                    }
                                }
                                else
                                {
                                    mipSampleBias -= 1;
                                    if (mipSampleBias < minMipBias)
                                    {
                                        mipSampleBias = minMipBias;
                                    }
                                }
                                updateMipSampleBias = mipSampleBias != lastMipSampleBias;
                                lastMipSampleBias = mipSampleBias;
                            }

                            //Determine final phase, since this is the major background thread it might
                            //have an effect on the current and suspended phase.

                            ThreadManager.invoke(() =>
                            {
                                if (wantsReset)
                                {
                                    phase = Phase.Reset;
                                    onSuspendedPhase = Phase.Reset;
                                    wantsReset = false;
                                }
                                else
                                {
                                    phase = Phase.Delay; //This means we always delay at least one frame, but this gives a chance for textures to upload to the gpu.
                                    onSuspendedPhase = Phase.Delay;
                                }
                            });
                        }
                    });
                    uploadStagingToGpu();
                    break;
                case Phase.InitialLoad:
                    phase = Phase.Waiting;
                    ThreadManager.RunInBackground(() =>
                    {
                        try
                        {
                            activateNewIndirectionTextures();

                            foreach (var indirectionTex in activeIndirectionTextures)
                            {
                                indirectionTex.finishPageUpdate();
                            }

                            textureLoader.updatePagesFromRequests();
                        }
                        finally
                        {
                            phase = Phase.RenderFeedback;
                        }
                    });
                    break;
                case Phase.Reset:
                    phase = Phase.Waiting;

                    //Reset indirection textures on the main thread
                    foreach (var indirectionTex in activeIndirectionTextures)
                    {
                        indirectionTex.reset();
                        indirectionTex.finishPageUpdate();
                    }

                    ThreadManager.RunInBackground(() =>
                    {
                        try
                        {
                            textureLoader.updatePagesFromRequests();

                            textureLoader.clearPhysicalPagePool();
                            textureLoader.clearCache();

                            //Clear waiting staging buffers too
                            StagingBufferSet stagingBuffers;
                            while (waitingGpuSyncs.TryDequeue(out stagingBuffers))
                            {
                                textureLoader.returnStagingBuffer(stagingBuffers);
                            }

                            //Restore permanent pages
                            foreach (var indirectionTex in activeIndirectionTextures)
                            {
                                indirectionTex.restorePermanentPages();
                            }
                        }
                        finally
                        {
                            phase = Phase.RenderFeedback;
                        }
                    });
                    break;
                case Phase.Delay:
                    if (++currentFeedbackDelayCount > maxFeedbackDelay)
                    {
                        currentFeedbackDelayCount = 0;
                        phase = Phase.RenderFeedback;
                    }
                    uploadStagingToGpu();
                    break;
                case Phase.Waiting:
                    uploadStagingToGpu();
                    break;
            }
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

        public void saveIndirectionTexture(String textureName, String outputFolder)
        {
            foreach (var item in activeIndirectionTextures)
            {
                if (textureName == item.TextureName)
                {
                    item.saveToPath(outputFolder);
                }
            }
        }

        internal PhysicalTexture getPhysicalTexture(string name)
        {
            return physicalTextures[name];
        }

        internal bool tryGetPhysicalTexture(string name, out PhysicalTexture physicalTexture)
        {
            return physicalTextures.TryGetValue(name, out physicalTexture);
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
        public float MipSampleBias
        {
            get
            {
                return mipSampleBias;
            }
            set
            {
                mipSampleBias = value;
                updateMipSampleBias = true;
            }
        }

        public bool AutoAdjustMipLevel
        {
            get
            {
                return autoAjustMipLevel;
            }
            set
            {
                autoAjustMipLevel = value;
            }
        }

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

        /// <summary>
        /// The maximum number of frames to delay before starting the feedback buffer
        /// render again.
        /// </summary>
        public int MaxFeedbackDelay
        {
            get
            {
                return maxFeedbackDelay;
            }
            set
            {
                maxFeedbackDelay = value;
            }
        }

        public bool Active
        {
            get
            {
                return active;
            }
        }

        /// <summary>
        /// The shared parameters for feedback buffer programs.
        /// </summary>
        internal GpuSharedParametersPtr SharedFeedbackParameters
        {
            get
            {
                return sharedFeedbackParameters;
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
                switch (OgreInterface.Instance.ChosenRenderSystem)
                {
                    case RenderSystemType.D3D11:
                        return TextureUsage.TU_STATIC_WRITE_ONLY;
                    //case RenderSystemType.OpenGLES2:
                    //    return TextureUsage.TU_DYNAMIC_WRITE_ONLY_DISCARDABLE;
                    default:
                        return TextureUsage.TU_DYNAMIC_WRITE_ONLY;
                }
            }
        }

        internal void syncToGpu(StagingBufferSet stagingBufferSet)
        {
            waitingGpuSyncs.Enqueue(stagingBufferSet);
        }

        private void uploadStagingToGpu()
        {
            if (waitingGpuSyncs.Count > 0)
            {
                PerformanceMonitor.start("Virtual Texture Staging Texture Upload");

                foreach (var tex in physicalTextures.Values)
                {
                    tex.prepareForUpdates();
                }

                StagingBufferSet current;
                for (int i = 0; i < maxSyncPerFrame && waitingGpuSyncs.TryDequeue(out current); ++i)
                {
                    toUploadList.Add(current);
                }

                lastNumTexturesUploaded = toUploadList.Count;

                //Since indirection textures are always fully up to date, process the list backwards and only
                //update indirection textures that haven't been uploaded, avoids extra uploads
                for (int i = toUploadList.Count - 1; i >= 0; --i)
                {
                    toUploadList[i].uploadTexturesToGpu(shouldUpload);
                    textureLoader.returnStagingBuffer(toUploadList[i]);
                }
                toUploadList.Clear();
                uploadedIndirectionTextures.Clear();

                foreach (var tex in physicalTextures.Values)
                {
                    tex.commitUpdates();
                }

                PerformanceMonitor.stop("Virtual Texture Staging Texture Upload");
            }
        }

        private bool shouldUpload(byte indirectionTextureId)
        {
            bool upload = !uploadedIndirectionTextures.Contains(indirectionTextureId);
            uploadedIndirectionTextures.Add(indirectionTextureId);
            return upload;
        }

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
