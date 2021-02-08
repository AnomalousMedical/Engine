using Anomalous.OSPlatform;
using DiligentEngine;
using Engine;
using Engine.Platform;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;
using Float32 = System.Single;
using Uint16 = System.UInt16;
using float3 = Engine.Vector3;
using float4 = Engine.Vector4;
using float4x4 = Engine.Matrix4x4;
using System.IO;
using Tutorial_99_Pbo.Shapes;

namespace Tutorial_99_Pbo
{
    class PboUpdateListener : UpdateListener, IDisposable
    {
        private readonly GraphicsEngine graphicsEngine;
        private readonly NativeOSWindow window;
        private readonly ShaderLoader shaderLoader;
        private readonly Plane shape;
        private bool m_bUseResourceCache = false; //This is just here, not everything is implemented for it

        private ISwapChain m_pSwapChain;
        private IRenderDevice m_pDevice;
        private IDeviceContext m_pImmediateContext;

        private GLTF_PBR_Renderer m_GLTFRenderer;
        AutoPtr<IBuffer> m_CameraAttribsCB;
        AutoPtr<IBuffer> m_LightAttribsCB;
        AutoPtr<IBuffer> m_EnvMapRenderAttribsCB;
        AutoPtr<ITextureView> m_EnvironmentMapSRV;
        AutoPtr<IShaderResourceBinding> pboMatBinding;

        public unsafe PboUpdateListener(GraphicsEngine graphicsEngine, NativeOSWindow window, ShaderLoader shaderLoader, Plane shape)
        {
            this.graphicsEngine = graphicsEngine;
            this.m_pSwapChain = graphicsEngine.SwapChain;
            this.m_pDevice = graphicsEngine.RenderDevice;
            this.m_pImmediateContext = graphicsEngine.ImmediateContext;
            this.window = window;
            this.shaderLoader = shaderLoader;
            this.shape = shape;
            Initialize();
        }

        public void Dispose()
        {
            pboMatBinding.Dispose();
            m_CameraAttribsCB.Dispose();
            m_LightAttribsCB.Dispose();
            m_EnvMapRenderAttribsCB.Dispose();
            m_GLTFRenderer.Dispose();
            m_EnvironmentMapSRV.Dispose();
        }

        unsafe void Initialize()
        {
            AutoPtr<ITexture> EnvironmentMap = null;
            try
            {
                var bytes = File.ReadAllBytes("textures/papermill.ktx");
                //Create environment map texture
                var loadInfo = new TextureLoadInfo()
                {
                    Name = "Environment map",
                };
                fixed (byte* data = bytes)
                {
                    EnvironmentMap = KtxLoader.CreateTextureFromKTX(new IntPtr(data), new UIntPtr((uint)bytes.Length), loadInfo, m_pDevice);
                }
                m_EnvironmentMapSRV = new AutoPtr<ITextureView>(EnvironmentMap.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));

                var BackBufferFmt = m_pSwapChain.GetDesc_ColorBufferFormat;
                var DepthBufferFmt = m_pSwapChain.GetDesc_DepthBufferFormat;

                var RendererCI = new GLTF_PBR_Renderer.CreateInfo();
                RendererCI.RTVFmt = BackBufferFmt;
                RendererCI.DSVFmt = DepthBufferFmt;
                RendererCI.AllowDebugView = true;
                RendererCI.UseIBL = true;
                RendererCI.FrontCCW = true;
                RendererCI.UseTextureAtals = m_bUseResourceCache;
                m_GLTFRenderer = new GLTF_PBR_Renderer(m_pDevice, m_pImmediateContext, RendererCI, shaderLoader);

                unsafe{
                    BufferDesc CBDesc = new BufferDesc();
                    CBDesc.Name = "Camera attribs buffer";
                    CBDesc.uiSizeInBytes = (uint)sizeof(CameraAttribs);
                    CBDesc.Usage = USAGE.USAGE_DYNAMIC;
                    CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                    CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;

                    m_CameraAttribsCB = m_pDevice.CreateBuffer(CBDesc);
                }

                {
                    BufferDesc CBDesc = new BufferDesc();
                    CBDesc.Name = "Light attribs buffer";
                    CBDesc.uiSizeInBytes = (uint)sizeof(LightAttribs);
                    CBDesc.Usage = USAGE.USAGE_DYNAMIC;
                    CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                    CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;

                    m_LightAttribsCB = m_pDevice.CreateBuffer(CBDesc);
                }

                {
                    BufferDesc CBDesc = new BufferDesc();
                    CBDesc.Name = "Env map render attribs buffer";
                    CBDesc.uiSizeInBytes = (uint)sizeof(EnvMapRenderAttribs);
                    CBDesc.Usage = USAGE.USAGE_DYNAMIC;
                    CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                    CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;

                    m_EnvMapRenderAttribsCB = m_pDevice.CreateBuffer(CBDesc);
                }

                var Barriers = new List<StateTransitionDesc>
                {
                    new StateTransitionDesc{pResource = m_CameraAttribsCB.Obj,        OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_CONSTANT_BUFFER, UpdateResourceState = true},
                    new StateTransitionDesc{pResource = m_LightAttribsCB.Obj,         OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_CONSTANT_BUFFER, UpdateResourceState = true},
                    new StateTransitionDesc{pResource = m_EnvMapRenderAttribsCB.Obj,  OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_CONSTANT_BUFFER, UpdateResourceState = true},
                    new StateTransitionDesc{pResource = EnvironmentMap.Obj,           OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_SHADER_RESOURCE, UpdateResourceState = true}
                };
                m_pImmediateContext.TransitionResourceStates(Barriers);

                m_GLTFRenderer.PrecomputeCubemaps(m_pDevice, m_pImmediateContext, m_EnvironmentMapSRV.Obj, shaderLoader);

                //These probably don't matter
                //CreateEnvMapPSO();

                //CreateBoundBoxPSO(BackBufferFmt, DepthBufferFmt);

                //m_LightDirection = normalize(float3(0.5f, -0.6f, -0.2f));

                //if (m_bUseResourceCache)
                //    CreateGLTFResourceCache();

                //LoadModel(GLTFModels[m_SelectedModel].second);
                pboMatBinding = m_GLTFRenderer.CreateMaterialSRB(
                    pCameraAttribs: m_CameraAttribsCB.Obj,
                    pLightAttribs: m_LightAttribsCB.Obj
                    );
            }
            finally
            {
                EnvironmentMap?.Dispose();
            }
        }

        public void exceededMaxDelta()
        {

        }

        public void loopStarting()
        {

        }

        static readonly float PI_F = (float)Math.PI;

        Matrix4x4 GetAdjustedProjectionMatrix(float FOV, float NearPlane, float FarPlane, float Width, float Height, SURFACE_TRANSFORM PreTransform = SURFACE_TRANSFORM.SURFACE_TRANSFORM_IDENTITY)
        {
            if (Height == 0.0f)
            {
                Height = 1.0f;
            }

            float AspectRatio = Width / Height;
            float XScale, YScale;
            if (PreTransform == SURFACE_TRANSFORM.SURFACE_TRANSFORM_ROTATE_90 ||
                PreTransform == SURFACE_TRANSFORM.SURFACE_TRANSFORM_ROTATE_270 ||
                PreTransform == SURFACE_TRANSFORM.SURFACE_TRANSFORM_HORIZONTAL_MIRROR_ROTATE_90 ||
                PreTransform == SURFACE_TRANSFORM.SURFACE_TRANSFORM_HORIZONTAL_MIRROR_ROTATE_270)
            {
                // When the screen is rotated, vertical FOV becomes horizontal FOV
                XScale = 1f / (float)Math.Tan(FOV / 2f);
                // Aspect ratio is inversed
                YScale = XScale * AspectRatio;
            }
            else
            {
                YScale = 1f / (float)Math.Tan(FOV / 2f);
                XScale = YScale / AspectRatio;
            }

            Matrix4x4 Proj = new Matrix4x4();
            Proj.m00 = XScale;
            Proj.m11 = YScale;
            Proj.SetNearFarClipPlanes(NearPlane, FarPlane, false);// genericEngineFactory.RenderDevice.GetDeviceCaps().IsGLDevice());
            return Proj;
        }

        public unsafe void sendUpdate(Clock clock)
        {
            var pRTV = m_pSwapChain.GetCurrentBackBufferRTV();
            var pDSV = m_pSwapChain.GetDepthBufferDSV();
            m_pImmediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            // Clear the back buffer
            var ClearColor = new Engine.Color(0.032f, 0.032f, 0.032f, 1.0f);
            m_pImmediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            m_pImmediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            float YFov = PI_F / 4.0f;
            float ZNear = 0.1f;
            float ZFar = 100f;

            float4x4 CameraView;
            //if (m_CameraId == 0)
            //{
            //CameraView = m_CameraRotation.ToMatrix() * float4x4.Translation(0.0f, 0.0f, m_CameraDist);
            CameraView = float4x4.Identity * float4x4.Translation(0.0f, 0.0f, 9.0f);

            //m_RenderParams.ModelTransform = m_ModelRotation.ToMatrix();
            //}
            //else
            //{
            //const auto* pCamera = m_Cameras[m_CameraId - 1];

            //// GLTF camera is defined such that the local +X axis is to the right,
            //// the lens looks towards the local -Z axis, and the top of the camera
            //// is aligned with the local +Y axis.
            //// https://github.com/KhronosGroup/glTF/tree/master/specification/2.0#cameras
            //// We need to inverse the Z axis as our camera looks towards +Z.
            //float4x4 InvZAxis = float4x4.Identity;
            //InvZAxis.m22 = -1;

            //CameraView = pCamera->matrix.Inverse() * InvZAxis;
            //YFov = pCamera->Perspective.YFov;
            //ZNear = pCamera->Perspective.ZNear;
            //ZFar = pCamera->Perspective.ZFar;

            //m_RenderParams.ModelTransform = float4x4::Identity();
            //}

            // Apply pretransform matrix that rotates the scene according the surface orientation
            //Skip for now
            //CameraView *= GetSurfacePretransformMatrix(float3{0, 0, 1});

            float4x4 CameraWorld = CameraView.inverse();

            // Get projection matrix adjusted to the current screen orientation
            var CameraProj = GetAdjustedProjectionMatrix(YFov, ZNear, ZFar, window.WindowWidth, window.WindowHeight);
            var CameraViewProj = CameraView * CameraProj;

            var CameraWorldPos = CameraWorld.GetTranslation();

            unsafe
            {
                IntPtr data = m_pImmediateContext.MapBuffer(m_CameraAttribsCB.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);

                var CamAttribs = (CameraAttribs*)data.ToPointer();// (m_pImmediateContext, m_CameraAttribsCB, MAP_WRITE, MAP_FLAG_DISCARD);
                CamAttribs->mProjT = CameraProj.Transpose();
                CamAttribs->mViewProjT = CameraViewProj.Transpose();
                CamAttribs->mViewProjInvT = CameraViewProj.inverse().Transpose();
                CamAttribs->f4Position = new Vector4(CameraWorldPos.x, CameraWorldPos.y, CameraWorldPos.z, 1);

                m_pImmediateContext.UnmapBuffer(m_CameraAttribsCB.Obj, MAP_TYPE.MAP_WRITE);
            }

            {
                float3 m_LightDirection = new float3(0.5f, -0.6f, -0.2f);
                float4 m_LightColor = new float4(1, 1, 1, 1);
                float m_LightIntensity = 3.0f;

                IntPtr data = m_pImmediateContext.MapBuffer(m_LightAttribsCB.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);

                var lightAttribs = (LightAttribs*)data.ToPointer();// (m_pImmediateContext, m_LightAttribsCB, MAP_WRITE, MAP_FLAG_DISCARD);
                lightAttribs->f4Direction = m_LightDirection.ToVector4();
                lightAttribs->f4Intensity = m_LightColor * m_LightIntensity;

                m_pImmediateContext.UnmapBuffer(m_LightAttribsCB.Obj, MAP_TYPE.MAP_WRITE);
            }

            //if (m_bUseResourceCache)
            //{
            //    m_GLTFRenderer->Begin(m_pDevice, m_pImmediateContext, m_CacheUseInfo, m_CacheBindings, m_CameraAttribsCB, m_LightAttribsCB);
            //    m_GLTFRenderer->Render(m_pImmediateContext, *m_Model, m_RenderParams, nullptr, &m_CacheBindings);
            //}
            //else
            //{
            m_GLTFRenderer.Begin(m_pImmediateContext);
            m_GLTFRenderer.Render(m_pImmediateContext, pboMatBinding.Obj, shape.VertexBuffer, shape.SkinVertexBuffer, shape.IndexBuffer, shape.NumIndices, ALPHA_MODE.ALPHA_MODE_OPAQUE);//, *m_Model, m_RenderParams, &m_ModelResourceBindings); //m_ModelResourceBindings aka the result of AutoPtr<IShaderResourceBinding> CreateMaterialSRB
            //}

            this.m_pSwapChain.Present(1);
        }
    }
}
