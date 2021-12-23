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
        private readonly string instanceName;
        private readonly RayTracingRenderer renderer;

        public BLASInstance Instance => instance;

        public SpriteInstance
        (
            String instanceName,
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
            this.instanceName = instanceName;
            this.renderer = renderer;

            renderer.AddShaderResourceBinder(Bind);
            renderer.AddShaderTableBinder(Bind);
        }

        public void Dispose()
        {
            renderer.RemoveShaderTableBinder(Bind);
            renderer.RemoveShaderResourceBinder(Bind);
            primaryHitShader.Dispose();
            spriteMaterialManager.Return(spriteMaterial);
        }

        private void Bind(IShaderResourceBinding rayTracingSRB)
        {
            primaryHitShader.BindBlas(instance, rayTracingSRB);
            primaryHitShader.BindTextures(rayTracingSRB, spriteMaterial);
        }

        public void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            sbt.BindHitGroupForInstance(tlas, instanceName, RtStructures.PRIMARY_RAY_INDEX, primaryHitShader.ShaderGroupName, IntPtr.Zero);
        }
    }
}
