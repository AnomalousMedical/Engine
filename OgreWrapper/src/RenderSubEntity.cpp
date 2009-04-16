#include "StdAfx.h"
#include "..\include\RenderSubEntity.h"
#include "Ogre.h"
#include "MarshalUtils.h"
#include "RenderMaterialManager.h"

namespace OgreWrapper{

RenderSubEntity::RenderSubEntity(Ogre::SubEntity* subEntity)
:subEntity(subEntity)
{

}

RenderSubEntity::~RenderSubEntity(void)
{

}

System::String^ RenderSubEntity::getMaterialName()
{
	return MarshalUtils::convertString(subEntity->getMaterialName());
}

void RenderSubEntity::setMaterialName(System::String^ name)
{
	subEntity->setMaterialName(MarshalUtils::convertString(name));
}

void RenderSubEntity::setVisible(bool visible)
{
	subEntity->setVisible(visible);
}

bool RenderSubEntity::isVisible()
{
	return subEntity->isVisible();
}

RenderMaterialPtr^ RenderSubEntity::getMaterial()
{
	return RenderMaterialManager::getInstance()->getObject(subEntity->getMaterial());
}

}