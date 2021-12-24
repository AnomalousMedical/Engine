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
                var instanceName = Guid.NewGuid().ToString("N");

                //TODO: This gives one shader each time we change the materials. To pool the shaders do it here.
                var shader = primaryHitShaderFactory.Create(instanceName, 1, PrimaryHitShaderType.Sprite);
                var material = spriteMaterialManager.Checkout(desc);

                var instance = new SpriteInstance(rayTracingRenderer, await shader, spriteBLAS.Instance, await material, spriteMaterialManager);
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
