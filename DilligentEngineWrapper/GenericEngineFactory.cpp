#include "StdAfx.h"

//#include "Graphics/GraphicsEngineD3D11/interface/EngineFactoryD3D11.h"
//#include "Graphics/GraphicsEngineD3D12/interface/EngineFactoryD3D12.h"
//#include "Graphics/GraphicsEngineOpenGL/interface/EngineFactoryOpenGL.h"
#include "Graphics/GraphicsEngineVulkan/interface/EngineFactoryVk.h"

#include "Graphics/GraphicsEngine/interface/RenderDevice.h"
#include "Graphics/GraphicsEngine/interface/DeviceContext.h"
#include "Graphics/GraphicsEngine/interface/SwapChain.h"

using namespace Diligent;

struct CreateDeviceAndSwapChainResult
{
	IRenderDevice* m_pDevice;
	IDeviceContext* m_pImmediateContext;
	ISwapChain* m_pSwapChain;
};

extern "C" _AnomalousExport CreateDeviceAndSwapChainResult GenericEngineFactory_CreateDeviceAndSwapChain(void* hWnd)
{
	CreateDeviceAndSwapChainResult result;
	SwapChainDesc SCDesc;

#   if EXPLICITLY_LOAD_ENGINE_VK_DLL
	// Load the dll and import GetEngineFactoryVk() function
	auto GetEngineFactoryVk = LoadGraphicsEngineVk();
#   endif

	EngineVkCreateInfo EngineCI;

#   ifdef DILIGENT_DEBUG
	EngineCI.EnableValidation = true;
#   endif

	auto* pFactoryVk = GetEngineFactoryVk();
	pFactoryVk->CreateDeviceAndContextsVk(EngineCI, &(result.m_pDevice), &(result.m_pImmediateContext));

	Win32NativeWindow Window{ hWnd };
	pFactoryVk->CreateSwapChainVk(result.m_pDevice, result.m_pImmediateContext, SCDesc, Window, &(result.m_pSwapChain));

	return result;
}

extern "C" _AnomalousExport void GenericEngineFactory_LazyRender(ISwapChain * m_pSwapChain, IDeviceContext * m_pImmediateContext)
{
    // Set render targets before issuing any draw command.
        // Note that Present() unbinds the back buffer if it is set as render target.
    auto* pRTV = m_pSwapChain->GetCurrentBackBufferRTV();
    auto* pDSV = m_pSwapChain->GetDepthBufferDSV();
    m_pImmediateContext->SetRenderTargets(1, &pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

    // Clear the back buffer
    const float ClearColor[] = { 0, 0, 1, 1.0f };
    // Let the engine perform required state transitions
    m_pImmediateContext->ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
    m_pImmediateContext->ClearDepthStencil(pDSV, CLEAR_DEPTH_FLAG, 1.f, 0, RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

    // Set the pipeline state in the immediate context
    //m_pImmediateContext->SetPipelineState(m_pPSO);

    //// Typically we should now call CommitShaderResources(), however shaders in this example don't
    //// use any resources.

    //DrawAttribs drawAttrs;
    //drawAttrs.NumVertices = 3; // Render 3 vertices
    //m_pImmediateContext->Draw(drawAttrs);

    //m_pSwapChain->Present();
}