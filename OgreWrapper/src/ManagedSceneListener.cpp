#include "StdAfx.h"
#include "..\include\ManagedSceneListener.h"

namespace Rendering
{

ManagedSceneListener::ManagedSceneListener(void)
:sceneListeners(gcnew SceneListenerList())
{

}

void ManagedSceneListener::preFindVisibleObjects(RenderScene^ sceneManager, RenderScene::IlluminationRenderStage irs, Camera^ camera)
{
	for each(SceneListener^ listener in sceneListeners)
	{
		listener->preFindVisibleObjects(sceneManager, irs, camera);
	}
}

void ManagedSceneListener::postFindVisibleObjects(RenderScene^ sceneManager, RenderScene::IlluminationRenderStage irs, Camera^ camera)
{
	for each(SceneListener^ listener in sceneListeners)
	{
		listener->postFindVisibleObjects(sceneManager, irs, camera);
	}
}

}