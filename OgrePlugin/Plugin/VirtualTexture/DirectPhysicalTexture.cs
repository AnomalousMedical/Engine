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
    /// <summary>
    /// A "normal" physical texture. Only has one buffer and updates directly. This should be used almost always
    /// unless the drivers on your platform are no good.
    /// </summary>
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

        private HashSet<TextureUnitState> textureUnits = new HashSet<TextureUnitState>();
        private PixelFormat pixelFormat;
        private String currentTextureName;

        public DirectPhysicalTexture(String name, IntSize2 size, VirtualTextureManager virtualTextureManager, int texelsPerPage, PixelFormat pixelFormat)
        {
            Index = currentId++;
            this.name = name;
            this.texelsPerPage = texelsPerPage;
            this.size = size;
            this.virtualTextureManager = virtualTextureManager;
            this.textureName = "PhysicalTexture" + name;
            this.currentTextureName = textureName;
            this.pixelFormat = pixelFormat;

            createTexture();
        }

        public void Dispose()
        {
            destroyTexture();
        }

        public void prepareForUpdates()
        {

        }

        public void addPage(PixelBox source, IntRect destRect)
        {
            if (physicalTexture != null)
            {
                buffer.Value.blitFromMemory(source, destRect);
            }
        }

        public void commitUpdates()
        {

        }

        public void createTextureUnit(Pass pass)
        {
            var texUnit = pass.createTextureUnitState(currentTextureName);
            texUnit.Name = name;
            textureUnits.Add(texUnit);
        }

        public void removeTextureUnit(TextureUnitState textureUnit)
        {
            textureUnits.Remove(textureUnit);
        }

        public void suspendTexture(String placeholderName)
        {
            currentTextureName = placeholderName;
            foreach (var unit in textureUnits)
            {
                unit.TextureName = placeholderName;
            }
            destroyTexture();
        }

        public void restoreTexture()
        {
            currentTextureName = textureName;
            createTexture();
            foreach (var unit in textureUnits)
            {
                unit.TextureName = textureName;
            }
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
                return pixelFormat;
            }
        }

        /// <summary>
        /// A numerical id for this texture, can be used in arrays.
        /// </summary>
        public int Index { get; private set; }

        private void createTexture()
        {
            physicalTexture = TextureManager.getInstance().createManual(textureName, VirtualTextureManager.ResourceGroup, TextureType.TEX_TYPE_2D,
                        (uint)size.Width, (uint)size.Height, 1, 0, pixelFormat, virtualTextureManager.RendersystemSpecificTextureUsage, null, false, 0);
            buffer = physicalTexture.Value.getBuffer();
        }

        private void destroyTexture()
        {
            if (physicalTexture != null)
            {
                buffer.Dispose();
                TextureManager.getInstance().remove(physicalTexture);
                physicalTexture.Dispose();
                physicalTexture = null;
            }
        }
    }
}
