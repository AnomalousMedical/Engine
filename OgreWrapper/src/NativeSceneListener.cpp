#include "StdAfx.h"
#include "..\include\NativeSceneListener.h"
#include "ManagedSceneListener.h"
#include "Ogre.h"
#include "Camera.h"
#include "SceneManager.h"

namespace OgreWrapper
{

NativeSceneListener::NativeSceneListener(gcroot<SceneManager^> ownerScene)
:ownerScene(ownerScene),
managedListener(gcnew ManagedSceneListener())
{
}

NativeSceneListener::~NativeSceneListener(void)
{
}

void NativeSceneListener::preFindVisibleObjects(Ogre::SceneManager* source, Ogre::SceneManager::IlluminationRenderStage irs, Ogre::Viewport* v)
{
	Ogre::Camera* cam = v->getCamera();
	VoidUserDefinedObject* userObj = (VoidUserDefinedObject*)cam->getUserObject();
	if(userObj != 0)
	{
		CameraGCRoot* camRoot = (CameraGCRoot*)userObj->object;
		managedListener->preFindVisibleObjects(ownerScene, (SceneManager::IlluminationRenderStage)irs, *camRoot);
	}
	else
	{
		managedListener->preFindVisibleObjects(ownerScene, (SceneManager::IlluminationRenderStage)irs, nullptr);
	}
}
	
void NativeSceneListener::postFindVisibleObjects(Ogre::SceneManager* source, Ogre::SceneManager::IlluminationRenderStage irs, Ogre::Viewport* v)
{
	Ogre::Camera* cam = v->getCamera();
	VoidUserDefinedObject* userObj = (VoidUserDefinedObject*)cam->getUserObject();
	if(userObj != 0)
	{
		CameraGCRoot* camRoot = (CameraGCRoot*)userObj->object;
		managedListener->postFindVisibleObjects(ownerScene, (SceneManager::IlluminationRenderStage)irs, *camRoot);
	}
	else
	{
		managedListener->preFindVisibleObjects(ownerScene, (SceneManager::IlluminationRenderStage)irs, nullptr);
	}
}

void NativeSceneListener::addSceneListener(gcroot<SceneListener^> sceneListener)
{
	managedListener->addSceneListener(sceneListener);
}

void NativeSceneListener::removeSceneListener(gcroot<SceneListener^> sceneListener)
{
	managedListener->removeSceneListener(sceneListener);
}

int NativeSceneListener::getNumListeners()
{
	return managedListener->getNumListeners();
}

}