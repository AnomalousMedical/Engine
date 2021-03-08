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
        private readonly ICoroutineRunner coroutineRunner;
        private readonly IBepuScene bepuScene;
        private readonly CameraMover cameraMover;
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

        private Player player;

        private bool changingLevels = false;
        private List<int> createdLevelSeeds = new List<int>();
        private int currentLevelIndex = 0;
        private Random levelRandom;
        private Level currentLevel;
        private Level nextLevel;
        private Level previousLevel;

        private List<(String floorTexture, String wallTexture)> biomes = new List<(String floorTexture, String wallTexture)>()
        {
            (
                floorTexture: "cc0Textures/Rocks023_1K",
                wallTexture: "cc0Textures/Ground037_1K"
            ),
            (
                floorTexture: "cc0Textures/Ground025_1K",
                wallTexture: "cc0Textures/Rock029_1K"
            ),
            (
                floorTexture: "cc0Textures/Snow006_1K",
                wallTexture: "cc0Textures/Rock022_1K"
            )
        };

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
            CameraMover cameraMover)
        {
            levelRandom = new Random(0);

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
            this.coroutineRunner = coroutineRunner;
            this.bepuScene = bepuScene;
            this.cameraMover = cameraMover;
            this.objectResolver = objectResolverFactory.Create();
            currentHour = new SharpSliderHorizontal() { Rect = scaleHelper.Scaled(new IntRect(100, 10, 500, 35)), Max = 24 };
            Initialize();
        }

        public void Dispose()
        {
            objectResolver.Dispose();
            bgMusicSound?.Dispose();
            environmentMapSRV.Dispose();
        }

        void Initialize()
        {
            environmentMapSRV = envMapBuilder.BuildEnvMapView(renderDevice, immediateContext, "papermill/Fixed-", "png");

            pbrRenderer.PrecomputeCubemaps(renderDevice, immediateContext, environmentMapSRV.Obj);

            //Make scene
            coroutineRunner.RunTask(async () =>
            {
                createdLevelSeeds.Add(levelRandom.Next(int.MinValue, int.MaxValue));
                createdLevelSeeds.Add(levelRandom.Next(int.MinValue, int.MaxValue));

                currentLevel = CreateLevel(createdLevelSeeds[0], new Vector3(0, 0, 0));
                nextLevel = CreateLevel(createdLevelSeeds[1], new Vector3(150, 0, 0));

                await currentLevel.WaitForLevelGeneration();

                currentLevel.SetupPhysics();

                player = this.objectResolver.Resolve<Player, Player.Description>(c =>
                {
                    c.Translation = currentLevel.StartPoint;
                });

                this.objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
                {
                    Enemy.Desc.MakeTinyDino(c);
                    c.Translation = currentLevel.StartPoint + new Vector3(-4, 0, -1);
                });
                this.objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
                {
                    Enemy.Desc.MakeTinyDino(c, skinMaterial: "cc0Textures/Leather011_1K");
                    c.Translation = currentLevel.StartPoint + new Vector3(-5, 0, -2);
                });
                this.objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
                {
                    Enemy.Desc.MakeSkeleton(c);
                    c.Translation = currentLevel.StartPoint + new Vector3(0, 0, -3);
                });
                this.objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
                {
                    Enemy.Desc.MakeTinyDino(c);
                    c.Translation = currentLevel.StartPoint + new Vector3(-6, 0, -3);
                });

                await nextLevel.WaitForLevelGeneration();
            });
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

            if (!changingLevels && sharpGui.Button(goNextLevel))
            {
                coroutineRunner.RunTask(GoNextLevel());
            }

            if (!changingLevels && sharpGui.Button(goPreviousLevel))
            {
                coroutineRunner.RunTask(GoPreviousLevel());
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

        private async Task GoNextLevel()
        {
            if (changingLevels)
            {
                return;
            }

            changingLevels = true;
            if (previousLevel != null)
            {
                await previousLevel.WaitForLevelGeneration(); //This is pretty unlikely, but have to stop here if level isn't created yet
            }
            await nextLevel.WaitForLevelGeneration(); //Also unlikely, but next level might not be loaded yet

            //Shuffle levels
            previousLevel?.RequestDestruction();
            previousLevel = currentLevel;
            currentLevel = nextLevel;

            //Change level index
            ++currentLevelIndex;
            var nextLevelIndex = currentLevelIndex + 1;
            if (nextLevelIndex == createdLevelSeeds.Count)
            {
                createdLevelSeeds.Add(levelRandom.Next(int.MinValue, int.MaxValue));
            }
            var levelSeed = createdLevelSeeds[nextLevelIndex];

            //Create new level
            nextLevel = CreateLevel(levelSeed, new Vector3(150, 0, 0));

            //Physics changeover
            previousLevel.DestroyPhysics();
            previousLevel.SetPosition(new Vector3(-150, 0, 0));
            currentLevel.SetPosition(new Vector3(0, 0, 0));
            currentLevel.SetupPhysics();

            player.SetLocation(currentLevel.StartPoint);

            changingLevels = false;
        }

        private async Task GoPreviousLevel()
        {
            if (changingLevels)
            {
                return;
            }

            //Change level index
            --currentLevelIndex;
            if (currentLevelIndex < 0)
            {
                //Below 0, do nothing
                currentLevelIndex = 0;
                return;
            }

            changingLevels = true;
            if (previousLevel != null)
            {
                await previousLevel.WaitForLevelGeneration(); //This is pretty unlikely, but have to stop here if level isn't created yet
            }
            await nextLevel.WaitForLevelGeneration(); //Also unlikely, but next level might not be loaded yet

            //Shuffle levels
            nextLevel?.RequestDestruction();
            nextLevel = currentLevel;
            currentLevel = previousLevel;

            if (currentLevelIndex > 0)
            {
                var levelSeed = createdLevelSeeds[currentLevelIndex - 1];
                previousLevel = CreateLevel(levelSeed, new Vector3(-150, 0, 0));
            }
            else
            {
                previousLevel = null;
            }

            //Physics changeover
            nextLevel.DestroyPhysics();
            nextLevel.SetPosition(new Vector3(150, 0, 0));
            currentLevel.SetPosition(new Vector3(0, 0, 0));
            currentLevel.SetupPhysics();

            player.SetLocation(currentLevel.StartPoint);

            changingLevels = false;
        }

        private Level CreateLevel(int levelSeed, Vector3 translation)
        {
            var random = new Random(levelSeed);
            var biome = biomes[random.Next(0, biomes.Count)];

            return this.objectResolver.Resolve<Level, Level.Description>(o =>
            {
                //o.MapUnitY = 1.0f;
                o.FloorTexture = biome.floorTexture;
                o.WallTexture = biome.wallTexture;

                o.Translation = translation;
                o.RandomSeed = levelSeed;
                o.Width = 50;
                o.Height = 50;
                o.CorridorSpace = 10;
                o.RoomDistance = 3;
                o.RoomMin = new IntSize2(2, 2);
                o.RoomMax = new IntSize2(6, 6); //Between 3-6 is good here, 3 for more cityish with small rooms, 6 for more open with more big rooms, sometimes connected
                o.CorridorMaxLength = 4;
            });
        }

        private unsafe void UpdateLight(Clock clock)
        {
            if (timeClock.IsDay)
            {
                var dayFactor = (timeClock.DayFactor - 0.5f) * 2.0f;
                var noonFactor = 1.0f - Math.Abs(dayFactor);
                lightDirection = new Vector3(dayFactor, -0.5f * noonFactor - 0.1f, 1f).normalized();
                lightIntensity = 6f * noonFactor + 1.0f;

                pbrRenderAttribs.AverageLogLum = 0.3f;
                ClearColor = Engine.Color.FromARGB(0xff2a63cc);
            }
            else
            {
                var nightFactor = (timeClock.NightFactor - 0.5f) * 2.0f;
                var midnightFactor = 1.0f - Math.Abs(nightFactor);
                lightDirection = new Vector3(nightFactor, -0.5f * midnightFactor - 0.1f, 1f).normalized();

                lightIntensity = 0.7f * midnightFactor + 1.0f;

                pbrRenderAttribs.AverageLogLum = 0.8f;
                ClearColor = Engine.Color.FromARGB(0xff030303);
            }

            var oneHour = 60l * 60l * Clock.SecondsToMicro;

            if (timeClock.CurrentTimeMicro > timeClock.DayStart - oneHour && timeClock.CurrentTimeMicro < timeClock.DayStart + oneHour)
            {
                ClearColor = Engine.Color.FromARGB(0xff242148);
            }

            if (timeClock.CurrentTimeMicro > timeClock.DayEnd - oneHour && timeClock.CurrentTimeMicro < timeClock.DayEnd + oneHour)
            {
                ClearColor = Engine.Color.FromARGB(0xff242148);
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

            objectResolver.Flush();

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
