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
        private readonly SharpGui sharpGui;
        private readonly EventManager eventManager;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext m_pImmediateContext;

        public SharpImGuiUpdateListener(GraphicsEngine graphicsEngine, NativeOSWindow window, SharpGui sharpGui, EventManager eventManager)
        {
            this.graphicsEngine = graphicsEngine;
            this.window = window;
            this.sharpGui = sharpGui;
            this.eventManager = eventManager;
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

        Guid button1Guid = Guid.NewGuid();
        Guid button2Guid = Guid.NewGuid();
        Guid button3Guid = Guid.NewGuid();

        public unsafe void sendUpdate(Clock clock)
        {
            var mouse = eventManager.Mouse;
            sharpGui.SetMouseState(mouse.AbsolutePosition.x, mouse.AbsolutePosition.y, mouse.buttonDown(MouseButtonCode.MB_BUTTON0));

            //Put things on the gui
            sharpGui.Begin();
            if(sharpGui.DrawButton(button1Guid, 50, 50, 100, 100))
            {
                Console.WriteLine("Clicked button 1");
            }
            if(sharpGui.DrawButton(button2Guid, 250, 250, 100, 100))
            {
                Console.WriteLine("Clicked button 2");
            }
            if(sharpGui.DrawButton(button3Guid, 550, 550, 100, 100))
            {
                Console.WriteLine("Clicked button 3");
            }
            sharpGui.End();

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
            sharpGui.Render(m_pImmediateContext);

            this.swapChain.Present(1);
        }
    }
}
