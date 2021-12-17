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
        }

        public void Dispose()
        {
            objectResolver.Dispose();
        }
    }
}
