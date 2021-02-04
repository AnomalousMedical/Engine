#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/ShaderResourceVariable.h"
using namespace Diligent;
extern "C" _AnomalousExport void IShaderResourceVariable_Set(
	IShaderResourceVariable* objPtr
, IDeviceObject* pObject)
{
	objPtr->Set(
		pObject
	);
}
