#include "Stdafx.h"

extern "C" _AnomalousExport const char* RenderSystem_validateConfigOptions(Ogre::RenderSystem* renderSystem)
{
	return createClrFreeableString(renderSystem->validateConfigOptions());
}

extern "C" _AnomalousExport void RenderSystem__initRenderTargets(Ogre::RenderSystem* renderSystem)
{
	renderSystem->_initRenderTargets();
}

extern "C" _AnomalousExport void RenderSystem_setConfigOption(Ogre::RenderSystem* renderSystem, const char* name, const char* value)
{
	renderSystem->setConfigOption(name, value);
}