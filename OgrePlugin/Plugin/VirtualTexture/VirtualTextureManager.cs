using Engine;
using Engine.ObjectManagement;
using Engine.Threads;
using OgrePlugin;
using System;
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
        private int padding;
        private CompressedTextureSupport textureFormat;
        private IntSize2 physicalTextureSize;

        private TextureLoader textureLoader;

        private Dictionary<String, PhysicalTexture> physicalTextures = new Dictionary<string, PhysicalTexture>();
        private Dictionary<String, IndirectionTexture> indirectionTextures = new Dictionary<string, IndirectionTexture>();
        private Dictionary<int, IndirectionTexture> indirectionTexturesById = new Dictionary<int, IndirectionTexture>();
        private List<IndirectionTexture> activeIndirectionTextures = new List<IndirectionTexture>();
        private List<IndirectionTexture> retiredIndirectionTextures = new List<IndirectionTexture>();
        private List<IndirectionTexture> newIndirectionTextures = new List<IndirectionTexture>();

        public VirtualTextureManager(int numPhysicalTextures, IntSize2 physicalTextureSize, int texelsPerPage, CompressedTextureSupport textureFormat, int padding, int stagingBufferCount, IntSize2 feedbackBufferSize)
        {
            this.physicalTextureSize = physicalTextureSize;
            this.texelsPerPage = texelsPerPage;
            if (!OgreResourceGroupManager.getInstance().resourceGroupExists(VirtualTextureManager.ResourceGroup))
            {
                OgreResourceGroupManager.getInstance().createResourceGroup(VirtualTextureManager.ResourceGroup);
            }

            this.padding = padding;
            this.textureFormat = textureFormat;

            opaqueFeedbackBuffer = new FeedbackBuffer(this, feedbackBufferSize, 0, 0x1);
            transparentFeedbackBuffer = new FeedbackBuffer(this, feedbackBufferSize, 1, 0x2);
            textureLoader = new TextureLoader(this, physicalTextureSize, texelsPerPage, padding, stagingBufferCount, numPhysicalTextures, 6, 500 * 1024 * 1024);

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
                foreach(var indirectionTex in retiredIndirectionTextures)
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
                                if(newIndirectionTextures.Count > 0)
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
                if(transparentFeedbackBuffer.TextureName != null)
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
        /// The bias to apply to mip maps when sampling the scene in the indirection texture.
        /// </summary>
        public float MipSampleBias { get; set; }

        /// <summary>
        /// The log2 of the page size, some shaders need this to compute correctly. To use this property
        /// in a shader program define a uniform named pageSizeLog2 in your shader.
        /// </summary>
        public int PageSizeLog2
        {
            get
            {
                return (int)Math.Log(texelsPerPage, 2.0); //Should be this
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

        internal Vector2 PhysicalSizeRecrip
        {
            get
            {
                float textelRatio = texelsPerPage + padding * 2;
                return new Vector2(1.0f / (physicalTextureSize.Width / textelRatio), 1.0f / (physicalTextureSize.Height / textelRatio));
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
                lock(newIndirectionTextures)
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
                if (gpuParams.Value.hasNamedConstant("pageSizeLog2"))
                {
                    gpuParams.Value.setNamedConstant("pageSizeLog2", new Vector2(PageSizeLog2, PageSizeLog2));
                }
                else
                {
                    var realSize = indirectionTexture.RealTextureSize;
                    gpuParams.Value.setNamedConstant("mipBiasSize", new Vector2(realSize.Width, realSize.Height));
                }
                gpuParams.Value.setNamedConstant("pagePaddingScale", TextureLoader.PagePaddingScale);
                gpuParams.Value.setNamedConstant("pagePaddingOffset", TextureLoader.PagePaddingOffset);
            }
            else
            {
                Logging.Log.Error("physicalSizeRecip varaible missing");
            }
        }
    }
}
