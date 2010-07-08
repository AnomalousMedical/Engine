#pragma once

#include "OgreSceneManager.h"

typedef void (*FindVisibleCallback)(Ogre::SceneManager* sceneManager, Ogre::SceneManager::IlluminationRenderStage irs, Ogre::Viewport* viewport);

class NativeSceneListener : public Ogre::SceneManager::Listener
{
private:
	FindVisibleCallback preFind;
	FindVisibleCallback postFind;

public:
	NativeSceneListener(FindVisibleCallback preFind, FindVisibleCallback postFind);

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
};
