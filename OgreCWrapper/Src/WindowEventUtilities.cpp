#include "Stdafx.h"

extern "C" __declspec(dllexport) void WindowEventUtilities_addWindowEventListener(Ogre::RenderWindow* window, Ogre::WindowEventListener* listener)
{
	Ogre::WindowEventUtilities::addWindowEventListener(window, listener);
}

extern "C" __declspec(dllexport) void WindowEventUtilities_removeWindowEventListener(Ogre::RenderWindow* window, Ogre::WindowEventListener* listener)
{
	Ogre::WindowEventUtilities::removeWindowEventListener(window, listener);
}