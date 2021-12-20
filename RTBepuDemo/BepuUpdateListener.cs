using Anomalous.OSPlatform;
using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using BepuPhysics;
using BepuUtilities;
using BepuUtilities.Memory;
using BepuPhysics.Collidables;
using Microsoft.Extensions.Logging;
using Engine.CameraMovement;
using DiligentEngine.RT;

namespace RTBepuDemo
{
    class BepuUpdateListener : UpdateListener, IDisposable
    {
        private readonly NativeOSWindow window;
        private readonly RayTracingRenderer renderer;
        private readonly FirstPersonFlyCamera cameraControls;
        private readonly GraphicsEngine graphicsEngine;

        //BEPU
        //If you intend to reuse the BufferPool, disposing the simulation is a good idea- it returns all the buffers to the pool for reuse.
        //Here, we dispose it, but it's not really required; we immediately thereafter clear the BufferPool of all held memory.
        //Note that failing to dispose buffer pools can result in memory leaks.
        Simulation simulation;
        SimpleThreadDispatcher threadDispatcher;
        BufferPool bufferPool;

        Box boxShape;
        BodyInertia boxInertia;
        private List<BodyPositionSync> spherePositionSyncs = new List<BodyPositionSync>();
        //END

        public unsafe BepuUpdateListener(
            NativeOSWindow window,
            DemoScene scene,
            RayTracingRenderer renderer,
            FirstPersonFlyCamera cameraControls,
            GraphicsEngine graphicsEngine)
        {
            this.window = window;
            this.renderer = renderer;
            this.cameraControls = cameraControls;
            this.graphicsEngine = graphicsEngine;
            cameraControls.Position = new Vector3(0, 2, -11);
            Initialize();
        }

        public void Dispose()
        {
            //If you intend to reuse the BufferPool, disposing the simulation is a good idea- it returns all the buffers to the pool for reuse.
            //Here, we dispose it, but it's not really required; we immediately thereafter clear the BufferPool of all held memory.
            //Note that failing to dispose buffer pools can result in memory leaks.
            simulation.Dispose();
            threadDispatcher.Dispose();
            bufferPool.Clear();
        }

        unsafe void Initialize()
        {

            SetupBepu();
        }

        private void SetupBepu()
        {
            //The buffer pool is a source of raw memory blobs for the engine to use.
            bufferPool = new BufferPool();
            //Note that you can also control the order of internal stage execution using a different ITimestepper implementation.
            //The PositionFirstTimestepper is the simplest timestepping mode in a technical sense, but since it integrates velocity into position at the start of the frame, 
            //directly modified velocities outside of the timestep will be integrated before collision detection or the solver has a chance to intervene.
            //PositionLastTimestepper avoids that by running collision detection and the solver first at the cost of a tiny amount of overhead.
            //(You could avoid the issue with PositionFirstTimestepper by modifying velocities in the PositionFirstTimestepper's BeforeCollisionDetection callback 
            //instead of outside the timestep, too, but it's a little more complicated.)
            simulation = Simulation.Create(bufferPool, new NarrowPhaseCallbacks(), new PoseIntegratorCallbacks(new System.Numerics.Vector3(0, -10, 0)), new PositionLastTimestepper());

            //Drop boxes on a big static box.
            boxShape = new Box(1, 1, 1);
            boxShape.ComputeInertia(1, out boxInertia);

            simulation.Statics.Add(new StaticDescription(new System.Numerics.Vector3(0, 0, 0), new CollidableDescription(simulation.Shapes.Add(new Box(1, 1, 1)), 0.1f)));

            simulation.Statics.Add(new StaticDescription(new System.Numerics.Vector3(0, -6, 0), new CollidableDescription(simulation.Shapes.Add(new Box(100, 10, 100)), 0.1f)));

            //Taking off 1 thread could help stability https://github.com/bepu/bepuphysics2/blob/master/Documentation/PerformanceTips.md#general
            var numThreads = Math.Max(Environment.ProcessorCount - 1, 1);
            threadDispatcher = new SimpleThreadDispatcher(numThreads); 
        }

        private void CreateBox(Box box, BodyInertia boxInertia, System.Numerics.Vector3 position)
        {
            var bodyHandle = simulation.Bodies.Add(
                BodyDescription.CreateDynamic(
                    position,
                    boxInertia, new CollidableDescription(simulation.Shapes.Add(box), 0.1f), new BodyActivityDescription(0.01f)));

            var spherePositionSync = new BodyPositionSync(bodyHandle, simulation);
            spherePositionSyncs.Add(spherePositionSync);
        }

        public void exceededMaxDelta()
        {

        }

        public void loopStarting()
        {

        }

        private long nextCubeSpawnTime = 0;
        private long nextCubeSpawnFrequency = 1 * Clock.SecondsToMicro;

        public unsafe void sendUpdate(Clock clock)
        {
            var swapChain = graphicsEngine.SwapChain;
            var immediateContext = graphicsEngine.ImmediateContext;

            cameraControls.UpdateInput(clock);
            UpdatePhysics(clock);
            //gui.Update(clock);

            renderer.Render(cameraControls.Position, cameraControls.Orientation, new Vector4(0, 0, -10, 0), new Vector4(0, 0, -10, 0));

            //This is the old clear loop, leaving in place in case we want or need the screen clear, but I think with pure rt there is no need
            //since we blit a texture to the full screen over and over.
            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            immediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            //var ClearColor = new Color(0.350f, 0.350f, 0.350f, 1.0f);

            // Clear the back buffer
            // Let the engine perform required state transitions
            //immediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            //sharpGui.Render(immediateContext);

            swapChain.Present(1);
        }

        private unsafe void UpdatePhysics(Clock clock)
        {
            if (clock.CurrentTimeMicro > nextCubeSpawnTime)
            {
                float highest = 2;
                //Find highest box
                foreach(var body in spherePositionSyncs)
                {
                    var world = body.GetWorldPosition();
                    world.y += 1.5f;
                    if(world.y > highest)
                    {
                        highest = world.y;
                    }
                }

                CreateBox(boxShape, boxInertia, new System.Numerics.Vector3(-0.8f, highest, 0.8f));
                nextCubeSpawnTime += nextCubeSpawnFrequency;
                window.Title = $"RTBepuDemo - {spherePositionSyncs.Count} boxes.";
            }

            //Multithreading is pretty pointless for a simulation of one ball, but passing a IThreadDispatcher instance is all you have to do to enable multithreading.
            //If you don't want to use multithreading, don't pass a IThreadDispatcher.
            simulation.Timestep(clock.DeltaSeconds, threadDispatcher); //Careful of variable timestep here, not so good
        }
    }
}
