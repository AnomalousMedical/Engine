#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/TopLevelAS.h"
using namespace Diligent;
extern "C" _AnomalousExport Uint32 ITopLevelAS_GetScratchBufferSizes_Build(
	ITopLevelAS* objPtr
)
{
	return objPtr->GetScratchBufferSizes().Build;
}

extern "C" _AnomalousExport Uint32 ITopLevelAS_GetScratchBufferSizes_Update(
	ITopLevelAS * objPtr
)
{
	return objPtr->GetScratchBufferSizes().Update;
}