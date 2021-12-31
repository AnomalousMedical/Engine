using DiligentEngine;
using DiligentEngine.RT;
using DiligentEngine.RT.Sprites;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSandbox
{
    internal class SceneSprite : IDisposable
    {
        public class Desc
        {
            public string InstanceName { get; set; } = RTId.CreateId("SceneSprite");

            public InstanceMatrix Transform = InstanceMatrix.Identity;
        }

        private readonly TLASBuildInstanceData instanceData;
        private readonly SpriteInstanceFactory spriteInstanceFactory;
        private readonly RTInstances rtInstances;
        private SpriteInstance spriteInstance;

        public SceneSprite
        (
            Desc description,
            SpriteInstanceFactory spriteInstanceFactory,
            IScopedCoroutine coroutine,
            RTInstances rtInstances
        )
        {
            this.spriteInstanceFactory = spriteInstanceFactory;
            this.rtInstances = rtInstances;

            this.instanceData = new TLASBuildInstanceData()
            {
                InstanceName = description.InstanceName,
                Mask = RtStructures.OPAQUE_GEOM_MASK,
                Transform = description.Transform,
            };

            coroutine.RunTask(async () =>
            {
                this.spriteInstance = await spriteInstanceFactory.Checkout(new SpriteMaterialDescription
                (
                    colorMap: "original/amg1_full4.png",
                    materials: new HashSet<SpriteMaterialTextureItem>
                    {
                        new SpriteMaterialTextureItem(0xffa854ff, "cc0Textures/Fabric012_1K", "jpg"),
                        new SpriteMaterialTextureItem(0xff909090, "cc0Textures/Fabric020_1K", "jpg"),
                        new SpriteMaterialTextureItem(0xff8c4800, "cc0Textures/Leather026_1K", "jpg"),
                        new SpriteMaterialTextureItem(0xffffe254, "cc0Textures/Metal038_1K", "jpg"),
                    }
                ));
                this.instanceData.pBLAS = spriteInstance.Instance.BLAS.Obj;

                rtInstances.AddTlasBuild(instanceData);
                rtInstances.AddShaderTableBinder(Bind);
            });
        }

        public void Dispose()
        {
            this.spriteInstanceFactory.TryReturn(spriteInstance);
            rtInstances.RemoveShaderTableBinder(Bind);
            rtInstances.RemoveTlasBuild(instanceData);
        }

        public void SetTransform(in Vector3 trans, in Quaternion rot)
        {
            this.instanceData.Transform = new InstanceMatrix(trans, rot);
        }

        private void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            var spriteFrame = new SpriteFrame()
            {
                Left = 0,
                Top = 0,
                Right = 1,
                Bottom = 1,
            };
            spriteInstance.Bind(this.instanceData.InstanceName, sbt, tlas, spriteFrame);
        }
    }
}
