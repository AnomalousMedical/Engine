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
using RogueLikeMapBuilder;
using DungeonGenerator;
using System.Diagnostics;
using System.Threading.Tasks;
using SharpGui;
using DiligentEngine.RT;
using BepuPlugin;

namespace RTDungeonGeneratorTest
{
    class DungeonUpdateListener : UpdateListener, IDisposable
    {
        private readonly GraphicsEngine graphicsEngine;
        private readonly NativeOSWindow window;
        private readonly FirstPersonFlyCamera cameraControls;
        private readonly ISharpGui sharpGui;
        private readonly IScaleHelper scaleHelper;
        private readonly ICoroutineRunner coroutineRunner;
        private readonly IBepuScene bepuScene;
        private readonly RayTracingRenderer renderer;
        private readonly RTGui gui;
        private readonly IObjectResolver objectResolver;

        private SceneDungeon currentDungeon;
        private SharpButton nextScene = new SharpButton() { Text = "Next Scene" };
        private bool loadingLevel = false;
        private int currentSeed = 23;

        public DungeonUpdateListener(
            GraphicsEngine graphicsEngine,
            NativeOSWindow window,
            FirstPersonFlyCamera cameraControls,
            ISharpGui sharpGui,
            IScaleHelper scaleHelper,
            ICoroutineRunner coroutineRunner,
            IBepuScene bepuScene,
            IObjectResolverFactory objectResolverFactory,
            RayTracingRenderer renderer,
            RTGui gui)
        {
            this.graphicsEngine = graphicsEngine;
            this.window = window;
            this.cameraControls = cameraControls;
            this.sharpGui = sharpGui;
            this.scaleHelper = scaleHelper;
            this.coroutineRunner = coroutineRunner;
            this.bepuScene = bepuScene;
            this.renderer = renderer;
            this.gui = gui;
            this.objectResolver = objectResolverFactory.Create();
            cameraControls.Position = new Vector3(0, 2, -11);
            Initialize();
            LoadNextScene();
        }

        private void LoadNextScene()
        {
            coroutineRunner.RunTask(async () =>
            {
                await Task.Run(async () =>
                {
                    loadingLevel = true;
                    var dungeon = this.objectResolver.Resolve<SceneDungeon, SceneDungeon.Desc>(o =>
                    {
                        o.Seed = currentSeed++;
                    });
                    await dungeon.LoadingTask;
                    currentDungeon?.RequestDestruction();
                    currentDungeon = dungeon;
                    loadingLevel = false;
                });
            });
        }

        public void Dispose()
        {
            this.objectResolver.Dispose();
        }

        unsafe void Initialize()
        {
            SetupBepu();
        }

        private void SetupBepu()
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
            cameraControls.UpdateInput(clock);
            bepuScene.Update(clock, new System.Numerics.Vector3(0, 0, 1));
            UpdateGui(clock);
            Render();
        }

        private void UpdateGui(Clock clock)
        {
            sharpGui.Begin(clock);

            if (!loadingLevel)
            {
                var layout =
                    new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                    new MaxWidthLayout(scaleHelper.Scaled(300),
                    new ColumnLayout(nextScene) { Margin = new IntPad(10) }
                    ));
                var desiredSize = layout.GetDesiredSize(sharpGui);
                layout.SetRect(new IntRect(window.WindowWidth - desiredSize.Width, window.WindowHeight - desiredSize.Height, desiredSize.Width, desiredSize.Height));

                //Buttons
                if (sharpGui.Button(nextScene))
                {
                    LoadNextScene();
                }
            }

            gui.Update(clock);

            sharpGui.End();
        }

        private unsafe void Render()
        {
            var swapChain = graphicsEngine.SwapChain;
            var immediateContext = graphicsEngine.ImmediateContext;

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
    }
}
