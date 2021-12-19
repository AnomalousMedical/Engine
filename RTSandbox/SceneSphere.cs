﻿using DiligentEngine;
using DiligentEngine.RT;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSandbox
{
    internal class SceneSphere : IDisposable, IShaderTableBinder
    {
        public class Desc
        {
            public string InstanceName { get; set; } = Guid.NewGuid().ToString();

            public uint BoxIndex { get; set; } = 0;

            public byte Mask { get; set; } = RtStructures.OPAQUE_GEOM_MASK;

            public InstanceMatrix Transform = InstanceMatrix.Identity;
        }

        private readonly TLASBuildInstanceData instanceData;
        private readonly RTInstances instances;

        public SceneSphere
        (
            Desc description,
            RTInstances instances, 
            ProceduralBLAS blas
        )
        {
            this.instances = instances;
            this.instanceData = new TLASBuildInstanceData()
            {
                InstanceName = description.InstanceName,
                CustomId = description.BoxIndex,
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
            sbt.BindHitGroupForInstance(tlas, this.instanceData.InstanceName, RtStructures.PRIMARY_RAY_INDEX, "SpherePrimaryHit", IntPtr.Zero);
        }
    }
}
