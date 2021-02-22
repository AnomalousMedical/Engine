using Anomalous.OSPlatform;
using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
using FreeImageAPI;
using Engine.CameraMovement;
using System.Linq;
using SharpGui;
using SoundPlugin;

namespace SceneTest
{
    class SceneTestUpdateListener : UpdateListener, IDisposable
    {
        //Camera Settings
        float YFov = (float)Math.PI / 4.0f;
        float ZNear = 0.1f;
        float ZFar = 100f;

        //Clear Color
        Engine.Color ClearColor = Engine.Color.FromARGB(0xff2a63cc);

        //Light
        Vector3 lightDirection = Vector3.Up;
        Vector4 lightColor = new Vector4(1, 1, 1, 1);
        float lightIntensity = 3;

        private readonly NativeOSWindow window;
        private readonly DoubleSizeCube shape;
        private readonly Plane plane;
        private readonly TextureLoader textureLoader;
        private readonly CC0TextureLoader cc0TextureLoader;
        private readonly EnvironmentMapBuilder envMapBuilder;
        private readonly IPbrCameraAndLight pbrCameraAndLight;
        private readonly ICC0MaterialTextureBuilder cC0MaterialTextureBuilder;
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
        private readonly IObjectResolver objectResolver;
        private SoundAndSource bgMusicSound;

        private PbrRenderer pbrRenderer;
        private AutoPtr<ITextureView> environmentMapSRV;
        private AutoPtr<IShaderResourceBinding> pboMatBindingTinyDinoSprite;
        private AutoPtr<IShaderResourceBinding> pboMatBindingSwordSprite;
        private AutoPtr<IShaderResourceBinding> pboMatBindingSceneObject;
        private AutoPtr<IShaderResourceBinding> pboMatBindingFloor;

        SharpButton makeDawn = new SharpButton() { Text = "Make Dawn" };
        SharpButton makeDusk = new SharpButton() { Text = "Make Dusk" };
        SharpSliderHorizontal currentHour;

        public unsafe SceneTestUpdateListener(
            GraphicsEngine graphicsEngine,
            NativeOSWindow window,
            PbrRenderer m_GLTFRenderer,
            DoubleSizeCube shape,
            Plane plane,
            TextureLoader textureLoader,
            CC0TextureLoader cc0TextureLoader,
            EnvironmentMapBuilder envMapBuilder,
            IPbrCameraAndLight pbrCameraAndLight,
            ICC0MaterialTextureBuilder cC0MaterialTextureBuilder,
            VirtualFileSystem virtualFileSystem,
            FirstPersonFlyCamera cameraControls,
            SimpleShadowMapRenderer shadowMapRenderer,
            TimeClock timeClock, 
            ISharpGui sharpGui, 
            IScaleHelper scaleHelper,
            SoundManager soundManager,
            SceneObjectManager sceneObjects,
            SpriteManager sprites,
            IObjectResolverFactory objectResolverFactory)
        {
            cameraControls.Position = new Vector3(0, 0, -12);

            this.pbrRenderer = m_GLTFRenderer;
            this.swapChain = graphicsEngine.SwapChain;
            this.renderDevice = graphicsEngine.RenderDevice;
            this.immediateContext = graphicsEngine.ImmediateContext;
            this.window = window;
            this.shape = shape;
            this.plane = plane;
            this.textureLoader = textureLoader;
            this.cc0TextureLoader = cc0TextureLoader;
            this.envMapBuilder = envMapBuilder;
            this.pbrCameraAndLight = pbrCameraAndLight;
            this.cC0MaterialTextureBuilder = cC0MaterialTextureBuilder;
            this.virtualFileSystem = virtualFileSystem;
            this.cameraControls = cameraControls;
            this.shadowMapRenderer = shadowMapRenderer;
            this.timeClock = timeClock;
            this.sharpGui = sharpGui;
            this.scaleHelper = scaleHelper;
            this.soundManager = soundManager;
            this.sceneObjects = sceneObjects;
            this.sprites = sprites;
            this.objectResolver = objectResolverFactory.Create();
            currentHour = new SharpSliderHorizontal() { Rect = scaleHelper.Scaled(new IntRect(100, 10, 500, 35)), Max = 24 };
            Initialize();

            this.objectResolver.Resolve<Player>();
        }

        public void Dispose()
        {
            objectResolver.Dispose();
            bgMusicSound?.Dispose();
            pboMatBindingSwordSprite.Dispose();
            pboMatBindingTinyDinoSprite.Dispose();
            pboMatBindingFloor.Dispose();
            pboMatBindingSceneObject.Dispose();
            environmentMapSRV.Dispose();
        }

        unsafe void Initialize()
        {
            environmentMapSRV = envMapBuilder.BuildEnvMapView(renderDevice, immediateContext, "papermill/Fixed-", "png");

            pbrRenderer.PrecomputeCubemaps(renderDevice, immediateContext, environmentMapSRV.Obj);

            LoadFloorTexture();
            LoadSceneObjectTexture();
            LoadTinyDinoSprite();
            LoadSwordSprite();

            //Create a manual shiny texture to see env map
            //CreateShinyTexture();

            //Make scene
            {
                //Background cubes
                pbrRenderAttribs.AlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE;

                AddBrick(new Vector3(-3, 0, 1), Quaternion.Identity);
                AddBrick(new Vector3(-3, 0, 2), Quaternion.Identity);
                AddBrick(new Vector3(-3, 0, 3), Quaternion.Identity);
                AddBrick(new Vector3(0, 0, 3), Quaternion.Identity);
                AddBrick(new Vector3(1, 0, 3), Quaternion.Identity);
                AddBrick(new Vector3(1, 0, 2), Quaternion.Identity);
                AddBrick(new Vector3(1, 0, 1), Quaternion.Identity);
                AddBrick(new Vector3(1, 0, 0), Quaternion.Identity);
                AddBrick(new Vector3(1, 0, -1), Quaternion.Identity);
                AddBrick(new Vector3(1, 0, -2), Quaternion.Identity);
            }

            {
                //Floor
                sceneObjects.Add(new SceneObject()
                {
                    vertexBuffer = plane.VertexBuffer,
                    skinVertexBuffer = plane.SkinVertexBuffer,
                    indexBuffer = plane.IndexBuffer,
                    numIndices = plane.NumIndices,
                    pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE,
                    position = new Vector3(0, -1, 0),
                    orientation = Quaternion.shortestArcQuat(Vector3.Forward, Vector3.Down),
                    scale = new Vector3(10, 10, 10),
                    shaderResourceBinding = pboMatBindingFloor.Obj,
                    GetShadows = true,
                });
            }

            {
                var sprite = new Sprite();
                //Tiny Dino
                sceneObjects.Add(new SceneObject()
                {
                    vertexBuffer = plane.VertexBuffer,
                    skinVertexBuffer = plane.SkinVertexBuffer,
                    indexBuffer = plane.IndexBuffer,
                    numIndices = plane.NumIndices,
                    pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_MASK,
                    position = new Vector3(-4, 0, -3),
                    orientation = Quaternion.Identity,
                    scale = new Vector3(1.466666666666667f, 1, 1),
                    shaderResourceBinding = pboMatBindingTinyDinoSprite.Obj,
                    RenderShadowPlaceholder = true,
                    Sprite = sprite,
                });
            }

            {
                var sprite = new Sprite();
                //Sword
                sceneObjects.Add(new SceneObject()
                {
                    vertexBuffer = plane.VertexBuffer,
                    skinVertexBuffer = plane.SkinVertexBuffer,
                    indexBuffer = plane.IndexBuffer,
                    numIndices = plane.NumIndices,
                    pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_MASK,
                    position = new Vector3(-1, 0, 0),
                    orientation = Quaternion.Identity,
                    scale = new Vector3(1, 1.714285714285714f, 1) * 0.5f,
                    shaderResourceBinding = pboMatBindingSwordSprite.Obj,
                    RenderShadowPlaceholder = true,
                    Sprite = sprite,
                });
            }
        }

        private unsafe void LoadFloorTexture()
        {
            using var ccoTextures = cc0TextureLoader.LoadTextureSet("cc0Textures/Ground037_1K");
            pboMatBindingFloor = pbrRenderer.CreateMaterialSRB(
                pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                pLightAttribs: pbrCameraAndLight.LightAttribs,
                baseColorMap: ccoTextures.BaseColorMap,
                normalMap: ccoTextures.NormalMap,
                physicalDescriptorMap: ccoTextures.PhysicalDescriptorMap,
                aoMap: ccoTextures.AmbientOcclusionMap,
                shadowMapSRV: shadowMapRenderer.ShadowMapSRV
            );
        }

        private unsafe void LoadSceneObjectTexture()
        {
            using var ccoTextures = cc0TextureLoader.LoadTextureSet("cc0Textures/Bricks045_1K");
            pboMatBindingSceneObject = pbrRenderer.CreateMaterialSRB(
                pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                pLightAttribs: pbrCameraAndLight.LightAttribs,
                baseColorMap: ccoTextures.BaseColorMap,
                normalMap: ccoTextures.NormalMap,
                physicalDescriptorMap: ccoTextures.PhysicalDescriptorMap,
                aoMap: ccoTextures.AmbientOcclusionMap
            );
        }

        private unsafe void LoadTinyDinoSprite()
        {
            using var stream = virtualFileSystem.openStream("original/TinyDino_Color.png", Engine.Resources.FileMode.Open);
            using var image = FreeImageBitmap.FromStream(stream);
            var materials = new Dictionary<uint, (String, String)>()
            {
                { 0xff168516, ( "cc0Textures/Leather008_1K", "jpg" ) }, //Skin (green)
                { 0xffff0000, ( "cc0Textures/SheetMetal004_1K", "jpg" ) }, //Spines (red)
            };
            var scale = Math.Min(1024 / image.Width, 1024 / image.Height);

            using var ccoTextures = cC0MaterialTextureBuilder.CreateMaterialSet(image, scale, materials);

            using var colorTexture = textureLoader.CreateTextureFromImage(image, 1, "colorTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false);

            using var normalTexture = ccoTextures.NormalMap != null ?
                textureLoader.CreateTextureFromImage(ccoTextures.NormalMap, 1, "normalTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

            using var physicalTexture = ccoTextures.PhysicalDescriptorMap != null ?
                textureLoader.CreateTextureFromImage(ccoTextures.PhysicalDescriptorMap, 1, "physicalTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

            using var aoTexture = ccoTextures.AmbientOcclusionMap != null ?
                textureLoader.CreateTextureFromImage(ccoTextures.AmbientOcclusionMap, 1, "aoTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

            pboMatBindingTinyDinoSprite = pbrRenderer.CreateMaterialSRB(
                pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                pLightAttribs: pbrCameraAndLight.LightAttribs,
                baseColorMap: colorTexture?.Obj,
                normalMap: normalTexture?.Obj,
                physicalDescriptorMap: physicalTexture?.Obj,
                aoMap: aoTexture?.Obj,
                alphaMode: PbrAlphaMode.ALPHA_MODE_MASK,
                isSprite: true
            );
        }

        private unsafe void LoadSwordSprite()
        {
            using var stream = virtualFileSystem.openStream("original/Sword.png", Engine.Resources.FileMode.Open);
            using var image = FreeImageBitmap.FromStream(stream);
            var materials = new Dictionary<uint, (String, String)>()
            {
                { 0xff6c351c, ( "cc0Textures/Wood049_1K", "jpg" ) }, //Hilt (brown)
                { 0xffadadad, ( "cc0Textures/Metal032_1K", "jpg" ) }, //Blade (grey)
            };
            var scale = Math.Min(1024 / image.Width, 1024 / image.Height);

            using var ccoTextures = cC0MaterialTextureBuilder.CreateMaterialSet(image, scale, materials);

            using var colorTexture = textureLoader.CreateTextureFromImage(image, 1, "colorTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false);

            using var normalTexture = ccoTextures.NormalMap != null ?
                textureLoader.CreateTextureFromImage(ccoTextures.NormalMap, 1, "normalTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

            using var physicalTexture = ccoTextures.PhysicalDescriptorMap != null ?
                textureLoader.CreateTextureFromImage(ccoTextures.PhysicalDescriptorMap, 1, "physicalTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

            using var aoTexture = ccoTextures.AmbientOcclusionMap != null ?
                textureLoader.CreateTextureFromImage(ccoTextures.AmbientOcclusionMap, 1, "aoTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

            pboMatBindingSwordSprite = pbrRenderer.CreateMaterialSRB(
                pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                pLightAttribs: pbrCameraAndLight.LightAttribs,
                baseColorMap: colorTexture?.Obj,
                normalMap: normalTexture?.Obj,
                physicalDescriptorMap: physicalTexture?.Obj,
                aoMap: aoTexture?.Obj,
                alphaMode: PbrAlphaMode.ALPHA_MODE_MASK,
                isSprite: true
            );
        }

        private void AddBrick(Vector3 trans, Quaternion rot)
        {
            sceneObjects.Add(new SceneObject()
            {
                vertexBuffer = shape.VertexBuffer,
                skinVertexBuffer = shape.SkinVertexBuffer,
                indexBuffer = shape.IndexBuffer,
                numIndices = shape.NumIndices,
                pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE,
                position = trans * 2,
                orientation = rot,
                scale = Vector3.ScaleIdentity,
                shaderResourceBinding = pboMatBindingSceneObject.Obj
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
                new ColumnLayout(makeDawn, makeDusk) { Margin = new IntPad(10) }
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
                var stream = virtualFileSystem.openStream("freepd/Rafael Krux - The Range-10.ogg", Engine.Resources.FileMode.Open, Engine.Resources.FileAccess.Read, Engine.Resources.FileShare.Read);
                bgMusicSound = soundManager.StreamPlaySound(stream);
                bgMusicSound.Sound.Repeat = true;
            }

            int currentTime = (int)(timeClock.CurrentTimeMicro * Clock.MicroToSeconds / (60 * 60));
            if(sharpGui.Slider(currentHour, ref currentTime) || sharpGui.ActiveItem == currentHour.Id)
            {
                timeClock.CurrentTimeMicro = (long)currentTime * 60L * 60L * Clock.SecondsToMicro;
            }
            var time = TimeSpan.FromMilliseconds(timeClock.CurrentTimeMicro * Clock.MicroToMilliseconds);
            sharpGui.Text(currentHour.Rect.Right, currentHour.Rect.Top, timeClock.IsDay ? Engine.Color.Black : Engine.Color.White, $"Time: {time}");

            sharpGui.End();
        }

        private unsafe void UpdateLight(Clock clock)
        {
            if (timeClock.IsDay)
            {
                var dayFactor = (timeClock.DayFactor - 0.5f) * 2.0f;
                var noonFactor = 1.0f - Math.Abs(dayFactor);
                lightDirection = (Vector3.Down * noonFactor + new Vector3(dayFactor, 0, dayFactor * 0.3f + 0.1f)).normalized();
                lightIntensity = 3 * noonFactor;

                pbrRenderAttribs.AverageLogLum = 0.3f;
                ClearColor = Engine.Color.FromARGB(0xff2a63cc);
            }
            else
            {
                var nightFactor = (timeClock.NightFactor - 0.5f) * 2.0f;
                var midnightFactor = 1.0f - Math.Abs(nightFactor);
                lightDirection = (Vector3.Down * midnightFactor + new Vector3(nightFactor, 0, nightFactor * 0.3f + 0.1f)).normalized();

                lightIntensity = 0.7f * midnightFactor;

                pbrRenderAttribs.AverageLogLum = 0.8f;
                ClearColor = Engine.Color.FromARGB(0xff030303);
            }
        }

        private void UpdateSprites(Clock clock)
        {
            foreach(var sprite in sprites)
            {
                sprite.Update(clock);
            }
        }

        public unsafe void sendUpdate(Clock clock)
        {
            //Update
            timeClock.Update(clock);
            UpdateGui(clock);
            cameraControls.UpdateInput(clock);
            UpdateLight(clock);
            UpdateSprites(clock);

            //Render
            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            var PreTransform = swapChain.GetDesc_PreTransform;
            var preTransformMatrix = CameraHelpers.GetSurfacePretransformMatrix(new Vector3(0, 0, 1), PreTransform);
            var cameraProjMatrix = CameraHelpers.GetAdjustedProjectionMatrix(YFov, ZNear, ZFar, window.WindowWidth, window.WindowHeight, PreTransform);

            //Draw Scene
            // Render shadow map
            shadowMapRenderer.BeginShadowMap(renderDevice, immediateContext, lightDirection);
            foreach (var sceneObj in sceneObjects.Where(i => i.shaderResourceBinding == pboMatBindingSceneObject.Obj || i.RenderShadowPlaceholder)) //Render all bricks for shadow map
            {
                shadowMapRenderer.RenderShadowMap(immediateContext, sceneObj.vertexBuffer, sceneObj.skinVertexBuffer, sceneObj.indexBuffer, sceneObj.numIndices, ref sceneObj.position, ref sceneObj.orientation, ref sceneObj.scale);
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
            //pbrCameraAndLight.SetCameraMatrices(ref cameraProjMatrix, ref WorldToShadowMapUVDepthMatr, ref Vector3.Forward);
            pbrCameraAndLight.SetCameraPosition(cameraControls.Position, cameraControls.Orientation, ref preTransformMatrix, ref cameraProjMatrix);

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
                pbrRenderer.Render(immediateContext, sceneObj.shaderResourceBinding, sceneObj.vertexBuffer, sceneObj.skinVertexBuffer, sceneObj.indexBuffer, sceneObj.numIndices, ref sceneObj.position, ref sceneObj.orientation, ref sceneObj.scale, pbrRenderAttribs);
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
