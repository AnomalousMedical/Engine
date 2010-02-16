#include "StdAfx.h"
#include "..\include\MaterialManager.h"
#include "Ogre.h"
#include "Material.h"
#include "MarshalUtils.h"

namespace OgreWrapper{

MaterialManager::MaterialManager()
:materialManager(Ogre::MaterialManager::getSingletonPtr())
{
}

MaterialPtr^ MaterialManager::getObject(const Ogre::MaterialPtr& materialPtr)
{
	return renderMaterials.getObject(materialPtr);
}

MaterialManager::~MaterialManager()
{
	renderMaterials.clearObjects();
}

MaterialManager^ MaterialManager::getInstance()
{
	return instance;
}

MaterialPtr^ MaterialManager::getByName(System::String^ name)
{
	return getObject((Ogre::MaterialPtr)materialManager->getByName(MarshalUtils::convertString(name)));
}

bool MaterialManager::resourceExists(System::String^ name)
{
	return materialManager->resourceExists(MarshalUtils::convertString(name));
}

System::String^ MaterialManager::getActiveScheme()
{
	return MarshalUtils::convertString(materialManager->getActiveScheme());
}

void MaterialManager::setActiveScheme(System::String^ name)
{
	return materialManager->setActiveScheme(MarshalUtils::convertString(name));
}

}