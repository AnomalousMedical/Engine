#include "Stdafx.h"

extern "C" _AnomalousExport CEGUI::WindowManager* WindowManager_getSingletonPtr()
{
	return CEGUI::WindowManager::getSingletonPtr();
}

extern "C" _AnomalousExport CEGUI::Window* WindowManager_loadWindowLayout(CEGUI::WindowManager* windowManager, String layoutName)
{
	return windowManager->loadWindowLayout(layoutName);
}