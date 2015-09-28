using Engine;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OgrePlugin.VirtualTexture
{
    public class IndirectionTexture : IDisposable
    {
        static byte currentId = 0;
        static byte maxId = 254;
        static HashSet<byte> pooledIds = new HashSet<byte>(); //A hash set of ids that have been returned to the pool, this should normally be empty unless you unload a huge amount of texture sets.
        static byte generateId()
        {
            byte retVal = currentId;

            lock (pooledIds)
            {
                if (pooledIds.Count > 0)
                {
                    retVal = pooledIds.First();
                    pooledIds.Remove(retVal);
                }
                else
                {
                    incrementCurrentId();
                }
            }

            return retVal;
        }

        static void incrementCurrentId()
        {
            currentId = (byte)((currentId + 1) % maxId);
        }

        private static PixelFormat bufferFormat = PixelFormat.PF_A8R8G8B8;
        private const  uint AValue = 0xff000000;
        private static int RShift = 16;
        private const  int GShift = 8;
        private static int BShift = 0;

        private const  uint AMask = 0xff000000;
        private static uint RMask = 0x00ff0000;
        private const  uint GMask = 0x0000ff00;
        private static uint BMask = 0x000000ff;

        /// <summary>
        /// Set by the VirtualTextureManager when it is first created, this will do a runtime test to see what format we get out of the
        /// hardware at runtime, this is then used to setup everything correctly. Do not set outside of te VirtualTextureManager.
        /// </summary>
        internal static PixelFormat BufferFormat
        {
            get
            {
                return bufferFormat;
            }
            set
            {
                if (bufferFormat != value)
                {
                    bufferFormat = value;
                    switch (bufferFormat)
                    {
                        case PixelFormat.PF_A8R8G8B8:
                            RShift = 16;
                            BShift = 0;

                            RMask = 0x00ff0000;
                            BMask = 0x000000ff;
                            break;
                        //case PixelFormat.PF_A8B8G8R8:
                        //    RShift = 0;
                        //    BShift = 16;

                        //    RMask = 0x000000ff;
                        //    BMask = 0x00ff0000;
                        //    break;
                        case PixelFormat.PF_A8B8G8R8:
                            RShift = 16;
                            BShift = 0;

                            RMask = 0x00ff0000;
                            BMask = 0x000000ff;
                            break;
                    }
                }
            }
        }

        private byte id = generateId();
        private IntSize2 realTextureSize;
        private TexturePtr indirectionTexture;
        private VirtualTextureManager virtualTextureManager;
        private IntSize2 numPages;
        private byte highestMip = 0; //The highest mip level that does not fall below one page in size
        private Image[] fiBitmap;
        private HardwarePixelBufferSharedPtr[] buffer;
        private PixelBox[] pixelBox;

        private HashSet<VTexPage> activePages = new HashSet<VTexPage>();
        private HashSet<VTexPage> visibleThisUpdate = new HashSet<VTexPage>();
        private List<VTexPage> removedPages = new List<VTexPage>();
        private HashSet<VTexPage> addedPages = new HashSet<VTexPage>();
        private List<OriginalTextureInfo> originalTextureUnits = new List<OriginalTextureInfo>(4);
        private bool keepHighestMip;

        public IndirectionTexture(String materialSetKey, IntSize2 realTextureSize, int texelsPerPage, VirtualTextureManager virtualTextureManager, bool keepHighestMip)
        {
            this.MaterialSetKey = materialSetKey;
            this.keepHighestMip = keepHighestMip;
            this.virtualTextureManager = virtualTextureManager;
            this.realTextureSize = realTextureSize;
            numPages = realTextureSize / texelsPerPage;
            if (numPages.Width == 0)
            {
                numPages.Width = 1;
            }
            if (numPages.Height == 0)
            {
                numPages.Height = 1;
            }
            for (highestMip = 0; realTextureSize.Width >> highestMip >= texelsPerPage && realTextureSize.Height >> highestMip >= texelsPerPage; ++highestMip) { }
            indirectionTexture = TextureManager.getInstance().createManual(String.Format("{0}_IndirectionTexture_{1}", materialSetKey, id), VirtualTextureManager.ResourceGroup, TextureType.TEX_TYPE_2D,
                (uint)numPages.Width, (uint)numPages.Height, 1, highestMip, bufferFormat, virtualTextureManager.RendersystemSpecificTextureUsage, null, false, 0);
            indirectionTexture.Value.AllowMipmapGeneration = false;

            fiBitmap = new Image[highestMip];
            buffer = new HardwarePixelBufferSharedPtr[highestMip];
            pixelBox = new PixelBox[highestMip];

            for (int i = 0; i < highestMip; ++i)
            {
                fiBitmap[i] = new Image(indirectionTexture.Value.Width >> i, indirectionTexture.Value.Height >> i, 1, indirectionTexture.Value.Format, 1, 0);
                buffer[i] = indirectionTexture.Value.getBuffer(0, (uint)i);
                pixelBox[i] = fiBitmap[i].getPixelBox();
            }

            if (keepHighestMip)
            {
                addedPages.Add(new VTexPage(0, 0, (byte)(highestMip - 1), id));
            }
        }

        public void Dispose()
        {
            lock (pooledIds)
            {
                pooledIds.Add(id); //Return id to list of pooled ids.
            }
            TextureManager.getInstance().remove(indirectionTexture);
            indirectionTexture.Dispose();
            for (int i = 0; i < highestMip; ++i)
            {
                pixelBox[i].Dispose();
                buffer[i].Dispose();
                fiBitmap[i].Dispose();
            }
        }

        public byte Id
        {
            get
            {
                return id;
            }
        }

        public bool KeepHighestMip
        {
            get
            {
                return keepHighestMip;
            }
            set
            {
                keepHighestMip = value;
            }
        }

        internal void processPage(float u, float v, byte mip)
        {
            VTexPage page;
            if (mip >= highestMip)
            {
                page = new VTexPage(0, 0, (byte)(highestMip - 1), id);
            }
            else
            {
                IntSize2 mipLevelNumPages = numPages / (1 << mip);
                byte x = (byte)(u * mipLevelNumPages.Width);
                byte y = (byte)(v * mipLevelNumPages.Height);
                if (x == mipLevelNumPages.Width)
                {
                    --x;
                }
                if (y == mipLevelNumPages.Height)
                {
                    --y;
                }
                page = new VTexPage(x, y, mip, id);
            }
            if (activePages.Contains(page))
            {
                visibleThisUpdate.Add(page);
            }
            else
            {
                addedPages.Add(page);
            }
        }

        internal void finishPageUpdate()
        {
            foreach (var page in activePages)
            {
                if (!visibleThisUpdate.Contains(page) && !(keepHighestMip && page.mip == highestMip - 1))
                {
                    removedPages.Add(page);
                }
            }

            if (addedPages.Count > 0 || removedPages.Count > 0)
            {
                foreach (var page in removedPages)
                {
                    virtualTextureManager.TextureLoader.removeRequestedPage(page);
                    activePages.Remove(page);
                }
                foreach (var page in addedPages)
                {
                    virtualTextureManager.TextureLoader.addRequestedPage(page);
                    activePages.Add(page);
                }
            }

            visibleThisUpdate.Clear();
            removedPages.Clear();
            addedPages.Clear();
        }

        /// <summary>
        /// Begin the retirement of this indirection texture, this puts it in a state so its next finishPageUpdate will
        /// unload all textures currently marked as active.
        /// </summary>
        internal void beginRetirement()
        {
            addedPages.Clear();
            visibleThisUpdate.Clear();
            removedPages.AddRange(activePages);
        }

        internal void copyToStaging(PixelBox[] destinations)
        {
            Logging.Log.Debug("Copy direct {0}", destinations[0].Format == pixelBox[0].Format);
            int srcIndex = 0;
            for (int i = destinations.Length - highestMip; i < destinations.Length; ++i)
            {
                PixelBox.BulkPixelConversion(pixelBox[srcIndex++], destinations[i]);
            }
        }

        internal void uploadStagingToGpu(PixelBox[] sources)
        {
            Logging.Log.Debug("Upload direct {0}", sources[0].Format == indirectionTexture.Value.Format);
            int destIndex = 0;
            for (int i = sources.Length - highestMip; i < sources.Length; ++i)
            {
                buffer[destIndex++].Value.blitFromMemory(sources[i]);
            }
        }

        internal void debug_CheckTexture(Image[] sources)
        {
            int destIndex = 0;
            for (int i = sources.Length - highestMip; i < sources.Length; ++i)
            {
                for (uint x = 0; x < sources[i].Width; ++x)
                {
                    for (uint y = 0; y < sources[i].Height; ++y)
                    {
                        var fiColor = fiBitmap[destIndex].getColorAtARGB(x, y, 0);
                        var ogreColor = sources[i].getColorAtARGB(x, y, 0);
                        if (fiColor != ogreColor)
                        {
                            Logging.Log.Debug("Indirection color mismatch {0}", Id);
                        }
                    }
                }
                ++destIndex;
            }
        }

        internal void addPhysicalPage(PTexPage pTexPage)
        {
            //Store 1x1 as mip 0, 2x2 as 1 4x4 as 2 etc, this way we can directly shift the decimal place
            //Then we will take fract from that
            //Store the page address as bytes
            var vTextPage = pTexPage.VirtualTexturePage;

            uint r = (uint)pTexPage.pageX;
            uint g = (uint)pTexPage.pageY;
            uint b = (uint)(highestMip - vTextPage.mip - 1); //Reverse the mip level (0 becomes highest level (least texels) and highesetMip becomes the lowest level (most texels, full size)

            UInt32 color = AValue + (r << RShift) + (g << GShift) + (b << BShift);

            fiBitmap[vTextPage.mip].setColorAtARGB(color, vTextPage.x, vTextPage.y, 0);
            fillOutLowerMips(vTextPage, color, (c1, c2) => (c1 & BMask) - (c2 & BMask) >= 0);
        }

        internal void removePhysicalPage(PTexPage pTexPage)
        {
            var vTextPage = pTexPage.VirtualTexturePage;
            //Replace color with the one on the higher mip level
            UInt32 color;
            if (vTextPage.mip + 1 < highestMip)
            {
                color = fiBitmap[vTextPage.mip + 1].getColorAtARGB((uint)vTextPage.x >> 1, (uint)vTextPage.y >> 1, 0);
            }
            else
            {
                color = (uint)((highestMip - vTextPage.mip - 1) << BShift);
            }
            uint replacementMipLevel = (uint)((highestMip - vTextPage.mip - 1) << BShift);
            fiBitmap[vTextPage.mip].setColorAtARGB(color, vTextPage.x, vTextPage.y, 0);
            fillOutLowerMips(vTextPage, color, (c1, c2) => (c2 & BMask) == replacementMipLevel);
        }

        private void fillOutLowerMips(VTexPage vTextPage, UInt32 color, Func<UInt32, UInt32, bool> writePixel)
        {
            //Fill in lower (more textels) mip levels
            uint x = vTextPage.x;
            uint y = vTextPage.y;
            int w = 1;
            int h = 1;
            for (int i = vTextPage.mip - 1; i >= 0; --i)
            {
                //This is probably really slow
                x = x << 1;
                y = y << 1;
                w = w << 1;
                h = h << 1;
                var mipLevelBitmap = fiBitmap[i];
                for (uint xi = 0; xi < w; ++xi)
                {
                    for (uint yi = 0; yi < h; ++yi)
                    {
                        var readPixel = mipLevelBitmap.getColorAtARGB(x + xi, y + yi, 0);
                        if (writePixel.Invoke(color, readPixel))
                        {
                            mipLevelBitmap.setColorAtARGB(color, x + xi, y + yi, 0);
                        }
                    }
                }
            }
        }

        public String TextureName
        {
            get
            {
                return indirectionTexture.Value.Name;
            }
        }

        internal IEnumerable<OriginalTextureInfo> OriginalTextures
        {
            get
            {
                return originalTextureUnits;
            }
        }

        public IntSize2 RealTextureSize
        {
            get
            {
                return realTextureSize;
            }
        }

        public IntSize2 NumPages
        {
            get
            {
                return numPages;
            }
        }

        public String MaterialSetKey { get; private set; }

        //New System
        public void addOriginalTexture(string textureUnit, string textureName, IntSize2 textureSize)
        {
            byte mipOffset = 0;
            while (realTextureSize.Width >> mipOffset > textureSize.Width)
            {
                ++mipOffset;
            }
            originalTextureUnits.Add(new OriginalTextureInfo(textureUnit, textureName, mipOffset));
        }

        public void setupFeedbackBufferTechnique(Material material, String vertexShaderName)
        {
            var technique = material.createTechnique();
            technique.setName(FeedbackBuffer.Scheme);
            technique.setSchemeName(FeedbackBuffer.Scheme);
            var pass = technique.createPass();

            pass.setVertexProgram(vertexShaderName);

            pass.setFragmentProgram(FeedbackBufferFPName);
            using (var gpuParams = pass.getFragmentProgramParameters())
            {
                gpuParams.Value.setNamedConstant("virtTexSize", new Vector2(realTextureSize.Width, realTextureSize.Height));
                gpuParams.Value.setNamedConstant("mipSampleBias", virtualTextureManager.MipSampleBias);
                gpuParams.Value.setNamedConstant("spaceId", (float)id);
            }
        }

        public String FeedbackBufferVPName
        {
            get
            {
                return "FeedbackBufferVP";
            }
        }

        public String FeedbackBufferFPName
        {
            get
            {
                return "FeedbackBufferFP";
            }
        }
    }
}
