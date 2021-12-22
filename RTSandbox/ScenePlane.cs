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
    internal class ScenePlane : IDisposable, IShaderTableBinder
    {
        public class Desc
        {
            public string InstanceName { get; set; } = Guid.NewGuid().ToString("N");

            public uint TextureIndex { get; set; } = 0;

            public byte Mask { get; set; } = RtStructures.OPAQUE_GEOM_MASK;

            public RAYTRACING_INSTANCE_FLAGS Flags { get; set; } = RAYTRACING_INSTANCE_FLAGS.RAYTRACING_INSTANCE_NONE;


            public InstanceMatrix Transform = InstanceMatrix.Identity;
        }

        private readonly TLASBuildInstanceData instanceData;
        private readonly PlaneBLAS blas;
        private readonly RayTracingRenderer renderer;

        public ScenePlane
        (
            Desc description,
            PlaneBLAS planeBLAS,
            RayTracingRenderer renderer
        )
        {
            this.blas = planeBLAS;
            this.renderer = renderer;
            this.instanceData = new TLASBuildInstanceData()
            {
                InstanceName = description.InstanceName,
                CustomId = description.TextureIndex, // texture index
                pBLAS = planeBLAS.Instance.BLAS.Obj,
                Mask = description.Mask,
                Transform = description.Transform,
                Flags = description.Flags,
            };

            renderer.AddTlasBuild(instanceData);
            renderer.AddShaderTableBinder(this);
        }

        public void Dispose()
        {
            renderer.RemoveShaderTableBinder(this);
            renderer.RemoveTlasBuild(instanceData);
        }

        public void SetTransform(in Vector3 trans, in Quaternion rot)
        {
            this.instanceData.Transform = new InstanceMatrix(trans, rot);
        }

        public void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            sbt.BindHitGroupForInstance(tlas, instanceData.InstanceName, RtStructures.PRIMARY_RAY_INDEX, blas.ShaderGroupName, IntPtr.Zero);
        }
    }
}