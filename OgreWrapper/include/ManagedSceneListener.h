#pragma once

#include "SceneListener.h"

namespace Engine
{

namespace Rendering
{

typedef System::Collections::Generic::LinkedList<SceneListener^> SceneListenerList;

ref class RenderScene;
ref class Camera;

ref class ManagedSceneListener
{
private:
	SceneListenerList^ sceneListeners;

public:
	ManagedSceneListener(void);

	/// <summary>
	/// Called prior to searching for visible objects in this SceneManager.
	/// Note that the render queue at this stage will be full of the last render's contents 
	/// and will be cleared after this method is called.
	/// </summary>
	/// <param name="sceneManager">The scene manager raising the event.</param>
	/// <param name="irs">The stage of illumination being dealt with. IRS_NONE for a regular 
	/// render, IRS_RENDER_TO_TEXTURE for a shadow caster render.</param>
	/// <param name="camera">The camera containing the viewport being updated.</param>
	void preFindVisibleObjects(RenderScene^ sceneManager, RenderScene::IlluminationRenderStage irs, Camera^ camera);

	/// <summary>
	/// Called after searching for visible objects in this SceneManager.
	/// Note that the render queue at this stage will be full of the current scenes contents, 
	/// ready for rendering. You may manually add renderables to this queue if you wish.
	/// </summary>
	/// <param name="sceneManager">The SceneManager instance raising this event.</param>
	/// <param name="irs">The stage of illumination being dealt with. IRS_NONE for a regular 
	/// render, IRS_RENDER_TO_TEXTURE for a shadow caster render.</param>
	/// <param name="camera">The camera containing the viewport being updated.</param>
	void postFindVisibleObjects(RenderScene^ sceneManager, RenderScene::IlluminationRenderStage irs, Camera^ camera);

	void addSceneListener(SceneListener^ listener)
	{
		sceneListeners->AddLast(listener);
	}

	void removeSceneListener(SceneListener^ listener)
	{
		sceneListeners->Remove(listener);
	}

	int getNumListeners()
	{
		return sceneListeners->Count;
	}
};

}

}