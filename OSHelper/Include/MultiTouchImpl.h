#pragma once

#include "MultiTouch.h"
#include "NativeOSWindow.h"

typedef void (*TouchEventDelegate)(TouchInfo touchInfo HANDLE_ARG);
typedef void (*TouchEventCanceledDelegate)(HANDLE_FIRST_ARG);

class MultiTouchImpl : public MultiTouch
{
public:
	MultiTouchImpl(NativeOSWindow* window, TouchEventDelegate touchStartedCB, TouchEventDelegate touchEndedCB, TouchEventDelegate touchMovedCB, TouchEventCanceledDelegate touchCanceledCB HANDLE_ARG)
		:window(window),
		touchStartedCB(touchStartedCB),
		touchEndedCB(touchEndedCB),
		touchMovedCB(touchMovedCB),
		touchCanceledCB(touchCanceledCB)
#ifdef WINDOWS
		, originalWindowFunction((WndFunc)GetWindowLongPtr((HWND)window->getHandle(), GWLP_WNDPROC))
#endif
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~MultiTouchImpl()
	{

	}

	virtual void fireTouchStarted(const TouchInfo& touchInfo)
	{
		touchStartedCB(touchInfo PASS_HANDLE_ARG);
	}

	virtual void fireTouchEnded(const TouchInfo& touchInfo)
	{
		touchEndedCB(touchInfo PASS_HANDLE_ARG);
	}

	virtual void fireTouchMoved(const TouchInfo& touchInfo)
	{
		touchMovedCB(touchInfo PASS_HANDLE_ARG);
	}

	virtual void fireAllTouchesCanceled()
	{
		touchCanceledCB(PASS_HANDLE);
	}

	#ifdef WINDOWS
		virtual LRESULT fireOriginalWindowFunc(HWND hwnd,UINT uMsg,WPARAM wParam,LPARAM lParam)
		{
			return originalWindowFunction(hwnd, uMsg, wParam, lParam);
		}
	#endif

private:
	NativeOSWindow* window;
	TouchEventDelegate touchStartedCB;
	TouchEventDelegate touchEndedCB;
	TouchEventDelegate touchMovedCB;
	TouchEventCanceledDelegate touchCanceledCB;
	#ifdef WINDOWS
		WndFunc originalWindowFunction;
	#endif
		HANDLE_INSTANCE
};
