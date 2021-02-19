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
        private readonly NativeOSWindow window;
        private readonly ISharpGui sharpGui;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext immediateContext;
        private String displayText = "Click on something!";
        private StringBuilder lastUpdateTimeBuilder = new StringBuilder();

        SharpButton button1 = new SharpButton() { Text = "Button 1" };
        SharpButton button2 = new SharpButton() { Text = "Button 2" };
        SharpButton button3 = new SharpButton() { Text = "Button 3" };
        SharpInput input = new SharpInput() { };

        SharpSliderVertical sliderVert;
        SharpSliderHorizontal sliderHorz;
        private int sliderValue = 0;

        public SharpImGuiUpdateListener(GraphicsEngine graphicsEngine, NativeOSWindow window, ISharpGui sharpGui, IScaleHelper scaleHelper)
        {
            sliderVert = new SharpSliderVertical() { Rect = scaleHelper.Scaled(new IntRect(10, 10, 35, 500)), Max = 15 };
            sliderHorz = new SharpSliderHorizontal() { Rect = scaleHelper.Scaled(new IntRect(100, 10, 500, 35)), Max = 15 };

            PerformanceMonitor.Enabled = true;

            this.window = window;
            this.sharpGui = sharpGui;
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

        public unsafe void sendUpdate(Clock clock)
        {
            PerformanceMonitor.start("Sharp Gui");
            //Put things on the gui
            sharpGui.Begin(clock);

            var columnLayout = new ColumnLayout(button1, button2, button3, input) { Margin = new IntPad(10) };
            var desiredSize = columnLayout.GetDesiredSize(sharpGui);
            columnLayout.SetRect(new IntRect(window.WindowWidth - desiredSize.Width, window.WindowHeight - desiredSize.Height, desiredSize.Width, desiredSize.Height));

            //Buttons
            Guid stealFocus = Guid.Empty;
            if (sharpGui.Button(button1, navUp: input.Id, navDown: button2.Id, navLeft: sliderHorz.Id, navRight: sliderVert.Id))
            {
                displayText = "Activated button 1";
            }

            if (sharpGui.Button(button2, navUp: button1.Id, navDown: button3.Id, navLeft: sliderHorz.Id, navRight: sliderVert.Id))
            {
                displayText = "Activated button 2";
            }

            if (sharpGui.Button(button3, navUp: button2.Id, navDown: input.Id, navLeft: sliderHorz.Id, navRight: sliderVert.Id))
            {
                displayText = "Activated button 3";
            }

            if (sharpGui.Input(input, navUp: button3.Id, navDown: button1.Id))
            {
                displayText = $"Changed input to: {input.Text.ToString()}";
            }

            if (sharpGui.Slider(sliderVert, ref sliderValue, navLeft: button1.Id, navRight: sliderHorz.Id))
            {
                displayText = $"New slider value {sliderValue}";
            }

            if (sharpGui.Slider(sliderHorz, ref sliderValue, navUp: sliderVert.Id, navDown: sliderVert.Id))
            {
                displayText = $"New slider value {sliderValue}";
            }

            sharpGui.Text(450, 400, Color.Black, $"Program has been running for {TimeSpan.FromMilliseconds(clock.CurrentTimeMicro * Clock.MicroToMilliseconds)}");
            sharpGui.Text(750, 500, Color.Black, displayText);
            sharpGui.Text(750, 600, Color.Black, lastUpdateTimeBuilder.ToString());

            sharpGui.End();
            if (stealFocus != Guid.Empty)
            {
                sharpGui.StealFocus(stealFocus);
            }

            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();

            immediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            // Clear the back buffer
            var ClearColor = new Color(0.350f, 0.350f, 0.350f, 1.0f);

            // Clear the back buffer
            // Let the engine perform required state transitions
            immediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            //Draw the gui
            sharpGui.Render(immediateContext);

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
