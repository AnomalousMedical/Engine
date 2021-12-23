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
        private readonly PrimaryHitShaderFactory primaryHitShaderFactory;
        private readonly RayTracingRenderer rayTracingRenderer;

        public SpriteInstanceFactory(SpritePlaneBLAS spriteBLAS, ISpriteMaterialManager spriteMaterialManager, PrimaryHitShaderFactory primaryHitShaderFactory, RayTracingRenderer rayTracingRenderer)
        {
            this.spriteBLAS = spriteBLAS;
            this.spriteMaterialManager = spriteMaterialManager;
            this.primaryHitShaderFactory = primaryHitShaderFactory;
            this.rayTracingRenderer = rayTracingRenderer;
        }

        public async Task<SpriteInstance> CreateSprite(String instanceName, SpriteMaterialDescription materialDescription)
        {
            var shader = primaryHitShaderFactory.Create(instanceName, 1, PrimaryHitShaderType.Sprite);
            var material = spriteMaterialManager.Checkout(materialDescription);

            return new SpriteInstance(instanceName, rayTracingRenderer, await shader, spriteBLAS.Instance, await material, spriteMaterialManager);
        }
    }
}
