#include "Stdafx.h"

extern "C" _AnomalousExport MyGUI::RenderManager* RenderManager_getInstance()
{
	return MyGUI::RenderManager::getInstancePtr();
}

extern "C" _AnomalousExport void RenderManager_manualFrameEvent(MyGUI::RenderManager* renderManager, float time)
{
	return renderManager->manualFrameEvent(time);
}