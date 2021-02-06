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
using System.IO;

namespace GLTFViewer
{
    class PboUpdateListener : UpdateListener, IDisposable
    {
        private readonly GraphicsEngine graphicsEngine;
        private readonly NativeOSWindow window;
        private bool m_bUseResourceCache = false;

        private ISwapChain m_pSwapChain;
        private IRenderDevice m_pDevice;

        public unsafe PboUpdateListener(GraphicsEngine graphicsEngine, NativeOSWindow window)
        {
            this.graphicsEngine = graphicsEngine;
            this.m_pSwapChain = graphicsEngine.SwapChain;
            this.m_pDevice = graphicsEngine.RenderDevice;
            this.window = window;

            Initialize();
        }

        public void Dispose()
        {

        }

        unsafe void Initialize()
        {
            //Seems like anything could be loaded for this
            //RefCntAutoPtr<ITexture> EnvironmentMap;
            //CreateTextureFromFile("textures/papermill.ktx", TextureLoadInfo{ "Environment map"}, m_pDevice, &EnvironmentMap);
            //m_EnvironmentMapSRV = EnvironmentMap->GetDefaultView(TEXTURE_VIEW_SHADER_RESOURCE);

            var BackBufferFmt = m_pSwapChain.GetDesc_ColorBufferFormat;
            var DepthBufferFmt = m_pSwapChain.GetDesc_DepthBufferFormat;

            CreateInfo RendererCI = new CreateInfo();
            RendererCI.RTVFmt = BackBufferFmt;
            RendererCI.DSVFmt = DepthBufferFmt;
            RendererCI.AllowDebugView = true;
            RendererCI.UseIBL = true;
            RendererCI.FrontCCW = true;
            RendererCI.UseTextureAtals = m_bUseResourceCache;
            m_GLTFRenderer.reset(new GLTF_PBR_Renderer(m_pDevice, m_pImmediateContext, RendererCI));

            //More to figure out here, these need to be structs that can write native memory
            //BufferDesc CBDesc = new BufferDesc();
            //CBDesc.Name = "Camera attribs buffer";
            //CBDesc.uiSizeInBytes = (uint)sizeof(CameraAttribs);
            //CBDesc.Usage = USAGE.USAGE_DYNAMIC;
            //CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
            //CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;
            //m_CameraAttribsCB = m_pDevice.CreateBuffer(CBDesc);

            //CBDesc.Name = "Light attribs buffer";
            //CBDesc.uiSizeInBytes = (uint)sizeof(LightAttribs);
            //CBDesc.Usage = USAGE.USAGE_DYNAMIC;
            //CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
            //CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;
            //m_LightAttribsCB = m_pDevice.CreateBuffer(CBDesc);

            //CBDesc.Name = "Env map render attribs buffer";
            //CBDesc.uiSizeInBytes = (uint)sizeof(EnvMapRenderAttribs);
            //CBDesc.Usage = USAGE.USAGE_DYNAMIC;
            //CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
            //CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;
            //m_EnvMapRenderAttribsCB = m_pDevice.CreateBuffer(CBDesc);

            //var Barriers = new List<StateTransitionDesc>
            //{
            //    {m_CameraAttribsCB,        RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_CONSTANT_BUFFER, true},
            //    {m_LightAttribsCB,         RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_CONSTANT_BUFFER, true},
            //    {m_EnvMapRenderAttribsCB,  RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_CONSTANT_BUFFER, true},
            //    {EnvironmentMap,           RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_SHADER_RESOURCE, true}
            //};
            //// clang-format on
            //m_pImmediateContext->TransitionResourceStates(_countof(Barriers), Barriers);

            //m_GLTFRenderer->PrecomputeCubemaps(m_pDevice, m_pImmediateContext, m_EnvironmentMapSRV);
        }

        public void exceededMaxDelta()
        {

        }

        public void loopStarting()
        {

        }

        public unsafe void sendUpdate(Clock clock)
        {

        }
    }
}
