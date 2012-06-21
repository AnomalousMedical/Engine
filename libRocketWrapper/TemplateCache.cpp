#include "StdAfx.h"
#include <../Source/Core/TemplateCache.h>

extern "C" _AnomalousExport void TemplateCache_Initialise()
{
	Rocket::Core::TemplateCache::Initialise();
}

extern "C" _AnomalousExport void TemplateCache_Shutdown()
{
	Rocket::Core::TemplateCache::Shutdown();
}