using DiligentEngine;
using DiligentEngine.RT;
using DiligentEngine.RT.Resources;
using DiligentEngine.RT.ShaderSets;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSandbox
{
    internal class SceneCube : IDisposable
    {
        public class Desc
        {
            public string InstanceName { get; set; } = RTId.CreateId("SceneCube");

            public uint TextureIndex { get; set; } = 0;

            public byte Mask { get; set; } = RtStructures.OPAQUE_GEOM_MASK;

            public RAYTRACING_INSTANCE_FLAGS Flags { get; set; } = RAYTRACING_INSTANCE_FLAGS.RAYTRACING_INSTANCE_NONE;

            public InstanceMatrix Transform = InstanceMatrix.Identity;

            public CCOTextureBindingDescription Texture { get; set; } = new CCOTextureBindingDescription("cc0Textures/Ground025_1K");

            public PrimaryHitShader.Desc Shader { get; set; }

            public Desc()
            {
                Shader = new PrimaryHitShader.Desc
                {
                    baseName = InstanceName,
                    numTextures = 1,
                    shaderType = PrimaryHitShaderType.Cube,
                    HasNormalMap = true,
                    HasPhysicalDescriptorMap = true,
                    Reflective = false
                };
            }
        }

        private readonly TLASBuildInstanceData instanceData;
        private readonly CubeBLAS cubeBLAS;
        private readonly RTInstances rtInstances;
        private readonly RayTracingRenderer renderer;
        private readonly TextureManager textureManager;
        private PrimaryHitShader primaryHitShader;
        CC0TextureResult cubeTexture;

        public SceneCube
        (
            Desc description,
            CubeBLAS cubeBLAS,
            IScopedCoroutine coroutine,
            RTInstances rtInstances,
            PrimaryHitShader.Factory primaryHitShaderFactory,
            RayTracingRenderer renderer,
            TextureManager textureManager
        )
        {
            this.cubeBLAS = cubeBLAS;
            this.rtInstances = rtInstances;
            this.renderer = renderer;
            this.textureManager = textureManager;
            this.instanceData = new TLASBuildInstanceData()
            {
                InstanceName = description.InstanceName,
                CustomId = description.TextureIndex, // texture index
                Mask = description.Mask,
                Transform = description.Transform,
                Flags = description.Flags,
            };

            coroutine.RunTask(async () =>
            {
                var setupShader = primaryHitShaderFactory.Create(description.Shader);
                var textureTask = textureManager.Checkout(description.Texture);

                this.primaryHitShader = await setupShader;
                this.cubeTexture = await textureTask;

                await this.cubeBLAS.WaitForLoad();
                this.instanceData.pBLAS = cubeBLAS.Instance.BLAS.Obj;
                
                rtInstances.AddTlasBuild(instanceData);
                rtInstances.AddShaderTableBinder(Bind);
                renderer.AddShaderResourceBinder(Bind);
            });
        }

        public void Dispose()
        {
            this.primaryHitShader?.Dispose();
            textureManager.TryReturn(cubeTexture);
            renderer.RemoveShaderResourceBinder(Bind);
            rtInstances.RemoveShaderTableBinder(Bind);
            rtInstances.RemoveTlasBuild(instanceData);
        }

        public void SetTransform(in Vector3 trans, in Quaternion rot)
        {
            this.instanceData.Transform = new InstanceMatrix(trans, rot);
        }

        public void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            primaryHitShader.BindSbt(instanceData.InstanceName, sbt, tlas, IntPtr.Zero, 0);
        }

        private void Bind(IShaderResourceBinding rayTracingSRB)
        {
            primaryHitShader.BindBlas(cubeBLAS.Instance, rayTracingSRB);
            primaryHitShader.BindTextures(rayTracingSRB, cubeTexture);
        }
    }
}
