using DiligentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class SpriteMaterial : ISpriteMaterial, IDisposable
    {
        private AutoPtr<IShaderResourceBinding> shaderResourceBinding;
        private readonly ISpriteMaterialTextureManager spriteMaterialTextureManager;
        private readonly SpriteMaterialTextures textures;

        public SpriteMaterial(
            AutoPtr<IShaderResourceBinding> shaderResourceBinding, 
            int imageWidth, 
            int imageHeight, 
            ISpriteMaterialTextureManager spriteMaterialTextureManager, 
            SpriteMaterialTextures textures)
        {
            this.shaderResourceBinding = shaderResourceBinding;
            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;
            this.spriteMaterialTextureManager = spriteMaterialTextureManager;
            this.textures = textures;
        }

        public void Dispose()
        {
            spriteMaterialTextureManager.Return(textures);
            shaderResourceBinding.Dispose();
        }

        public IShaderResourceBinding ShaderResourceBinding => shaderResourceBinding.Obj;

        public int ImageWidth { get; }

        public int ImageHeight { get; }
    }
}
