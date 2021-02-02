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

extern "C" _AnomalousExport IPipelineState* GraphicsPipelineStateCreateInfo_OneShot(GraphicsPipelineStateCreateInfo * PSOCreateInfo, ISwapChain * m_pSwapChain, IRenderDevice* m_pDevice, IShader * pVS, IShader * pPS)
{
    //// Finally, create the pipeline state
    PSOCreateInfo->pVS = pVS;
    PSOCreateInfo->pPS = pPS;


    // Pipeline state object encompasses configuration of all GPU stages

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

    IPipelineState* m_pPSO = nullptr;
    m_pDevice->CreateGraphicsPipelineState(*PSOCreateInfo, &m_pPSO);

    return m_pPSO;
}