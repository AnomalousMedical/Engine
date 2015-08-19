//#define DRAW_MIP_MARKERS
//#define DRAW_SKIPS

using Engine;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin.VirtualTexture
{
    public enum PixelFormatUsageHint
    {
        NotSpecial,
        NormalMap,
        OpacityMap,
    }

    public class PhysicalTexture : IDisposable
    {
        private TexturePtr physicalTexture;
        private HardwarePixelBufferSharedPtr buffer;
        private String textureName;
        private VirtualTextureManager virtualTextureManager;
        private String name;
        private int texelsPerPage;
        private IntSize2 size;
        private static int currentId = 0;

        public PhysicalTexture(String name, IntSize2 size, VirtualTextureManager virtualTextureManager, int texelsPerPage, CompressedTextureSupport texFormat, PixelFormatUsageHint textureUsage)
        {
            Index = currentId++;
            this.name = name;
            this.texelsPerPage = texelsPerPage;
            this.size = size;
            this.virtualTextureManager = virtualTextureManager;
            this.textureName = "PhysicalTexture" + name;
            PixelFormat pixelFormat;
            switch (texFormat)
            {
                case CompressedTextureSupport.DXT_BC4_BC5:
                    switch (textureUsage)
                    {
                        case PixelFormatUsageHint.NormalMap:
                            pixelFormat = PixelFormat.PF_BC5_UNORM;
                            break;
                        default:
                            pixelFormat = PixelFormat.PF_DXT5;
                            break;
                    }
                    break;
                case CompressedTextureSupport.DXT:
                    pixelFormat = PixelFormat.PF_DXT5;
                    break;
                case CompressedTextureSupport.None:
                    pixelFormat = PixelFormat.PF_A8R8G8B8;
                    break;
                default:
                    throw new NotSupportedException(String.Format("Cannot create virtual textures for format {0}", texFormat));
            }

            physicalTexture = TextureManager.getInstance().createManual(textureName, VirtualTextureManager.ResourceGroup, TextureType.TEX_TYPE_2D,
                        (uint)size.Width, (uint)size.Height, 1, 0, pixelFormat, TextureUsage.TU_DEFAULT, null, false, 0);
            buffer = physicalTexture.Value.getBuffer();
        }

        public void Dispose()
        {
            buffer.Dispose();
            TextureManager.getInstance().remove(physicalTexture);
            physicalTexture.Dispose();
        }

        public unsafe void color(Color color)
        {
            if (physicalTexture.Value.Format == PixelFormat.PF_A8R8G8B8)
            {
                using (var fiBitmap = new FreeImageAPI.FreeImageBitmap((int)physicalTexture.Value.Width, (int)physicalTexture.Value.Height, FreeImageAPI.PixelFormat.Format32bppArgb))
                {
                    var fiColor = new FreeImageAPI.Color();
                    fiColor.R = (byte)(color.r * 255);
                    fiColor.G = (byte)(color.g * 255);
                    fiColor.B = (byte)(color.b * 255);
                    fiColor.A = (byte)(color.a * 255);
                    fiBitmap.FillBackground(new FreeImageAPI.RGBQUAD(fiColor));

                    using (var buffer = physicalTexture.Value.getBuffer())
                    {
                        using (PixelBox pixelBox = new PixelBox(0, 0, fiBitmap.Width, fiBitmap.Height, OgreDrawingUtility.getOgreFormat(fiBitmap.PixelFormat), fiBitmap.GetScanlinePointer(0).ToPointer()))
                        {
                            buffer.Value.blitFromMemory(pixelBox);
                        }
                    }
                }
            }
        }

        public void addPage(PixelBox source, IntRect destRect)
        {
            buffer.Value.blitFromMemory(source, destRect);
        }

        public String TextureName
        {
            get
            {
                return textureName;
            }
        }

        public PixelFormat TextureFormat
        {
            get
            {
                return physicalTexture.Value.Format;
            }
        }

        /// <summary>
        /// A numerical id for this texture, can be used in arrays.
        /// </summary>
        internal int Index { get; private set; }
    }
}
