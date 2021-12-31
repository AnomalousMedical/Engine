using DiligentEngine;
using DiligentEngine.RT;
using DiligentEngine.RT.HLSL;
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
                    ShaderType = PrimaryHitShaderType.Cube,
                    HasNormalMap = true,
                    HasPhysicalDescriptorMap = true,
                    Reflective = false
                };
            }
        }

        private readonly TLASBuildInstanceData instanceData;
        private readonly CubeBLAS cubeBLAS;
        private readonly RTInstances rtInstances;
        private readonly PrimaryHitShader.Factory primaryHitShaderFactory;
        private readonly RayTracingRenderer renderer;
        private readonly TextureManager textureManager;
        private readonly ActiveTextures activeTextures;
        private PrimaryHitShader primaryHitShader;
        private CC0TextureResult cubeTexture;
        private BlasInstanceData blasInstanceData;

        public SceneCube
        (
            Desc description,
            CubeBLAS cubeBLAS,
            IScopedCoroutine coroutine,
            RTInstances rtInstances,
            PrimaryHitShader.Factory primaryHitShaderFactory,
            RayTracingRenderer renderer,
            TextureManager textureManager,
            ActiveTextures activeTextures
        )
        {
            this.cubeBLAS = cubeBLAS;
            this.rtInstances = rtInstances;
            this.primaryHitShaderFactory = primaryHitShaderFactory;
            this.renderer = renderer;
            this.textureManager = textureManager;
            this.activeTextures = activeTextures;
            this.instanceData = new TLASBuildInstanceData()
            {
                InstanceName = description.InstanceName,
                Mask = description.Mask,
                Transform = description.Transform,
                Flags = description.Flags,
            };

            coroutine.RunTask(async () =>
            {
                var primaryHitShaderTask = primaryHitShaderFactory.Checkout(description.Shader);
                var cubeTextureTask = textureManager.Checkout(description.Texture);

                await Task.WhenAll
                (
                    cubeBLAS.WaitForLoad(),
                    primaryHitShaderTask,
                    cubeTextureTask
                );

                this.instanceData.pBLAS = cubeBLAS.Instance.BLAS.Obj;
                this.primaryHitShader = primaryHitShaderTask.Result;
                this.cubeTexture = cubeTextureTask.Result;
                blasInstanceData = this.activeTextures.AddActiveTexture(this.cubeTexture);
                rtInstances.AddTlasBuild(instanceData);
                rtInstances.AddShaderTableBinder(Bind);
            });
        }

        public void Dispose()
        {
            this.activeTextures.RemoveActiveTexture(this.cubeTexture);
            primaryHitShaderFactory.TryReturn(primaryHitShader);
            textureManager.TryReturn(cubeTexture);
            rtInstances.RemoveShaderTableBinder(Bind);
            rtInstances.RemoveTlasBuild(instanceData);
        }

        public void SetTransform(in Vector3 trans, in Quaternion rot)
        {
            this.instanceData.Transform = new InstanceMatrix(trans, rot);
        }

        private unsafe void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            blasInstanceData.vertexOffset = cubeBLAS.Instance.VertexOffset;
            blasInstanceData.indexOffset = cubeBLAS.Instance.IndexOffset;
            fixed (BlasInstanceData* ptr = &blasInstanceData)
            {
                primaryHitShader.BindSbt(instanceData.InstanceName, sbt, tlas, new IntPtr(ptr), (uint)sizeof(BlasInstanceData));
            }
        }
    }
}
