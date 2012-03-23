#include "Stdafx.h"

extern "C" _AnomalousExport MyGUI::RenderManager* RenderManager_getInstance()
{
	return MyGUI::RenderManager::getInstancePtr();
}

extern "C" _AnomalousExport void RenderManager_manualFrameEvent(MyGUI::RenderManager* renderManager, float time)
{
	return renderManager->manualFrameEvent(time);
}

extern "C" _AnomalousExport int RenderManager_getViewWidth(MyGUI::RenderManager* renderManager)
{
	return renderManager->getViewSize().width;
}

extern "C" _AnomalousExport int RenderManager_getViewHeight(MyGUI::RenderManager* renderManager)
{
	return renderManager->getViewSize().height;
}