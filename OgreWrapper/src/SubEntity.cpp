#include "StdAfx.h"
#include "..\include\SubEntity.h"
#include "Ogre.h"
#include "MarshalUtils.h"
#include "RenderMaterialManager.h"

namespace OgreWrapper{

SubEntity::SubEntity(Ogre::SubEntity* subEntity)
:subEntity(subEntity)
{

}

SubEntity::~SubEntity(void)
{

}

System::String^ SubEntity::getMaterialName()
{
	return MarshalUtils::convertString(subEntity->getMaterialName());
}

void SubEntity::setMaterialName(System::String^ name)
{
	subEntity->setMaterialName(MarshalUtils::convertString(name));
}

void SubEntity::setVisible(bool visible)
{
	subEntity->setVisible(visible);
}

bool SubEntity::isVisible()
{
	return subEntity->isVisible();
}

RenderMaterialPtr^ SubEntity::getMaterial()
{
	return RenderMaterialManager::getInstance()->getObject(subEntity->getMaterial());
}

}