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
        }

        public void Dispose()
        {
            objectResolver.Dispose();
        }
    }
}
