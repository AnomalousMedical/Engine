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
        private readonly BLASBuilder blasBuilder;
        private readonly ISpriteMaterialManager spriteMaterialManager;
        private readonly PrimaryHitShaderFactory primaryHitShaderFactory;
        private readonly RayTracingRenderer rayTracingRenderer;

        public SpriteInstanceFactory(BLASBuilder blasBuilder, ISpriteMaterialManager spriteMaterialManager, PrimaryHitShaderFactory primaryHitShaderFactory, RayTracingRenderer rayTracingRenderer)
        {
            this.blasBuilder = blasBuilder;
            this.spriteMaterialManager = spriteMaterialManager;
            this.primaryHitShaderFactory = primaryHitShaderFactory;
            this.rayTracingRenderer = rayTracingRenderer;
        }

        public async Task<SpriteInstance> CreateSprite(String instanceName, SpriteMaterialDescription materialDescription)
        {
            //This is in the diligent coords
            var blasDesc = new BLASDesc()
            {
                Flags = RAYTRACING_GEOMETRY_FLAGS.RAYTRACING_GEOMETRY_FLAG_NONE
            };

            blasDesc.CubePos = new Vector3[]
            {
                new Vector3(-0.5f,-0.5f,+0.0f), new Vector3(+0.5f,-0.5f,+0.0f), new Vector3(+0.5f,+0.5f,+0.0f), new Vector3(-0.5f,+0.5f,+0.0f), //Front +z
            };

            blasDesc.CubeUV = new Vector4[]
            {
                new Vector4(1,0,0,0), new Vector4(0,0,0,0), new Vector4(0,1,0,0), new Vector4(1,1,0,0)  //Front +z
            };

            blasDesc.CubeNormals = new Vector4[]
            {
                new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0)  //Front +z
            };

            blasDesc.Indices = new uint[]
            {
                0,1,2, 0,2,3  //Front +z
            };

            var shader = primaryHitShaderFactory.Create(blasDesc.Name, 1, PrimaryHitShaderType.Sprite);
            var instance = blasBuilder.CreateBLAS(blasDesc);
            var material = spriteMaterialManager.Checkout(materialDescription);

            return new SpriteInstance(instanceName, rayTracingRenderer, await shader, await instance, await material, spriteMaterialManager);
        }
    }
}
