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
    class SimpleUpdateListener : UpdateListener, IDisposable
    {
        private readonly GenericEngineFactory genericEngineFactory;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext immediateContext;
        private readonly IPipelineState pipelineState;

        public SimpleUpdateListener(GenericEngineFactory genericEngineFactory)
        {
            this.genericEngineFactory = genericEngineFactory;
            this.swapChain = genericEngineFactory.SwapChain;
            this.immediateContext = genericEngineFactory.ImmediateContext;

            //using var shaderCreate = new ShaderCreateInfo();
            //shaderCreate.Lazy_VS();
            //using var vertexShader = this.genericEngineFactory.RenderDevice.CreateShader(shaderCreate);
            //shaderCreate.Lazy_PS();
            //using var pixelShader = this.genericEngineFactory.RenderDevice.CreateShader(shaderCreate);

            using var psoCreate = new GraphicsPipelineStateCreateInfo();
            //psoCreate.LazySetup(genericEngineFactory.SwapChain, genericEngineFactory.RenderDevice, pixelShader, vertexShader);
            //this.pipelineState = genericEngineFactory.RenderDevice.CreateGraphicsPipelineState(psoCreate);

            psoCreate.OneShot(genericEngineFactory.SwapChain, genericEngineFactory.RenderDevice);
        }

        public void Dispose()
        {
            //pipelineState.Dispose();
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

            var color = Color.LightBlue;
            color.r = (color.r + (clock.CurrentTimeMicro % 3000000f) / 3000000f) % 1.0f;
            color.g = (color.g + (clock.CurrentTimeMicro % 6000000f) / 6000000f) % 1.0f;
            color.b = (color.b + (clock.CurrentTimeMicro % 9000000f) / 9000000f) % 1.0f;

            // Clear the back buffer
            // Let the engine perform required state transitions
            immediateContext.ClearRenderTarget(pRTV, color, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            this.swapChain.Present();
        }
    }
}
