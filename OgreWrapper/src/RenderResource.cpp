#include "StdAfx.h"
#include "..\include\RenderResource.h"
#include "Ogre.h"
#include "MarshalUtils.h"

namespace OgreWrapper
{

RenderResource::RenderResource(Ogre::Resource* ogreResource)
:ogreResource(ogreResource)
{
}

System::String^ RenderResource::getName()
{
	return MarshalUtils::convertString(ogreResource->getName());
}

unsigned long RenderResource::getHandle()
{
	return ogreResource->getHandle();
}

System::String^ RenderResource::getGroup()
{
	return MarshalUtils::convertString(ogreResource->getGroup());
}

void RenderResource::prepare()
{
	ogreResource->prepare();
}

void RenderResource::load(bool backgroundThread)
{
	ogreResource->load(backgroundThread);
}

void RenderResource::reload()
{
	ogreResource->reload();
}

bool RenderResource::isReloadable()
{
	return ogreResource->isReloadable();
}

bool RenderResource::isManuallyLoaded()
{
	return ogreResource->isManuallyLoaded();
}

void RenderResource::unload()
{
	ogreResource->unload();
}

unsigned int RenderResource::getSize()
{
	return ogreResource->getSize();
}

void RenderResource::touch()
{
	ogreResource->touch();
}

bool RenderResource::isPrepared()
{
	return ogreResource->isPrepared();
}

bool RenderResource::isLoaded()
{
	return ogreResource->isLoaded();
}

bool RenderResource::isLoading()
{
	return ogreResource->isLoading();
}

RenderResource::LoadingState RenderResource::getLoadingState()
{
	return (RenderResource::LoadingState)ogreResource->getLoadingState();
}

bool RenderResource::isBackgroundLoaded()
{
	return ogreResource->isBackgroundLoaded();
}

void RenderResource::setBackgroundLoaded(bool bl)
{
	ogreResource->setBackgroundLoaded(bl);
}

void RenderResource::escalateLoading()
{
	ogreResource->escalateLoading();
}

System::String^ RenderResource::getOrigin()
{
	return MarshalUtils::convertString(ogreResource->getOrigin());
}

unsigned int RenderResource::getStateCount()
{
	return ogreResource->getStateCount();
}

void RenderResource::setParameter(System::String^ name, System::String^ value)
{
	ogreResource->setParameter(MarshalUtils::convertString(name), MarshalUtils::convertString(value));
}

System::String^ RenderResource::getParameter(System::String^ name)
{
	return MarshalUtils::convertString(ogreResource->getParameter(MarshalUtils::convertString(name)));
}

}