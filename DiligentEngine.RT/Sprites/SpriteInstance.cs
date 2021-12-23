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
        private readonly SpriteMaterial spriteMaterial;
        private readonly ISpriteMaterialManager spriteMaterialManager;
        private PrimaryHitShader primaryHitShader;
        private readonly RayTracingRenderer renderer;

        public BLASInstance Instance => instance;

        public SpriteInstance
        (
            RayTracingRenderer renderer, 
            PrimaryHitShader primaryHitShader,
            BLASInstance blas,
            SpriteMaterial spriteMaterial,
            ISpriteMaterialManager spriteMaterialManager
        )
        {
            this.primaryHitShader = primaryHitShader;
            this.instance = blas;
            this.spriteMaterial = spriteMaterial;
            this.spriteMaterialManager = spriteMaterialManager;
            this.renderer = renderer;

            renderer.AddShaderResourceBinder(Bind);
        }

        public void Dispose()
        {
            renderer.RemoveShaderResourceBinder(Bind);
            primaryHitShader.Dispose();
            spriteMaterialManager.Return(spriteMaterial);
        }

        private void Bind(IShaderResourceBinding rayTracingSRB)
        {
            primaryHitShader.BindBlas(instance, rayTracingSRB);
            primaryHitShader.BindTextures(rayTracingSRB, spriteMaterial);
        }

        static int count = 0; //Cheaty way to get some dynamic data

        public unsafe void Bind(String instanceName, IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            var info = new HLSL.SpriteFrame() { u = count++ % 2 == 0 ? 0.1f : 0.9f };
            sbt.BindHitGroupForInstance(tlas, instanceName, RtStructures.PRIMARY_RAY_INDEX, primaryHitShader.ShaderGroupName, new IntPtr(&info), (uint)sizeof(HLSL.SpriteFrame));
        }
    }
}
