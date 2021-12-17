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

            objectResolver.Resolve<ScenePlane, ScenePlane.Desc>(o =>
            {
                
            });

            objectResolver.Resolve<ScenePlane, ScenePlane.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(3, 3, 0);
            });

            objectResolver.Resolve<ScenePlane, ScenePlane.Desc>(o =>
            {
                o.Transform = new InstanceMatrix(-3, 0, 0);
            });
        }

        public void Dispose()
        {
            objectResolver.Dispose();
        }
    }
}
