#include "Stdafx.h"

extern "C" __declspec(dllexport) void RenderWindow_windowMovedOrResized(Ogre::RenderWindow* renderWindow)
{
	renderWindow->windowMovedOrResized();
}