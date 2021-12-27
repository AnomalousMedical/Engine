using DiligentEngine.RT.ShaderSets;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT.Sprites
{
    public class SpriteInstanceFactory
    {
        private readonly SpritePlaneBLAS spriteBLAS;
        private readonly ISpriteMaterialManager spriteMaterialManager;
        private readonly PrimaryHitShader.Factory primaryHitShaderFactory;
        private readonly RayTracingRenderer rayTracingRenderer;

        private readonly PooledResourceManager<SpriteMaterialDescription, SpriteInstance> pooledResources
            = new PooledResourceManager<SpriteMaterialDescription, SpriteInstance>();

        public SpriteInstanceFactory(SpritePlaneBLAS spriteBLAS, ISpriteMaterialManager spriteMaterialManager, PrimaryHitShader.Factory primaryHitShaderFactory, RayTracingRenderer rayTracingRenderer)
        {
            this.spriteBLAS = spriteBLAS;
            this.spriteMaterialManager = spriteMaterialManager;
            this.primaryHitShaderFactory = primaryHitShaderFactory;
            this.rayTracingRenderer = rayTracingRenderer;
        }

        public Task<SpriteInstance> Checkout(SpriteMaterialDescription desc)
        {
            return pooledResources.Checkout(desc, async () =>
            {
                var instanceName = RTId.CreateId("SpriteInstanceFactory");

                var material = await spriteMaterialManager.Checkout(desc);

                //TODO: This gives one shader each time we change the materials. To pool the shaders do it here.
                var shader = await primaryHitShaderFactory.Create(new PrimaryHitShader.Desc()
                {
                    baseName = instanceName,
                    numTextures = 1,
                    shaderType = PrimaryHitShaderType.Sprite,
                    HasNormalMap = material.NormalSRV != null,
                    HasPhysicalDescriptorMap = desc.Shiny && material.PhysicalSRV != null
                });

                var instance = new SpriteInstance(rayTracingRenderer, shader, spriteBLAS.Instance, material, spriteMaterialManager);
                return pooledResources.CreateResult(instance);
            });
        }

        public void TryReturn(SpriteInstance item)
        {
            if (item != null)
            {
                pooledResources.Return(item);
            }
        }
    }
}
