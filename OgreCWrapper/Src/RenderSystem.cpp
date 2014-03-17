#include "Stdafx.h"

typedef void (*SetConfigInfo)(String name, String currentValue, bool immutable);
typedef void (*AddPossibleValue)(String possibleValue);

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

extern "C" _AnomalousExport bool RenderSystem_hasConfigOption(Ogre::RenderSystem* renderSystem, String option)
{
	Ogre::ConfigOptionMap& optionMap = renderSystem->getConfigOptions();
	return optionMap.find(option) != optionMap.end();
}

extern "C" _AnomalousExport void RenderSystem_getConfigOptionInfo(Ogre::RenderSystem* renderSystem, String option, SetConfigInfo setInfo, AddPossibleValue addValues)
{
	Ogre::ConfigOptionMap& optionMap = renderSystem->getConfigOptions();
	Ogre::_ConfigOption& configOption = optionMap[option];
	setInfo(configOption.name.c_str(), configOption.currentValue.c_str(), configOption.immutable);
	//Should use a std iter, but it causes access violation exceptions for some reason.
	/*size_t numEntries = configOption.possibleValues.size();
	for(int i = 0; i < numEntries; ++i)
	{
		addValues(configOption.possibleValues[i].c_str());
	}*/
	Ogre::StringVector::iterator end = configOption.possibleValues.end();
	for(Ogre::StringVector::iterator iter = configOption.possibleValues.begin(); iter != end; ++iter)
	{
		addValues((*iter).c_str());
	}
}

extern "C" _AnomalousExport void RenderSystem_addListener(Ogre::RenderSystem* renderSystem, Ogre::RenderSystem::Listener* listener)
{
	renderSystem->addListener(listener);
}

extern "C" _AnomalousExport void RenderSystem_removeListener(Ogre::RenderSystem* renderSystem, Ogre::RenderSystem::Listener* listener)
{
	renderSystem->removeListener(listener);
}

extern "C" _AnomalousExport const char* RenderSystem_getName(Ogre::RenderSystem* renderSystem)
{
	return renderSystem->getName().c_str();
}

extern "C" _AnomalousExport void RenderSystem_clearFrameBuffer(Ogre::RenderSystem* renderSystem, unsigned int buffers, Color color, float depth, unsigned short stencil)
{
	renderSystem->clearFrameBuffer(buffers, color.toOgre(), depth, stencil);
}