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
renderNodes( gcnew NodeDictionary() ),
renderEntities( gcnew EntityDictionary() ),
lights( gcnew LightDictionary() ),
cameras( gcnew CameraDictionary() ),
nativeSceneListener(new NativeSceneListener(this)),
manualObjects(gcnew ManualObjectDictionary())
{
	rootNode = gcnew SceneNode( sceneManager->getRootSceneNode(), "Root" );
}

SceneManager::~SceneManager()
{
	delete rootNode;
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
	Camera^ camera = gcnew Camera( ogreCam, name );
	cameras[name] = camera;

	if(onCameraAdded != nullptr)
	{
		onCameraAdded->Invoke(camera);
	}

	return camera;
}

Camera^ SceneManager::getCamera(System::String^ name)
{
	if( cameras.ContainsKey( name ) )
	{
		return cameras[name];
	}
	return nullptr;
}

CameraEnum^ SceneManager::getCameras()
{
	return cameras.Values;
}

bool SceneManager::hasCamera(System::String^ name)
{
	return cameras.ContainsKey( name );
}

void SceneManager::destroyCamera( Camera^ camera )
{
	if(onCameraRemoved != nullptr)
	{
		onCameraRemoved->Invoke(camera);
	}

	sceneManager->destroyCamera( camera->getCamera() );
	cameras.Remove( camera->getName() );
	delete camera;
}

Light^ SceneManager::createLight(System::String^ name)
{
	Ogre::Light* ogreLight = sceneManager->createLight(MarshalUtils::convertString(name));
	Light^ light = gcnew Light( ogreLight, name );
	lights[name] = light;

	if(onLightAdded != nullptr)
	{
		onLightAdded->Invoke(light);
	}

	return light;
}

Light^ SceneManager::getLight(System::String^ name)
{
	if( lights.ContainsKey( name ) )
	{
		return lights[name];
	}
	return nullptr;
}

LightEnum^ SceneManager::getLights()
{
	return lights.Values;
}

bool SceneManager::hasLight(System::String^ name)
{
	return lights.ContainsKey(name);
}

void SceneManager::destroyLight( Light^ light )
{
	if(onLightRemoved != nullptr)
	{
		onLightRemoved->Invoke(light);
	}

	sceneManager->destroyLight( light->getLight() );
	lights.Remove( light->getName() );
	delete light;
}

SceneNode^ SceneManager::createSceneNode(System::String^ name)
{
	Ogre::SceneNode* ogreNode = sceneManager->createSceneNode( MarshalUtils::convertString( name ) );
	SceneNode^ node = gcnew SceneNode( ogreNode, name );
	renderNodes[name] = node;

	if(onSceneNodeAdded != nullptr)
	{
		onSceneNodeAdded->Invoke(node);
	}

	return node;
}

SceneNode^ SceneManager::getRootSceneNode(void)
{
	return rootNode;
}

SceneNode^ SceneManager::getSceneNode(System::String^ name)
{
	if( renderNodes.ContainsKey( name ) )
	{
		return renderNodes[name];
	}
	return nullptr;
}

NodeEnum^ SceneManager::getSceneNodes()
{
	return renderNodes.Values;
}

bool SceneManager::hasSceneNode(System::String^ name)
{
	return renderNodes.ContainsKey( name );
}

void SceneManager::destroySceneNode( SceneNode^ node )
{
	if( !node->Equals( rootNode ) )
	{
		if(onSceneNodeRemoved != nullptr)
		{
			onSceneNodeRemoved->Invoke(node);
		}

		sceneManager->destroySceneNode( node->getSceneNode()->getName() );
		renderNodes.Remove( node->getName() );
		delete node;
	}
	else
	{
		Logging::Log::Default->sendMessage( "Attempted to destroy the root node, node not destroyed.", Logging::LogLevel::Warning, "Renderer");
	}
}

Entity^ SceneManager::createRenderEntity(System::String^ entityName, String^ meshName)
{
	Ogre::Entity* ogreEntity = sceneManager->createEntity(MarshalUtils::convertString( entityName ), MarshalUtils::convertString( meshName ) );
	Entity^ renderEntity = gcnew Entity( ogreEntity, entityName, meshName );
	renderEntities[entityName] = renderEntity;

	if(onRenderEntityAdded != nullptr)
	{
		onRenderEntityAdded->Invoke(renderEntity);
	}

	return renderEntity;
}

Entity^ SceneManager::getRenderEntity(System::String^ name)
{
	if( renderEntities.ContainsKey( name ) )
	{
		return renderEntities[name];
	}
	return nullptr;
}

EntityEnum^ SceneManager::getRenderEntities()
{
	return renderEntities.Values;
}

bool SceneManager::hasRenderEntity(System::String^ name)
{
	return renderEntities.ContainsKey( name );
}

void SceneManager::destroyRenderEntity( Entity^ entity )
{
	if(onRenderEntityRemoved != nullptr)
	{
		onRenderEntityRemoved->Invoke(entity);
	}

	sceneManager->destroyEntity( entity->getEntity() );
	renderEntities.Remove( entity->getName() );
	delete entity;
}

ManualObject^ SceneManager::createManualObject(System::String^ name)
{
	Ogre::ManualObject* ogreManual = sceneManager->createManualObject(MarshalUtils::convertString(name));
	ManualObject^ manualObject = gcnew ManualObject(ogreManual, name);
	manualObjects.Add(name, manualObject);

	if(onManualObjectAdded != nullptr)
	{
		onManualObjectAdded->Invoke(manualObject);
	}

	return manualObject;
}

ManualObject^ SceneManager::getManualObject(System::String^ name)
{
	if(manualObjects.ContainsKey(name))
	{
		return manualObjects[name];
	}
	return nullptr;
}

ManualObjectEnum^ SceneManager::getManualObjects()
{
	return manualObjects.Values;
}

bool SceneManager::hasManualObject(System::String^ name)
{
	return manualObjects.ContainsKey(name);
}

void SceneManager::destroyManualObject(ManualObject^ obj)
{
	if(onManualObjectRemoved != nullptr)
	{
		onManualObjectRemoved->Invoke(obj);
	}

	sceneManager->destroyManualObject(obj->getManualObject());
	manualObjects.Remove(obj->getName());
	delete obj;
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

RaySceneQuery^ SceneManager::createRaySceneQuery(EngineMath::Ray3 ray, unsigned long mask)
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