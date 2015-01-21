#include "StdAfx.h"
#include "../Include/NativeSceneListener.h"

NativeSceneListener::NativeSceneListener(FindVisibleCallback preFind, FindVisibleCallback postFind HANDLE_ARG)
	:preFind(preFind),
	postFind(postFind)
	ASSIGN_HANDLE_INITIALIZER
{
}

NativeSceneListener::~NativeSceneListener(void)
{
}

void NativeSceneListener::preFindVisibleObjects(Ogre::SceneManager* source, Ogre::SceneManager::IlluminationRenderStage irs, Ogre::Viewport* v)
{
	preFind(source, irs, v PASS_HANDLE_ARG);
}

void NativeSceneListener::postFindVisibleObjects(Ogre::SceneManager* source, Ogre::SceneManager::IlluminationRenderStage irs, Ogre::Viewport* v)
{
	postFind(source, irs, v PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport NativeSceneListener* NativeSceneListener_Create(FindVisibleCallback preFind, FindVisibleCallback postFind HANDLE_ARG)
{
	return new NativeSceneListener(preFind, postFind PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void NativeSceneListener_Delete(NativeSceneListener* nativeSceneListener)
{
	delete nativeSceneListener;
}