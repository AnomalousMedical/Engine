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
        private String displayText = "Click on something!";
        private StringBuilder lastUpdateTimeBuilder = new StringBuilder();

        public SharpImGuiUpdateListener(GraphicsEngine graphicsEngine, NativeOSWindow window, ISharpGui sharpGui, EventManager eventManager)
        {
            PerformanceMonitor.Enabled = true;

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

        SharpButton button1 = new SharpButton() { Text = "Button 1" };
        SharpButton button2 = new SharpButton() { Text = "Button 2" };
        SharpButton button3 = new SharpButton() { Text = "Button 3" };

        SharpSlider slider = new SharpSlider(350, 250, 32, 500, 15);
        private int sliderValue = 0;

        public unsafe void sendUpdate(Clock clock)
        {
            PerformanceMonitor.start("Sharp Gui");
            //Put things on the gui
            sharpGui.Begin();

            var columnLayout = new ColumnLayout(button1, button2, button3);
            var desiredSize = columnLayout.GetDesiredSize(sharpGui);
            columnLayout.SetRect(new IntRect(window.WindowWidth - desiredSize.Width, window.WindowHeight - desiredSize.Height, desiredSize.Width, desiredSize.Height));

            //Buttons
            if (sharpGui.Button(button1))
            {
                displayText = "Clicked button 1";
            }
            if (sharpGui.Button(button2))
            {
                displayText = "Clicked button 2";
            }
            if (sharpGui.Button(button3))
            {
                displayText = "Clicked button 3";
            }

            //if(sharpGui.Slider(slider, ref sliderValue))
            //{
            //    Console.WriteLine($"New slider value {sliderValue}");
            //}

            sharpGui.Text(450, 400, Color.Black, $"Program has been running for {TimeSpan.FromMilliseconds(clock.CurrentTimeMicro * Clock.MicroToMilliseconds)}");
            sharpGui.Text(750, 500, Color.Black, displayText);
            sharpGui.Text(750, 600, Color.Black, lastUpdateTimeBuilder.ToString());

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

            PerformanceMonitor.stop("Sharp Gui");

            lastUpdateTimeBuilder.Clear();
            foreach (var value in PerformanceMonitor.Timelapses)
            {
                lastUpdateTimeBuilder.AppendFormat("{0}: {1} {2} {3} {4}", value.Name, value.Duration, value.Min, value.Max, value.Average);
                lastUpdateTimeBuilder.AppendLine();
            }

            foreach (var value in PerformanceMonitor.PerformanceValues)
            {
                lastUpdateTimeBuilder.AppendFormat("{0}: {1}", value.Name, value.Value);
                lastUpdateTimeBuilder.AppendLine();
            }

            this.swapChain.Present(1);
        }
    }
}
