#include "StdAfx.h"
#include "..\include\OgreResourceGroupManager.h"
#include "Ogre.h"
#include "MarshalUtils.h"
#include "ResourceDeclaration.h"
#include "FileInfo.h"

namespace OgreWrapper
{

OgreResourceGroupManager^ OgreResourceGroupManager::getInstance()
{
	if(instance == nullptr)
	{
		instance = gcnew OgreResourceGroupManager();
	}
	return instance;
}

OgreResourceGroupManager::OgreResourceGroupManager(void)
:resourceManager(Ogre::ResourceGroupManager::getSingletonPtr())
{
	
}

void OgreResourceGroupManager::createResourceGroup(System::String^ name)
{
	resourceManager->createResourceGroup(MarshalUtils::convertString(name));
}

void OgreResourceGroupManager::initializeAllResourceGroups()
{
	try
	{
		resourceManager->initialiseAllResourceGroups();

		if(onResourcesInitialized != nullptr)
		{
			onResourcesInitialized->Invoke();
		}
	}
	catch(Ogre::Exception e)
	{
		Logging::Log::Default->sendMessage("Error loading resources {0}", Logging::LogLevel::Error, "Renderer", MarshalUtils::convertString(e.getFullDescription()));
	}
}

void OgreResourceGroupManager::initializeResourceGroup(System::String^ name)
{
	try
	{
		resourceManager->initialiseResourceGroup(MarshalUtils::convertString(name));

		if(onResourcesInitialized != nullptr)
		{
			onResourcesInitialized->Invoke();
		}
	}
	catch(Ogre::Exception e)
	{
		Logging::Log::Default->sendMessage("Error loading resources {0}", Logging::LogLevel::Error, "Renderer", MarshalUtils::convertString(e.getFullDescription()));
	}
}

void OgreResourceGroupManager::destroyResourceGroup(System::String^ name)
{
	resourceManager->destroyResourceGroup(MarshalUtils::convertString(name));
}

void OgreResourceGroupManager::addResourceLocation(System::String^ name, System::String^ locType, System::String^ group, bool recursive)
{
	try
	{
		resourceManager->addResourceLocation(MarshalUtils::convertString(name),
											   MarshalUtils::convertString(locType),
											   MarshalUtils::convertString(group),
											   recursive);
	}
	catch(Ogre::Exception e)
	{
		Logging::Log::Default->sendMessage("Error adding resource location {0}.", Logging::LogLevel::Error, "Ogre", MarshalUtils::convertString(e.getDescription()));
	}
}

void OgreResourceGroupManager::removeResourceLocation(System::String^ name, System::String^ resGroup)
{
	resourceManager->removeResourceLocation(MarshalUtils::convertString(name), MarshalUtils::convertString(resGroup));
}

GroupEnum^ OgreResourceGroupManager::getResourceGroups()
{
	System::Collections::Generic::List<System::String^>^ groups = gcnew System::Collections::Generic::List<System::String^>();
	Ogre::StringVector ogreStrings = resourceManager->getResourceGroups();
	for(Ogre::StringVector::iterator iter = ogreStrings.begin(); iter != ogreStrings.end(); iter++)
	{
		groups->Add(MarshalUtils::convertString(*iter));
	}
	return groups;
}

DeclarationEnum^ OgreResourceGroupManager::getResourceDeclarationList(System::String^ groupName)
{
	System::Collections::Generic::List<ResourceDeclaration>^ declarations = gcnew System::Collections::Generic::List<ResourceDeclaration>();
	Ogre::ResourceGroupManager::ResourceDeclarationList ogreDecl = resourceManager->getResourceDeclarationList(MarshalUtils::convertString(groupName));
	for(Ogre::ResourceGroupManager::ResourceDeclarationList::iterator iter = ogreDecl.begin(); iter != ogreDecl.end(); iter++)
	{
		declarations->Add(ResourceDeclaration(MarshalUtils::convertString((*iter).resourceName), MarshalUtils::convertString((*iter).resourceType)));
	}
	return declarations;
}

ResourceNameEnum^ OgreResourceGroupManager::listResourceNames(System::String^ groupName)
{
	return listResourceNames(groupName, false);
}

ResourceNameEnum^ OgreResourceGroupManager::listResourceNames(System::String^ groupName, bool dirs)
{
	System::Collections::Generic::List<System::String^>^ files = gcnew System::Collections::Generic::List<System::String^>();
	Ogre::StringVectorPtr ogreVec = resourceManager->listResourceNames(MarshalUtils::convertString(groupName), dirs);
	for(Ogre::StringVector::iterator iter = ogreVec->begin(); iter != ogreVec->end(); iter++)
	{
		files->Add(MarshalUtils::convertString(*iter));
	}
	return files;
}

FileInfoEnum^ OgreResourceGroupManager::listResourceFileInfo(System::String^ groupName, bool dirs)
{
	Ogre::FileInfoListPtr fileInfo = resourceManager->listResourceFileInfo(MarshalUtils::convertString(groupName), dirs);
	FileInfoEnum^ files = gcnew FileInfoEnum(fileInfo->size());
	for(Ogre::FileInfoList::iterator iter = fileInfo->begin(); iter != fileInfo->end(); iter++)
	{
		files->Add(gcnew FileInfo(MarshalUtils::convertString(iter->filename), MarshalUtils::convertString(iter->path), 
			MarshalUtils::convertString(iter->basename), iter->compressedSize, iter->uncompressedSize));
	}
	return files;
}

FileInfoEnum^ OgreResourceGroupManager::findResourceFileInfo(System::String^ group, System::String^ pattern, bool dirs)
{
	Ogre::FileInfoListPtr fileInfo = resourceManager->findResourceFileInfo(MarshalUtils::convertString(group), MarshalUtils::convertString(pattern), dirs);
	FileInfoEnum^ files = gcnew FileInfoEnum(fileInfo->size());
	for(Ogre::FileInfoList::iterator iter = fileInfo->begin(); iter != fileInfo->end(); iter++)
	{
		files->Add(gcnew FileInfo(MarshalUtils::convertString(iter->filename), MarshalUtils::convertString(iter->path), 
			MarshalUtils::convertString(iter->basename), iter->compressedSize, iter->uncompressedSize));
	}
	return files;
}

bool OgreResourceGroupManager::resourceGroupExists(System::String^ name)
{
	return resourceManager->resourceGroupExists(MarshalUtils::convertString(name));
}

System::String^ OgreResourceGroupManager::findGroupContainingResource(System::String^ resourceName)
{
	if(resourceName == nullptr)
	{
		return nullptr;
	}
	try
	{
		return gcnew System::String(resourceManager->findGroupContainingResource(MarshalUtils::convertString(resourceName)).c_str());
	}
	catch(Ogre::Exception &e)
	{
		return nullptr;
	}
}

}