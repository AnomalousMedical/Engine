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
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext immediateContext;
        private readonly IObjectResolver objectResolver;

        private SharpButton thingButton = new SharpButton();
        private AnnoyingThing annoyingThing;

        private SharpButton breakInCoroutineButton = new SharpButton() { Text = "Break In Coroutine" };
        private SharpButton breakFgButton = new SharpButton() { Text = "Break Coroutine Async" };
        private SharpButton breakBgButton = new SharpButton() { Text = "Break Coroutine Background Async" };

        public SyncContextTestUpdateListener(
            GraphicsEngine graphicsEngine,
            NativeOSWindow window,
            ISharpGui sharpGui, 
            IScaleHelper scaleHelper,
            IObjectResolverFactory objectResolverFactory,
            ICoroutineRunner coroutine)
        {
            this.swapChain = graphicsEngine.SwapChain;
            this.immediateContext = graphicsEngine.ImmediateContext;
            this.window = window;
            this.sharpGui = sharpGui;
            this.scaleHelper = scaleHelper;
            this.coroutine = coroutine;
            this.objectResolver = objectResolverFactory.Create();

            //This is a coroutine function
            //These can be any IEnumerator function.
            IEnumerator<YieldAction> woot()
            {
                //Do a normal time based wait
                Console.WriteLine($"Inside coroutine {Thread.CurrentThread.ManagedThreadId}");
                yield return coroutine.WaitSeconds(1);

                //Run a background task.
                Console.WriteLine($"Inside coroutine - start background thread await {Thread.CurrentThread.ManagedThreadId}");
                //Start the bg task
                var bgTask = Task.Run(async () =>
                {
                    //This runs on a background thread
                    //Must be careful in these since the owning object could be destroyed after any await
                    //This does not have the automatic management the coroutines do since nothing can see inside this
                    //async task
                    Console.WriteLine($"Inside background task await {Thread.CurrentThread.ManagedThreadId}");
                    await Task.Delay(3000);
                    Console.WriteLine($"Inside background task after await {Thread.CurrentThread.ManagedThreadId}");
                });
                //Pretend other processing happens here. The task will be going at this time.
                yield return coroutine.Await(bgTask); //When you need the results from the task, await it
                Console.WriteLine($"Inside coroutine - end background thread await {Thread.CurrentThread.ManagedThreadId}");

                //Run and await a task that will happen totally on the main thread
                Console.WriteLine($"Inside coroutine - start same thread await {Thread.CurrentThread.ManagedThreadId}");
                yield return coroutine.Await(async () =>
                {
                    Console.WriteLine($"Task start {Thread.CurrentThread.ManagedThreadId}");
                    await Task.Delay(1000);
                    Console.WriteLine($"Task wait {Thread.CurrentThread.ManagedThreadId}");
                    await Task.Delay(4000);
                    Console.WriteLine($"Task wait again {Thread.CurrentThread.ManagedThreadId}");
                    //await Task.Delay(2000);
                    //throw new Exception("Broken");
                });
                Console.WriteLine($"Inside coroutine - await same thread complete {Thread.CurrentThread.ManagedThreadId}");
                objectResolver.Resolve<SyncThing>();
            }
            coroutine.Run(woot()); //This is where the function above actually starts running.
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

            bool createThing = annoyingThing == null;
            thingButton.Text = createThing ? "Create Annoying Thing" : "Destory Annoying Thing";

            var layout =
                new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                new MaxWidthLayout(scaleHelper.Scaled(500),
                new ColumnLayout(thingButton, breakInCoroutineButton, breakFgButton, breakBgButton) { Margin = new IntPad(10) }
                ));
            var desiredSize = layout.GetDesiredSize(sharpGui);
            layout.SetRect(new IntRect(window.WindowWidth - desiredSize.Width, window.WindowHeight - desiredSize.Height, desiredSize.Width, desiredSize.Height));

            //Buttons
            if (sharpGui.Button(thingButton))
            {
                if (createThing)
                {
                    annoyingThing = objectResolver.Resolve<AnnoyingThing>();
                }
                else
                {
                    annoyingThing.RequestDestruction();
                    annoyingThing = null;
                }
            }

            //All of these should throw exceptions back to the main thread and cause
            //the message box to appear (when appropriate).
            if (sharpGui.Button(breakInCoroutineButton))
            {
                IEnumerator<YieldAction> co()
                {
                    Console.WriteLine("Going to break in 1 second!");
                    yield return coroutine.WaitSeconds(1);
                    throw new Exception("Broken inside a queued coroutine. Thread: {Thread.CurrentThread.ManagedThreadId}");
                }
                coroutine.Queue(co());
            }

            if (sharpGui.Button(breakFgButton))
            {
                IEnumerator<YieldAction> co()
                {
                    yield return coroutine.Await(async () =>
                    {
                        Console.WriteLine("Going to break in 1 second!");
                        await Task.Delay(1000);
                        throw new Exception($"Broken inside a coroutine await. Thread: {Thread.CurrentThread.ManagedThreadId}");
                    });
                }
                coroutine.Queue(co());
            }

            if (sharpGui.Button(breakBgButton))
            {
                IEnumerator<YieldAction> co()
                {
                    yield return coroutine.Await(Task.Run(async () =>
                    {
                        Console.WriteLine("Going to break in 1 second!");
                        await Task.Delay(1000);
                        throw new Exception($"Broken inside a coroutine background thread await. Thread: {Thread.CurrentThread.ManagedThreadId}");
                    }));
                }
                coroutine.Queue(co());
            }

            sharpGui.End();
        }

        public unsafe void sendUpdate(Clock clock)
        {
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
