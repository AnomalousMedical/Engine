#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/ShaderResourceVariable.h"
using namespace Diligent;

extern "C" _AnomalousExport void IShaderResourceVariable_SetArray(
	IShaderResourceVariable* objPtr
, IDeviceObject* const* ppObjects, Uint32 FirstElement, Uint32 NumElements)
{
	objPtr->SetArray(
		ppObjects
		, FirstElement
		, NumElements
	);
}
