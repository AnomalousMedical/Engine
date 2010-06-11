#include "Stdafx.h"

extern "C" _AnomalousExport CEGUI::WindowManager* WindowManager_getSingletonPtr()
{
	return CEGUI::WindowManager::getSingletonPtr();
}

extern "C" _AnomalousExport CEGUI::Window* WindowManager_createWindow(CEGUI::WindowManager* windowManager, String windowType, String name)
{
	return windowManager->createWindow(windowType, name);
}

extern "C" _AnomalousExport void WindowManager_destroyWindow(CEGUI::WindowManager* windowManager, CEGUI::Window* window)
{
	windowManager->destroyWindow(window);
}

extern "C" _AnomalousExport void WindowManager_destroyWindowString(CEGUI::WindowManager* windowManager, String window)
{
	windowManager->destroyWindow(window);
}

extern "C" _AnomalousExport CEGUI::Window* WindowManager_getWindow(CEGUI::WindowManager* windowManager, String name)
{
	return windowManager->getWindow(name);
}

extern "C" _AnomalousExport bool WindowManager_isWindowPresent(CEGUI::WindowManager* windowManager, String name)
{
	return windowManager->isWindowPresent(name);
}

extern "C" _AnomalousExport void WindowManager_destroyAllWindows(CEGUI::WindowManager* windowManager)
{
	windowManager->destroyAllWindows();
}

extern "C" _AnomalousExport CEGUI::Window* WindowManager_loadWindowLayout(CEGUI::WindowManager* windowManager, String layoutName)
{
	return windowManager->loadWindowLayout(layoutName);
}

extern "C" _AnomalousExport bool WindowManager_isDeadPoolEmpty(CEGUI::WindowManager* windowManager)
{
	return windowManager->isDeadPoolEmpty();
}

extern "C" _AnomalousExport void WindowManager_cleanDeadPool(CEGUI::WindowManager* windowManager)
{
	windowManager->cleanDeadPool();
}

extern "C" _AnomalousExport void WindowManager_renameWindowString(CEGUI::WindowManager* windowManager, String window, String newName)
{
	windowManager->renameWindow(window, newName);
}

extern "C" _AnomalousExport void WindowManager_renameWindow(CEGUI::WindowManager* windowManager, CEGUI::Window* window, String newName)
{
	windowManager->renameWindow(window, newName);
}

extern "C" _AnomalousExport void WindowManager_lockChanges(CEGUI::WindowManager* windowManager)
{
	windowManager->lock();
}

extern "C" _AnomalousExport void WindowManager_unlockChanges(CEGUI::WindowManager* windowManager)
{
	windowManager->unlock();
}

extern "C" _AnomalousExport bool WindowManager_isLocked(CEGUI::WindowManager* windowManager)
{
	return windowManager->isLocked();
}

extern "C" _AnomalousExport void WindowManager_DEBUG_dumpWindowNames(CEGUI::WindowManager* windowManager, String zone)
{
	windowManager->DEBUG_dumpWindowNames(zone);
}