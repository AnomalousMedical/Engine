/// <file>SceneManager.cpp</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "StdAfx.h"
#include "..\include\SceneManager.h"

#include "Camera.h"
#include "Light.h"
#include "Entity.h"
#include "SceneNode.h"
#include "ManualObject.h"
#include "MathUtils.h"
#include "RaySceneQuery.h"
#include "PlaneBoundedVolume.h"

#include "Ogre.h"
#include "MarshalUtils.h"

namespace OgreWrapper{

using namespace System;

SceneManager::SceneManager(Ogre::SceneManager* sceneManager)
:sceneManager(sceneManager),
nativeSceneListener(new NativeSceneListener(this))
{
	rootNode = gcnew SceneNode(sceneManager->getRootSceneNode());
}

SceneManager::~SceneManager()
{
	delete rootNode;
	sceneManager = 0;
}

Ogre::SceneManager* SceneManager::getSceneManager()
{
	return sceneManager;
}

String^ SceneManager::getName()
{
	return MarshalUtils::convertString(sceneManager->getName());
}

Camera^ SceneManager::createCamera(System::String^ name)
{
	Ogre::Camera* ogreCam = sceneManager->createCamera( MarshalUtils::convertString(name) );
	ogreCam->setNearClipDistance(1);
	ogreCam->setAutoAspectRatio(true);
	return cameras.getObject(ogreCam);
}

Camera^ SceneManager::getCamera(System::String^ name)
{
	return cameras.getObject(sceneManager->getCamera(MarshalUtils::convertString(name)));
}

bool SceneManager::hasCamera(System::String^ name)
{
	return sceneManager->hasCamera(MarshalUtils::convertString(name));
}

void SceneManager::destroyCamera( Camera^ camera )
{
	Ogre::Camera* ogreCam = camera->getCamera();
	cameras.destroyObject(ogreCam);
	sceneManager->destroyCamera(ogreCam);
}

Light^ SceneManager::createLight(System::String^ name)
{
	Ogre::Light* ogreLight = sceneManager->createLight(MarshalUtils::convertString(name));
	return lights.getObject(ogreLight);
}

Light^ SceneManager::getLight(System::String^ name)
{
	return lights.getObject(sceneManager->getLight(MarshalUtils::convertString(name)));
}

bool SceneManager::hasLight(System::String^ name)
{
	return sceneManager->hasLight(MarshalUtils::convertString(name));
}

void SceneManager::destroyLight( Light^ light )
{
	Ogre::Light* ogreLight = light->getLight();
	lights.destroyObject(ogreLight);
	sceneManager->destroyLight(ogreLight);
}

SceneNode^ SceneManager::createSceneNode(System::String^ name)
{
	Ogre::SceneNode* ogreNode = sceneManager->createSceneNode( MarshalUtils::convertString( name ) );
	return sceneNodes.getObject(ogreNode);
}

SceneNode^ SceneManager::getRootSceneNode(void)
{
	return rootNode;
}

SceneNode^ SceneManager::getSceneNode(System::String^ name)
{
	return sceneNodes.getObject(sceneManager->getSceneNode(MarshalUtils::convertString(name)));
}

bool SceneManager::hasSceneNode(System::String^ name)
{
	return sceneManager->hasSceneNode(MarshalUtils::convertString(name));
}

void SceneManager::destroySceneNode( SceneNode^ node )
{
	if( !node->Equals( rootNode ) )
	{
		Ogre::SceneNode* ogreNode = node->getSceneNode();
		sceneNodes.destroyObject(ogreNode);
		sceneManager->destroySceneNode(ogreNode);
	}
	else
	{
		Logging::Log::Default->sendMessage( "Attempted to destroy the root node, node not destroyed.", Logging::LogLevel::Warning, "Renderer");
	}
}

Entity^ SceneManager::createEntity(System::String^ entityName, String^ meshName)
{
	Ogre::Entity* ogreEntity = sceneManager->createEntity(MarshalUtils::convertString( entityName ), MarshalUtils::convertString( meshName ) );
	return entities.getObject(ogreEntity);
}

Entity^ SceneManager::getEntity(System::String^ name)
{
	return entities.getObject(sceneManager->getEntity(MarshalUtils::convertString(name)));
}

bool SceneManager::hasEntity(System::String^ name)
{
	return sceneManager->hasEntity(MarshalUtils::convertString(name));
}

void SceneManager::destroyEntity( Entity^ entity )
{
	Ogre::Entity* ogreEntity = entity->getEntity();
	entities.destroyObject(ogreEntity);
	sceneManager->destroyEntity(ogreEntity);
}

ManualObject^ SceneManager::createManualObject(System::String^ name)
{
	Ogre::ManualObject* ogreManual = sceneManager->createManualObject(MarshalUtils::convertString(name));
	return manualObjects.getObject(ogreManual);
}

ManualObject^ SceneManager::getManualObject(System::String^ name)
{
	return manualObjects.getObject(sceneManager->getManualObject(MarshalUtils::convertString(name)));
}

bool SceneManager::hasManualObject(System::String^ name)
{
	return sceneManager->hasManualObject(MarshalUtils::convertString(name));
}

void SceneManager::destroyManualObject(ManualObject^ obj)
{
	Ogre::ManualObject* ogreManual = obj->getManualObject();
	manualObjects.destroyObject(ogreManual);
	sceneManager->destroyManualObject(ogreManual);
}

void SceneManager::setVisibilityMask(unsigned int mask)
{
	sceneManager->setVisibilityMask(mask);
}

unsigned int SceneManager::getVisibilityMask()
{
	return sceneManager->getVisibilityMask();
}

void SceneManager::addSceneListener(SceneListener^ listener)
{
	nativeSceneListener.Get()->addSceneListener(listener);
	if(nativeSceneListener.Get()->getNumListeners() == 1)
	{
		sceneManager->addListener(nativeSceneListener.Get());
	}
}

void SceneManager::removeSceneListener(SceneListener^ listener)
{
	nativeSceneListener.Get()->removeSceneListener(listener);
	if(nativeSceneListener.Get()->getNumListeners() == 0)
	{
		sceneManager->removeListener(nativeSceneListener.Get());
	}
}

RaySceneQuery^ SceneManager::createRaySceneQuery(Engine::Ray3 ray, unsigned long mask)
{
	return gcnew RaySceneQuery(sceneManager->createRayQuery(MathUtils::copyRay(ray), mask));
}

void SceneManager::destroyQuery(RaySceneQuery^ query)
{
	sceneManager->destroyQuery(query->query);
	delete query;
}

PlaneBoundedVolumeListSceneQuery^ SceneManager::createPlaneBoundedVolumeQuery(PlaneBoundedVolumeList^ volumes)
{
	return createPlaneBoundedVolumeQuery(volumes, -1);
}

PlaneBoundedVolumeListSceneQuery^ SceneManager::createPlaneBoundedVolumeQuery(PlaneBoundedVolumeList^ volumes, unsigned long mask)
{
	Ogre::PlaneBoundedVolumeList ogreList;
	if(volumes != nullptr)
	{
		for each(PlaneBoundedVolume^ vol in volumes)
		{
			ogreList.push_back(*vol->getVolume());
		}
	}
	Ogre::PlaneBoundedVolumeListSceneQuery* ogreQuery = sceneManager->createPlaneBoundedVolumeQuery(ogreList, mask);
	return gcnew PlaneBoundedVolumeListSceneQuery(ogreQuery);
}

void SceneManager::destroyQuery(PlaneBoundedVolumeListSceneQuery^ query)
{
	sceneManager->destroyQuery(query->getQuery());
}

void SceneManager::setDisplaySceneNodes(bool display)
{
	sceneManager->setDisplaySceneNodes(display);
}

bool SceneManager::getDisplaySceneNodes()
{
	return sceneManager->getDisplaySceneNodes();
}

void SceneManager::showBoundingBoxes(bool bShow)
{
	sceneManager->showBoundingBoxes(bShow);
}

bool SceneManager::getShowBoundingBoxes()
{
	return sceneManager->getShowBoundingBoxes();
}

void SceneManager::setShowDebugShadows(bool debug)
{
	sceneManager->setShowDebugShadows(debug);
}

bool SceneManager::getShowDebugShadows()
{
	return sceneManager->getShowDebugShadows();
}

}