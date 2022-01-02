using DiligentEngine.RT.Resources;
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
        private readonly ActiveTextures activeTextures;

        private readonly PooledResourceManager<SpriteMaterialDescription, SpriteInstance> pooledResources
            = new PooledResourceManager<SpriteMaterialDescription, SpriteInstance>();

        public SpriteInstanceFactory
        (
            SpritePlaneBLAS spriteBLAS, 
            ISpriteMaterialManager spriteMaterialManager, 
            PrimaryHitShader.Factory primaryHitShaderFactory,
            ActiveTextures activeTextures
        )
        {
            this.spriteBLAS = spriteBLAS;
            this.spriteMaterialManager = spriteMaterialManager;
            this.primaryHitShaderFactory = primaryHitShaderFactory;
            this.activeTextures = activeTextures;
        }

        public Task<SpriteInstance> Checkout(SpriteMaterialDescription desc)
        {
            return pooledResources.Checkout(desc, async () =>
            {
                var instanceName = RTId.CreateId("SpriteInstanceFactory");

                var material = await spriteMaterialManager.Checkout(desc);

                var shader = await primaryHitShaderFactory.Checkout(new PrimaryHitShader.Desc()
                {
                    ShaderType = PrimaryHitShaderType.Sprite,
                    HasNormalMap = material.NormalSRV != null,
                    HasPhysicalDescriptorMap = material.PhysicalSRV != null,
                    Reflective = material.Reflective
                });

                var instance = new SpriteInstance(spriteBLAS, shader, primaryHitShaderFactory, material, spriteMaterialManager, activeTextures);
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
