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
        private readonly GenericEngineFactory genericEngineFactory;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext immediateContext;
        //private readonly IPipelineState pipelineState;

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

        public SimpleUpdateListener(GenericEngineFactory genericEngineFactory)
        {
            this.genericEngineFactory = genericEngineFactory;
            this.swapChain = genericEngineFactory.SwapChain;
            this.immediateContext = genericEngineFactory.ImmediateContext;

            var ShaderCI = new ShaderCreateInfo();
            ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;
            // OpenGL backend requires emulated combined HLSL texture samplers (g_Texture + g_Texture_sampler combination)
            ShaderCI.UseCombinedTextureSamplers = true;
            // Create a vertex shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_VERTEX;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Triangle vertex shader";
            ShaderCI.Source = VSSource;
            using var vertexShader = this.genericEngineFactory.RenderDevice.CreateShader(ShaderCI);

            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_PIXEL;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Triangle pixel shader";
            ShaderCI.Source = PSSource;
            using var pixelShader = this.genericEngineFactory.RenderDevice.CreateShader(ShaderCI);

            //using var psoCreate = new GraphicsPipelineStateCreateInfo();
            //psoCreate.pVS = vertexShader;
            //psoCreate.pPS = pixelShader;
            //this.pipelineState = genericEngineFactory.RenderDevice.CreateGraphicsPipelineState(psoCreate, genericEngineFactory.SwapChain);
        }

        public void Dispose()
        {
            //pipelineState.Dispose();
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

            //immediateContext.SetPipelineState(this.pipelineState);

            //// Typically we should now call CommitShaderResources(), however shaders in this example don't
            //// use any resources.

            ////DrawAttribs drawAttrs;
            ////drawAttrs.NumVertices = 3; // Render 3 vertices
            //immediateContext.Draw();

            this.swapChain.Present(1);
        }
    }
}
