#include "StdAfx.h"

#include "Primitives/interface/Object.h"

using namespace Diligent;

extern "C" _AnomalousExport void IObject_AddRef(IObject * obj)
{
	obj->AddRef();
}

extern "C" _AnomalousExport void IObject_Release(IObject * obj)
{
	obj->Release();
}