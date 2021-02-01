#include "StdAfx.h"

#include "Graphics/GraphicsEngine/interface/SwapChain.h"

using namespace Diligent;

extern "C" _AnomalousExport void ISwapChain_Present(ISwapChain* objPtr)
{
    objPtr->Present();
}