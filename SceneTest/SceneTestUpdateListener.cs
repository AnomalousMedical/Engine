using Anomalous.OSPlatform;
using BepuPlugin;
using DiligentEngine;
using DiligentEngine.GltfPbr;
using Engine;
using Engine.CameraMovement;
using Engine.Platform;
using SharpGui;
using SoundPlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SceneTest
{
    class SceneTestUpdateListener : UpdateListener, IDisposable
    {
        //Camera Settings
        float YFov = MathFloat.PI / 4.0f;
        float ZNear = 0.1f;
        float ZFar = 350f;

        private readonly NativeOSWindow window;
        private readonly EnvironmentMapBuilder envMapBuilder;
        private readonly IPbrCameraAndLight pbrCameraAndLight;
        private readonly VirtualFileSystem virtualFileSystem;
        private readonly FirstPersonFlyCamera cameraControls;
        private readonly SimpleShadowMapRenderer shadowMapRenderer;
        private readonly TimeClock timeClock;
        private readonly ISharpGui sharpGui;
        private readonly IScaleHelper scaleHelper;
        private readonly ISwapChain swapChain;
        private readonly IRenderDevice renderDevice;
        private readonly IDeviceContext immediateContext;
        private readonly PbrRenderAttribs pbrRenderAttribs = PbrRenderAttribs.CreateDefault();

        private readonly SoundManager soundManager;
        private readonly SceneObjectManager<ILevelManager> levelSceneObjects;
        private readonly SceneObjectManager<IBattleManager> battleSceneObjects;
        private readonly SpriteManager sprites;
        private readonly IObjectResolverFactory objectResolverFactory;
        private readonly ICoroutineRunner coroutineRunner;
        private readonly IBepuScene bepuScene;
        private readonly CameraMover cameraMover;
        private readonly ILevelManager levelManager;
        private readonly Sky sky;
        private readonly IBattleManager battleManager;
        private readonly IObjectResolver objectResolver;

        private PbrRenderer pbrRenderer;
        private AutoPtr<ITextureView> environmentMapSRV;

        SharpButton goNextLevel = new SharpButton() { Text = "Next Level" };
        SharpButton goPreviousLevel = new SharpButton() { Text = "Previous Level" };
        SharpButton toggleCamera = new SharpButton() { Text = "Toggle Camera" };
        SharpButton battle = new SharpButton() { Text = "Battle" };
        SharpSliderHorizontal currentHour;

        private bool useFirstPersonCamera = false;
        private bool showDebugGui = true;

        private int dangerCounter = 0;
        private long dangerCounterAccumulator = 0;
        private const long DangerCounterTick = Clock.SecondsToMicro / 3;
        private Random battleRandom = new Random();

        public unsafe SceneTestUpdateListener(
            GraphicsEngine graphicsEngine,
            NativeOSWindow window,
            PbrRenderer m_GLTFRenderer,
            EnvironmentMapBuilder envMapBuilder,
            IPbrCameraAndLight pbrCameraAndLight,
            VirtualFileSystem virtualFileSystem,
            FirstPersonFlyCamera cameraControls,
            SimpleShadowMapRenderer shadowMapRenderer,
            TimeClock timeClock,
            ISharpGui sharpGui,
            IScaleHelper scaleHelper,
            SoundManager soundManager,
            SceneObjectManager<ILevelManager> sceneObjects,
            SceneObjectManager<IBattleManager> battleSceneObjects,
            SpriteManager sprites,
            IObjectResolverFactory objectResolverFactory,
            ICoroutineRunner coroutineRunner,
            IBepuScene bepuScene,
            CameraMover cameraMover,
            ILevelManager levelManager,
            Sky sky,
            IBattleManager battleManager)
        {
            cameraControls.Position = new Vector3(0, 0, -12);

            this.pbrRenderer = m_GLTFRenderer;
            this.swapChain = graphicsEngine.SwapChain;
            this.renderDevice = graphicsEngine.RenderDevice;
            this.immediateContext = graphicsEngine.ImmediateContext;
            this.window = window;
            this.envMapBuilder = envMapBuilder;
            this.pbrCameraAndLight = pbrCameraAndLight;
            this.virtualFileSystem = virtualFileSystem;
            this.cameraControls = cameraControls;
            this.shadowMapRenderer = shadowMapRenderer;
            this.timeClock = timeClock;
            this.sharpGui = sharpGui;
            this.scaleHelper = scaleHelper;
            this.soundManager = soundManager;
            this.levelSceneObjects = sceneObjects;
            this.battleSceneObjects = battleSceneObjects;
            this.sprites = sprites;
            this.objectResolverFactory = objectResolverFactory;
            this.coroutineRunner = coroutineRunner;
            this.bepuScene = bepuScene;
            this.cameraMover = cameraMover;
            this.levelManager = levelManager;
            this.sky = sky;
            this.battleManager = battleManager;
            this.objectResolver = objectResolverFactory.Create();
            currentHour = new SharpSliderHorizontal() { Rect = scaleHelper.Scaled(new IntRect(100, 10, 500, 35)), Max = 24 };
            coroutineRunner.RunTask(Initialize());
        }

        public void Dispose()
        {
            objectResolver.Dispose();
            environmentMapSRV.Dispose();
        }

        async Task Initialize()
        {
            environmentMapSRV = envMapBuilder.BuildEnvMapView(renderDevice, immediateContext, "papermill/Fixed-", "png");

            pbrRenderer.PrecomputeCubemaps(renderDevice, immediateContext, environmentMapSRV.Obj);

            await levelManager.Initialize();
        }

        public void exceededMaxDelta()
        {

        }

        public void loopStarting()
        {

        }

        private void UpdateDebugGui()
        {
            var layout =
                new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                new MaxWidthLayout(scaleHelper.Scaled(300),
                new ColumnLayout(battle, goNextLevel, goPreviousLevel, toggleCamera) { Margin = new IntPad(10) }
                ));
            var desiredSize = layout.GetDesiredSize(sharpGui);
            layout.SetRect(new IntRect(window.WindowWidth - desiredSize.Width, window.WindowHeight - desiredSize.Height, desiredSize.Width, desiredSize.Height));

            if (sharpGui.Button(battle, navUp: toggleCamera.Id, navDown: goNextLevel.Id))
            {
                StartBattle();
            }

            if (!levelManager.ChangingLevels && sharpGui.Button(goNextLevel, navUp: battle.Id, navDown: goPreviousLevel.Id))
            {
                coroutineRunner.RunTask(levelManager.GoNextLevel());
            }

            if (!levelManager.ChangingLevels && sharpGui.Button(goPreviousLevel, navUp: goNextLevel.Id, navDown: toggleCamera.Id))
            {
                coroutineRunner.RunTask(levelManager.GoPreviousLevel());
            }

            if (sharpGui.Button(toggleCamera, navUp: goPreviousLevel.Id, navDown: battle.Id))
            {
                useFirstPersonCamera = !useFirstPersonCamera;
            }

            int currentTime = (int)(timeClock.CurrentTimeMicro * Clock.MicroToSeconds / (60 * 60));
            if (sharpGui.Slider(currentHour, ref currentTime) || sharpGui.ActiveItem == currentHour.Id)
            {
                timeClock.CurrentTimeMicro = (long)currentTime * 60L * 60L * Clock.SecondsToMicro;
            }
            var time = TimeSpan.FromMilliseconds(timeClock.CurrentTimeMicro * Clock.MicroToMilliseconds);
            sharpGui.Text(currentHour.Rect.Right, currentHour.Rect.Top, timeClock.IsDay ? Engine.Color.Black : Engine.Color.White, $"Time: {time}");
        }

        private void StartBattle()
        {
            battleManager.SetupBattle();
            battleManager.SetActive(true);
        }

        private void UpdateSprites(Clock clock)
        {
            foreach (var sprite in sprites)
            {
                sprite.Update(clock);
            }
        }

        public unsafe void sendUpdate(Clock clock)
        {
            IEnumerable<SceneObject> sceneObjects;

            //Update
            timeClock.Update(clock);
            sharpGui.Begin(clock);
            if (battleManager.Active)
            {
                sceneObjects = battleSceneObjects;
                battleManager.Update(clock);
            }
            else
            {
                UpdateRandomEncounter(clock);
                sceneObjects = levelSceneObjects;
                bepuScene.Update(clock, new System.Numerics.Vector3(0, 0, 1));
                if (showDebugGui)
                {
                    UpdateDebugGui();
                }
            }
            sharpGui.End();
            sky.UpdateLight(clock);
            UpdateSprites(clock);

            pbrRenderAttribs.AverageLogLum = sky.AverageLogLum;

            objectResolverFactory.Flush();

            //Render
            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            var PreTransform = swapChain.GetDesc_PreTransform;
            var preTransformMatrix = CameraHelpers.GetSurfacePretransformMatrix(new Vector3(0, 0, 1), PreTransform);
            var cameraProjMatrix = CameraHelpers.GetAdjustedProjectionMatrix(YFov, ZNear, ZFar, window.WindowWidth, window.WindowHeight, PreTransform);

            //Draw Scene
            // Render shadow map
            shadowMapRenderer.BeginShadowMap(renderDevice, immediateContext, sky.LightDirection, Vector3.Zero, 90); //Centering scene on player seems to work the best
            foreach (var sceneObj in sceneObjects.Where(i => i.RenderShadow))
            {
                var pos = sceneObj.position - cameraMover.SceneCenter;
                shadowMapRenderer.RenderShadowMap(immediateContext, sceneObj.vertexBuffer, sceneObj.skinVertexBuffer, sceneObj.indexBuffer, sceneObj.numIndices, ref pos, ref sceneObj.orientation, ref sceneObj.scale);
            }

            //Render scene colors
            pbrRenderer.Begin(immediateContext);
            immediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            // Clear the back buffer
            immediateContext.ClearRenderTarget(pRTV, sky.ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            // Set Light
            var WorldToShadowMapUVDepthMatrix = shadowMapRenderer.WorldToShadowMapUVDepthMatr;
            pbrCameraAndLight.SetLightAndShadow(sky.LightDirection, sky.LightColor, sky.LightIntensity, WorldToShadowMapUVDepthMatrix);

            // Set Camera
            if (useFirstPersonCamera)
            {
                cameraControls.UpdateInput(clock);
                pbrCameraAndLight.SetCameraPosition(cameraControls.Position, cameraControls.Orientation, preTransformMatrix, cameraProjMatrix);
            }
            else
            {
                pbrCameraAndLight.SetCameraPosition(cameraMover.Position - cameraMover.SceneCenter, cameraMover.Orientation, preTransformMatrix, cameraProjMatrix);
            }

            foreach (var sceneObj in sceneObjects)
            {
                pbrRenderAttribs.AlphaMode = sceneObj.pbrAlphaMode;
                pbrRenderAttribs.GetShadows = sceneObj.GetShadows;
                pbrRenderAttribs.IsSprite = sceneObj.Sprite != null;
                if (pbrRenderAttribs.IsSprite)
                {
                    var frame = sceneObj.Sprite.GetCurrentFrame();
                    pbrRenderAttribs.SpriteUVLeft = frame.Left;
                    pbrRenderAttribs.SpriteUVTop = frame.Top;
                    pbrRenderAttribs.SpriteUVRight = frame.Right;
                    pbrRenderAttribs.SpriteUVBottom = frame.Bottom;
                }

                var pos = sceneObj.position - cameraMover.SceneCenter;

                pbrRenderer.Render(immediateContext, sceneObj.shaderResourceBinding, sceneObj.vertexBuffer, sceneObj.skinVertexBuffer, sceneObj.indexBuffer, sceneObj.numIndices, ref pos, ref sceneObj.orientation, ref sceneObj.scale, pbrRenderAttribs);
            }

            this.shadowMapRenderer.RenderShadowMapVis(immediateContext);

            RenderGui();

            this.swapChain.Present(1);
        }

        private unsafe void UpdateRandomEncounter(Clock clock)
        {
            dangerCounterAccumulator += clock.DeltaTimeMicro;
            if (dangerCounterAccumulator > DangerCounterTick)
            {
                dangerCounterAccumulator -= DangerCounterTick;
                if (levelManager.IsPlayerMoving)
                {
                    dangerCounter += 4096 / 64; //This will be encounter value
                    Console.WriteLine(dangerCounter);
                    int battleChance = battleRandom.Next(256);
                    var check = dangerCounter / 256;
                    if (battleChance < check)
                    {
                        IEnumerator<YieldAction> co()
                        {
                            StartBattle();
                            dangerCounter = 0;
                            Console.WriteLine("Battle started");
                            yield break;
                        }

                        coroutineRunner.Queue(co());
                    }
                }

            }
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
