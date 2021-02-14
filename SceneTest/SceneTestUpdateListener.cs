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
        private ISwapChain swapChain;
        private IRenderDevice renderDevice;
        private IDeviceContext immediateContext;
        private PbrRenderAttribs pbrRenderAttribs = PbrRenderAttribs.CreateDefault();

        private PbrRenderer pbrRenderer;
        private AutoPtr<ITextureView> environmentMapSRV;
        private AutoPtr<IShaderResourceBinding> pboMatBindingSprite;
        private AutoPtr<IShaderResourceBinding> pboMatBindingTinyDinoSprite;
        private AutoPtr<IShaderResourceBinding> pboMatBindingSceneObject;
        private AutoPtr<IShaderResourceBinding> pboMatBindingFloor;

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
            FirstPersonFlyCamera cameraControls)
        {
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
            Initialize();
        }

        public void Dispose()
        {
            pboMatBindingTinyDinoSprite.Dispose();
            pboMatBindingFloor.Dispose();
            pboMatBindingSceneObject.Dispose();
            pboMatBindingSprite.Dispose();
            environmentMapSRV.Dispose();
        }

        unsafe void Initialize()
        {
            environmentMapSRV = envMapBuilder.BuildEnvMapView(renderDevice, immediateContext, "papermill/Fixed-", "png");

            

            pbrRenderer.PrecomputeCubemaps(renderDevice, immediateContext, environmentMapSRV.Obj);


            LoadFloorTexture();
            LoadSceneObjectTexture();
            LoadTinyDinoSprite();
            LoadPlayerSprite();

            //Create a manual shiny texture to see env map
            //CreateShinyTexture();
        }

        private unsafe void CreateShinyTexture()
        {
            const uint texDim = 10;
            var physDesc = new UInt32[texDim * texDim];
            var physDescSpan = new Span<UInt32>(physDesc);
            physDescSpan.Fill(0xff0000ff);

            var TexDesc = new TextureDesc();
            TexDesc.Type = RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D;
            TexDesc.Usage = USAGE.USAGE_IMMUTABLE;
            TexDesc.BindFlags = BIND_FLAGS.BIND_SHADER_RESOURCE;
            TexDesc.Depth = 1;
            TexDesc.Format = TEXTURE_FORMAT.TEX_FORMAT_BGRA8_UNORM;
            TexDesc.MipLevels = 1;
            TexDesc.Format = TEXTURE_FORMAT.TEX_FORMAT_BGRA8_UNORM;
            TexDesc.Width = 10;
            TexDesc.Height = 10;

            fixed (UInt32* pPhysDesc = physDesc)
            {
                var Level0Data = new TextureSubResData { pData = new IntPtr(pPhysDesc), Stride = texDim * 4 };
                var InitData = new TextureData { pSubResources = new List<TextureSubResData> { Level0Data } };

                using var physicalDescriptorMap = renderDevice.CreateTexture(TexDesc, InitData);

                pboMatBindingSprite = pbrRenderer.CreateMaterialSRB(
                    pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                    pLightAttribs: pbrCameraAndLight.LightAttribs,
                    physicalDescriptorMap: physicalDescriptorMap.Obj
                );
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
                aoMap: ccoTextures.AmbientOcclusionMap
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

        private unsafe void LoadPlayerSprite()
        {
            using var stream = virtualFileSystem.openStream("spritewalk/Simple_Color.png", Engine.Resources.FileMode.Open);
            using var image = FreeImageBitmap.FromStream(stream);
            var materials = new Dictionary<uint, (String, String)>()
            {
                { 0xff6a0e91, ( "cc0Textures/Fabric048_1K", "jpg" ) }, //Shirt (purple)
                { 0xffbf1b00, ( "cc0Textures/Fabric045_1K", "jpg" ) }, //Pants (red)
                //{ 0xfff0b878, ( "cc0Textures/Carpet008_1K", "jpg" ) }, //Skin
                { 0xff492515, ( "cc0Textures/Carpet008_1K", "jpg" ) }, //Hair (brown)
                { 0xff0002bf, ( "cc0Textures/Leather026_1K", "jpg" ) }, //Shoes (blue)
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

            pboMatBindingSprite = pbrRenderer.CreateMaterialSRB(
                pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                pLightAttribs: pbrCameraAndLight.LightAttribs,
                baseColorMap: colorTexture?.Obj,
                normalMap: normalTexture?.Obj,
                physicalDescriptorMap: physicalTexture?.Obj,
                aoMap: aoTexture?.Obj
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
                aoMap: aoTexture?.Obj
            );
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

            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            var PreTransform = swapChain.GetDesc_PreTransform;
            immediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            // Clear the back buffer
            immediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            // Set Camera
            var preTransform = CameraHelpers.GetSurfacePretransformMatrix(new Vector3(0, 0, 1), PreTransform);
            var cameraProj = CameraHelpers.GetAdjustedProjectionMatrix(YFov, ZNear, ZFar, window.WindowWidth, window.WindowHeight, PreTransform);
            pbrCameraAndLight.SetCameraPosition(cameraControls.Position, cameraControls.Orientation, ref preTransform, ref cameraProj);

            //Set Light
            {
                var rotAmount = clock.CurrentTimeMicro * Clock.MicroToSeconds % (2 * (float)Math.PI);
                var rot = new Quaternion(0f, rotAmount, 0f);
                lightDirection = rot.toRotationMatrix4x4() * Vector3.Down;
                if (lightDirection.y < 0)
                {
                    //This is the right math for light facing down, but normals on textures will be lit from below, which is wrong
                    lightIntensity = -3 * lightDirection.y;
                    pbrRenderAttribs.AverageLogLum = 0.3f;
                }
                else
                {
                    pbrRenderAttribs.AverageLogLum = Math.Max(lightDirection.y * 4f, 0.3f);
                    lightIntensity = 0;
                }
                pbrCameraAndLight.SetLight(ref lightDirection, ref lightColor, lightIntensity);
            }

            //Draw Scene
            pbrRenderer.Begin(immediateContext);
            {
                //Background cubes
                pbrRenderAttribs.AlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE;

                DrawBrick(new Vector3(-1, 0, 1), Quaternion.Identity);
                DrawBrick(new Vector3(-1, 0, 2), Quaternion.Identity);
                DrawBrick(new Vector3(-1, 0, 3), Quaternion.Identity);
                DrawBrick(new Vector3(0, 0, 3), Quaternion.Identity);
                DrawBrick(new Vector3(1, 0, 3), Quaternion.Identity);
                DrawBrick(new Vector3(1, 0, 2), Quaternion.Identity);
                DrawBrick(new Vector3(1, 0, 1), Quaternion.Identity);
            }

            {
                //Floor
                var trans = new Vector3(0, -1, 0);
                var rot = Quaternion.shortestArcQuat(Vector3.Forward, Vector3.Down);// Quaternion.Identity;
                var scale = new Vector3(10, 10, 10);
                pbrRenderAttribs.AlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE;
                pbrRenderer.Render(immediateContext, pboMatBindingFloor.Obj, plane.VertexBuffer, plane.SkinVertexBuffer, plane.IndexBuffer, plane.NumIndices, ref trans, ref rot, ref scale, pbrRenderAttribs);
            }

            {
                //Tiny Dino
                var trans = new Vector3(-4, 0, -3);
                var rot = Quaternion.Identity;
                var scale = new Vector3(1.466666666666667f, 1, 1);
                pbrRenderAttribs.AlphaMode = PbrAlphaMode.ALPHA_MODE_MASK;
                pbrRenderer.Render(immediateContext, pboMatBindingTinyDinoSprite.Obj, plane.VertexBuffer, plane.SkinVertexBuffer, plane.IndexBuffer, plane.NumIndices, ref trans, ref rot, ref scale, pbrRenderAttribs);
            }

            {
                //Player
                var trans = new Vector3(0, 0.291666666666667f, 0);
                var rot = Quaternion.Identity;
                var scale = new Vector3(1, 1.291666666666667f, 1);
                pbrRenderAttribs.AlphaMode = PbrAlphaMode.ALPHA_MODE_MASK;
                pbrRenderer.Render(immediateContext, pboMatBindingSprite.Obj, plane.VertexBuffer, plane.SkinVertexBuffer, plane.IndexBuffer, plane.NumIndices, ref trans, ref rot, ref scale, pbrRenderAttribs);
            }
            this.swapChain.Present(1);
        }

        private void DrawBrick(Vector3 trans, Quaternion rot)
        {
            trans *= 2;
            pbrRenderer.Render(immediateContext, pboMatBindingSceneObject.Obj, shape.VertexBuffer, shape.SkinVertexBuffer, shape.IndexBuffer, shape.NumIndices, ref trans, ref rot, pbrRenderAttribs);
        }
    }
}
