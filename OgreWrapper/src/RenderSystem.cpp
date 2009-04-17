#include "StdAfx.h"
#include "..\include\RenderSystem.h"

#include "OgreRenderSystem.h"
#include "MarshalUtils.h"

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

}