//#define DEBUG_MIPMAP_LEVELS

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
# if DEBUG_MIPMAP_LEVELS
        static FreeImageAPI.Color[] mipColors = new FreeImageAPI.Color[17];
        static IndirectionTexture()
        {
            //Colors from https://en.wikipedia.org/wiki/List_of_Crayola_crayon_colors#24_pack_Mini_Twistables

            mipColors[0] = new FreeImageAPI.Color() { R = 28, G = 172, B = 120, A = 255 }; //Green
            mipColors[1] = new FreeImageAPI.Color() { R = 238, G = 32, B = 77, A = 255 }; //Red
            mipColors[2] = new FreeImageAPI.Color() { R = 31, G = 117, B = 254, A = 255 }; //Blue
            mipColors[3] = new FreeImageAPI.Color() { R = 115, G = 102, B = 189, A = 255 }; //Blue Violet
            mipColors[4] = new FreeImageAPI.Color() { R = 180, G = 103, B = 77, A = 255 }; //Brown
            mipColors[5] = new FreeImageAPI.Color() { R = 255, G = 170, B = 204, A = 255 }; //Carnation Pink
            mipColors[6] = new FreeImageAPI.Color() { R = 253, G = 219, B = 109, A = 255 }; //Dandelion
            mipColors[7] = new FreeImageAPI.Color() { R = 149, G = 145, B = 140, A = 255 }; //Gray
            mipColors[8] = new FreeImageAPI.Color() { R = 255, G = 117, B = 56, A = 255 }; //Orange
            mipColors[9] = new FreeImageAPI.Color() { R = 0, G = 0, B = 0, A = 255 }; //Black
            mipColors[10] = new FreeImageAPI.Color() { R = 253, G = 217, B = 181, A = 255 }; //Apricot
            mipColors[11] = new FreeImageAPI.Color() { R = 192, G = 68, B = 143, A = 255 }; //Red Violet
            mipColors[12] = new FreeImageAPI.Color() { R = 255, G = 255, B = 255, A = 255 }; //White
            mipColors[13] = new FreeImageAPI.Color() { R = 252, G = 232, B = 131, A = 255 }; //Yellow
            mipColors[14] = new FreeImageAPI.Color() { R = 197, G = 227, B = 132, A = 255 }; //Yellow Green
            mipColors[15] = new FreeImageAPI.Color() { R = 255, G = 174, B = 66, A = 255 }; //Yellow Orange
            mipColors[16] = new FreeImageAPI.Color() { R = 246, G = 83, B = 166, A = 255 }; //Permanent Magenta
        }
#endif

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

        private byte id = generateId();
        private IntSize2 realTextureSize;
        private TexturePtr indirectionTexture;
        private VirtualTextureManager virtualTextureManager;
        private IntSize2 numPages;
        private byte highestMip = 0; //The highest mip level that does not fall below one page in size
        private FreeImageAPI.FreeImageBitmap[] fiBitmap; //Can we do this without this bitmap? (might be ok to keep, but will be using 2x as much memory, however, allows for background modification, could even double buffer)
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
                (uint)numPages.Width, (uint)numPages.Height, 1, highestMip, PixelFormat.PF_A8R8G8B8, virtualTextureManager.RendersystemSpecificTextureUsage, null, false, 0);
            indirectionTexture.Value.AllowMipmapGeneration = false;

            fiBitmap = new FreeImageAPI.FreeImageBitmap[highestMip];
            buffer = new HardwarePixelBufferSharedPtr[highestMip];
            pixelBox = new PixelBox[highestMip];

            for (int i = 0; i < highestMip; ++i)
            {
                fiBitmap[i] = new FreeImageAPI.FreeImageBitmap((int)indirectionTexture.Value.Width >> i, (int)indirectionTexture.Value.Height >> i, FreeImageAPI.PixelFormat.Format32bppArgb);
#if DEBUG_MIPMAP_LEVELS
                fiBitmap[i].FillBackground(new FreeImageAPI.RGBQUAD(mipColors[i]));
#endif
                buffer[i] = indirectionTexture.Value.getBuffer(0, (uint)i);
                unsafe
                {
                    pixelBox[i] = new PixelBox(0, 0, fiBitmap[i].Width, fiBitmap[i].Height, OgreDrawingUtility.getOgreFormat(fiBitmap[i].PixelFormat), fiBitmap[i].GetScanlinePointer(0).ToPointer());
                }
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
            int srcIndex = 0;
            for (int i = destinations.Length - highestMip; i < destinations.Length; ++i)
            {
                PixelBox.BulkPixelConversion(pixelBox[srcIndex++], destinations[i]);
            }
        }

        internal void uploadStagingToGpu(PixelBox[] sources)
        {
            int destIndex = 0;
            for (int i = sources.Length - highestMip; i < sources.Length; ++i)
            {
                buffer[destIndex++].Value.blitFromMemory(sources[i]);
            }
        }

        internal void addPhysicalPage(PTexPage pTexPage)
        {
#if !DEBUG_MIPMAP_LEVELS
            //Store 1x1 as mip 0, 2x2 as 1 4x4 as 2 etc, this way we can directly shift the decimal place
            //Then we will take fract from that
            //Store the page address as bytes
            var vTextPage = pTexPage.VirtualTexturePage;
            var color = new FreeImageAPI.Color();
            color.A = 255; //Using this for now for page enabled (255) / disabled (0)
            //Reverse the mip level (0 becomes highest level (least texels) and highesetMip becomes the lowest level (most texels, full size)
            color.B = (byte)(highestMip - vTextPage.mip - 1); //Typecast bad, try changing the type in the struct to byte
            color.R = (byte)pTexPage.pageX;
            color.G = (byte)pTexPage.pageY;

            fiBitmap[vTextPage.mip].SetPixel(vTextPage.x, vTextPage.y, color);
            fillOutLowerMips(vTextPage, color, (c1, c2) => c1.B - c2.B >= 0);
#endif
        }

        internal void removePhysicalPage(PTexPage pTexPage)
        {
            var vTextPage = pTexPage.VirtualTexturePage;
            //Replace color with the one on the higher mip level
            FreeImageAPI.Color color;
            if (vTextPage.mip + 1 < highestMip)
            {
                color = fiBitmap[vTextPage.mip + 1].GetPixel(vTextPage.x >> 1, vTextPage.y >> 1);
            }
            else
            {
                color = new FreeImageAPI.Color();
                color.B = (byte)(highestMip - vTextPage.mip - 1);
            }
            byte replacementMipLevel = (byte)(highestMip - vTextPage.mip - 1);
            fiBitmap[vTextPage.mip].SetPixel(vTextPage.x, vTextPage.y, color);
            fillOutLowerMips(vTextPage, color, (c1, c2) => c2.B == replacementMipLevel);
        }

        private void fillOutLowerMips(VTexPage vTextPage, FreeImageAPI.Color color, Func<FreeImageAPI.Color, FreeImageAPI.Color, bool> writePixel)
        {
            //Fill in lower (more textels) mip levels
            int x = vTextPage.x;
            int y = vTextPage.y;
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
                for (int xi = 0; xi < w; ++xi)
                {
                    for (int yi = 0; yi < h; ++yi)
                    {
                        var readPixel = mipLevelBitmap.GetPixel(x + xi, y + yi);
                        if (writePixel.Invoke(color, readPixel))
                        {
                            mipLevelBitmap.SetPixel(x + xi, y + yi, color);
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
