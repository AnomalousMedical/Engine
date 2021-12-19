using DiligentEngine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT
{
    class RTImageBlitter : IDisposable
    {
        const TEXTURE_FORMAT ColorBufferFormat = TEXTURE_FORMAT.TEX_FORMAT_RGBA8_UNORM;

        private readonly GraphicsEngine graphicsEngine;
        private readonly OSWindow window;

        AutoPtr<ITexture> colorRT;
        AutoPtr<IPipelineState> imageBlitPSO;
        AutoPtr<IShaderResourceBinding> imageBlitSRB;

        public RTImageBlitter(ShaderLoader<RTShaders> shaderLoader, GraphicsEngine graphicsEngine, OSWindow window)
        {
            this.graphicsEngine = graphicsEngine;
            this.window = window;

            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pSwapChain = graphicsEngine.SwapChain;

            // Create graphics pipeline to blit render target into swapchain image.

            GraphicsPipelineStateCreateInfo PSOCreateInfo = new GraphicsPipelineStateCreateInfo();

            PSOCreateInfo.PSODesc.Name = "Image blit PSO";
            PSOCreateInfo.PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_GRAPHICS;

            PSOCreateInfo.GraphicsPipeline.NumRenderTargets = 1;
            PSOCreateInfo.GraphicsPipeline.RTVFormats_0 = m_pSwapChain.GetDesc_ColorBufferFormat;
            PSOCreateInfo.GraphicsPipeline.PrimitiveTopology = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_STRIP;
            PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE.CULL_MODE_NONE;
            PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthEnable = false;

            ShaderCreateInfo ShaderCI = new ShaderCreateInfo();
            ShaderCI.UseCombinedTextureSamplers = true;
            ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;
            ShaderCI.ShaderCompiler = SHADER_COMPILER.SHADER_COMPILER_DXC;

            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_VERTEX;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Image blit VS";
            ShaderCI.Source = shaderLoader.LoadShader("assets/ImageBlit.vsh");
            using var pVS = m_pDevice.CreateShader(ShaderCI);
            //VERIFY_EXPR(pVS != nullptr);

            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_PIXEL;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Image blit PS";
            ShaderCI.Source = shaderLoader.LoadShader("assets/ImageBlit.psh");
            using var pPS = m_pDevice.CreateShader(ShaderCI);
            //VERIFY_EXPR(pPS != nullptr);

            PSOCreateInfo.pVS = pVS.Obj;
            PSOCreateInfo.pPS = pPS.Obj;

            var SamLinearClampDesc = new SamplerDesc
            {
                MinFilter = FILTER_TYPE.FILTER_TYPE_LINEAR,
                MagFilter = FILTER_TYPE.FILTER_TYPE_LINEAR,
                MipFilter = FILTER_TYPE.FILTER_TYPE_LINEAR,
                AddressU = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_CLAMP,
                AddressV = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_CLAMP,
                AddressW = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_CLAMP
            };

            var ImmutableSamplers = new List<ImmutableSamplerDesc>
            {
                new ImmutableSamplerDesc{ ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, SamplerOrTextureName = "g_Texture", Desc = SamLinearClampDesc }
            };

            PSOCreateInfo.PSODesc.ResourceLayout.ImmutableSamplers = ImmutableSamplers;
            PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC;

            imageBlitPSO = m_pDevice.CreateGraphicsPipelineState(PSOCreateInfo);
            //VERIFY_EXPR(m_pImageBlitPSO != nullptr);

            imageBlitSRB = imageBlitPSO.Obj.CreateShaderResourceBinding(true);
            
            //VERIFY_EXPR(m_pImageBlitSRB != nullptr);

            WindowResize((uint)window.WindowWidth, (uint)window.WindowHeight);
            window.Resized += Window_Resized;
        }

        public void Dispose()
        {
            window.Resized -= Window_Resized;
            colorRT?.Dispose();
            imageBlitPSO.Dispose();
            imageBlitSRB.Dispose();
        }

        public void Blit()
        {
            var swapChain = graphicsEngine.SwapChain;
            var immediateContext = graphicsEngine.ImmediateContext;

            imageBlitSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "g_Texture").Set(colorRT.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));

            var pRTV = swapChain.GetCurrentBackBufferRTV();
            immediateContext.SetRenderTarget(pRTV, null, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            immediateContext.SetPipelineState(imageBlitPSO.Obj);
            immediateContext.CommitShaderResources(imageBlitSRB.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            var Attribs = new DrawAttribs();
            Attribs.NumVertices = 4;
            immediateContext.Draw(Attribs);
        }

        private void Window_Resized(OSWindow window)
        {
            WindowResize((uint)window.WindowWidth, (uint)window.WindowHeight);
        }

        public IDeviceObject TextureView => colorRT.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_UNORDERED_ACCESS);

        public uint Width => colorRT.Obj.GetDesc_Width;

        public uint Height => colorRT.Obj.GetDesc_Height;

        public void WindowResize(UInt32 Width, UInt32 Height)
        {
            var m_pDevice = graphicsEngine.RenderDevice;
            // Update projection matrix.
            //float AspectRatio = (float)Width / (float)Height;
            //m_Camera.SetProjAttribs(m_Constants.ClipPlanes.x, m_Constants.ClipPlanes.y, AspectRatio, PI_F / 4.f,
            //                        m_pSwapChain->GetDesc().PreTransform, m_pDevice->GetDeviceCaps().IsGLDevice());

            // Check if the image needs to be recreated.
            if (colorRT != null &&
                colorRT.Obj.GetDesc_Width == Width &&
                colorRT.Obj.GetDesc_Height == Height)
            {
                return;
            }

            if (Width == 0 || Height == 0)
            {
                return;
            }

            colorRT?.Dispose();
            colorRT = null;

            // Create window-size color image.
            var RTDesc = new TextureDesc();
            RTDesc.Name = "Color buffer";
            RTDesc.Type = RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D;
            RTDesc.Width = Width;
            RTDesc.Height = Height;
            RTDesc.BindFlags = BIND_FLAGS.BIND_UNORDERED_ACCESS | BIND_FLAGS.BIND_SHADER_RESOURCE;
            RTDesc.ClearValue.Format = ColorBufferFormat;
            RTDesc.Format = ColorBufferFormat;

            colorRT = m_pDevice.CreateTexture(RTDesc, null);
        }
    }
}
