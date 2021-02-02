#include "StdAfx.h"

#include "Graphics/GraphicsEngine/interface/SwapChain.h"
#include "Graphics/GraphicsEngine/interface/BlendState.h"
#include "Graphics/GraphicsEngine/interface/RenderDevice.h"

using namespace Diligent;

extern "C" _AnomalousExport IPipelineState * IRenderDevice_CreateGraphicsPipelineState(IRenderDevice * objPtr, GraphicsPipelineStateCreateInfo * PSOCreateInfo
    //All this is for testing
    , ISwapChain * m_pSwapChain)
{
    // Pipeline state object encompasses configuration of all GPU stages

    // Pipeline state name is used by the engine to report issues.
    // It is always a good idea to give objects descriptive names.
    PSOCreateInfo->PSODesc.Name = "Simple triangle PSO";

    // This is a graphics pipeline
    PSOCreateInfo->PSODesc.PipelineType = PIPELINE_TYPE_GRAPHICS;

    // clang-format off
    // This tutorial will render to a single render target
    PSOCreateInfo->GraphicsPipeline.NumRenderTargets = 1;
    // Primitive topology defines what kind of primitives will be rendered by this pipeline state
    PSOCreateInfo->GraphicsPipeline.PrimitiveTopology = PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
    // No back face culling for this tutorial
    PSOCreateInfo->GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE_NONE;
    // Disable depth testing
    PSOCreateInfo->GraphicsPipeline.DepthStencilDesc.DepthEnable = False;
    // clang-format on


    
    // Set render target format which is the format of the swap chain's color buffer
    PSOCreateInfo->GraphicsPipeline.RTVFormats[0] = m_pSwapChain->GetDesc().ColorBufferFormat;
    // Use the depth buffer format from the swap chain
    PSOCreateInfo->GraphicsPipeline.DSVFormat = m_pSwapChain->GetDesc().DepthBufferFormat;

    IPipelineState* m_pPSO = nullptr;
    objPtr->CreateGraphicsPipelineState(*PSOCreateInfo, &m_pPSO);

    return m_pPSO;
}

extern "C" _AnomalousExport IShader * IRenderDevice_CreateShader(IRenderDevice * objPtr, ShaderCreateInfo * ShaderCI)
{
	IShader* ret = nullptr;
	objPtr->CreateShader(*ShaderCI, &ret);
	return ret;
}