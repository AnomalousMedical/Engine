#include "Stdafx.h"

extern "C" _AnomalousExport String Resource_getName(Ogre::Resource* resource)
{
	return resource->getName().c_str();
}

extern "C" _AnomalousExport unsigned long Resource_getHandle(Ogre::Resource* resource)
{
	return resource->getHandle();
}

extern "C" _AnomalousExport String Resource_getGroup(Ogre::Resource* resource)
{
	return resource->getGroup().c_str();
}

extern "C" _AnomalousExport void Resource_prepare(Ogre::Resource* resource)
{
	resource->prepare();
}

extern "C" _AnomalousExport void Resource_load(Ogre::Resource* resource, bool backgroundThread)
{
	resource->load();
}

extern "C" _AnomalousExport void Resource_reload(Ogre::Resource* resource)
{
	resource->reload();
}

extern "C" _AnomalousExport bool Resource_isReloadable(Ogre::Resource* resource)
{
	return resource->isReloadable();
}

extern "C" _AnomalousExport bool Resource_isManuallyLoaded(Ogre::Resource* resource)
{
	return resource->isManuallyLoaded();
}

extern "C" _AnomalousExport void Resource_unload(Ogre::Resource* resource)
{
	resource->unload();
}

extern "C" _AnomalousExport uint Resource_getSize(Ogre::Resource* resource)
{
	return resource->getSize();
}

extern "C" _AnomalousExport void Resource_touch(Ogre::Resource* resource)
{
	resource->touch();
}

extern "C" _AnomalousExport bool Resource_isPrepared(Ogre::Resource* resource)
{
	return resource->isPrepared();
}

extern "C" _AnomalousExport bool Resource_isLoaded(Ogre::Resource* resource)
{
	return resource->isLoaded();
}

extern "C" _AnomalousExport bool Resource_isLoading(Ogre::Resource* resource)
{
	return resource->isLoading();
}

extern "C" _AnomalousExport Ogre::Resource::LoadingState Resource_getLoadingState(Ogre::Resource* resource)
{
	return resource->getLoadingState();
}

extern "C" _AnomalousExport bool Resource_isBackgroundLoaded(Ogre::Resource* resource)
{
	return resource->isBackgroundLoaded();
}

extern "C" _AnomalousExport void Resource_setBackgroundLoaded(Ogre::Resource* resource, bool bl)
{
	resource->setBackgroundLoaded(bl);
}

extern "C" _AnomalousExport void Resource_escalateLoading(Ogre::Resource* resource)
{
	resource->escalateLoading();
}

extern "C" _AnomalousExport String Resource_getOrigin(Ogre::Resource* resource)
{
	return resource->getOrigin().c_str();
}

extern "C" _AnomalousExport uint Resource_getStateCount(Ogre::Resource* resource)
{
	return resource->getStateCount();
}

extern "C" _AnomalousExport void Resource_setParameter(Ogre::Resource* resource, String name, String value)
{
	resource->setParameter(name, value);
}

extern "C" _AnomalousExport String Resource_getParameter(Ogre::Resource* resource, String name)
{
	return createClrFreeableString(resource->getParameter(name));
}