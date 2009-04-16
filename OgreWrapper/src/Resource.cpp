#include "StdAfx.h"
#include "..\include\Resource.h"
#include "Ogre.h"
#include "MarshalUtils.h"

namespace OgreWrapper
{

Resource::Resource(Ogre::Resource* ogreResource)
:ogreResource(ogreResource)
{
}

System::String^ Resource::getName()
{
	return MarshalUtils::convertString(ogreResource->getName());
}

unsigned long Resource::getHandle()
{
	return ogreResource->getHandle();
}

System::String^ Resource::getGroup()
{
	return MarshalUtils::convertString(ogreResource->getGroup());
}

void Resource::prepare()
{
	ogreResource->prepare();
}

void Resource::load(bool backgroundThread)
{
	ogreResource->load(backgroundThread);
}

void Resource::reload()
{
	ogreResource->reload();
}

bool Resource::isReloadable()
{
	return ogreResource->isReloadable();
}

bool Resource::isManuallyLoaded()
{
	return ogreResource->isManuallyLoaded();
}

void Resource::unload()
{
	ogreResource->unload();
}

unsigned int Resource::getSize()
{
	return ogreResource->getSize();
}

void Resource::touch()
{
	ogreResource->touch();
}

bool Resource::isPrepared()
{
	return ogreResource->isPrepared();
}

bool Resource::isLoaded()
{
	return ogreResource->isLoaded();
}

bool Resource::isLoading()
{
	return ogreResource->isLoading();
}

Resource::LoadingState Resource::getLoadingState()
{
	return (Resource::LoadingState)ogreResource->getLoadingState();
}

bool Resource::isBackgroundLoaded()
{
	return ogreResource->isBackgroundLoaded();
}

void Resource::setBackgroundLoaded(bool bl)
{
	ogreResource->setBackgroundLoaded(bl);
}

void Resource::escalateLoading()
{
	ogreResource->escalateLoading();
}

System::String^ Resource::getOrigin()
{
	return MarshalUtils::convertString(ogreResource->getOrigin());
}

unsigned int Resource::getStateCount()
{
	return ogreResource->getStateCount();
}

void Resource::setParameter(System::String^ name, System::String^ value)
{
	ogreResource->setParameter(MarshalUtils::convertString(name), MarshalUtils::convertString(value));
}

System::String^ Resource::getParameter(System::String^ name)
{
	return MarshalUtils::convertString(ogreResource->getParameter(MarshalUtils::convertString(name)));
}

}