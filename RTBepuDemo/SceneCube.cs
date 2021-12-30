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
    internal class SceneCube : IDisposable
    {
        public class Desc
        {
            public string InstanceName { get; set; } = RTId.CreateId("SceneCube");

            public uint TextureIndex { get; set; } = 0;

            public byte Mask { get; set; } = RtStructures.OPAQUE_GEOM_MASK;

            public InstanceMatrix Transform = InstanceMatrix.Identity;
        }

        private readonly TLASBuildInstanceData instanceData;
        private readonly CubeBLAS cubeBLAS;
        private readonly RTInstances rtInstances;

        public SceneCube
        (
            Desc description,
            CubeBLAS cubeBLAS,
            RTInstances rtInstances,
            IScopedCoroutine coroutine
        )
        {
            this.cubeBLAS = cubeBLAS;
            this.rtInstances = rtInstances;
            this.instanceData = new TLASBuildInstanceData()
            {
                InstanceName = description.InstanceName,
                CustomId = description.TextureIndex, // texture index
                Mask = description.Mask,
                Transform = description.Transform
            };

            coroutine.RunTask(async () =>
            {
                await cubeBLAS.WaitForLoad();

                instanceData.pBLAS = cubeBLAS.Instance.BLAS.Obj;

                rtInstances.AddTlasBuild(instanceData);
                rtInstances.AddShaderTableBinder(Bind);
            });
        }

        public void Dispose()
        {
            rtInstances.RemoveShaderTableBinder(Bind);
            rtInstances.RemoveTlasBuild(instanceData);
        }

        public void SetTransform(InstanceMatrix matrix)
        {
            this.instanceData.Transform = matrix;
        }

        private void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            cubeBLAS.PrimaryHitShader.BindSbt(instanceData.InstanceName, sbt, tlas, IntPtr.Zero, 0);
        }
    }
}
