#include "Stdafx.h"

extern "C" _AnomalousExport MyGUI::RenderManager* RenderManager_getInstance()
{
	return MyGUI::RenderManager::getInstancePtr();
}

extern "C" _AnomalousExport int RenderManager_getViewWidth(MyGUI::RenderManager* renderManager)
{
	return renderManager->getViewSize().width;
}

extern "C" _AnomalousExport int RenderManager_getViewHeight(MyGUI::RenderManager* renderManager)
{
	return renderManager->getViewSize().height;
}

extern "C" _AnomalousExport void RenderManager_destroyTextureByName(MyGUI::RenderManager* renderManager, String name)
{
	MyGUI::ITexture* texture = renderManager->getTexture(name);
	if(texture != nullptr)
	{
		renderManager->destroyTexture(texture);
	}
}