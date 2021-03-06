#include "stdafx.h"
#include "MultiTouch.h"
#include "MultiTouchImpl.h"
#include "NativeOSWindow.h"

#ifdef WINDOWS
#define MULTITOUCH 1
#endif

#if defined(MAC_OSX) || defined(APPLE_IOS) || defined(ANDROID)
#define MULTITOUCH 1
#endif

#if MULTITOUCH != 0

#ifdef WINDOWS

HMODULE mtDriver = NULL;
WINDOWS_REGISTRATION_FUNC registerWithWindows = NULL;

extern "C" _AnomalousExport MultiTouch* MultiTouch_new(NativeOSWindow* osWindow, TouchEventDelegate touchStartedCB, TouchEventDelegate touchEndedCB, TouchEventDelegate touchMovedCB, TouchEventCanceledDelegate touchCanceledCB HANDLE_ARG)
{
	MultiTouch* multiTouch = new MultiTouchImpl(osWindow, touchStartedCB, touchEndedCB, touchMovedCB, touchCanceledCB PASS_HANDLE_ARG);
	registerWithWindows((HWND)osWindow->getHandle(), multiTouch);
	return multiTouch;
}

extern "C" _AnomalousExport void MultiTouch_delete(MultiTouch* multiTouch)
{
	delete multiTouch;
}

extern "C" _AnomalousExport bool MultiTouch_isMultitouchAvailable()
{
	bool loaded = false;
	if(mtDriver == NULL)
	{
		mtDriver = LoadLibraryEx(L"WinMTDriver.dll", NULL, 0);
		if(mtDriver != NULL)
		{
			registerWithWindows = (WINDOWS_REGISTRATION_FUNC)GetProcAddress(mtDriver, "registerWithWindows");
			if(registerWithWindows != NULL)
			{
				loaded = true;
			}
		}
	}
	else if(registerWithWindows != NULL)
	{
		loaded = true;
	}
	return loaded;
}

#endif //WINDOWS

#if defined(MAC_OSX) || defined(APPLE_IOS) || defined(ANDROID)

extern "C" _AnomalousExport MultiTouch* MultiTouch_new(NativeOSWindow* window, TouchEventDelegate touchStartedCB, TouchEventDelegate touchEndedCB, TouchEventDelegate touchMovedCB, TouchEventCanceledDelegate touchCanceledCB HANDLE_ARG)
{
	MultiTouch* multiTouch = new MultiTouchImpl(window, touchStartedCB, touchEndedCB, touchMovedCB, touchCanceledCB PASS_HANDLE_ARG);
	window->setupMultitouch(multiTouch);
	return multiTouch;
}

extern "C" _AnomalousExport void MultiTouch_delete(MultiTouch* multiTouch)
{
	delete multiTouch;
}

extern "C" _AnomalousExport bool MultiTouch_isMultitouchAvailable()
{
	return true;
}

#endif //defined(MAC_OSX) || defined(APPLE_IOS) || defined(ANDROID)

#else //Else from MULTITOUCH

extern "C" _AnomalousExport MultiTouch* MultiTouch_new(NativeOSWindow* window, TouchEventDelegate touchStartedCB, TouchEventDelegate touchEndedCB, TouchEventDelegate touchMovedCB, TouchEventCanceledDelegate touchCanceledCB)
{
    return NULL;
}

extern "C" _AnomalousExport void MultiTouch_delete(MultiTouch* multiTouch)
{
    
}

extern "C" _AnomalousExport bool MultiTouch_isMultitouchAvailable()
{
    return false;
}

#endif //MULTITOUCH Else