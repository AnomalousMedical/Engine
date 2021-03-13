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

        //Clear Color
        Engine.Color ClearColor = Engine.Color.FromARGB(0xff2a63cc);

        //Light
        Vector3 lightDirection = Vector3.Up;
        Vector4 lightColor = new Vector4(1, 1, 1, 1);
        float lightIntensity = 3;

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
        private readonly SceneObjectManager sceneObjects;
        private readonly SpriteManager sprites;
        private readonly IObjectResolverFactory objectResolverFactory;
        private readonly ICoroutineRunner coroutineRunner;
        private readonly IBepuScene bepuScene;
        private readonly CameraMover cameraMover;
        private readonly ILevelManager levelManager;
        private readonly IObjectResolver objectResolver;
        private SoundAndSource bgMusicSound;

        private PbrRenderer pbrRenderer;
        private AutoPtr<ITextureView> environmentMapSRV;

        SharpButton makeDawn = new SharpButton() { Text = "Make Dawn" };
        SharpButton makeDusk = new SharpButton() { Text = "Make Dusk" };
        SharpButton goNextLevel = new SharpButton() { Text = "Next Level" };
        SharpButton goPreviousLevel = new SharpButton() { Text = "Previous Level" };
        SharpSliderHorizontal currentHour;

        private bool useFirstPersonCamera = false;

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
            SceneObjectManager sceneObjects,
            SpriteManager sprites,
            IObjectResolverFactory objectResolverFactory,
            ICoroutineRunner coroutineRunner,
            IBepuScene bepuScene,
            CameraMover cameraMover,
            ILevelManager levelManager)
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
            this.sceneObjects = sceneObjects;
            this.sprites = sprites;
            this.objectResolverFactory = objectResolverFactory;
            this.coroutineRunner = coroutineRunner;
            this.bepuScene = bepuScene;
            this.cameraMover = cameraMover;
            this.levelManager = levelManager;
            this.objectResolver = objectResolverFactory.Create();
            currentHour = new SharpSliderHorizontal() { Rect = scaleHelper.Scaled(new IntRect(100, 10, 500, 35)), Max = 24 };
            coroutineRunner.RunTask(Initialize());
        }

        public void Dispose()
        {
            objectResolver.Dispose();
            bgMusicSound?.Dispose();
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

        private void UpdateGui(Clock clock)
        {
            sharpGui.Begin(clock);

            var layout =
                new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                new MaxWidthLayout(scaleHelper.Scaled(300),
                new ColumnLayout(makeDawn, makeDusk, goNextLevel, goPreviousLevel) { Margin = new IntPad(10) }
                ));
            var desiredSize = layout.GetDesiredSize(sharpGui);
            layout.SetRect(new IntRect(window.WindowWidth - desiredSize.Width, window.WindowHeight - desiredSize.Height, desiredSize.Width, desiredSize.Height));

            //Buttons
            if (sharpGui.Button(makeDawn, navUp: makeDusk.Id, navDown: makeDusk.Id))
            {
                timeClock.CurrentTimeMicro = timeClock.DayStart;
            }

            if (sharpGui.Button(makeDusk, navUp: makeDawn.Id, navDown: makeDawn.Id))
            {
                bgMusicSound?.Dispose();

                timeClock.CurrentTimeMicro = timeClock.DayEnd;
                var stream = virtualFileSystem.openStream("freepd/Rafael Krux - The Range-10.ogg", FileMode.Open, FileAccess.Read, FileShare.Read);
                bgMusicSound = soundManager.StreamPlaySound(stream);
                bgMusicSound.Sound.Repeat = true;
            }

            if (!levelManager.ChangingLevels && sharpGui.Button(goNextLevel))
            {
                coroutineRunner.RunTask(levelManager.GoNextLevel());
            }

            if (!levelManager.ChangingLevels && sharpGui.Button(goPreviousLevel))
            {
                coroutineRunner.RunTask(levelManager.GoPreviousLevel());
            }

            int currentTime = (int)(timeClock.CurrentTimeMicro * Clock.MicroToSeconds / (60 * 60));
            if (sharpGui.Slider(currentHour, ref currentTime) || sharpGui.ActiveItem == currentHour.Id)
            {
                timeClock.CurrentTimeMicro = (long)currentTime * 60L * 60L * Clock.SecondsToMicro;
            }
            var time = TimeSpan.FromMilliseconds(timeClock.CurrentTimeMicro * Clock.MicroToMilliseconds);
            sharpGui.Text(currentHour.Rect.Right, currentHour.Rect.Top, timeClock.IsDay ? Engine.Color.Black : Engine.Color.White, $"Time: {time}");

            sharpGui.End();
        }

        const long OneHour = 60L * 60L * Clock.SecondsToMicro;
        readonly Color DaySky = Color.FromARGB(0xff2a63cc);
        readonly Color NightSky = Color.FromARGB(0xff030303);
        readonly Color DawnSky = Color.FromARGB(0xff242148);
        readonly Color DuskSky = Color.FromARGB(0xff242148);

        private unsafe void UpdateLight(Clock clock)
        {
            if (timeClock.IsDay)
            {
                var dayFactor = (timeClock.DayFactor - 0.5f) * 2.0f;
                var noonFactor = 1.0f - Math.Abs(dayFactor);
                lightDirection = new Vector3(dayFactor, -0.5f * noonFactor - 0.1f, 1f).normalized();
                lightIntensity = 5f * noonFactor + 2.0f;

                pbrRenderAttribs.AverageLogLum = 0.3f;
                ClearColor = DaySky;

                if (timeClock.CurrentTimeMicro < timeClock.DayStart + OneHour)
                {
                    float timeFactor = (timeClock.CurrentTimeMicro - timeClock.DayStart) / (float)OneHour;
                    ClearColor = Color.FadeColors(timeFactor, DawnSky, DaySky);
                }

                if (timeClock.CurrentTimeMicro > timeClock.DayEnd - OneHour)
                {
                    float timeFactor = (timeClock.CurrentTimeMicro - (timeClock.DayEnd - OneHour)) / (float)OneHour;
                    ClearColor = Color.FadeColors(timeFactor, DaySky, DuskSky);
                }
            }
            else
            {
                var nightFactor = (timeClock.NightFactor - 0.5f) * 2.0f;
                var midnightFactor = 1.0f - Math.Abs(nightFactor);
                lightDirection = new Vector3(nightFactor, -0.5f * midnightFactor - 0.1f, 1f).normalized();

                lightIntensity = 0.7f * midnightFactor + 2.0f;

                pbrRenderAttribs.AverageLogLum = 0.8f;
                ClearColor = NightSky;

                if (timeClock.CurrentTimeMicro > timeClock.DayStart - OneHour && timeClock.CurrentTimeMicro <= timeClock.DayStart)
                {
                    float timeFactor = (timeClock.CurrentTimeMicro - (timeClock.DayStart - OneHour)) / (float)OneHour;
                    ClearColor = Color.FadeColors(timeFactor, NightSky, DawnSky);
                }

                if (timeClock.CurrentTimeMicro >= timeClock.DayEnd && timeClock.CurrentTimeMicro < timeClock.DayEnd + OneHour)
                {
                    float timeFactor = (timeClock.CurrentTimeMicro - timeClock.DayEnd) / (float)OneHour;
                    ClearColor = Color.FadeColors(timeFactor, DuskSky, NightSky);
                }
            }
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
            //Update
            bepuScene.Update(clock, new System.Numerics.Vector3(0, 0, 1));
            timeClock.Update(clock);
            UpdateGui(clock);
            UpdateLight(clock);
            UpdateSprites(clock);

            objectResolverFactory.Flush();

            //Render
            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            var PreTransform = swapChain.GetDesc_PreTransform;
            var preTransformMatrix = CameraHelpers.GetSurfacePretransformMatrix(new Vector3(0, 0, 1), PreTransform);
            var cameraProjMatrix = CameraHelpers.GetAdjustedProjectionMatrix(YFov, ZNear, ZFar, window.WindowWidth, window.WindowHeight, PreTransform);

            //Draw Scene
            // Render shadow map
            shadowMapRenderer.BeginShadowMap(renderDevice, immediateContext, lightDirection, Vector3.Zero, 90); //Centering scene on player seems to work the best
            foreach (var sceneObj in sceneObjects.Where(i => i.RenderShadow))
            {
                var pos = sceneObj.position - cameraMover.SceneCenter;
                shadowMapRenderer.RenderShadowMap(immediateContext, sceneObj.vertexBuffer, sceneObj.skinVertexBuffer, sceneObj.indexBuffer, sceneObj.numIndices, ref pos, ref sceneObj.orientation, ref sceneObj.scale);
            }

            //Render scene colors
            pbrRenderer.Begin(immediateContext);
            immediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            // Clear the back buffer
            immediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            // Set Light
            var WorldToShadowMapUVDepthMatrix = shadowMapRenderer.WorldToShadowMapUVDepthMatr;
            pbrCameraAndLight.SetLightAndShadow(ref lightDirection, ref lightColor, lightIntensity, ref WorldToShadowMapUVDepthMatrix);

            // Set Camera
            if (useFirstPersonCamera)
            {
                cameraControls.UpdateInput(clock);
                pbrCameraAndLight.SetCameraPosition(cameraControls.Position, cameraControls.Orientation, ref preTransformMatrix, ref cameraProjMatrix);
            }
            else
            {
                pbrCameraAndLight.SetCameraPosition(cameraMover.Position - cameraMover.SceneCenter, cameraMover.Orientation, ref preTransformMatrix, ref cameraProjMatrix);
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

        private void RenderGui()
        {
            //Draw the gui
            var pDSV = swapChain.GetDepthBufferDSV();
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            sharpGui.Render(immediateContext);
        }
    }
}
