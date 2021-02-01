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