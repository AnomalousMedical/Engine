#include "StdAfx.h"
#include "../Include/NativeWindowListener.h"

NativeWindowListener::NativeWindowListener(WindowEventDelegate windowMovedCallback, WindowEventDelegate windowResizedCallback, WindowClosingDelegate windowClosingCallback, WindowEventDelegate windowClosedCallback, WindowEventDelegate windowFocusChangeCallback HANDLE_ARG)
:windowMovedCallback(windowMovedCallback), 
windowResizedCallback(windowResizedCallback), 
windowClosingCallback(windowClosingCallback), 
windowClosedCallback(windowClosedCallback), 
windowFocusChangeCallback(windowFocusChangeCallback)
ASSIGN_HANDLE_INITIALIZER
{
}

NativeWindowListener::~NativeWindowListener(void)
{
}

void NativeWindowListener::windowMoved (Ogre::RenderWindow *rw)
{
	windowMovedCallback(rw PASS_HANDLE_ARG);
}

void NativeWindowListener::windowResized (Ogre::RenderWindow *rw)
{
	windowResizedCallback(rw PASS_HANDLE_ARG);
}

bool NativeWindowListener::windowClosing (Ogre::RenderWindow *rw)
{
	return windowClosingCallback(rw PASS_HANDLE_ARG);
}

void NativeWindowListener::windowClosed (Ogre::RenderWindow *rw)
{
	windowClosedCallback(rw PASS_HANDLE_ARG);
}

void NativeWindowListener::windowFocusChange (Ogre::RenderWindow *rw)
{
	windowFocusChangeCallback(rw PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport NativeWindowListener* NativeWindowListener_Create(WindowEventDelegate windowMovedCallback, WindowEventDelegate windowResizedCallback, WindowClosingDelegate windowClosingCallback, WindowEventDelegate windowClosedCallback, WindowEventDelegate windowFocusChangeCallback HANDLE_ARG)
{
	return new NativeWindowListener(windowMovedCallback, windowResizedCallback, windowClosingCallback, windowClosedCallback, windowFocusChangeCallback PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void NativeWindowListener_Delete(NativeWindowListener* windowListener)
{
	delete windowListener;
}