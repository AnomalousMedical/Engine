#include "StdAfx.h"
#include "NativeOSWindow.h"
#include "NativeKeyboard.h"
#include "NativeMouse.h"

NativeOSWindow::NativeOSWindow()
:keyboard(0),
mouse(0),
exclusiveFullscreen(false)
{

}

NativeOSWindow::~NativeOSWindow(void)
{
	deleteCB(PASS_HANDLE);
}

void NativeOSWindow::setCallbacks(NativeAction deleteCB, NativeAction sizedCB, NativeAction closingCB, NativeAction closedCB, ActivateCB activateCB, ModifyResourcesCB createInternalResourcesCB, ModifyResourcesCB destroyInternalResourcesCB HANDLE_ARG)
{
	this->deleteCB = deleteCB;
	this->sizedCB = sizedCB;
	this->closingCB = closingCB;
	this->closedCB = closedCB;
	this->activateCB = activateCB;
	this->createInternalResourcesCB = createInternalResourcesCB;
	this->destroyInternalResourcesCB = destroyInternalResourcesCB;
	ASSIGN_HANDLE
}

void NativeOSWindow::setKeyboard(NativeKeyboard* keyboard)
{
	this->keyboard = keyboard;
}

void NativeOSWindow::fireKeyDown(KeyboardButtonCode keyCode, uint character)
{
	if (keyboard != 0)
	{
		keyboard->fireKeyDown(keyCode, character);
	}
}

void NativeOSWindow::fireKeyUp(KeyboardButtonCode keyCode)
{
	if (keyboard != 0)
	{
		keyboard->fireKeyUp(keyCode);
	}
}

void NativeOSWindow::setMouse(NativeMouse* mouse)
{
	this->mouse = mouse;
}

void NativeOSWindow::fireMouseButtonDown(MouseButtonCode id)
{
	if (mouse != 0)
	{
		mouse->fireMouseButtonDown(id);
	}
}

void NativeOSWindow::fireMouseButtonUp(MouseButtonCode id)
{
	if (mouse != 0)
	{
		mouse->fireMouseButtonUp(id);
	}
}

void NativeOSWindow::fireMouseMove(int absX, int absY)
{
	if (mouse != 0)
	{
		mouse->fireMouseMove(absX, absY);
	}
}

void NativeOSWindow::fireMouseWheel(int relZ)
{
	if (mouse != 0)
	{
		mouse->fireMouseWheel(relZ);
	}
}

//Shared Pinvoke
extern "C" _AnomalousExport void NativeOSWindow_destroy(NativeOSWindow* nativeWindow)
{
	delete nativeWindow;
}

extern "C" _AnomalousExport void NativeOSWindow_setTitle(NativeOSWindow* nativeWindow, String title)
{
	nativeWindow->setTitle(title);
}

extern "C" _AnomalousExport void NativeOSWindow_setSize(NativeOSWindow* nativeWindow, int width, int height)
{
    nativeWindow->setSize(width, height);
}

extern "C" _AnomalousExport int NativeOSWindow_getWidth(NativeOSWindow* nativeWindow)
{
    return nativeWindow->getWidth();
}

extern "C" _AnomalousExport int NativeOSWindow_getHeight(NativeOSWindow* nativeWindow)
{
    return nativeWindow->getHeight();
}

extern "C" _AnomalousExport void* NativeOSWindow_getHandle(NativeOSWindow* nativeWindow)
{
    return nativeWindow->getHandle();
}

extern "C" _AnomalousExport void NativeOSWindow_show(NativeOSWindow* nativeWindow)
{
    nativeWindow->show();
}

extern "C" _AnomalousExport void NativeOSWindow_close(NativeOSWindow* nativeWindow)
{
    nativeWindow->close();
}

extern "C" _AnomalousExport void NativeOSWindow_setMaximized(NativeOSWindow* nativeWindow, bool maximize)
{
	nativeWindow->setMaximized(maximize);
}

extern "C" _AnomalousExport bool NativeOSWindow_getMaximized(NativeOSWindow* nativeWindow)
{
	return nativeWindow->getMaximized();
}

extern "C" _AnomalousExport void NativeOSWindow_setExclusiveFullscreen(NativeOSWindow* nativeWindow, bool exclusiveFullscreen)
{
	nativeWindow->setExclusiveFullscreen(exclusiveFullscreen);
}

extern "C" _AnomalousExport bool NativeOSWindow_getExclusiveFullscreen(NativeOSWindow* nativeWindow)
{
	return nativeWindow->getExclusiveFullscreen();
}

extern "C" _AnomalousExport void NativeOSWindow_setCursor(NativeOSWindow* nativeWindow, CursorType cursor)
{
	nativeWindow->setCursor(cursor);
}

extern "C" _AnomalousExport float NativeOSWindow_getWindowScaling(NativeOSWindow* nativeWindow)
{
	return nativeWindow->getWindowScaling();
}

extern "C" _AnomalousExport void NativeOSWindow_toggleFullscreen(NativeOSWindow* nativeWindow)
{
	return nativeWindow->toggleFullscreen();
}

extern "C" _AnomalousExport void NativeOSWindow_toggleBorderless(NativeOSWindow* nativeWindow)
{
	return nativeWindow->toggleBorderless();
}

extern "C" _AnomalousExport void NativeOSWindow_setOnscreenKeyboardMode(NativeOSWindow* nativeWindow, OnscreenKeyboardMode mode)
{
	nativeWindow->setOnscreenKeyboardMode(mode);
}

extern "C" _AnomalousExport OnscreenKeyboardMode NativeOSWindow_getOnscreenKeyboardMode(NativeOSWindow* nativeWindow)
{
	return nativeWindow->getOnscreenKeyboardMode();
}

extern "C" _AnomalousExport void NativeOSWindow_setCallbacks(NativeOSWindow* nativeWindow, NativeAction deleteCB, NativeAction sizedCB, NativeAction closingCB, NativeAction closedCB, ActivateCB activateCB, ModifyResourcesCB createInternalResourcesCB, ModifyResourcesCB destroyInternalResourcesCB HANDLE_ARG)
{
	nativeWindow->setCallbacks(deleteCB, sizedCB, closingCB, closedCB, activateCB, createInternalResourcesCB, destroyInternalResourcesCB PASS_HANDLE_ARG);
}