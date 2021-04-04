using DiligentEngine;
using DiligentEngine.GltfPbr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class EnvMapManager : IEnvMapManager, IDisposable
    {
        private AutoPtr<ITextureView> environmentMapSRV;

        public EnvMapManager(
            GraphicsEngine graphicsEngine,
            PbrRenderer pbrRenderer,
            EnvironmentMapBuilder envMapBuilder)
        {
            environmentMapSRV = envMapBuilder.BuildEnvMapView(graphicsEngine.RenderDevice, graphicsEngine.ImmediateContext, "papermill/Fixed-", "png");

            pbrRenderer.PrecomputeCubemaps(graphicsEngine.RenderDevice, graphicsEngine.ImmediateContext, environmentMapSRV.Obj);
        }

        public void Dispose()
        {
            environmentMapSRV.Dispose();
        }
    }
}
