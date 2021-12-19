using DiligentEngine;
using Engine;
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

        private static readonly Matrix4x4 PlaneScale = Matrix4x4.Scale(1, 1, 1);
        private static readonly Matrix4x4 Rot = new Quaternion(Vector3.UnitX, 0).toRotationMatrix4x4();

        public PlaneScene(IObjectResolverFactory objectResolverFactory)
        {
            objectResolver = objectResolverFactory.Create();

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Rot * PlaneScale * Matrix4x4.Translation(0, 0, 0));
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Rot * PlaneScale * Matrix4x4.Translation(3, 3, 0));
                o.TextureIndex = 1;
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Rot * PlaneScale * Matrix4x4.Translation(6, 3, 0));
                o.TextureIndex = 2;
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Rot * PlaneScale * Matrix4x4.Translation(9, 3, 0));
                o.TextureIndex = 3;
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Rot * PlaneScale * Matrix4x4.Translation(12, 3, 0));
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Rot * PlaneScale * Matrix4x4.Translation(3, 6, 0));
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Rot * PlaneScale * Matrix4x4.Translation(-3, 0, 0));
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Rot * PlaneScale * Matrix4x4.Translation(-3, -3, 0));
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Rot * PlaneScale * Matrix4x4.Translation(-3, -6, 0));
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Rot * PlaneScale * Matrix4x4.Translation(-3, -9, 0));
            });

            objectResolver.Resolve<SceneGround, SceneGround.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Matrix3x3.Scale(100.0f, 0.1f, 100.0f), 0.0f, 6.0f, 0.0f);
            });
        }

        public void Dispose()
        {
            objectResolver.Dispose();
        }
    }
}
