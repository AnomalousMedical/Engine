using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPlugin;
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

namespace RTBepuDemo
{
    /// <summary>
    /// This class helps keep the position in sync between physics objects and their owner.
    /// </summary>
    class BodyPositionSync : IDisposable
    {
        public class Desc
        {
            public Box box;
            public BodyInertia boxInertia;
            public System.Numerics.Vector3 position;

            public string InstanceName { get; set; } = RTId.CreateId("BodyPosition");

            public byte Mask { get; set; } = RtStructures.OPAQUE_GEOM_MASK;

            public CCOTextureBindingDescription Texture { get; set; } = new CCOTextureBindingDescription("cc0Textures/Ground025_1K");

            public PrimaryHitShader.Desc Shader { get; set; }

            public Desc()
            {
                Shader = new PrimaryHitShader.Desc
                {
                    ShaderType = PrimaryHitShaderType.Mesh,
                    HasNormalMap = true,
                    HasPhysicalDescriptorMap = true,
                    Reflective = false
                };
            }
        }

        private BodyHandle bodyHandle;

        private readonly TLASBuildInstanceData instanceData;
        private readonly CubeBLAS cubeBLAS;
        private readonly IBepuScene bepuScene;
        private readonly PrimaryHitShader.Factory primaryHitShaderFactory;
        private readonly RTInstances rtInstances;
        private readonly TextureManager textureManager;
        private readonly ActiveTextures activeTextures;
        private CC0TextureResult cubeTexture;
        private PrimaryHitShader primaryHitShader;
        private BlasInstanceData blasInstanceData;

        public BodyPositionSync
        (
            Desc description,
            CubeBLAS cubeBLAS,
            IBepuScene bepuScene,
            IScopedCoroutine scopedCoroutine,
            PrimaryHitShader.Factory primaryHitShaderFactory,
            RTInstances rtInstances,
            TextureManager textureManager,
            ActiveTextures activeTextures
        )
        {
            this.cubeBLAS = cubeBLAS;
            this.bepuScene = bepuScene;
            this.primaryHitShaderFactory = primaryHitShaderFactory;
            this.rtInstances = rtInstances;
            this.textureManager = textureManager;
            this.activeTextures = activeTextures;            
            this.instanceData = new TLASBuildInstanceData()
            {
                InstanceName = description.InstanceName,
                Mask = description.Mask,
                Transform = new InstanceMatrix(new Vector3(description.position.X, description.position.Y, description.position.Z), Quaternion.Identity),
                Flags = RAYTRACING_INSTANCE_FLAGS.RAYTRACING_INSTANCE_NONE,
            };

            var bodyDesc = BodyDescription.CreateDynamic(
                    description.position,
                    description.boxInertia, new CollidableDescription(bepuScene.Simulation.Shapes.Add(description.box), 0.1f), new BodyActivityDescription(0.01f));

            bodyHandle = bepuScene.Simulation.Bodies.Add(bodyDesc);

            bepuScene.AddToInterpolation(bodyHandle);

            scopedCoroutine.RunTask(async () =>
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

                if (cubeTexture.HasOpacity)
                {
                    this.instanceData.Flags = RAYTRACING_INSTANCE_FLAGS.RAYTRACING_INSTANCE_FORCE_NO_OPAQUE;
                }

                blasInstanceData = this.activeTextures.AddActiveTexture(this.cubeTexture);
                rtInstances.AddTlasBuild(instanceData);
                rtInstances.AddShaderTableBinder(Bind);
            });
        }

        public void Dispose()
        {
            bepuScene.Simulation.Bodies.Remove(this.bodyHandle);

            this.activeTextures.RemoveActiveTexture(this.cubeTexture);
            primaryHitShaderFactory.TryReturn(primaryHitShader);
            textureManager.TryReturn(cubeTexture);
            rtInstances.RemoveShaderTableBinder(Bind);
            rtInstances.RemoveTlasBuild(instanceData);
        }

        public Vector3 GetWorldPosition()
        {
            var bodyReference = bepuScene.Simulation.Bodies.GetBodyReference(bodyHandle);
            var bodPos = bodyReference.Pose.Position;
            return new Vector3(bodPos.X, bodPos.Y, bodPos.Z);
        }

        public Quaternion GetWorldOrientation()
        {
            var bodyReference = bepuScene.Simulation.Bodies.GetBodyReference(bodyHandle);
            var bodOrientation = bodyReference.Pose.Orientation;
            return new Quaternion(bodOrientation.X, bodOrientation.Y, bodOrientation.Z, bodOrientation.W);
        }

        public void SyncPhysics(IBepuScene bepuScene)
        {
            Vector3 position = new Vector3();
            Quaternion orientation = Quaternion.Identity;
            bepuScene.GetInterpolatedPosition(bodyHandle, ref position, ref orientation);
            this.instanceData.Transform = new InstanceMatrix(position, orientation);
        }

        public unsafe void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
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
