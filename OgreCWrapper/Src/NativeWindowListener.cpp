#include "StdAfx.h"
#include "../Include/NativeWindowListener.h"

NativeWindowListener::NativeWindowListener(WindowEventDelegate windowMovedCallback, WindowEventDelegate windowResizedCallback, WindowClosingDelegate windowClosingCallback, WindowEventDelegate windowClosedCallback, WindowEventDelegate windowFocusChangeCallback)
:windowMovedCallback(windowMovedCallback), 
windowResizedCallback(windowResizedCallback), 
windowClosingCallback(windowClosingCallback), 
windowClosedCallback(windowClosedCallback), 
windowFocusChangeCallback(windowFocusChangeCallback)
{
}

NativeWindowListener::~NativeWindowListener(void)
{
}

void NativeWindowListener::windowMoved (Ogre::RenderWindow *rw)
{
	windowMovedCallback(rw);
}

void NativeWindowListener::windowResized (Ogre::RenderWindow *rw)
{
	windowResizedCallback(rw);
}

bool NativeWindowListener::windowClosing (Ogre::RenderWindow *rw)
{
	return windowClosingCallback(rw);
}

void NativeWindowListener::windowClosed (Ogre::RenderWindow *rw)
{
	windowClosedCallback(rw);
}

void NativeWindowListener::windowFocusChange (Ogre::RenderWindow *rw)
{
	windowFocusChangeCallback(rw);
}

extern "C" _AnomalousExport NativeWindowListener* NativeWindowListener_Create(WindowEventDelegate windowMovedCallback, WindowEventDelegate windowResizedCallback, WindowClosingDelegate windowClosingCallback, WindowEventDelegate windowClosedCallback, WindowEventDelegate windowFocusChangeCallback)
{
	return new NativeWindowListener(windowMovedCallback, windowResizedCallback, windowClosingCallback, windowClosedCallback, windowFocusChangeCallback);
}

extern "C" _AnomalousExport void NativeWindowListener_Delete(NativeWindowListener* windowListener)
{
	delete windowListener;
}