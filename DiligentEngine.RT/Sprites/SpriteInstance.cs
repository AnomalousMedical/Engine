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
            primaryHitShader.BindTextures(rayTracingSRB, spriteMaterial);
        }

        public unsafe void Bind(String instanceName, IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            var info = new HLSL.SpriteFrame()
            {
                u1 = 1f, v1 = 0f,
                u2 = 0f, v2 = 0f,
                u3 = 0f, v3 = 1f,
                u4 = 1f, v4 = 1f,
            };

            primaryHitShader.BindSbt(instanceName, sbt, tlas, new IntPtr(&info), (uint)sizeof(HLSL.SpriteFrame));
        }

        public unsafe void Bind(String instanceName, IShaderBindingTable sbt, ITopLevelAS tlas, SpriteFrame frame)
        {
            var info = new HLSL.SpriteFrame()
            {
                u1 = frame.Right, v1 = frame.Top,
                u2 = frame.Left, v2 = frame.Top,
                u3 = frame.Left, v3 = frame.Bottom,
                u4 = frame.Right, v4 = frame.Bottom,
            };

            primaryHitShader.BindSbt(instanceName, sbt, tlas, new IntPtr(&info), (uint)sizeof(HLSL.SpriteFrame));
        }
    }
}
