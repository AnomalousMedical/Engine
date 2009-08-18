#include "StdAfx.h"
#include "..\include\SubEntity.h"
#include "Ogre.h"
#include "MarshalUtils.h"
#include "MaterialManager.h"

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

MaterialPtr^ SubEntity::getMaterial()
{
	return MaterialManager::getInstance()->getObject(subEntity->getMaterial());
}

void SubEntity::setCustomParameter(size_t index, Engine::Quaternion value)
{
	subEntity->setCustomParameter(index, Ogre::Vector4(value.x, value.y, value.z, value.w));
}

Engine::Quaternion SubEntity::getCustomParameter(size_t index)
{
	Ogre::Vector4 value = subEntity->getCustomParameter(index);
	return Engine::Quaternion(value.x, value.y, value.z, value.w);
}

}