#include "Stdafx.h"

enum MovableTypes
{
	Entity,
	Light,
	Camera,
	ManualObject,
	BillboardChain,
	RibbonTrail,
	BillboardSet,
	Frustrum,
	BatchInstance,
	MovablePlane,
	ParticleSystem,
	SimpleRenderable,
	Other
};

extern "C" _AnomalousExport bool MovableObject_isAttached(Ogre::MovableObject* movableObject)
{
	return movableObject->isAttached();
}

extern "C" _AnomalousExport void MovableObject_detachFromParent(Ogre::MovableObject* movableObject)
{
	movableObject->detachFromParent();
}

extern "C" _AnomalousExport bool MovableObject_isInScene(Ogre::MovableObject* movableObject)
{
	return movableObject->isInScene();
}

extern "C" _AnomalousExport const char* MovableObject_getName(Ogre::MovableObject* movableObject)
{
	return movableObject->getName().c_str();
}

extern "C" _AnomalousExport void MovableObject_setVisible(Ogre::MovableObject* movableObject, bool visible)
{
	movableObject->setVisible(visible);
}

extern "C" _AnomalousExport bool MovableObject_isVisible(Ogre::MovableObject* movableObject)
{
	return movableObject->isVisible();
}

extern "C" _AnomalousExport void MovableObject_setVisibilityFlags(Ogre::MovableObject* movableObject, uint flags)
{
	movableObject->setVisibilityFlags(flags);
}

extern "C" _AnomalousExport void MovableObject_addVisiblityFlags(Ogre::MovableObject* movableObject, uint flags)
{
	movableObject->addVisibilityFlags(flags);
}

extern "C" _AnomalousExport void MovableObject_removeVisibilityFlags(Ogre::MovableObject* movableObject, uint flags)
{
	movableObject->removeVisibilityFlags(flags);
}

extern "C" _AnomalousExport uint MovableObject_getVisibilityFlags(Ogre::MovableObject* movableObject)
{
	return movableObject->getVisibilityFlags();
}

extern "C" _AnomalousExport MovableTypes MovableObject_getMovableType(Ogre::MovableObject* movableObject)
{
	Ogre::String type = movableObject->getMovableType();
	if(type.compare(Ogre::EntityFactory::FACTORY_TYPE_NAME) == 0)
	{
		return Entity;	
	}
	else if(type.compare(Ogre::LightFactory::FACTORY_TYPE_NAME) == 0)
	{
		return Light;	
	}
	else if(type.compare("Camera") == 0)
	{
		return Camera;	
	}
	else if(type.compare(Ogre::ManualObjectFactory::FACTORY_TYPE_NAME) == 0)
	{
		return ManualObject;	
	}
	else if(type.compare(Ogre::BillboardChainFactory::FACTORY_TYPE_NAME) == 0)
	{
		return BillboardChain;	
	}
	else if(type.compare(Ogre::RibbonTrailFactory::FACTORY_TYPE_NAME) == 0)
	{
		return RibbonTrail;	
	}
	else if(type.compare(Ogre::BillboardSetFactory::FACTORY_TYPE_NAME) == 0)
	{
		return BillboardSet;	
	}
	else if(type.compare("Frustum") == 0)
	{
		return Frustrum;	
	}
	else if(type.compare("InstancedGeometry") == 0)
	{
		return BatchInstance;	
	}
	else if(type.compare("MovablePlane") == 0)
	{
		return MovablePlane;	
	}
	else if(type.compare(Ogre::ParticleSystemFactory::FACTORY_TYPE_NAME) == 0)
	{
		return ParticleSystem;	
	}
	else if(type.compare("SimpleRenderable") == 0)
	{
		return SimpleRenderable;	
	}
	else
	{
		return Other;	
	}
}

extern "C" _AnomalousExport const char* MovableObject_getOgreMovableType(Ogre::MovableObject* movableObject)
{
	return movableObject->getMovableType().c_str();
}

extern "C" _AnomalousExport AxisAlignedBox MovableObject_getBoundingBox(Ogre::MovableObject* movableObject)
{
	return movableObject->getBoundingBox();
}

extern "C" _AnomalousExport AxisAlignedBox MovableObject_getWorldBoundingBox(Ogre::MovableObject* movableObject)
{
	return movableObject->getWorldBoundingBox();
}

extern "C" _AnomalousExport void MovableObject_setDebugDisplayEnabled(Ogre::MovableObject* movableObject, bool enabled)
{
	movableObject->setDebugDisplayEnabled(enabled);
}

extern "C" _AnomalousExport bool MovableObject_isDebugDisplayEnabled(Ogre::MovableObject* movableObject)
{
	return movableObject->isDebugDisplayEnabled();
}

extern "C" _AnomalousExport void MovableObject_setRenderQueueGroup(Ogre::MovableObject* movableObject, byte queueID)
{
	movableObject->setRenderQueueGroup(queueID);
}

extern "C" _AnomalousExport byte MovableObject_getRenderQueueGroup(Ogre::MovableObject* movableObject)
{
	return movableObject->getRenderQueueGroup();
}

extern "C" _AnomalousExport Ogre::SceneNode* MovableObject_getParentSceneNode(Ogre::MovableObject* movableObject)
{
	return movableObject->getParentSceneNode();
}