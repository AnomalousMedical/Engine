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

        public PlaneScene(IObjectResolverFactory objectResolverFactory)
        {
            objectResolver = objectResolverFactory.Create();

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Matrix3x3.Scale(1, 1, 0.01f), 0, 0, 0);
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Matrix3x3.Scale(1, 1, 0.01f), 3, 3, 0);
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Matrix3x3.Scale(1, 1, 0.01f), 6, 3, 0);
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Matrix3x3.Scale(1, 1, 0.01f), 9, 3, 0);
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Matrix3x3.Scale(1, 1, 0.01f), 12, 3, 0);
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Matrix3x3.Scale(1, 1, 0.01f), 3, 6, 0);
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Matrix3x3.Scale(1, 1, 0.01f), -3, 0, 0);
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Matrix3x3.Scale(1, 1, 0.01f), -3, -3, 0);
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Matrix3x3.Scale(1, 1, 0.01f), -3, -6, 0);
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Matrix3x3.Scale(1, 1, 0.01f), -3, -9, 0);
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
