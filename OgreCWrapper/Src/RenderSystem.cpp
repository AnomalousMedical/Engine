#include "Stdafx.h"

extern "C" __declspec(dllexport) const char* RenderSystem_validateConfigOptions(Ogre::RenderSystem* renderSystem)
{
	return renderSystem->validateConfigOptions().c_str();
}

extern "C" __declspec(dllexport) void RenderSystem__initRenderTargets(Ogre::RenderSystem* renderSystem)
{
	renderSystem->_initRenderTargets();
}

extern "C" __declspec(dllexport) void RenderSystem_setConfigOption(Ogre::RenderSystem* renderSystem, const char* name, const char* value)
{
	renderSystem->setConfigOption(name, value);
}