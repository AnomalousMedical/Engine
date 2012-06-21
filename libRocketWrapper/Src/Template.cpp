#include "StdAfx.h"
#include <../Source/Core/Template.h>

extern "C" _AnomalousExport String Template_GetName(Rocket::Core::Template* rktTemplate)
{
	return rktTemplate->GetName().CString();
}

extern "C" _AnomalousExport String Template_GetContent(Rocket::Core::Template* rktTemplate)
{
	return rktTemplate->GetContent().CString();
}