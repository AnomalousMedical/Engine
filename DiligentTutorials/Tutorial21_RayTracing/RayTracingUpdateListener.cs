using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngineRayTracing
{
    class RayTracingUpdateListener : UpdateListener, IDisposable
    {
        private readonly GraphicsEngine graphicsEngine;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext immediateContext;

        public RayTracingUpdateListener(GraphicsEngine graphicsEngine)
        {
            this.graphicsEngine = graphicsEngine;
            this.swapChain = graphicsEngine.SwapChain;
            this.immediateContext = graphicsEngine.ImmediateContext;
        }

        public void Dispose()
        {
            
        }

        public void exceededMaxDelta()
        {

        }

        public void loopStarting()
        {

        }

        public void sendUpdate(Clock clock)
        {
            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            immediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            var ClearColor = new Color(0.350f, 0.350f, 0.350f, 1.0f);

            // Clear the back buffer
            // Let the engine perform required state transitions
            immediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            this.swapChain.Present(1);
        }
    }
}
