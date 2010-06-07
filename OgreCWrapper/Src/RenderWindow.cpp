#include "Stdafx.h"
#include <sstream>

extern "C" __declspec(dllexport) void RenderWindow_windowMovedOrResized(Ogre::RenderWindow* renderWindow)
{
	renderWindow->windowMovedOrResized();
}

typedef void (*GiveWindowStringDelegate)(String handleStr);

extern "C" __declspec(dllexport) void RenderWindow_getWindowHandleStr(Ogre::RenderWindow* renderWindow, GiveWindowStringDelegate giveWindowString)
{
	size_t winHandle = 0;
	renderWindow->getCustomAttribute("WINDOW", &winHandle);
	std::stringstream winHandleStr;
	winHandleStr << winHandle;
	giveWindowString(winHandleStr.str().c_str());
}