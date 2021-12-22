using DiligentEngine;
using DiligentEngine.RT.ShaderSets;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT
{
    public class PlaneBLAS : IDisposable, IShaderResourceBinder
    {
        private BLASInstance instance;
        private PrimaryHitShader primaryHitShader;
        private readonly TextureManager textureManager;
        private readonly RayTracingRenderer renderer;

        public BLASInstance Instance => instance;

        public String ShaderGroupName => primaryHitShader.ShaderGroupName;

        public unsafe PlaneBLAS(BLASBuilder blasBuilder, RTShaders shaders, TextureManager textureManager, RayTracingRenderer renderer)
        {

            this.textureManager = textureManager;
            this.renderer = renderer;
            var blasDesc = new BLASDesc();

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

            instance = blasBuilder.CreateBLAS(blasDesc);

            primaryHitShader = shaders.CreatePrimaryHitShader(new PrimaryHitShader.Desc()
            {
                NumTextures = 5,
                BaseName = blasDesc.Name,
            });

            renderer.AddShaderResourceBinder(this);
        }

        public void Dispose()
        {
            renderer.RemoveShaderResourceBinder(this);
            primaryHitShader?.Dispose();
            instance.Dispose();
        }

        public void Bind(IShaderResourceBinding rayTracingSRB)
        {
            primaryHitShader.BindBlas(instance, rayTracingSRB);
            primaryHitShader.BindTextures(rayTracingSRB, textureManager);
        }
    }
}
