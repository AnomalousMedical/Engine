#include "StdAfx.h"

#include "Graphics/GraphicsEngine/interface/SwapChain.h"

using namespace Diligent;

extern "C" _AnomalousExport void ISwapChain_Present(ISwapChain * objPtr)
{
	objPtr->Present();
}

extern "C" _AnomalousExport void ISwapChain_Resize(ISwapChain * objPtr, Uint32 NewWidth, Uint32 NewHeight)
{
	objPtr->Resize(NewWidth, NewHeight);
}

extern "C" _AnomalousExport ITextureView * ISwapChain_GetCurrentBackBufferRTV(ISwapChain * objPtr) 
{
	return objPtr->GetCurrentBackBufferRTV();
}

extern "C" _AnomalousExport ITextureView * ISwapChain_GetDepthBufferDSV(ISwapChain * objPtr)
{
	return objPtr->GetDepthBufferDSV();
}