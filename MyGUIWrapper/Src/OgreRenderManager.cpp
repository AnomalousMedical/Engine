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

extern "C" _AnomalousExport void OgreRenderManager_windowMovedOrResized(MyGUI::OgreRenderManager* renderManager)
{
	renderManager->windowMovedOrResized();
}

extern "C" _AnomalousExport void OgreRenderManager_destroyTextureString(MyGUI::OgreRenderManager* renderManager, const char* name)
{
	MyGUI::ITexture* texture = renderManager->getTexture(name);
	if(texture != nullptr)
	{
		renderManager->destroyTexture(texture);
	}
}