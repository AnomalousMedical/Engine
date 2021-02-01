using DilligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DilligentEngineTest
{
    class SimpleUpdateListener : UpdateListener
    {
        private readonly GenericEngineFactory genericEngineFactory;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext immediateContext;

        public SimpleUpdateListener(GenericEngineFactory genericEngineFactory, ISwapChain swapChain, IDeviceContext immediateContext)
        {
            this.genericEngineFactory = genericEngineFactory;
            this.swapChain = swapChain;
            this.immediateContext = immediateContext;
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

            // Clear the back buffer
            // Let the engine perform required state transitions
            immediateContext.ClearRenderTarget(pRTV, Color.LightBlue, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            this.swapChain.Present();
        }
    }
}
