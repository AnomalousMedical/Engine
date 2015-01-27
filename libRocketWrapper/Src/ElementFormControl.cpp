#include "StdAfx.h"

extern "C" _AnomalousExport void ElementFormControl_GetValue(Rocket::Controls::ElementFormControl* elementFormControl, StringRetrieverCallback retrieve, void* handle)
{
	retrieve(elementFormControl->GetValue().CString(), handle);
}

extern "C" _AnomalousExport void ElementFormControl_SetValue(Rocket::Controls::ElementFormControl* elementFormControl, String value)
{
	elementFormControl->SetValue(value);
}