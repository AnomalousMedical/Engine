#pragma once

#include "MouseButtonCode.h"

class NativeOSWindow;

class NativeMouse
{
public:
	typedef void(*MouseButtonDownDelegate)(MouseButtonCode id HANDLE_ARG);
	typedef void(*MouseButtonUpDelegate)(MouseButtonCode id HANDLE_ARG);
	typedef void(*MouseMoveDelegate)(int absX, int absY HANDLE_ARG);
	typedef void(*MouseWheelDelegate)(int relZ HANDLE_ARG);
    
	NativeMouse(NativeOSWindow* osWindow, MouseButtonDownDelegate mouseButtonDownCB, MouseButtonUpDelegate mouseButtonUpCB, MouseMoveDelegate mouseMoveCB, MouseWheelDelegate mouseWheelCB HANDLE_ARG);
    
	virtual ~NativeMouse();

	void fireMouseButtonDown(MouseButtonCode id)
	{
		mouseButtonDownCB(id PASS_HANDLE_ARG);
	}

	void fireMouseButtonUp(MouseButtonCode id)
	{
		mouseButtonUpCB(id PASS_HANDLE_ARG);
	}

	void fireMouseMove(int absX, int absY)
	{
		mouseMoveCB(absX, absY PASS_HANDLE_ARG);
	}

	void fireMouseWheel(int relZ)
	{
		mouseWheelCB(relZ PASS_HANDLE_ARG);
	}
    
private:
	MouseButtonDownDelegate mouseButtonDownCB;
	MouseButtonUpDelegate mouseButtonUpCB;
	MouseMoveDelegate mouseMoveCB;
	MouseWheelDelegate mouseWheelCB;
	NativeOSWindow* osWindow;
	HANDLE_INSTANCE
};
