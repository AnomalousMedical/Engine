#include "StdAfx.h"
#include "..\include\ManagedSceneListener.h"

namespace OgreWrapper
{

ManagedSceneListener::ManagedSceneListener(void)
:sceneListeners(gcnew SceneListenerList())
{

}

void ManagedSceneListener::preFindVisibleObjects(SceneManager^ sceneManager, SceneManager::IlluminationRenderStage irs, Camera^ camera)
{
	for each(SceneListener^ listener in sceneListeners)
	{
		listener->preFindVisibleObjects(sceneManager, irs, camera);
	}
}

void ManagedSceneListener::postFindVisibleObjects(SceneManager^ sceneManager, SceneManager::IlluminationRenderStage irs, Camera^ camera)
{
	for each(SceneListener^ listener in sceneListeners)
	{
		listener->postFindVisibleObjects(sceneManager, irs, camera);
	}
}

}