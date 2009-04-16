#pragma once

#include "OgreSceneManager.h"
#include "gcroot.h"

namespace Rendering
{

ref class ManagedSceneListener;
ref class RenderScene;
interface class SceneListener;

class NativeSceneListener : public Ogre::SceneManager::Listener
{
private:
	gcroot<RenderScene^> ownerScene;
	gcroot<ManagedSceneListener^> managedListener;

public:
	NativeSceneListener(gcroot<RenderScene^> ownerScene);

	virtual ~NativeSceneListener(void);

	virtual void preFindVisibleObjects(Ogre::SceneManager* source, Ogre::SceneManager::IlluminationRenderStage irs, Ogre::Viewport* v);
	
	virtual void postFindVisibleObjects(Ogre::SceneManager* source, Ogre::SceneManager::IlluminationRenderStage irs, Ogre::Viewport* v);

	virtual void shadowTexturesUpdated(size_t numberOfShadowTextures)
	{

	}

	virtual void shadowTextureCasterPreViewProj(Ogre::Light* light, Ogre::Camera* camera, size_t iteration)
	{

	}

	virtual void shadowTextureReceiverPreViewProj(Ogre::Light* light, Ogre::Frustum* frustum)
	{

	}

	virtual bool sortLightsAffectingFrustum( Ogre::LightList& lightList ) 
	{ 
		return false; 
	}

	void addSceneListener(gcroot<SceneListener^> sceneListener);

	void removeSceneListener(gcroot<SceneListener^> sceneListener);

	int getNumListeners();
};

}