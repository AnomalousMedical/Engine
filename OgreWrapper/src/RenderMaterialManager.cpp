#include "StdAfx.h"
#include "..\include\RenderMaterialManager.h"
#include "Ogre.h"
#include "RenderMaterial.h"
#include "MarshalUtils.h"

namespace Engine{

namespace Rendering{

RenderMaterialManager::RenderMaterialManager()
:materialManager(Ogre::MaterialManager::getSingletonPtr())
{
}

RenderMaterialPtr^ RenderMaterialManager::getObject(const Ogre::MaterialPtr& materialPtr)
{
	return renderMaterials.getObject(materialPtr);
}

RenderMaterialManager::~RenderMaterialManager()
{
	renderMaterials.clearObjects();
}

RenderMaterialManager^ RenderMaterialManager::getInstance()
{
	return instance;
}

RenderMaterialPtr^ RenderMaterialManager::getByName(System::String^ name)
{
	return getObject((Ogre::MaterialPtr)materialManager->getByName(MarshalUtils::convertString(name)));
}

bool RenderMaterialManager::resourceExists(System::String^ name)
{
	return materialManager->resourceExists(MarshalUtils::convertString(name));
}

}

}