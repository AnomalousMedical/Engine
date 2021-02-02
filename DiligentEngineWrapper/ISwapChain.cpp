#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/SwapChain.h"
using namespace Diligent;
extern "C" _AnomalousExport void ISwapChain_Resize(ISwapChain* objPtr, Uint32 NewWidth, Uint32 NewHeight, SURFACE_TRANSFORM NewTransform)
{
	objPtr->Resize(NewWidth, NewHeight, NewTransform);
}
