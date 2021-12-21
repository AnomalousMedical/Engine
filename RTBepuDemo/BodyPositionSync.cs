using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPlugin;
using DiligentEngine;
using DiligentEngine.RT;
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
    class BodyPositionSync : IDisposable, IShaderTableBinder
    {
        public class Desc
        {
            public Box box;
            public BodyInertia boxInertia;
            public System.Numerics.Vector3 position;

            public string InstanceName { get; set; } = Guid.NewGuid().ToString();

            public uint TextureIndex { get; set; } = 0;

            public byte Mask { get; set; } = RtStructures.OPAQUE_GEOM_MASK;
        }

        private BodyHandle bodyHandle;

        private readonly TLASBuildInstanceData instanceData;
        private readonly CubeBLAS cubeBLAS;
        private readonly RayTracingRenderer renderer;
        private readonly IBepuScene bepuScene;

        public BodyPositionSync( 
            Desc description,
            CubeBLAS cubeBLAS,
            RayTracingRenderer renderer,
            IBepuScene bepuScene)
        {
            this.cubeBLAS = cubeBLAS;
            this.renderer = renderer;
            this.bepuScene = bepuScene;
            this.instanceData = new TLASBuildInstanceData()
            {
                InstanceName = description.InstanceName,
                CustomId = description.TextureIndex,
                pBLAS = cubeBLAS.Instance.BLAS.Obj,
                Mask = description.Mask,
                Transform = new InstanceMatrix(new Vector3(description.position.X, description.position.Y, description.position.Z), Quaternion.Identity)
            };

            renderer.AddTlasBuild(instanceData);
            renderer.AddShaderTableBinder(this);

            var bodyDesc = BodyDescription.CreateDynamic(
                    description.position,
                    description.boxInertia, new CollidableDescription(bepuScene.Simulation.Shapes.Add(description.box), 0.1f), new BodyActivityDescription(0.01f));

            bodyHandle = bepuScene.Simulation.Bodies.Add(bodyDesc);

            bepuScene.AddToInterpolation(bodyHandle);
        }

        public void Dispose()
        {
            bepuScene.Simulation.Bodies.Remove(this.bodyHandle);

            renderer.RemoveShaderTableBinder(this);
            renderer.RemoveTlasBuild(instanceData);
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

        public void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            sbt.BindHitGroupForInstance(tlas, instanceData.InstanceName, RtStructures.PRIMARY_RAY_INDEX, cubeBLAS.ShaderGroupName, IntPtr.Zero);
        }
    }
}
