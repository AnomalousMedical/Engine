#include "StdAfx.h"

#include "MyGUI_OgreRenderManager.h"

extern "C" _AnomalousExport void OgreRenderManager_windowResized(MyGUI::OgreRenderManager* renderManager, int windowWidth, int windowHeight)
{
	renderManager->windowResized(windowWidth, windowHeight);
}

extern "C" _AnomalousExport void OgreRenderManager_destroyTextureString(MyGUI::OgreRenderManager* renderManager, const char* name)
{
	MyGUI::ITexture* texture = renderManager->getTexture(name);
	if(texture != nullptr)
	{
		renderManager->destroyTexture(texture);
	}
}

extern "C" _AnomalousExport void OgreRenderManager_update(MyGUI::OgreRenderManager* renderManager)
{
	renderManager->update();
}