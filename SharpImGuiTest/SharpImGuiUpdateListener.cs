using Anomalous.OSPlatform;
using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{
    class SharpImGuiUpdateListener : UpdateListener, IDisposable
    {
        private readonly GraphicsEngine graphicsEngine;
        private readonly NativeOSWindow window;
        private readonly SharpGuiBuffer sharpGuiBuffer;
        private readonly SharpGuiRenderer guiRenderer;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext m_pImmediateContext;

        public SharpImGuiUpdateListener(GraphicsEngine graphicsEngine, NativeOSWindow window, SharpGuiBuffer sharpGuiBuffer, SharpGuiRenderer guiRenderer)
        {
            this.graphicsEngine = graphicsEngine;
            this.window = window;
            this.sharpGuiBuffer = sharpGuiBuffer;
            this.guiRenderer = guiRenderer;
            this.swapChain = graphicsEngine.SwapChain;
            this.m_pImmediateContext = graphicsEngine.ImmediateContext;

            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pSwapChain = graphicsEngine.SwapChain;

            
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

        public unsafe void sendUpdate(Clock clock)
        {
            //Put things on the gui
            sharpGuiBuffer.Begin();
            sharpGuiBuffer.DrawQuad(50, 50, 100, 100, Color.Blue);
            sharpGuiBuffer.DrawQuad(250, 250, 100, 100, Color.Green);
            sharpGuiBuffer.DrawQuad(550, 550, 100, 100, Color.Red);

            //sharpGuiBuffer.DrawQuad(0, 0, window.WindowWidth / 3, window.WindowHeight / 3, Color.Blue);
            //sharpGuiBuffer.DrawQuad(window.WindowWidth / 3, window.WindowHeight / 3, window.WindowWidth / 3, window.WindowHeight / 3, Color.Green);
            //sharpGuiBuffer.DrawQuad(window.WindowWidth - window.WindowWidth / 3, window.WindowHeight - window.WindowHeight / 3, window.WindowWidth / 3, window.WindowHeight / 3, Color.Red);

            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            var preTransform = swapChain.GetDesc_PreTransform;
            m_pImmediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            // Clear the back buffer
            var ClearColor = new Color(0.350f, 0.350f, 0.350f, 1.0f);

            // Clear the back buffer
            // Let the engine perform required state transitions
            m_pImmediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            m_pImmediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            //Draw the gui
            guiRenderer.Render(sharpGuiBuffer, m_pImmediateContext);

            this.swapChain.Present(1);
        }
    }
}
