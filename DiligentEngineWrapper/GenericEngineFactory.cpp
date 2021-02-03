#include "StdAfx.h"

//#include "Graphics/GraphicsEngineD3D11/interface/EngineFactoryD3D11.h"
//#include "Graphics/GraphicsEngineD3D12/interface/EngineFactoryD3D12.h"
//#include "Graphics/GraphicsEngineOpenGL/interface/EngineFactoryOpenGL.h"
#include "Graphics/GraphicsEngineVulkan/interface/EngineFactoryVk.h"

#include "Graphics/GraphicsEngine/interface/RenderDevice.h"
#include "Graphics/GraphicsEngine/interface/DeviceContext.h"
#include "Graphics/GraphicsEngine/interface/SwapChain.h"
#include "Color.h"

using namespace Diligent;

struct CreateDeviceAndSwapChainResult
{
	IRenderDevice* m_pDevice;
	IDeviceContext* m_pImmediateContext;
	ISwapChain* m_pSwapChain;
};

extern "C" _AnomalousExport CreateDeviceAndSwapChainResult GenericEngineFactory_CreateDeviceAndSwapChain(
	void* hWnd
	, Uint32 Width
	, Uint32 Height
	, TEXTURE_FORMAT ColorBufferFormat
	, TEXTURE_FORMAT DepthBufferFormat
	, SWAP_CHAIN_USAGE_FLAGS Usage
	, SURFACE_TRANSFORM PreTransform
	, Uint32 BufferCount
	, Float32 DefaultDepthValue
	, Uint8 DefaultStencilValue
	, bool IsPrimary
)
{
	CreateDeviceAndSwapChainResult result;
	SwapChainDesc SCDesc;
	SCDesc.Width = Width;
	SCDesc.Height = Height;
	SCDesc.ColorBufferFormat = ColorBufferFormat;
	SCDesc.DepthBufferFormat = DepthBufferFormat;
	SCDesc.Usage = Usage;
	SCDesc.PreTransform = PreTransform;
	SCDesc.BufferCount = BufferCount;
	SCDesc.DefaultDepthValue = DefaultDepthValue;
	SCDesc.DefaultStencilValue = DefaultStencilValue;
	SCDesc.IsPrimary = IsPrimary;

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