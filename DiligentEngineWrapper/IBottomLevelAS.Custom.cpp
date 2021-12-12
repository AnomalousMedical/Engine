#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/BottomLevelAS.h"
using namespace Diligent;
extern "C" _AnomalousExport Uint32 IBottomLevelAS_GetScratchBufferSizes_Build(
	IBottomLevelAS* objPtr
)
{
	return objPtr->GetScratchBufferSizes().Build;
}

extern "C" _AnomalousExport Uint32 IBottomLevelAS_GetScratchBufferSizes_Update(
	IBottomLevelAS * objPtr
)
{
	return objPtr->GetScratchBufferSizes().Update;
}