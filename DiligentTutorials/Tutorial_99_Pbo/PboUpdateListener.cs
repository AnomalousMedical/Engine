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

namespace Tutorial_99_Pbo
{
    class PboUpdateListener : UpdateListener, IDisposable
    {
        private readonly GraphicsEngine graphicsEngine;
        private readonly NativeOSWindow window;
        private readonly ShaderLoader shaderLoader;
        private bool m_bUseResourceCache = false;

        private ISwapChain m_pSwapChain;
        private IRenderDevice m_pDevice;
        private IDeviceContext m_pImmediateContext;

        private GLTF_PBR_Renderer m_GLTFRenderer;
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

        public unsafe void sendUpdate(Clock clock)
        {

        }
    }
}
