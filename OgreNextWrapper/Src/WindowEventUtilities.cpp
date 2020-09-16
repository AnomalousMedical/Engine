#include "Stdafx.h"

extern "C" _AnomalousExport void WindowEventUtilities_addWindowEventListener(Ogre::RenderWindow* window, Ogre::WindowEventListener* listener)
{
	Ogre::WindowEventUtilities::addWindowEventListener(window, listener);
}

extern "C" _AnomalousExport void WindowEventUtilities_removeWindowEventListener(Ogre::RenderWindow* window, Ogre::WindowEventListener* listener)
{
	Ogre::WindowEventUtilities::removeWindowEventListener(window, listener);
}