#include "stdafx.h"
#include "MovableObject.h"
#include "MarshalUtils.h"
#include "Ogre.h"
#include "AxisAlignedBox.h"

namespace OgreWrapper
{

MovableObject::MovableObject(Ogre::MovableObject* movableObject)
	:movableObject(movableObject)
{

}

Ogre::MovableObject* MovableObject::getMovableObject()
{
	return movableObject;
}

bool MovableObject::isAttached()
{
	return movableObject->isAttached();
}

void MovableObject::detatchFromParent()
{
	return movableObject->detatchFromParent();
}

bool MovableObject::isInScene()
{
	return movableObject->isInScene();
}

void MovableObject::setVisible(bool visible)
{
	movableObject->setVisible(visible);
}

bool MovableObject::isVisible()
{
	return movableObject->isVisible();
}

void MovableObject::setVisibilityFlags(unsigned int flags)
{
	movableObject->setVisibilityFlags(flags);
}

void MovableObject::addVisiblityFlags(unsigned int flags)
{
	movableObject->addVisibilityFlags(flags);
}

void MovableObject::removeVisibilityFlags(unsigned int flags)
{
	movableObject->removeVisibilityFlags(flags);
}

unsigned int MovableObject::getVisibilityFlags()	
{
	return movableObject->getVisibilityFlags();
}

MovableTypes MovableObject::getMovableType()
{
	Ogre::String type = movableObject->getMovableType();
	if(type.compare(Ogre::EntityFactory::FACTORY_TYPE_NAME) == 0)
	{
		return MovableTypes::Entity;	
	}
	else if(type.compare(Ogre::LightFactory::FACTORY_TYPE_NAME) == 0)
	{
		return MovableTypes::Light;	
	}
	else if(type.compare("Camera") == 0)
	{
		return MovableTypes::Camera;	
	}
	else if(type.compare(Ogre::ManualObjectFactory::FACTORY_TYPE_NAME) == 0)
	{
		return MovableTypes::ManualObject;	
	}
	else if(type.compare(Ogre::BillboardChainFactory::FACTORY_TYPE_NAME) == 0)
	{
		return MovableTypes::BillboardChain;	
	}
	else if(type.compare(Ogre::RibbonTrailFactory::FACTORY_TYPE_NAME) == 0)
	{
		return MovableTypes::RibbonTrail;	
	}
	else if(type.compare(Ogre::BillboardSetFactory::FACTORY_TYPE_NAME) == 0)
	{
		return MovableTypes::BillboardSet;	
	}
	else if(type.compare("Frustum") == 0)
	{
		return MovableTypes::Frustrum;	
	}
	else if(type.compare("InstancedGeometry") == 0)
	{
		return MovableTypes::BatchInstance;	
	}
	else if(type.compare("MovablePlane") == 0)
	{
		return MovableTypes::MovablePlane;	
	}
	else if(type.compare(Ogre::ParticleSystemFactory::FACTORY_TYPE_NAME) == 0)
	{
		return MovableTypes::ParticleSystem;	
	}
	else if(type.compare("SimpleRenderable") == 0)
	{
		return MovableTypes::SimpleRenderable;	
	}
	else
	{
		return MovableTypes::Other;	
	}
}

System::String^ MovableObject::getOgreMovableType()
{
	return MarshalUtils::convertString(movableObject->getMovableType());
}

AxisAlignedBox^ MovableObject::getBoundingBox()
{
	return gcnew AxisAlignedBox(&movableObject->getBoundingBox());
}

void MovableObject::setDebugDisplayEnabled(bool enabled)
{
	movableObject->setDebugDisplayEnabled(enabled);
}

bool MovableObject::isDebugDisplayEnabled()
{
	return movableObject->isDebugDisplayEnabled();
}

void MovableObject::setRenderQueueGroup(unsigned char queueID)
{
	movableObject->setRenderQueueGroup(queueID);
}

unsigned char MovableObject::getRenderQueueGroup()
{
	return movableObject->getRenderQueueGroup();
}

}