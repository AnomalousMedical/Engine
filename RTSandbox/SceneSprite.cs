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
            public string InstanceName { get; set; } = Guid.NewGuid().ToString("N");

            public InstanceMatrix Transform = InstanceMatrix.Identity;
        }

        private TLASBuildInstanceData instanceData;
        private readonly RayTracingRenderer renderer;
        private readonly SpriteInstanceFactory spriteInstanceFactory;
        private SpriteInstance spriteInstance;

        public SceneSprite
        (
            Desc description,
            RayTracingRenderer renderer,
            SpriteInstanceFactory spriteInstanceFactory,
            IScopedCoroutine coroutine
        )
        {
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

                renderer.AddTlasBuild(instanceData);
                renderer.AddShaderTableBinder(Bind);
            });
            this.renderer = renderer;
            this.spriteInstanceFactory = spriteInstanceFactory;
        }

        public void Dispose()
        {
            this.spriteInstanceFactory.TryReturn(spriteInstance);
            renderer.RemoveShaderTableBinder(Bind);
            renderer.RemoveTlasBuild(instanceData);
        }

        public void SetTransform(in Vector3 trans, in Quaternion rot)
        {
            this.instanceData.Transform = new InstanceMatrix(trans, rot);
        }

        public void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            spriteInstance.Bind(this.instanceData.InstanceName, sbt, tlas);
        }
    }
}
