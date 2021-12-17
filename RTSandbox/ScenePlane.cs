using DiligentEngine;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSandbox
{
    internal class ScenePlane : IDisposable, IShaderTableBinder
    {
        public class Desc
        {
            public string InstanceName { get; set; } = Guid.NewGuid().ToString();

            public uint TextureIndex { get; set; } = 0;

            public byte Mask { get; set; } = RtStructures.OPAQUE_GEOM_MASK;

            public InstanceMatrix Transform = InstanceMatrix.Identity;
        }

        private readonly TLASBuildInstanceData instanceData;
        private readonly RTInstances instances;

        public ScenePlane
        (
            Desc description,
            RTInstances instances, 
            PlaneBLAS blas
        )
        {
            this.instances = instances;
            this.instanceData = new TLASBuildInstanceData()
            {
                InstanceName = description.InstanceName,
                CustomId = description.TextureIndex, // texture index
                pBLAS = blas.BLAS,
                Mask = description.Mask,
                Transform = description.Transform
            };

            instances.AddTlasBuild(instanceData);
            instances.AddShaderTableBinder(this);
        }

        public void Dispose()
        {
            instances.RemoveShaderTableBinder(this);
            instances.RemoveTlasBuild(instanceData);
        }

        public void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            sbt.BindHitGroupForInstance(tlas, instanceData.InstanceName, RtStructures.PRIMARY_RAY_INDEX, "CubePrimaryHit", IntPtr.Zero);
        }
    }
}
