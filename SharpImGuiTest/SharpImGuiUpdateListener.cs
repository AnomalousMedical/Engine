using Anomalous.OSPlatform;
using DiligentEngine;
using Engine;
using Engine.Platform;
using SharpGui;
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
        private readonly ISharpGui sharpGui;
        private readonly EventManager eventManager;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext m_pImmediateContext;

        public SharpImGuiUpdateListener(GraphicsEngine graphicsEngine, NativeOSWindow window, ISharpGui sharpGui, EventManager eventManager)
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

        SharpButton button1 = new SharpButton(50, 50, 100, 100, text: "Button 1");
        SharpButton button2 = new SharpButton(600, 250, 100, 100, text: "Button 2");
        SharpButton button3 = new SharpButton(550, 500, 100, 100, text: "Button 3");

        Guid sliderId = Guid.NewGuid();
        SharpSlider slider = new SharpSlider(350, 250, 32, 500, 15);
        private int sliderValue = 0;

        public unsafe void sendUpdate(Clock clock)
        {
            //Put things on the gui
            sharpGui.Begin();
            
            //Buttons
            if(sharpGui.Button(button1))
            {
                Console.WriteLine("Clicked button 1");
            }
            if(sharpGui.Button(button2))
            {
                Console.WriteLine("Clicked button 2");
            }
            if(sharpGui.Button(button3))
            {
                Console.WriteLine("Clicked button 3");
            }

            if(sharpGui.Slider(slider, ref sliderValue))
            {
                Console.WriteLine($"New slider value {sliderValue}");
            }

            sharpGui.Text(750, 500, Color.Black, "Hello World!");

            sharpGui.End();

            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();

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
