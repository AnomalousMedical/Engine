#include "StdAfx.h"
#include "NativeOSWindow.h"

NativeOSWindow::NativeOSWindow()
:keyDownCB(0),
keyUpCB(0),
mouseButtonDownCB(0),
mouseButtonUpCB(0),
mouseMoveCB(0),
mouseWheelCB(0),
exclusiveFullscreen(false)
{

}

NativeOSWindow::~NativeOSWindow(void)
{
	deleteCB();
}

void NativeOSWindow::setCallbacks(NativeAction deleteCB, NativeAction sizedCB, NativeAction closingCB, NativeAction closedCB, NativeAction_Bool activateCB HANDLE_ARG)
{
	this->deleteCB = deleteCB;
	this->sizedCB = sizedCB;
	this->closingCB = closingCB;
	this->closedCB = closedCB;
	this->activateCB = activateCB;
	ASSIGN_HANDLE
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

extern "C" _AnomalousExport void NativeOSWindow_setCallbacks(NativeOSWindow* nativeWindow, NativeAction deleteCB, NativeAction sizedCB, NativeAction closingCB, NativeAction closedCB, NativeAction_Bool activateCB HANDLE_ARG)
{
	nativeWindow->setCallbacks(deleteCB, sizedCB, closingCB, closedCB, activateCB PASS_HANDLE_ARG);
}