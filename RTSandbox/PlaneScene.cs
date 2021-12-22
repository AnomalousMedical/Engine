using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSandbox
{
    internal class PlaneScene : IDisposable, IRTSandboxScene
    {
        private readonly IObjectResolver objectResolver;

        private SceneCube rotateCube;
        private Quaternion currentRot = Quaternion.Identity;

        public PlaneScene(IObjectResolverFactory objectResolverFactory)
        {
            objectResolver = objectResolverFactory.Create();

            rotateCube = objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(new Vector3(0, 0, 0), Quaternion.Identity);
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(new Vector3(-3, -3, 0), Quaternion.Identity);
                o.TextureIndex = 1;
                o.Flags = RAYTRACING_INSTANCE_FLAGS.RAYTRACING_INSTANCE_FORCE_NO_OPAQUE;
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(new Vector3(-6, -3, 0), Quaternion.Identity);
                o.TextureIndex = 2;
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(new Vector3(-9, -3, 0), Quaternion.Identity);
                o.TextureIndex = 3;
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(new Vector3(-12, -3, 0), Quaternion.Identity);
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(new Vector3(-3, -6, 0), Quaternion.Identity);
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(new Vector3(3, 0, 0), Quaternion.Identity);
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(new Vector3(3, 3, 0), Quaternion.Identity);
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(new Vector3(3, 6, 0), Quaternion.Identity);
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(new Vector3(3, 9, 0), Quaternion.Identity);
            });

            for (var x = -20; x < 20; ++x)
            {
                for (var z = -20; z < 20; ++z)
                {
                    objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
                    {
                        o.Transform = new InstanceMatrix(new Vector3(x, -6.0f, z), Quaternion.Identity);
                        o.TextureIndex = 4;
                    });
                }
            }
        }

        public void Update(Clock clock)
        {
            currentRot *= new Quaternion(new Vector3(0, 1, 0), 0.35f * clock.DeltaSeconds);
            rotateCube.SetTransform(Vector3.Zero, currentRot);
        }

        public void Dispose()
        {
            objectResolver.Dispose();
        }
    }
}
