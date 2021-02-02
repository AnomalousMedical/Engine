#include "StdAfx.h"

#include "Graphics/GraphicsEngine/interface/PipelineState.h"
#include "Graphics/GraphicsEngine/interface/SwapChain.h"
#include "Graphics/GraphicsEngine/interface/RenderDevice.h"

#include "Common/interface/RefCntAutoPtr.hpp"

using namespace Diligent;

extern "C" _AnomalousExport GraphicsPipelineStateCreateInfo * GraphicsPipelineStateCreateInfo_Create()
{
	return new GraphicsPipelineStateCreateInfo;
}

extern "C" _AnomalousExport void GraphicsPipelineStateCreateInfo_Delete(GraphicsPipelineStateCreateInfo * obj)
{
	delete obj;
}

static const char* VSSource = R"(
struct PSInput 
{ 
    float4 Pos   : SV_POSITION; 
    float3 Color : COLOR; 
};

void main(in  uint    VertId : SV_VertexID,
          out PSInput PSIn) 
{
    float4 Pos[3];
    Pos[0] = float4(-0.5, -0.5, 0.0, 1.0);
    Pos[1] = float4( 0.0, +0.5, 0.0, 1.0);
    Pos[2] = float4(+0.5, -0.5, 0.0, 1.0);

    float3 Col[3];
    Col[0] = float3(1.0, 0.0, 0.0); // red
    Col[1] = float3(0.0, 1.0, 0.0); // green
    Col[2] = float3(0.0, 0.0, 1.0); // blue

    PSIn.Pos   = Pos[VertId];
    PSIn.Color = Col[VertId];
}
)";

// Pixel shader simply outputs interpolated vertex color
static const char* PSSource = R"(
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
}
)";

extern "C" _AnomalousExport void GraphicsPipelineStateCreateInfo_LazySetup(GraphicsPipelineStateCreateInfo * PSOCreateInfo, ISwapChain* m_pSwapChain, IShader* pVS, IShader* pPS)
{
    // Pipeline state name is used by the engine to report issues.
        // It is always a good idea to give objects descriptive names.
    PSOCreateInfo->PSODesc.Name = "Simple triangle PSO";

    // This is a graphics pipeline
    PSOCreateInfo->PSODesc.PipelineType = PIPELINE_TYPE_GRAPHICS;

    // clang-format off
    // This tutorial will render to a single render target
    PSOCreateInfo->GraphicsPipeline.NumRenderTargets = 1;
    // Set render target format which is the format of the swap chain's color buffer
    PSOCreateInfo->GraphicsPipeline.RTVFormats[0] = m_pSwapChain->GetDesc().ColorBufferFormat;
    // Use the depth buffer format from the swap chain
    PSOCreateInfo->GraphicsPipeline.DSVFormat = m_pSwapChain->GetDesc().DepthBufferFormat;
    // Primitive topology defines what kind of primitives will be rendered by this pipeline state
    PSOCreateInfo->GraphicsPipeline.PrimitiveTopology = PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
    // No back face culling for this tutorial
    PSOCreateInfo->GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE_NONE;
    // Disable depth testing
    PSOCreateInfo->GraphicsPipeline.DepthStencilDesc.DepthEnable = False;
    // clang-format on

    // Finally, create the pipeline state
    PSOCreateInfo->pVS = pVS;
    PSOCreateInfo->pPS = pPS;
}