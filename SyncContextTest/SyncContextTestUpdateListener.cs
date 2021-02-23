using Anomalous.OSPlatform;
using DiligentEngine;
using Engine;
using Engine.Platform;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncContextTest
{
    class SyncContextTestUpdateListener : UpdateListener, IDisposable
    {
        private readonly NativeOSWindow window;
        private readonly ISharpGui sharpGui;
        private readonly IScaleHelper scaleHelper;
        private readonly ICoroutineRunner coroutine;
        private readonly SingleThreadSynchronizationContext singleThreadSynchronizationContext;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext immediateContext;
        private readonly IObjectResolver objectResolver;

        SharpButton thingButton = new SharpButton();

        public SyncContextTestUpdateListener(
            GraphicsEngine graphicsEngine,
            NativeOSWindow window,
            ISharpGui sharpGui, 
            IScaleHelper scaleHelper,
            IObjectResolverFactory objectResolverFactory,
            ICoroutineRunner coroutine,
            SingleThreadSynchronizationContext singleThreadSynchronizationContext)
        {
            this.swapChain = graphicsEngine.SwapChain;
            this.immediateContext = graphicsEngine.ImmediateContext;
            this.window = window;
            this.sharpGui = sharpGui;
            this.scaleHelper = scaleHelper;
            this.coroutine = coroutine;
            this.singleThreadSynchronizationContext = singleThreadSynchronizationContext;
            this.objectResolver = objectResolverFactory.Create();

            IEnumerator<YieldAction> woot()
            {
                //yield return coroutine.WaitSeconds(1);
                //Console.WriteLine($"Hi {Thread.CurrentThread.ManagedThreadId}");
                //yield return coroutine.WaitSeconds(2);
                //Console.WriteLine($"Hi again {Thread.CurrentThread.ManagedThreadId}");
                //yield return coroutine.WaitSeconds(2);
                //Console.WriteLine($"Hi again {Thread.CurrentThread.ManagedThreadId}");
                //yield return coroutine.WaitSeconds(2);
                //Console.WriteLine($"Hi again {Thread.CurrentThread.ManagedThreadId}");
                yield return coroutine.Await(async () =>
                {
                    Console.WriteLine($"Task hi {Thread.CurrentThread.ManagedThreadId}");
                    await Task.Delay(1000);
                    Console.WriteLine($"Task hi after task wait {Thread.CurrentThread.ManagedThreadId}");
                    await Task.Delay(4000);
                    Console.WriteLine($"Task hi after task wait again {Thread.CurrentThread.ManagedThreadId}");
                });
                Console.WriteLine($"Hi after await task {Thread.CurrentThread.ManagedThreadId}");
            }
            coroutine.Start(woot());
        }

        public void Dispose()
        {
            objectResolver.Dispose();
        }

        public void exceededMaxDelta()
        {

        }

        public void loopStarting()
        {

        }

        private void UpdateGui(Clock clock)
        {
            sharpGui.Begin(clock);

            bool createThing = false;
            thingButton.Text = createThing ? "Create Thing" : "Destory Thing";

            var layout =
                new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                new MaxWidthLayout(scaleHelper.Scaled(300),
                new ColumnLayout(thingButton) { Margin = new IntPad(10) }
                ));
            var desiredSize = layout.GetDesiredSize(sharpGui);
            layout.SetRect(new IntRect(window.WindowWidth - desiredSize.Width, window.WindowHeight - desiredSize.Height, desiredSize.Width, desiredSize.Height));

            //Buttons
            if (sharpGui.Button(thingButton))
            {
                if (createThing)
                {
                    
                }
                else
                {
                   
                }
            }

            sharpGui.End();
        }

        public unsafe void sendUpdate(Clock clock)
        {
            singleThreadSynchronizationContext.PumpCurrentQueue();

            //Update
            UpdateGui(clock);

            //Flush any removed objects
            objectResolver.Flush();

            //Render
            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            var ClearColor = new Color(0.350f, 0.350f, 0.350f, 1.0f);

            //Draw Scene
            immediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            // Clear the back buffer
            immediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            RenderGui();

            this.swapChain.Present(1);
        }

        private void RenderGui()
        {
            //Draw the gui
            var pDSV = swapChain.GetDepthBufferDSV();
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            sharpGui.Render(immediateContext);
        }
    }
}
