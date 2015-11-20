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
    public class DirectPhysicalTexture : PhysicalTexture
    {
        private TexturePtr physicalTexture;
        private HardwarePixelBufferSharedPtr buffer;
        private String textureName;
        private VirtualTextureManager virtualTextureManager;
        private String name;
        private int texelsPerPage;
        private IntSize2 size;
        private static int currentId = 0;

        public DirectPhysicalTexture(String name, IntSize2 size, VirtualTextureManager virtualTextureManager, int texelsPerPage, PixelFormat pixelFormat)
        {
            Index = currentId++;
            this.name = name;
            this.texelsPerPage = texelsPerPage;
            this.size = size;
            this.virtualTextureManager = virtualTextureManager;
            this.textureName = "PhysicalTexture" + name;

            physicalTexture = TextureManager.getInstance().createManual(textureName, VirtualTextureManager.ResourceGroup, TextureType.TEX_TYPE_2D,
                        (uint)size.Width, (uint)size.Height, 1, 0, pixelFormat, virtualTextureManager.RendersystemSpecificTextureUsage, null, false, 0);
            buffer = physicalTexture.Value.getBuffer();
        }

        public void Dispose()
        {
            buffer.Dispose();
            TextureManager.getInstance().remove(physicalTexture);
            physicalTexture.Dispose();
        }

        public void prepareForUpdates()
        {

        }

        public void addPage(PixelBox source, IntRect destRect)
        {
            buffer.Value.blitFromMemory(source, destRect);
        }

        public void commitUpdates()
        {

        }

        public void createTextureUnit(Pass pass)
        {
            var texUnit = pass.createTextureUnitState(textureName);
            texUnit.Name = name;
        }

        public void removeTextureUnit(TextureUnitState textureUnit)
        {

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
        public int Index { get; private set; }
    }
}
