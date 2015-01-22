#include "StdAfx.h"
#include "NativeMouse.h"
#include "NativeOSWindow.h"

NativeMouse::NativeMouse(NativeOSWindow* osWindow, MouseButtonDownDelegate mouseButtonDownCB, MouseButtonUpDelegate mouseButtonUpCB, MouseMoveDelegate mouseMoveCB, MouseWheelDelegate mouseWheelCB HANDLE_ARG)
:mouseButtonDownCB(mouseButtonDownCB),
mouseButtonUpCB(mouseButtonUpCB),
mouseMoveCB(mouseMoveCB),
mouseWheelCB(mouseWheelCB),
osWindow(osWindow)
ASSIGN_HANDLE_INITIALIZER
{
    osWindow->setMouse(this);
}

NativeMouse::~NativeMouse()
{
    
}

//PInvoke
extern "C" _AnomalousExport NativeMouse* NativeMouse_new(NativeOSWindow* osWindow, NativeMouse::MouseButtonDownDelegate mouseButtonDownCB, NativeMouse::MouseButtonUpDelegate mouseButtonUpCB, NativeMouse::MouseMoveDelegate mouseMoveCB, NativeMouse::MouseWheelDelegate mouseWheelCB HANDLE_ARG)
{
	return new NativeMouse(osWindow, mouseButtonDownCB, mouseButtonUpCB, mouseMoveCB, mouseWheelCB PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void NativeMouse_delete(NativeMouse* mouse)
{
	delete mouse;
}