using DiligentEngine;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTBepuDemo
{
    class DemoScene : IDisposable
    {
        private readonly IObjectResolver objectResolver;

        public DemoScene(IObjectResolverFactory objectResolverFactory)
        {
            this.objectResolver = objectResolverFactory.Create();

            objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
            {

            });

            for (var x = -20; x < 20; ++x)
            {
                for (var z = -20; z < 20; ++z)
                {
                    objectResolver.Resolve<SceneCube, SceneCube.Desc>(o =>
                    {
                        o.Transform = new InstanceMatrix(new Vector3(x, -1.5f, z), Quaternion.Identity);
                        o.TextureIndex = 4;
                    });
                }
            }
        }

        public void Dispose()
        {
            objectResolver.Dispose();
        }
    }
}
