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
