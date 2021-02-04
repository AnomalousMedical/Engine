using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngineCube
{
    class CubeUpdateListener : UpdateListener, IDisposable
    {
        private readonly GenericEngineFactory genericEngineFactory;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext immediateContext;

        private readonly IPipelineState pipelineState;
        private IBuffer m_VSConstants;

        public CubeUpdateListener(GenericEngineFactory genericEngineFactory)
        {
            this.genericEngineFactory = genericEngineFactory;
            this.swapChain = genericEngineFactory.SwapChain;
            this.immediateContext = genericEngineFactory.ImmediateContext;

            var m_pDevice = genericEngineFactory.RenderDevice;

            var ShaderCI = new ShaderCreateInfo();
            ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;
            // OpenGL backend requires emulated combined HLSL texture samplers (g_Texture + g_Texture_sampler combination)
            ShaderCI.UseCombinedTextureSamplers = true;
            
            // Create a vertex shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_VERTEX;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Cube VS";
            ShaderCI.Source = VSSource;
            using var pVS = this.genericEngineFactory.RenderDevice.CreateShader(ShaderCI);

            {
                BufferDesc CBDesc = new BufferDesc();
                CBDesc.Name = "VS constants CB";
                CBDesc.uiSizeInBytes = 64;// sizeof(float4x4);
                CBDesc.Usage = USAGE.USAGE_DYNAMIC;
                CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;
                m_VSConstants = m_pDevice.CreateBuffer(CBDesc);
            }

            //Create pixel shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_PIXEL;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Cube PS";
            ShaderCI.Source = PSSource;
            using var pPS = this.genericEngineFactory.RenderDevice.CreateShader(ShaderCI);

            var PSOCreateInfo = new GraphicsPipelineStateCreateInfo();

            // Pipeline state name is used by the engine to report issues.
            // It is always a good idea to give objects descriptive names.
            PSOCreateInfo.PSODesc.Name = "Simple triangle PSO";

            // This is a graphics pipeline
            PSOCreateInfo.PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_GRAPHICS;

            // clang-format off
            // This tutorial will render to a single render target
            PSOCreateInfo.GraphicsPipeline.NumRenderTargets = 1;
            // Set render target format which is the format of the swap chain's color buffer
            PSOCreateInfo.GraphicsPipeline.RTVFormats_0 = TEXTURE_FORMAT.TEX_FORMAT_BGRA8_UNORM_SRGB; //Hardcoded, should use// m_pSwapChain->GetDesc().ColorBufferFormat;
            // Use the depth buffer format from the swap chain
            PSOCreateInfo.GraphicsPipeline.DSVFormat = TEXTURE_FORMAT.TEX_FORMAT_D32_FLOAT;//Hardcoded, should use// m_pSwapChain->GetDesc().DepthBufferFormat;
            // Primitive topology defines what kind of primitives will be rendered by this pipeline state
            PSOCreateInfo.GraphicsPipeline.PrimitiveTopology = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
            // No back face culling for this tutorial
            PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE.CULL_MODE_NONE;
            // Disable depth testing
            PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthEnable = false;
            // clang-format on

            PSOCreateInfo.pVS = pVS;
            PSOCreateInfo.pPS = pPS;
            this.pipelineState = genericEngineFactory.RenderDevice.CreateGraphicsPipelineState(PSOCreateInfo);
        }

        public void Dispose()
        {
            pipelineState.Dispose();
            m_VSConstants.Dispose();
        }

        public void exceededMaxDelta()
        {

        }

        public void loopStarting()
        {

        }

        public void sendUpdate(Clock clock)
        {
            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            immediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            var color = Color.LightBlue;
            color.r = (color.r + (clock.CurrentTimeMicro % 3000000f) / 3000000f) % 1.0f;
            color.g = (color.g + (clock.CurrentTimeMicro % 6000000f) / 6000000f) % 1.0f;
            color.b = (color.b + (clock.CurrentTimeMicro % 9000000f) / 9000000f) % 1.0f;

            // Clear the back buffer
            // Let the engine perform required state transitions
            immediateContext.ClearRenderTarget(pRTV, color, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            immediateContext.SetPipelineState(this.pipelineState);

            //// Typically we should now call CommitShaderResources(), however shaders in this example don't
            //// use any resources.

            DrawAttribs drawAttrs = new DrawAttribs();
            drawAttrs.NumVertices = 3; // Render 3 vertices
            immediateContext.Draw(drawAttrs);

            this.swapChain.Present(1);
        }

        const String VSSource =
@"cbuffer Constants
{
    float4x4 g_WorldViewProj;
};

// Vertex shader takes two inputs: vertex position and color.
// By convention, Diligent Engine expects vertex shader inputs to be 
// labeled 'ATTRIBn', where n is the attribute number.
struct VSInput
{
    float3 Pos   : ATTRIB0;
    float4 Color : ATTRIB1;
};

struct PSInput 
{ 
    float4 Pos   : SV_POSITION; 
    float4 Color : COLOR0; 
};

// Note that if separate shader objects are not supported (this is only the case for old GLES3.0 devices), vertex
// shader output variable name must match exactly the name of the pixel shader input variable.
// If the variable has structure type (like in this example), the structure declarations must also be indentical.
void main(in  VSInput VSIn,
          out PSInput PSIn) 
{
    PSIn.Pos   = mul( float4(VSIn.Pos,1.0), g_WorldViewProj);
    PSIn.Color = VSIn.Color;
}";

        const String PSSource =
@"struct PSInput 
{ 
    float4 Pos   : SV_POSITION; 
    float4 Color : COLOR0; 
};

struct PSOutput
{ 
    float4 Color : SV_TARGET; 
};

// Note that if separate shader objects are not supported (this is only the case for old GLES3.0 devices), vertex
// shader output variable name must match exactly the name of the pixel shader input variable.
// If the variable has structure type (like in this example), the structure declarations must also be indentical.
void main(in  PSInput  PSIn,
          out PSOutput PSOut)
{
    PSOut.Color = PSIn.Color; 
}";
    }
}
