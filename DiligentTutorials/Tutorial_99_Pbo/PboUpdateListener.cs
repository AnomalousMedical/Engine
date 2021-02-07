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
using float4x4 = Engine.Matrix4x4;
using System.IO;

namespace Tutorial_99_Pbo
{
    class PboUpdateListener : UpdateListener, IDisposable
    {
        private readonly GraphicsEngine graphicsEngine;
        private readonly NativeOSWindow window;
        private readonly ShaderLoader shaderLoader;
        private bool m_bUseResourceCache = false; //This is just here, not everything is implemented for it

        private ISwapChain m_pSwapChain;
        private IRenderDevice m_pDevice;
        private IDeviceContext m_pImmediateContext;

        private GLTF_PBR_Renderer m_GLTFRenderer;
        AutoPtr<IBuffer> m_CameraAttribsCB;
        AutoPtr<IBuffer> m_LightAttribsCB;
        AutoPtr<IBuffer> m_EnvMapRenderAttribsCB;
        AutoPtr<ITextureView> m_EnvironmentMapSRV;

        public unsafe PboUpdateListener(GraphicsEngine graphicsEngine, NativeOSWindow window, ShaderLoader shaderLoader)
        {
            this.graphicsEngine = graphicsEngine;
            this.m_pSwapChain = graphicsEngine.SwapChain;
            this.m_pDevice = graphicsEngine.RenderDevice;
            this.m_pImmediateContext = graphicsEngine.ImmediateContext;
            this.window = window;
            this.shaderLoader = shaderLoader;
            Initialize();
        }

        public void Dispose()
        {
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

        public unsafe void sendUpdate(Clock clock)
        {
            var pRTV = m_pSwapChain.GetCurrentBackBufferRTV();
            var pDSV = m_pSwapChain.GetDepthBufferDSV();
            // Clear the back buffer
            var ClearColor = new Engine.Color(0.032f, 0.032f, 0.032f, 1.0f);
            m_pImmediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            m_pImmediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            //float YFov  = PI_F / 4.0f;
            //float ZNear = 0.1f;
            //float ZFar  = 100f;

            //float4x4 CameraView;
            ////if (m_CameraId == 0)
            ////{
            ////CameraView = m_CameraRotation.ToMatrix() * float4x4.Translation(0.0f, 0.0f, m_CameraDist);
            //CameraView = float4x4.Identity * float4x4.Translation(0.0f, 0.0f, 9.0f);

            ////m_RenderParams.ModelTransform = m_ModelRotation.ToMatrix();
            ////}
            ////else
            ////{
            ////    const auto* pCamera = m_Cameras[m_CameraId - 1];

            ////    // GLTF camera is defined such that the local +X axis is to the right,
            ////    // the lens looks towards the local -Z axis, and the top of the camera
            ////    // is aligned with the local +Y axis.
            ////    // https://github.com/KhronosGroup/glTF/tree/master/specification/2.0#cameras
            ////    // We need to inverse the Z axis as our camera looks towards +Z.
            ////    float4x4 InvZAxis = float4x4::Identity();
            ////    InvZAxis._33 = -1;

            ////    CameraView = pCamera->matrix.Inverse() * InvZAxis;
            ////    YFov = pCamera->Perspective.YFov;
            ////    ZNear = pCamera->Perspective.ZNear;
            ////    ZFar = pCamera->Perspective.ZFar;

            ////    m_RenderParams.ModelTransform = float4x4::Identity();
            ////}

            //// Apply pretransform matrix that rotates the scene according the surface orientation
            ////Skip for now
            ////CameraView *= GetSurfacePretransformMatrix(float3{0, 0, 1});

            //float4x4 CameraWorld = CameraView.Inverse();

            //// Get projection matrix adjusted to the current screen orientation
            //const auto CameraProj     = GetAdjustedProjectionMatrix(YFov, ZNear, ZFar);
            //const auto CameraViewProj = CameraView * CameraProj;

            //float3 CameraWorldPos = float3::MakeVector(CameraWorld[3]);

            //unsafe
            //{
            //    MapHelper<CameraAttribs> CamAttribs(m_pImmediateContext, m_CameraAttribsCB, MAP_WRITE, MAP_FLAG_DISCARD);
            //    CamAttribs->mProjT        = CameraProj.Transpose();
            //    CamAttribs->mViewProjT    = CameraViewProj.Transpose();
            //    CamAttribs->mViewProjInvT = CameraViewProj.Inverse().Transpose();
            //    CamAttribs->f4Position    = float4(CameraWorldPos, 1);
            //}

            //{
            //    MapHelper<LightAttribs> lightAttribs(m_pImmediateContext, m_LightAttribsCB, MAP_WRITE, MAP_FLAG_DISCARD);
            //    lightAttribs->f4Direction = m_LightDirection;
            //    lightAttribs->f4Intensity = m_LightColor * m_LightIntensity;
            //}

            //if (m_bUseResourceCache)
            //{
            //    m_GLTFRenderer->Begin(m_pDevice, m_pImmediateContext, m_CacheUseInfo, m_CacheBindings, m_CameraAttribsCB, m_LightAttribsCB);
            //    m_GLTFRenderer->Render(m_pImmediateContext, *m_Model, m_RenderParams, nullptr, &m_CacheBindings);
            //}
            //else
            //{
            //    m_GLTFRenderer->Begin(m_pImmediateContext);
            //    m_GLTFRenderer->Render(m_pImmediateContext, *m_Model, m_RenderParams, &m_ModelResourceBindings);
            //}

            this.m_pSwapChain.Present(1);
        }
    }
}
