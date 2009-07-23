#include "StdAfx.h"
#include "..\include\RenderSystem.h"

#include "OgreRenderSystem.h"
#include "MarshalUtils.h"
#include "ConfigOption.h"

namespace OgreWrapper
{

RenderSystem::RenderSystem(Ogre::RenderSystem* renderSystem)
:renderSystem( renderSystem )
{

}

RenderSystem::~RenderSystem()
{
	renderSystem = 0;
}

Ogre::RenderSystem* RenderSystem::getRenderSystem()
{
	return renderSystem;
}

System::String^ RenderSystem::validateConfigOptions()
{
	return MarshalUtils::convertString(renderSystem->validateConfigOptions());
}

void RenderSystem::_initRenderTargets()
{
	return renderSystem->_initRenderTargets();
}

System::Collections::Generic::Dictionary<System::String^, ConfigOption^>^ RenderSystem::getConfigOptions()
{
	System::Collections::Generic::Dictionary<System::String^, ConfigOption^>^ optionMap = gcnew System::Collections::Generic::Dictionary<System::String^, ConfigOption^>();
	Ogre::ConfigOptionMap::iterator iter;
	Ogre::ConfigOptionMap& configOptions = renderSystem->getConfigOptions();
	for(iter = configOptions.begin(); iter != configOptions.end(); ++iter)
	{
		ConfigOption^ option = gcnew ConfigOption(&iter->second);
		optionMap->Add(option->Name, option);
	}
	return optionMap;
}

void RenderSystem::setConfigOption(System::String^ name, System::String^ value)
{
	renderSystem->setConfigOption(MarshalUtils::convertString(name), MarshalUtils::convertString(value));
}

}