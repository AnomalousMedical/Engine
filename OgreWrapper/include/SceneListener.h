#pragma once

#include "RenderScene.h"

namespace OgreWrapper
{

ref class RenderScene;
ref class Camera;

public interface class SceneListener
{
public:

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
};

}