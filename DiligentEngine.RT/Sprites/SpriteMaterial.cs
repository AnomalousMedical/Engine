using DiligentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT.Sprites
{
    class SpriteMaterial : ISpriteMaterial, IDisposable
    {
        private readonly ISpriteMaterialTextureManager spriteMaterialTextureManager;
        private readonly SpriteMaterialTextures textures;
        private readonly AutoPtr<ITexture> colorTexture;

        public SpriteMaterial(
            int imageWidth, 
            int imageHeight, 
            ISpriteMaterialTextureManager spriteMaterialTextureManager, 
            SpriteMaterialTextures textures,
            ITexture colorTexture)
        {
            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;
            this.spriteMaterialTextureManager = spriteMaterialTextureManager;
            this.textures = textures;
            this.colorTexture = new AutoPtr<ITexture>(colorTexture);
        }

        public void Dispose()
        {
            colorTexture.Dispose();
            spriteMaterialTextureManager.Return(textures);
        }

        public int ImageWidth { get; }

        public int ImageHeight { get; }
    }
}
