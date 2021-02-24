using DiligentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    public class SpriteMaterial : ISpriteMaterial
    {
        private AutoPtr<IShaderResourceBinding> shaderResourceBinding;

        public SpriteMaterial(AutoPtr<IShaderResourceBinding> shaderResourceBinding, int imageWidth, int imageHeight)
        {
            this.shaderResourceBinding = shaderResourceBinding;
            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;
        }

        public void Dispose()
        {
            shaderResourceBinding.Dispose();
        }

        public IShaderResourceBinding ShaderResourceBinding => shaderResourceBinding.Obj;

        public int ImageWidth { get; private set; }

        public int ImageHeight { get; private set; }
    }
}
