#include "Stdafx.h"

#include <sstream>

extern "C" _AnomalousExport void RenderWindow_windowMovedOrResized(Ogre::RenderWindow* renderWindow)
{
	renderWindow->windowMovedOrResized();
}

typedef void (*GiveWindowStringDelegate)(String handleStr);

extern "C" _AnomalousExport void RenderWindow_getWindowHandleStr(Ogre::RenderWindow* renderWindow, GiveWindowStringDelegate giveWindowString)
{
	size_t winHandle = 0;
	renderWindow->getCustomAttribute("WINDOW", &winHandle);
	std::stringstream winHandleStr;
	winHandleStr << winHandle;
	giveWindowString(winHandleStr.str().c_str());
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