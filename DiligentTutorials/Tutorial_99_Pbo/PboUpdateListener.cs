using Anomalous.OSPlatform;
using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using Tutorial_99_Pbo.Shapes;
using DiligentEngine.GltfPbr;

namespace Tutorial_99_Pbo
{
    class PboUpdateListener : UpdateListener, IDisposable
    {

        //Camera Settings
        float YFov = (float)Math.PI / 4.0f;
        float ZNear = 0.1f;
        float ZFar = 100f;

        //Clear Color
        Engine.Color ClearColor = new Engine.Color(0.032f, 0.032f, 0.032f, 1.0f);

        //Light
        Vector3 lightDirection = Vector3.Forward;// (new float3(0.5f, -0.6f, -0.2f)).normalized();
        Vector4 lightColor = new Vector4(1, 1, 1, 1);
        float lightIntensity = 3.0f;

        private readonly NativeOSWindow window;
        private readonly Cube shape;
        private readonly TextureLoader textureLoader;
        private readonly CC0TextureLoader cc0TextureLoader;
        private readonly EnvironmentMapBuilder envMapBuilder;
        private readonly IPbrCameraAndLight pbrCameraAndLight;
        private ISwapChain m_pSwapChain;
        private IRenderDevice m_pDevice;
        private IDeviceContext m_pImmediateContext;
        private PbrRenderAttribs pbrRenderAttribs = PbrRenderAttribs.CreateDefault();

        private PbrRenderer m_GLTFRenderer;
        private AutoPtr<ITextureView> m_EnvironmentMapSRV;
        private AutoPtr<IShaderResourceBinding> pboMatBinding;

        public unsafe PboUpdateListener(
            GraphicsEngine graphicsEngine,
            NativeOSWindow window,
            PbrRenderer m_GLTFRenderer,
            Cube shape,
            TextureLoader textureLoader,
            CC0TextureLoader cc0TextureLoader,
            EnvironmentMapBuilder envMapBuilder,
            IPbrCameraAndLight pbrCameraAndLight)
        {
            this.m_GLTFRenderer = m_GLTFRenderer;
            this.m_pSwapChain = graphicsEngine.SwapChain;
            this.m_pDevice = graphicsEngine.RenderDevice;
            this.m_pImmediateContext = graphicsEngine.ImmediateContext;
            this.window = window;
            this.shape = shape;
            this.textureLoader = textureLoader;
            this.cc0TextureLoader = cc0TextureLoader;
            this.envMapBuilder = envMapBuilder;
            this.pbrCameraAndLight = pbrCameraAndLight;
            Initialize();
        }

        public void Dispose()
        {
            pboMatBinding.Dispose();
            m_EnvironmentMapSRV.Dispose();
        }

        unsafe void Initialize()
        {
            m_EnvironmentMapSRV = envMapBuilder.BuildEnvMapView(m_pDevice, m_pImmediateContext, "papermill/Fixed-", "png");

            

            m_GLTFRenderer.PrecomputeCubemaps(m_pDevice, m_pImmediateContext, m_EnvironmentMapSRV.Obj);


            //Only one of these
            //Load a cc0 texture
            LoadCCoTexture();

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

                using var physicalDescriptorMap = m_pDevice.CreateTexture(TexDesc, InitData);

                pboMatBinding = m_GLTFRenderer.CreateMaterialSRB(
                    pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                    pLightAttribs: pbrCameraAndLight.LightAttribs,
                    physicalDescriptorMap: physicalDescriptorMap.Obj
                );
            }
        }

        private unsafe void LoadCCoTexture()
        {
            using var ccoTextures = cc0TextureLoader.LoadTextureSet("cc0Textures/Chainmail004_1K");
            pboMatBinding = m_GLTFRenderer.CreateMaterialSRB(
                pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                pLightAttribs: pbrCameraAndLight.LightAttribs,
                baseColorMap: ccoTextures.BaseColorMap,
                normalMap: ccoTextures.NormalMap,
                physicalDescriptorMap: ccoTextures.PhysicalDescriptorMap,
                aoMap: ccoTextures.AmbientOcclusionMap
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
            var pRTV = m_pSwapChain.GetCurrentBackBufferRTV();
            var pDSV = m_pSwapChain.GetDepthBufferDSV();
            var PreTransform = m_pSwapChain.GetDesc_PreTransform;
            m_pImmediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            // Clear the back buffer
            m_pImmediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            m_pImmediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            var camRotAmount = 0f; // (clock.CurrentTimeMicro * Clock.MicroToSeconds) / 10 % (2 * (float)Math.PI);
            Matrix4x4 CameraView = Matrix4x4.RotationX(camRotAmount) * Matrix4x4.Translation(0.0f, 0.0f, 5.0f);

            // Apply pretransform matrix that rotates the scene according the surface orientation
            CameraView *= CameraHelpers.GetSurfacePretransformMatrix(new Vector3(0, 0, 1), PreTransform);

            Matrix4x4 CameraWorld = CameraView.inverse();

            // Get projection matrix adjusted to the current screen orientation
            var CameraProj = CameraHelpers.GetAdjustedProjectionMatrix(YFov, ZNear, ZFar, window.WindowWidth, window.WindowHeight, PreTransform);
            var CameraViewProj = CameraView * CameraProj;
            var CameraWorldPos = CameraWorld.GetTranslation();

            pbrCameraAndLight.SetCamera(ref CameraProj, ref CameraViewProj, ref CameraWorldPos);
            pbrCameraAndLight.SetLight(ref lightDirection, ref lightColor, lightIntensity);

            var trans = Vector3.Zero;
            var rotAmount = (clock.CurrentTimeMicro * Clock.MicroToSeconds) / 10 % (2 * (float)Math.PI);
            var rot = new Quaternion(rotAmount, 0f, 0f);

            var CubeModelTransform = rot.toRotationMatrix4x4(trans);

            m_GLTFRenderer.Begin(m_pImmediateContext);
            m_GLTFRenderer.Render(m_pImmediateContext, pboMatBinding.Obj, shape.VertexBuffer, shape.SkinVertexBuffer, shape.IndexBuffer, shape.NumIndices, PbrAlphaMode.ALPHA_MODE_OPAQUE, ref CubeModelTransform, pbrRenderAttribs);

            this.m_pSwapChain.Present(1);
        }
    }
}
