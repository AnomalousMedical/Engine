#include "Stdafx.h"

#include <sstream>

extern "C" _AnomalousExport void RenderWindow_destroy(Ogre::RenderWindow* renderWindow)
{
	renderWindow->destroy();
}

extern "C" _AnomalousExport void RenderWindow_windowMovedOrResized(Ogre::RenderWindow* renderWindow)
{
	renderWindow->windowMovedOrResized();
}

extern "C" _AnomalousExport void RenderWindow_getWindowHandleStr(Ogre::RenderWindow* renderWindow, StringRetrieverCallback stringCb, void* handle)
{
	size_t winHandle = 0;
	renderWindow->getCustomAttribute("WINDOW", &winHandle);
	std::stringstream winHandleStr;
	winHandleStr << winHandle;
	stringCb(winHandleStr.str().c_str(), handle);
}

extern "C" _AnomalousExport void RenderWindow_setFullscreen(Ogre::RenderWindow* renderWindow, bool fullscreen, uint width, uint height)
{
	renderWindow->setFullscreen(fullscreen, width, height);
}

extern "C" _AnomalousExport bool RenderWindow_isDeactivatedOnFocusChange(Ogre::RenderWindow* renderWindow)
{
	return renderWindow->isDeactivatedOnFocusChange();
}

extern "C" _AnomalousExport void RenderWindow_setDeactivatedOnFocusChange(Ogre::RenderWindow* renderWindow, bool deactivate)
{
	renderWindow->setDeactivateOnFocusChange(deactivate);
}

extern "C" _AnomalousExport bool RenderWindow_isVisible(Ogre::RenderWindow* renderWindow)
{
	return renderWindow->isVisible();
}

extern "C" _AnomalousExport void RenderWindow_setVisible(Ogre::RenderWindow* renderWindow, bool visible)
{
	renderWindow->setVisible(visible);
}