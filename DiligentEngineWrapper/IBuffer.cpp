#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/Buffer.h"
using namespace Diligent;
extern "C" _AnomalousExport IBufferView* IBuffer_GetDefaultView(
	IBuffer* objPtr
, BUFFER_VIEW_TYPE ViewType)
{
	return objPtr->GetDefaultView(
		ViewType
	);
}
