//#define DEBUG_MIP_LEVELS

using Engine;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.IO;
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
                bufferFormat = value;
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
                (uint)numPages.Width, (uint)numPages.Height, 1, highestMip - 1, bufferFormat, virtualTextureManager.RendersystemSpecificTextureUsage, null, false, 0);
            indirectionTexture.Value.AllowMipmapGeneration = false;

            fiBitmap = new Image[highestMip];
            buffer = new HardwarePixelBufferSharedPtr[highestMip];
            pixelBox = new PixelBox[highestMip];

            for (int i = 0; i < highestMip; ++i)
            {
                fiBitmap[i] = new Image(indirectionTexture.Value.Width >> i, indirectionTexture.Value.Height >> i, 1, indirectionTexture.Value.Format, 1, 0);
                buffer[i] = indirectionTexture.Value.getBuffer(0, (uint)i);
                pixelBox[i] = fiBitmap[i].getPixelBox();

#if DEBUG_MIP_LEVELS
                //Temp, debug mip levels
                for (uint x = 0; x < fiBitmap[i].Width; ++x)
                {
                    for (uint y = 0; y < fiBitmap[i].Height; ++y)
                    {
                        fiBitmap[i].setColorAtARGB(new IntColor((byte)255, (byte)x, (byte)y, (byte)i).ARGB, x, y, 0);
                    }
                }
#endif
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
            //Logging.Log.Debug("Copy direct {0}", destinations[0].Format == pixelBox[0].Format);
            int srcIndex = 0;
            for (int i = destinations.Length - highestMip; i < destinations.Length; ++i)
            {
                PixelBox.BulkPixelConversion(pixelBox[srcIndex++], destinations[i]);
            }
        }

        internal void uploadStagingToGpu(PixelBox[] sources)
        {
            //Logging.Log.Debug("Upload direct {0}", sources[0].Format == indirectionTexture.Value.Format);
            int destIndex = 0;
            for (int i = sources.Length - highestMip; i < sources.Length; ++i)
            {
                //buffer[destIndex++].Value.blitFromMemory(sources[i]);
                buffer[destIndex++].Value.stagingBufferBlit(sources[i]);
            }
        }

        internal void uploadStagingToGpu(Image image)
        {
            indirectionTexture.Value.blitFromImage(image);
            //indirectionTexture.Value.loadImage(image);
        }

        public void debug_dumpTextures(String outputFolder)
        {
            for (int i = 0; i < fiBitmap.Length; ++i)
            {
                fiBitmap[i].save(Path.Combine(outputFolder, String.Format("{0}_Cpu_{1}.png", TextureName, i)));
            }
        }

        public void debug_dumpOgreTextures(String outputFolder)
        {
            uint numMips = (uint)(indirectionTexture.Value.NumMipmaps + 1);
            uint width = indirectionTexture.Value.Width;
            uint height = indirectionTexture.Value.Height;
            for (uint mip = 0; mip < numMips; ++mip)
            {
                using (var buffer = indirectionTexture.Value.getBuffer(0, mip))
                {
                    using (var blitBitmap = new Image(width, height, 1, PixelFormat.PF_A8R8G8B8, 1, 0))
                    {
                        using (var blitBitmapBox = blitBitmap.getPixelBox())
                        {
                            buffer.Value.blitToMemory(blitBitmapBox);
                        }

                        String fileName = String.Format("{0}_ogre_{1}.png", TextureName, mip);
                        fileName = Path.Combine(outputFolder, fileName);
                        blitBitmap.save(fileName);
                    }
                    width >>= 1;
                    height >>= 1;
                }
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
#if !DEBUG_MIP_LEVELS
            //Store 1x1 as mip 0, 2x2 as 1 4x4 as 2 etc, this way we can directly shift the decimal place
            //Then we will take fract from that
            //Store the page address as bytes
            var vTextPage = pTexPage.VirtualTexturePage;
            IntColor color = new IntColor();
            color.A = 255;
            //Reverse the mip level (0 becomes highest level (least texels) and highesetMip becomes the lowest level (most texels, full size)
            color.B = (byte)(highestMip - vTextPage.mip - 1); //Typecast bad, try changing the type in the struct to byte
            color.R = (byte)pTexPage.pageX;
            color.G = (byte)pTexPage.pageY;

            fiBitmap[vTextPage.mip].setColorAtARGB(color.ARGB, vTextPage.x, vTextPage.y, 0);
            fillOutLowerMips(vTextPage, color, (c1, c2) => c1.B - c2.B >= 0);
#endif
        }

        internal void removePhysicalPage(PTexPage pTexPage)
        {
#if !DEBUG_MIP_LEVELS
            var vTextPage = pTexPage.VirtualTexturePage;
            //Replace color with the one on the higher mip level
            IntColor color;
            if (vTextPage.mip + 1 < highestMip)
            {
                color = new IntColor(fiBitmap[vTextPage.mip + 1].getColorAtARGB((uint)vTextPage.x >> 1, (uint)vTextPage.y >> 1, 0));
            }
            else
            {
                color = new IntColor();
                color.B = (byte)(highestMip - vTextPage.mip - 1);
            }
            byte replacementMipLevel = (byte)(highestMip - vTextPage.mip - 1);
            fiBitmap[vTextPage.mip].setColorAtARGB(color.ARGB, vTextPage.x, vTextPage.y, 0);
            fillOutLowerMips(vTextPage, color, (c1, c2) => c2.B == replacementMipLevel);
#endif
        }

        /// <summary>
        /// Save the backing buffers to the specified folder, helpful in debugging.
        /// </summary>
        /// <param name="outputFolder"></param>
        internal void saveToPath(string outputFolder)
        {
            for (uint mip = 0; mip < highestMip; ++mip)
            {
                String fileName = String.Format("{0}_source_mip_{1}.png", TextureName, mip);
                fileName = Path.Combine(outputFolder, fileName);
                fiBitmap[mip].save(fileName);
            }
        }

        private void fillOutLowerMips(VTexPage vTextPage, IntColor color, Func<IntColor, IntColor, bool> writePixel)
        {
            //Fill in lower (more textels) mip levels
            uint x = vTextPage.x;
            uint y = vTextPage.y;
            uint w = 1;
            uint h = 1;
            IntColor readPixel = new IntColor();
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
                        readPixel.ARGB = mipLevelBitmap.getColorAtARGB(x + xi, y + yi, 0);
                        if (writePixel.Invoke(color, readPixel))
                        {
                            mipLevelBitmap.setColorAtARGB(color.ARGB, x + xi, y + yi, 0);
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
