#include "StdAfx.h"

#include "Primitives/interface/Object.h"

using namespace Diligent;

extern "C" _AnomalousExport void DilligentObject_Release(IObject* obj)
{
	obj->Release();
}