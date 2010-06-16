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

extern "C" _AnomalousExport void RenderSystem__setViewMatrix(Ogre::RenderSystem* renderSystem, Matrix4x4 view)
{
	renderSystem->_setViewMatrix(view.toOgre());
}

extern "C" _AnomalousExport void RenderSystem__setProjectionMatrix(Ogre::RenderSystem* renderSystem, Matrix4x4 projection)
{
	renderSystem->_setProjectionMatrix(projection.toOgre());
}

extern "C" _AnomalousExport void RenderSystem__setViewport(Ogre::RenderSystem* renderSystem, Ogre::Viewport* vp)
{
	renderSystem->_setViewport(vp);
}