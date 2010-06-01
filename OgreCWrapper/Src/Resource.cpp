#include "Stdafx.h"

extern "C" __declspec(dllexport) String Resource_getName(Ogre::Resource* resource)
{
	return resource->getName().c_str();
}

extern "C" __declspec(dllexport) unsigned long Resource_getHandle(Ogre::Resource* resource)
{
	return resource->getHandle();
}

extern "C" __declspec(dllexport) String Resource_getGroup(Ogre::Resource* resource)
{
	return resource->getGroup().c_str();
}

extern "C" __declspec(dllexport) void Resource_prepare(Ogre::Resource* resource)
{
	resource->prepare();
}

extern "C" __declspec(dllexport) void Resource_load(Ogre::Resource* resource, bool backgroundThread)
{
	resource->load();
}

extern "C" __declspec(dllexport) void Resource_reload(Ogre::Resource* resource)
{
	resource->reload();
}

extern "C" __declspec(dllexport) bool Resource_isReloadable(Ogre::Resource* resource)
{
	return resource->isReloadable();
}

extern "C" __declspec(dllexport) bool Resource_isManuallyLoaded(Ogre::Resource* resource)
{
	return resource->isManuallyLoaded();
}

extern "C" __declspec(dllexport) void Resource_unload(Ogre::Resource* resource)
{
	resource->unload();
}

extern "C" __declspec(dllexport) uint Resource_getSize(Ogre::Resource* resource)
{
	return resource->getSize();
}

extern "C" __declspec(dllexport) void Resource_touch(Ogre::Resource* resource)
{
	resource->touch();
}

extern "C" __declspec(dllexport) bool Resource_isPrepared(Ogre::Resource* resource)
{
	return resource->isPrepared();
}

extern "C" __declspec(dllexport) bool Resource_isLoaded(Ogre::Resource* resource)
{
	return resource->isLoaded();
}

extern "C" __declspec(dllexport) bool Resource_isLoading(Ogre::Resource* resource)
{
	return resource->isLoading();
}

extern "C" __declspec(dllexport) Ogre::Resource::LoadingState Resource_getLoadingState(Ogre::Resource* resource)
{
	return resource->getLoadingState();
}

extern "C" __declspec(dllexport) bool Resource_isBackgroundLoaded(Ogre::Resource* resource)
{
	return resource->isBackgroundLoaded();
}

extern "C" __declspec(dllexport) void Resource_setBackgroundLoaded(Ogre::Resource* resource, bool bl)
{
	resource->setBackgroundLoaded(bl);
}

extern "C" __declspec(dllexport) void Resource_escalateLoading(Ogre::Resource* resource)
{
	resource->escalateLoading();
}

extern "C" __declspec(dllexport) String Resource_getOrigin(Ogre::Resource* resource)
{
	return resource->getOrigin().c_str();
}

extern "C" __declspec(dllexport) uint Resource_getStateCount(Ogre::Resource* resource)
{
	return resource->getStateCount();
}

extern "C" __declspec(dllexport) void Resource_setParameter(Ogre::Resource* resource, String name, String value)
{
	resource->setParameter(name, value);
}

extern "C" __declspec(dllexport) String Resource_getParameter(Ogre::Resource* resource, String name)
{
	return createClrFreeableString(resource->getParameter(name));
}