#include "StdAfx.h"

//#include "Graphics/GraphicsEngineD3D11/interface/EngineFactoryD3D11.h"
//#include "Graphics/GraphicsEngineD3D12/interface/EngineFactoryD3D12.h"
//#include "Graphics/GraphicsEngineOpenGL/interface/EngineFactoryOpenGL.h"
#include "Graphics/GraphicsEngineVulkan/interface/EngineFactoryVk.h"

#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"
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