/// <file>RenderScene.cpp</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "StdAfx.h"
#include "..\include\RenderScene.h"

#include "Camera.h"
#include "Light.h"
#include "RenderEntity.h"
#include "SceneNode.h"
#include "ManualObject.h"
#include "MathUtils.h"
#include "RaySceneQuery.h"
#include "PlaneBoundedVolume.h"

#include "Ogre.h"
#include "MarshalUtils.h"

namespace OgreWrapper{

using namespace System;

RenderScene::RenderScene(Ogre::SceneManager* sceneManager)
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

RenderScene::~RenderScene()
{
	delete rootNode;
}

Ogre::SceneManager* RenderScene::getSceneManager()
{
	return sceneManager;
}

String^ RenderScene::getName()
{
	return MarshalUtils::convertString(sceneManager->getName());
}

Camera^ RenderScene::createCamera(System::String^ name)
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

Camera^ RenderScene::getCamera(System::String^ name)
{
	if( cameras.ContainsKey( name ) )
	{
		return cameras[name];
	}
	return nullptr;
}

CameraEnum^ RenderScene::getCameras()
{
	return cameras.Values;
}

bool RenderScene::hasCamera(System::String^ name)
{
	return cameras.ContainsKey( name );
}

void RenderScene::destroyCamera( Camera^ camera )
{
	if(onCameraRemoved != nullptr)
	{
		onCameraRemoved->Invoke(camera);
	}

	sceneManager->destroyCamera( camera->getCamera() );
	cameras.Remove( camera->getName() );
	delete camera;
}

Light^ RenderScene::createLight(System::String^ name)
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

Light^ RenderScene::getLight(System::String^ name)
{
	if( lights.ContainsKey( name ) )
	{
		return lights[name];
	}
	return nullptr;
}

LightEnum^ RenderScene::getLights()
{
	return lights.Values;
}

bool RenderScene::hasLight(System::String^ name)
{
	return lights.ContainsKey(name);
}

void RenderScene::destroyLight( Light^ light )
{
	if(onLightRemoved != nullptr)
	{
		onLightRemoved->Invoke(light);
	}

	sceneManager->destroyLight( light->getLight() );
	lights.Remove( light->getName() );
	delete light;
}

SceneNode^ RenderScene::createSceneNode(System::String^ name)
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

SceneNode^ RenderScene::getRootSceneNode(void)
{
	return rootNode;
}

SceneNode^ RenderScene::getSceneNode(System::String^ name)
{
	if( renderNodes.ContainsKey( name ) )
	{
		return renderNodes[name];
	}
	return nullptr;
}

NodeEnum^ RenderScene::getSceneNodes()
{
	return renderNodes.Values;
}

bool RenderScene::hasSceneNode(System::String^ name)
{
	return renderNodes.ContainsKey( name );
}

void RenderScene::destroySceneNode( SceneNode^ node )
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

RenderEntity^ RenderScene::createRenderEntity(System::String^ entityName, String^ meshName)
{
	Ogre::Entity* ogreEntity = sceneManager->createEntity(MarshalUtils::convertString( entityName ), MarshalUtils::convertString( meshName ) );
	RenderEntity^ renderEntity = gcnew RenderEntity( ogreEntity, entityName, meshName );
	renderEntities[entityName] = renderEntity;

	if(onRenderEntityAdded != nullptr)
	{
		onRenderEntityAdded->Invoke(renderEntity);
	}

	return renderEntity;
}

RenderEntity^ RenderScene::getRenderEntity(System::String^ name)
{
	if( renderEntities.ContainsKey( name ) )
	{
		return renderEntities[name];
	}
	return nullptr;
}

EntityEnum^ RenderScene::getRenderEntities()
{
	return renderEntities.Values;
}

bool RenderScene::hasRenderEntity(System::String^ name)
{
	return renderEntities.ContainsKey( name );
}

void RenderScene::destroyRenderEntity( RenderEntity^ entity )
{
	if(onRenderEntityRemoved != nullptr)
	{
		onRenderEntityRemoved->Invoke(entity);
	}

	sceneManager->destroyEntity( entity->getEntity() );
	renderEntities.Remove( entity->getName() );
	delete entity;
}

ManualObject^ RenderScene::createManualObject(System::String^ name)
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

ManualObject^ RenderScene::getManualObject(System::String^ name)
{
	if(manualObjects.ContainsKey(name))
	{
		return manualObjects[name];
	}
	return nullptr;
}

ManualObjectEnum^ RenderScene::getManualObjects()
{
	return manualObjects.Values;
}

bool RenderScene::hasManualObject(System::String^ name)
{
	return manualObjects.ContainsKey(name);
}

void RenderScene::destroyManualObject(ManualObject^ obj)
{
	if(onManualObjectRemoved != nullptr)
	{
		onManualObjectRemoved->Invoke(obj);
	}

	sceneManager->destroyManualObject(obj->getManualObject());
	manualObjects.Remove(obj->getName());
	delete obj;
}

void RenderScene::setVisibilityMask(unsigned int mask)
{
	sceneManager->setVisibilityMask(mask);
}

unsigned int RenderScene::getVisibilityMask()
{
	return sceneManager->getVisibilityMask();
}

void RenderScene::addSceneListener(SceneListener^ listener)
{
	nativeSceneListener.Get()->addSceneListener(listener);
	if(nativeSceneListener.Get()->getNumListeners() == 1)
	{
		sceneManager->addListener(nativeSceneListener.Get());
	}
}

void RenderScene::removeSceneListener(SceneListener^ listener)
{
	nativeSceneListener.Get()->removeSceneListener(listener);
	if(nativeSceneListener.Get()->getNumListeners() == 0)
	{
		sceneManager->removeListener(nativeSceneListener.Get());
	}
}

RaySceneQuery^ RenderScene::createRaySceneQuery(EngineMath::Ray3 ray, unsigned long mask)
{
	return gcnew RaySceneQuery(sceneManager->createRayQuery(MathUtils::copyRay(ray), mask));
}

void RenderScene::destroyQuery(RaySceneQuery^ query)
{
	sceneManager->destroyQuery(query->query);
	delete query;
}

PlaneBoundedVolumeListSceneQuery^ RenderScene::createPlaneBoundedVolumeQuery(PlaneBoundedVolumeList^ volumes)
{
	return createPlaneBoundedVolumeQuery(volumes, -1);
}

PlaneBoundedVolumeListSceneQuery^ RenderScene::createPlaneBoundedVolumeQuery(PlaneBoundedVolumeList^ volumes, unsigned long mask)
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

void RenderScene::destroyQuery(PlaneBoundedVolumeListSceneQuery^ query)
{
	sceneManager->destroyQuery(query->getQuery());
}

void RenderScene::setDisplaySceneNodes(bool display)
{
	sceneManager->setDisplaySceneNodes(display);
}

bool RenderScene::getDisplaySceneNodes()
{
	return sceneManager->getDisplaySceneNodes();
}

void RenderScene::showBoundingBoxes(bool bShow)
{
	sceneManager->showBoundingBoxes(bShow);
}

bool RenderScene::getShowBoundingBoxes()
{
	return sceneManager->getShowBoundingBoxes();
}

void RenderScene::setShowDebugShadows(bool debug)
{
	sceneManager->setShowDebugShadows(debug);
}

bool RenderScene::getShowDebugShadows()
{
	return sceneManager->getShowDebugShadows();
}

}