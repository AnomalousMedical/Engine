using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngineTest
{
    class SimpleUpdateListener : UpdateListener, IDisposable
    {
        private readonly GraphicsEngine graphicsEngine;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext immediateContext;
        private readonly IPipelineState pipelineState;

        const String VSSource = @"
struct PSInput
{
    float4 Pos   : SV_POSITION; 
    float3 Color : COLOR; 
};

void main(in uint VertId : SV_VertexID,
            out PSInput PSIn)
{
    float4 Pos[3];
    Pos[0] = float4(-0.5, -0.5, 0.0, 1.0);
    Pos[1] = float4(0.0, +0.5, 0.0, 1.0);
    Pos[2] = float4(+0.5, -0.5, 0.0, 1.0);

    float3 Col[3];
    Col[0] = float3(1.0, 0.0, 0.0); // red
    Col[1] = float3(0.0, 1.0, 0.0); // green
    Col[2] = float3(0.0, 0.0, 1.0); // blue

    PSIn.Pos = Pos[VertId];
    PSIn.Color = Col[VertId];
}";

        const String PSSource = @"
struct PSInput 
{ 
    float4 Pos   : SV_POSITION; 
    float3 Color : COLOR; 
};

struct PSOutput
{ 
    float4 Color : SV_TARGET; 
};

void main(in  PSInput  PSIn,
          out PSOutput PSOut)
{
    PSOut.Color = float4(PSIn.Color.rgb, 1.0);
}";

        public SimpleUpdateListener(GraphicsEngine graphicsEngine)
        {
            this.graphicsEngine = graphicsEngine;
            this.swapChain = graphicsEngine.SwapChain;
            this.immediateContext = graphicsEngine.ImmediateContext;

            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pSwapChain = graphicsEngine.SwapChain;

            var ShaderCI = new ShaderCreateInfo();
            ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;
            // OpenGL backend requires emulated combined HLSL texture samplers (g_Texture + g_Texture_sampler combination)
            ShaderCI.UseCombinedTextureSamplers = true;
            // Create a vertex shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_VERTEX;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Triangle vertex shader";
            ShaderCI.Source = VSSource;
            using var vertexShader = this.graphicsEngine.RenderDevice.CreateShader(ShaderCI);

            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_PIXEL;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Triangle pixel shader";
            ShaderCI.Source = PSSource;
            using var pixelShader = this.graphicsEngine.RenderDevice.CreateShader(ShaderCI);

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
            PSOCreateInfo.GraphicsPipeline.RTVFormats_0 = m_pSwapChain.GetDesc_ColorBufferFormat;
            // Use the depth buffer format from the swap chain
            PSOCreateInfo.GraphicsPipeline.DSVFormat = m_pSwapChain.GetDesc_DepthBufferFormat;

            // Primitive topology defines what kind of primitives will be rendered by this pipeline state
            PSOCreateInfo.GraphicsPipeline.PrimitiveTopology = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
            // No back face culling for this tutorial
            PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE.CULL_MODE_NONE;
            // Disable depth testing
            PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthEnable = false;
            // clang-format on

            PSOCreateInfo.pVS = vertexShader;
            PSOCreateInfo.pPS = pixelShader;
            this.pipelineState = graphicsEngine.RenderDevice.CreateGraphicsPipelineState(PSOCreateInfo);
        }

        public void Dispose()
        {
            pipelineState.Dispose();
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
    }
}
