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

        public unsafe void Bind(String instanceName, IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            //new Vector4(0.5f,0.5f,0,0), new Vector4(0.75f,0.5f,0,0), new Vector4(0.75f,1,0,0), new Vector4(0.5f,1,0,0)

            var info = new HLSL.SpriteFrame()
            {
                u1 = 0.5f, v1 = 0.5f,
                u2 = 0.75f, v2 = 0.5f,
                u3 = 0.75f, v3 = 1.0f,
                u4 = 0.5f, v4 = 1.0f,
            };

            sbt.BindHitGroupForInstance(tlas, instanceName, RtStructures.PRIMARY_RAY_INDEX, primaryHitShader.ShaderGroupName, new IntPtr(&info), (uint)sizeof(HLSL.SpriteFrame));
        }
    }
}
