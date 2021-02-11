#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/SwapChain.h"
using namespace Diligent;

extern "C" _AnomalousExport TEXTURE_FORMAT ISwapChain_GetDesc_ColorBufferFormat(
	ISwapChain * objPtr
)
{
	return objPtr->GetDesc().ColorBufferFormat;
}

extern "C" _AnomalousExport TEXTURE_FORMAT ISwapChain_GetDesc_DepthBufferFormat(
	ISwapChain * objPtr
)
{
	return objPtr->GetDesc().DepthBufferFormat;
}

extern "C" _AnomalousExport Uint32 ISwapChain_GetDesc_BufferCount(
	ISwapChain * objPtr
)
{
	return objPtr->GetDesc().BufferCount;
}

extern "C" _AnomalousExport SURFACE_TRANSFORM ISwapChain_GetDesc_PreTransform(
	ISwapChain * objPtr
)
{
	return objPtr->GetDesc().PreTransform;
}

extern "C" _AnomalousExport ITextureView * ISwapChain_GetCurrentBackBufferRTV(
	ISwapChain * objPtr
)
{
	return objPtr->GetCurrentBackBufferRTV();
}
