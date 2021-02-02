#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/DeviceContext.h"
#include "Color.h"
using namespace Diligent;
extern "C" _AnomalousExport void IDeviceContext_Flush(IDeviceContext* objPtr)
{
	objPtr->Flush();
}
