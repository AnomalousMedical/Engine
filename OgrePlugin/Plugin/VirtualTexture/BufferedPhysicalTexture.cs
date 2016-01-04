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
    /// This physical texture uses multiple front buffers to avoid pipeline stalls
    /// updating virtual textures. This is mostly only needed on arm devices since their
    /// drivers handle this situation the worse way possible.
    /// </summary>
    public class BufferedPhysicalTexture : PhysicalTexture
    {
        private static int currentId = 0;

        private String textureName;
        private VirtualTextureManager virtualTextureManager;
        private String name;
        private int texelsPerPage;
        private IntSize2 size;
        private FrontBuffer[] frontBuffers;
        private int frontBufferIndex = 0;
        private PixelFormat pixelFormat;
        private String placeholderTextureName;

        private HashSet<TextureUnitState> textureUnits = new HashSet<TextureUnitState>();

        public BufferedPhysicalTexture(String name, IntSize2 size, VirtualTextureManager virtualTextureManager, int texelsPerPage, PixelFormat pixelFormat)
        {
            Index = currentId++;
            this.name = name;
            this.texelsPerPage = texelsPerPage;
            this.size = size;
            this.virtualTextureManager = virtualTextureManager;
            this.textureName = "PhysicalTexture" + name;
            this.pixelFormat = pixelFormat;
            createTexture();
        }

        public void Dispose()
        {
            destroyTexture();
        }

        public void prepareForUpdates()
        {
            if (frontBuffers != null)
            {
                //Increment the current front buffer and reset
                frontBufferIndex = (frontBufferIndex + 1) % frontBuffers.Length;
                frontBuffers[frontBufferIndex].Reset();

                //Merge changes from other front buffer updates
                int updateIndex = (frontBufferIndex + 1) % frontBuffers.Length;
                while (updateIndex != frontBufferIndex)
                {
                    frontBuffers[frontBufferIndex].mergeUpdates(frontBuffers[updateIndex]);
                    updateIndex = (updateIndex + 1) % frontBuffers.Length;
                }
            }
        }

        public void addPage(PixelBox source, IntRect destRect)
        {
            if (frontBuffers != null)
            {
                frontBuffers[frontBufferIndex].addPage(source, destRect);
            }
        }

        public void commitUpdates()
        {
            if (frontBuffers != null)
            {
                foreach (var unit in textureUnits)
                {
                    unit.TextureName = frontBuffers[frontBufferIndex].Name;
                }
            }
            else
            {
                foreach (var unit in textureUnits)
                {
                    unit.TextureName = placeholderTextureName;
                }
            }
        }

        public void createTextureUnit(Pass pass)
        {
            String finalName = placeholderTextureName;
            if(frontBuffers != null)
            {
                finalName = frontBuffers[frontBufferIndex].Name;
            }

            var texUnit = pass.createTextureUnitState(finalName);
            texUnit.Name = name;
            textureUnits.Add(texUnit);
        }

        public void removeTextureUnit(TextureUnitState textureUnit)
        {
            textureUnits.Remove(textureUnit);
        }

        public void suspendTexture(String placeholderName)
        {
            this.placeholderTextureName = placeholderName;
            foreach (var unit in textureUnits)
            {
                unit.TextureName = placeholderName;
            }
            destroyTexture();
        }

        public void restoreTexture()
        {
            createTexture();
            foreach (var unit in textureUnits)
            {
                unit.TextureName = frontBuffers[frontBufferIndex].Name;
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
            frontBuffers = new FrontBuffer[2];
            for (int i = 0; i < frontBuffers.Length; ++i)
            {
                frontBuffers[i] = new FrontBuffer(TextureManager.getInstance().createManual(String.Format("{0}_BackBuffer_{1}", textureName, i), VirtualTextureManager.ResourceGroup, TextureType.TEX_TYPE_2D,
                    (uint)size.Width, (uint)size.Height, 1, 0, pixelFormat, virtualTextureManager.RendersystemSpecificTextureUsage, null, false, 0));
            }
        }

        private void destroyTexture()
        {
            if (frontBuffers != null)
            {
                for (int i = 0; i < frontBuffers.Length; ++i)
                {
                    frontBuffers[i].Dispose();
                }
                frontBuffers = null;
            }
        }

        class PageInfo : IDisposable
        {
            private Image image;
            public PixelBox pixelBox;
            public IntRect destRect;

            public PageInfo(PixelBox source, IntRect destRect, PixelFormat format)
            {
                this.image = new Image((uint)source.getWidth(), (uint)source.getHeight(), (uint)source.getDepth(), format, 1, 1);
                pixelBox = image.getPixelBox();
                PixelBox.BulkPixelConversion(source, pixelBox);
                this.destRect = destRect;
            }

            public void Dispose()
            {
                pixelBox.Dispose();
                image.Dispose();
            }
        }

        class FrontBuffer : IDisposable
        {
            private TexturePtr texture;
            private HardwarePixelBufferSharedPtr pixelBuffer;
            private List<PageInfo> updatedPages = new List<PageInfo>();

            public FrontBuffer(TexturePtr texture)
            {
                this.texture = texture;
                pixelBuffer = texture.Value.getBuffer();
            }

            public void Dispose()
            {
                Reset();
                pixelBuffer.Dispose();
                TextureManager.getInstance().remove(texture);
                texture.Dispose();
            }

            public void Reset()
            {
                foreach (var pageInfo in updatedPages)
                {
                    pageInfo.Dispose();
                }
                updatedPages.Clear();
            }

            public void addPage(PixelBox source, IntRect destRect)
            {
                pixelBuffer.Value.blitFromMemory(source, destRect);
                updatedPages.Add(new PageInfo(source, destRect, texture.Value.Format));
            }

            public void mergeUpdates(FrontBuffer otherBuffer)
            {
                foreach (var page in otherBuffer.updatedPages)
                {
                    pixelBuffer.Value.blitFromMemory(page.pixelBox, page.destRect);
                }
            }

            public String Name
            {
                get
                {
                    return texture.Value.Name;
                }
            }

            public PixelFormat Format
            {
                get
                {
                    return texture.Value.Format;
                }
            }
        }
    }
}
