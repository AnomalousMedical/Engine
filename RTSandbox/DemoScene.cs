using DiligentEngine;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSandbox
{
    internal class DemoScene : IDisposable, IRTSandboxScene
    {
        private readonly IObjectResolver objectResolver;

        public DemoScene(IObjectResolverFactory objectResolverFactory)
        {
            objectResolver = objectResolverFactory.Create();

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform.SetTranslation(1, 1, 1);
                o.TextureIndex = 0;
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform.SetTranslation(2, 0, -1);
                o.TextureIndex = 1;
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform.SetTranslation(-1, 1, 2);
                o.TextureIndex = 2;
            });

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {
                o.Transform.SetTranslation(-2, 0, -1);
                o.TextureIndex = 3;
            });

            objectResolver.Resolve<SceneGlassCube, SceneGlassCube.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(Matrix3x3.Scale(1.5f, 1.5f, 1.5f), 3.0f, 4.0f, -5.0f);
            });

            objectResolver.Resolve<SceneSphere, SceneSphere.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(-3.0f, 3.0f, -5f);
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
