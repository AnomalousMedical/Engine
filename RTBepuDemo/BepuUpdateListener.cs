﻿using Anomalous.OSPlatform;
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
using BepuPlugin;
using SharpGui;

namespace RTBepuDemo
{
    class BepuUpdateListener : UpdateListener, IDisposable
    {
        private readonly NativeOSWindow window;
        private readonly RayTracingRenderer renderer;
        private readonly FirstPersonFlyCamera cameraControls;
        private readonly GraphicsEngine graphicsEngine;
        private readonly IObjectResolverFactory objectResolverFactory;
        private readonly IBepuScene bepuScene;
        private readonly RTGui gui;
        private readonly ISharpGui sharpGui;
        private IObjectResolver objectResolver;
        private List<BodyPositionSync> bodyPositionSyncs = new List<BodyPositionSync>();
        private Random Random = new Random();

        Box boxShape;
        BodyInertia boxInertia;
        //END

        public unsafe BepuUpdateListener(
            NativeOSWindow window,
            DemoScene scene,
            RayTracingRenderer renderer,
            FirstPersonFlyCamera cameraControls,
            GraphicsEngine graphicsEngine,
            IObjectResolverFactory objectResolverFactory,
            IBepuScene bepuScene,
            RTGui gui,
            ISharpGui sharpGui
        )
        {
            this.window = window;
            this.renderer = renderer;
            this.cameraControls = cameraControls;
            this.graphicsEngine = graphicsEngine;
            this.objectResolverFactory = objectResolverFactory;
            this.bepuScene = bepuScene;
            this.gui = gui;
            this.sharpGui = sharpGui;
            cameraControls.Position = new Vector3(0, 2, -11);
            SetupBepu();
            this.objectResolver = objectResolverFactory.Create();
        }

        public void Dispose()
        {
            this.objectResolver.Dispose();
        }

        private void SetupBepu()
        {
            var simulation = bepuScene.Simulation;

            //Drop boxes on a big static box.
            boxShape = new Box(1, 1, 1);
            boxShape.ComputeInertia(1, out boxInertia);

            simulation.Statics.Add(new StaticDescription(new System.Numerics.Vector3(0, 0, 0), new CollidableDescription(simulation.Shapes.Add(new Box(1, 1, 1)), 0.1f)));

            simulation.Statics.Add(new StaticDescription(new System.Numerics.Vector3(0, -6, 0), new CollidableDescription(simulation.Shapes.Add(new Box(100, 10, 100)), 0.1f)));
        }

        private void CreateBox(Box box, BodyInertia boxInertia, System.Numerics.Vector3 position)
        {
            var body = objectResolver.Resolve<BodyPositionSync, BodyPositionSync.Desc>(o =>
            {
                o.position = position;
                o.box = box;
                o.boxInertia = boxInertia;
                if(o.TextureIndex == 1)
                {
                    o.Flags = RAYTRACING_INSTANCE_FLAGS.RAYTRACING_INSTANCE_FORCE_NO_OPAQUE;
                }
            });

            bodyPositionSyncs.Add(body);
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
            sharpGui.Begin(clock);
            gui.Update(clock);
            sharpGui.End();
            objectResolverFactory.Flush();

            renderer.Render(cameraControls.Position, cameraControls.Orientation, gui.LightPos, gui.LightPos);

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
            sharpGui.Render(immediateContext);

            swapChain.Present(1);
        }

        private unsafe void UpdatePhysics(Clock clock)
        {
            if (clock.CurrentTimeMicro > nextCubeSpawnTime)
            {
                float highest = 2;
                //Find highest box
                foreach(var body in bodyPositionSyncs) //Need a more generic way to handle the instances, more like the other method
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
                window.Title = $"RTBepuDemo - {bodyPositionSyncs.Count} boxes.";
            }

            bepuScene.Update(clock, new System.Numerics.Vector3(0, 0, 1));

            foreach (var body in bodyPositionSyncs)
            {
                body.SyncPhysics(bepuScene);
            }
        }
    }
}
