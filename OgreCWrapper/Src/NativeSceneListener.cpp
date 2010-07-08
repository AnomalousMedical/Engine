#include "StdAfx.h"
#include "../Include/NativeSceneListener.h"

NativeSceneListener::NativeSceneListener(FindVisibleCallback preFind, FindVisibleCallback postFind)
:preFind(preFind),
postFind(postFind)
{
}

NativeSceneListener::~NativeSceneListener(void)
{
}

void NativeSceneListener::preFindVisibleObjects(Ogre::SceneManager* source, Ogre::SceneManager::IlluminationRenderStage irs, Ogre::Viewport* v)
{
	preFind(source, irs, v);
}

void NativeSceneListener::postFindVisibleObjects(Ogre::SceneManager* source, Ogre::SceneManager::IlluminationRenderStage irs, Ogre::Viewport* v)
{
	postFind(source, irs, v);
}

extern "C" _AnomalousExport NativeSceneListener* NativeSceneListener_Create(FindVisibleCallback preFind, FindVisibleCallback postFind)
{
	return new NativeSceneListener(preFind, postFind);
}

extern "C" _AnomalousExport void NativeSceneListener_Delete(NativeSceneListener* nativeSceneListener)
{
	delete nativeSceneListener;
}