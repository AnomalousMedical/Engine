#include "StdAfx.h"

#include "MyGUI_OgreRenderManager.h"

extern "C" _AnomalousExport void OgreRenderManager_setRenderWindow(MyGUI::OgreRenderManager* renderManager, Ogre::RenderWindow* window)
{
	renderManager->setRenderWindow(window);
}

extern "C" _AnomalousExport void OgreRenderManager_setSceneManager(MyGUI::OgreRenderManager* renderManager, Ogre::SceneManager* scene)
{
	renderManager->setSceneManager(scene);
}

extern "C" _AnomalousExport size_t OgreRenderManager_getActiveViewport(MyGUI::OgreRenderManager* renderManager)
{
	return renderManager->getActiveViewport();
}

extern "C" _AnomalousExport void OgreRenderManager_setActiveViewport(MyGUI::OgreRenderManager* renderManager, size_t num)
{
	renderManager->setActiveViewport(num);
}