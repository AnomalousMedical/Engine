using Anomalous.OSPlatform;
using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
using FreeImageAPI;

namespace Tutorial_99_Pbo
{
    class PboUpdateListener : UpdateListener, IDisposable
    {

        //Camera Settings
        float YFov = (float)Math.PI / 4.0f;
        float ZNear = 0.1f;
        float ZFar = 100f;

        float camRotSpeed = 0f;

        //Cube
        float yawFactor = 1.0f;
        float pitchFactor = 0.0f;
        float rollFactor = 0.0f;
        float cubeRotSpeed = 4.0f;

        //Clear Color
        Engine.Color ClearColor = new Engine.Color(0.032f, 0.032f, 0.032f, 1.0f);

        //Light
        Vector3 lightDirection = Vector3.Forward;
        Vector4 lightColor = new Vector4(1, 1, 1, 1);
        float lightIntensity = 3;

        private readonly NativeOSWindow window;
        private readonly DoubleSizeCube shape;
        private readonly TextureLoader textureLoader;
        private readonly CC0TextureLoader cc0TextureLoader;
        private readonly EnvironmentMapBuilder envMapBuilder;
        private readonly IPbrCameraAndLight pbrCameraAndLight;
        private readonly ICC0MaterialTextureBuilder cC0MaterialTextureBuilder;
        private readonly VirtualFileSystem virtualFileSystem;
        private ISwapChain swapChain;
        private IRenderDevice renderDevice;
        private IDeviceContext immediateContext;
        private PbrRenderAttribs pbrRenderAttribs = PbrRenderAttribs.CreateDefault();

        private PbrRenderer pbrRenderer;
        private AutoPtr<ITextureView> environmentMapSRV;
        private AutoPtr<IShaderResourceBinding> pboMatBinding;

        public unsafe PboUpdateListener(
            GraphicsEngine graphicsEngine,
            NativeOSWindow window,
            PbrRenderer m_GLTFRenderer,
            DoubleSizeCube shape,
            TextureLoader textureLoader,
            CC0TextureLoader cc0TextureLoader,
            EnvironmentMapBuilder envMapBuilder,
            IPbrCameraAndLight pbrCameraAndLight,
            ICC0MaterialTextureBuilder cC0MaterialTextureBuilder,
            VirtualFileSystem virtualFileSystem)
        {
            this.pbrRenderer = m_GLTFRenderer;
            this.swapChain = graphicsEngine.SwapChain;
            this.renderDevice = graphicsEngine.RenderDevice;
            this.immediateContext = graphicsEngine.ImmediateContext;
            this.window = window;
            this.shape = shape;
            this.textureLoader = textureLoader;
            this.cc0TextureLoader = cc0TextureLoader;
            this.envMapBuilder = envMapBuilder;
            this.pbrCameraAndLight = pbrCameraAndLight;
            this.cC0MaterialTextureBuilder = cC0MaterialTextureBuilder;
            this.virtualFileSystem = virtualFileSystem;
            Initialize();
        }

        public void Dispose()
        {
            pboMatBinding.Dispose();
            environmentMapSRV.Dispose();
        }

        unsafe void Initialize()
        {
            environmentMapSRV = envMapBuilder.BuildEnvMapView(renderDevice, immediateContext, "papermill/Fixed-", "png");

            

            pbrRenderer.PrecomputeCubemaps(renderDevice, immediateContext, environmentMapSRV.Obj);


            //Only one of these
            //Load a cc0 texture
            LoadCCoTexture();

            //Load a multi texture material
            //LoadCCoMaterial();

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

                pboMatBinding = pbrRenderer.CreateMaterialSRB(
                    pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                    pLightAttribs: pbrCameraAndLight.LightAttribs,
                    physicalDescriptorMap: physicalDescriptorMap.Obj
                );
            }
        }

        private unsafe void LoadCCoTexture()
        {
            using var ccoTextures = cc0TextureLoader.LoadTextureSet("cc0Textures/Chainmail004_1K");
            pboMatBinding = pbrRenderer.CreateMaterialSRB(
                pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                pLightAttribs: pbrCameraAndLight.LightAttribs,
                baseColorMap: ccoTextures.BaseColorMap,
                normalMap: ccoTextures.NormalMap,
                physicalDescriptorMap: ccoTextures.PhysicalDescriptorMap,
                aoMap: ccoTextures.AmbientOcclusionMap
            );
        }

        private unsafe void LoadCCoMaterial()
        {
            using var stream = virtualFileSystem.openStream("spritewalk/Simple_Color.png", Engine.Resources.FileMode.Open);
            using var image = FreeImageBitmap.FromStream(stream);
            var materials = new Dictionary<uint, (String, String)>()
            {
                { 0xff6a0e91, ( "cc0Textures/Fabric048_1K", "jpg" ) }, //Shirt (purple)
                { 0xffbf1b00, ( "cc0Textures/Fabric045_1K", "jpg" ) }, //Pants (red)
                //{ 0xfff0b878, ( "cc0Textures/Carpet008_1K", "jpg" ) }, //Skin
                { 0xff492515, ( "cc0Textures/Carpet008_1K", "jpg" ) }, //Hair (brown)
                { 0xff0002bf, ( "cc0Textures/Leather026_1K", "jpg" ) } //Shoes (blue)
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

            pboMatBinding = pbrRenderer.CreateMaterialSRB(
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
            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            var PreTransform = swapChain.GetDesc_PreTransform;
            immediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            // Clear the back buffer
            immediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            var camRotAmount = camRotSpeed < 0.00001f ? 0f : (clock.CurrentTimeMicro * Clock.MicroToSeconds) / camRotSpeed % (2 * (float)Math.PI);
            var CameraView = Matrix4x4.RotationX(camRotAmount) * Matrix4x4.Translation(0.0f, 0.0f, 5.0f);

            // Apply pretransform matrix that rotates the scene according the surface orientation
            CameraView *= CameraHelpers.GetSurfacePretransformMatrix(new Vector3(0, 0, 1), PreTransform);

            var CameraWorld = CameraView.inverse();

            // Get projection matrix adjusted to the current screen orientation
            var CameraProj = CameraHelpers.GetAdjustedProjectionMatrix(YFov, ZNear, ZFar, window.WindowWidth, window.WindowHeight, PreTransform);
            var CameraViewProj = CameraView * CameraProj;
            var CameraWorldPos = CameraWorld.GetTranslation();

            pbrCameraAndLight.SetCameraMatrices(ref CameraProj, ref CameraViewProj, ref CameraWorldPos);
            pbrCameraAndLight.SetLight(ref lightDirection, ref lightColor, lightIntensity);

            var trans = new Vector3(0, 0, 0);
            var rotAmount = cubeRotSpeed < 0.0001f ? 0f : (clock.CurrentTimeMicro * Clock.MicroToSeconds) / cubeRotSpeed % (2 * (float)Math.PI);
            var rot = new Quaternion(rotAmount * yawFactor, rotAmount * pitchFactor, rotAmount * rollFactor);

            pbrRenderer.Begin(immediateContext);
            pbrRenderAttribs.AlphaMode = PbrAlphaMode.ALPHA_MODE_MASK;
            pbrRenderer.Render(immediateContext, pboMatBinding.Obj, shape.VertexBuffer, shape.SkinVertexBuffer, shape.IndexBuffer, shape.NumIndices, ref trans, ref rot, pbrRenderAttribs);

            this.swapChain.Present(1);
        }
    }
}
