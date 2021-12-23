using DiligentEngine;
using DiligentEngine.RT.ShaderSets;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT.Sprites
{
    public class SpriteInstance : IDisposable
    {
        private BLASInstance instance;
        private PrimaryHitShader primaryHitShader;
        private readonly RayTracingRenderer renderer;

        public BLASInstance Instance => instance;

        public PrimaryHitShader PrimaryHitShader => primaryHitShader;

        public SpriteInstance
        (
            RayTracingRenderer renderer, 
            PrimaryHitShader primaryHitShader
        )
        {
            this.primaryHitShader = primaryHitShader;
            this.renderer = renderer;
        }

        public void Dispose()
        {
            renderer.RemoveShaderResourceBinder(Bind);
            instance.Dispose();
        }

        private void Bind(IShaderResourceBinding rayTracingSRB)
        {
            primaryHitShader.BindBlas(instance, rayTracingSRB);
        }
    }
}
