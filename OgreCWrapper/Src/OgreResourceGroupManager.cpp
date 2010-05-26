#include "Stdafx.h"

extern "C" __declspec(dllexport) void ResourceGroupManager_createResourceGroup(String name)
{
	Ogre::ResourceGroupManager::getSingleton().createResourceGroup(name);
}

extern "C" __declspec(dllexport) bool ResourceGroupManager_initializeAllResourceGroups()
{
	try
	{
		Ogre::ResourceGroupManager::getSingleton().initialiseAllResourceGroups();
	}
	catch(Ogre::Exception e)
	{
		return false;
	}
	return true;
}

extern "C" __declspec(dllexport) bool ResourceGroupManager_initializeResourceGroup(String name)
{
	try
	{
		Ogre::ResourceGroupManager::getSingleton().initialiseResourceGroup(name);
	}
	catch(Ogre::Exception e)
	{
		return false;
	}
	return true;
}

extern "C" __declspec(dllexport) void ResourceGroupManager_destroyResourceGroup(String name)
{
	Ogre::ResourceGroupManager::getSingleton().destroyResourceGroup(name);
}

extern "C" __declspec(dllexport) void ResourceGroupManager_addResourceLocation(String name, String locType, String group, bool recursive)
{
	try
	{
		Ogre::ResourceGroupManager::getSingleton().addResourceLocation(name, locType, group, recursive);
	}
	catch(Ogre::Exception e)
	{
	}
}

extern "C" __declspec(dllexport) void ResourceGroupManager_removeResourceLocation(String name, String resGroup)
{
	Ogre::ResourceGroupManager::getSingleton().removeResourceLocation(name);
}

extern "C" __declspec(dllexport) String ResourceGroupManager_findGroupContainingResource(String resourceName)
{
	return Ogre::ResourceGroupManager::getSingleton().findGroupContainingResource(resourceName).c_str();
}

extern "C" __declspec(dllexport) bool ResourceGroupManager_resourceGroupExists(String name)
{
	return Ogre::ResourceGroupManager::getSingleton().resourceGroupExists(name);
}