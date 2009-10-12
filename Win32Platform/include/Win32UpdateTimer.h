#pragma once

#define WIN32_LEAN_AND_MEAN
#include "windows.h"

using namespace System;

namespace Engine
{

namespace Platform
{

[System::Runtime::InteropServices::StructLayout(System::Runtime::InteropServices::LayoutKind::Sequential)]
public value struct WinMsg
{
internal:
	WinMsg(const MSG& msg)
	{
		hwnd = IntPtr(msg.hwnd);
		message = msg.message;
		wParam = IntPtr((void*)msg.wParam);
		lParam = IntPtr(msg.lParam);
		time = msg.time;
		pt_x = msg.pt.x;
		pt_y = msg.pt.y;
	}

public:
	IntPtr hwnd;
	int message;
	IntPtr wParam;
	IntPtr lParam;
	int time;
	int pt_x;
	int pt_y;
};

public delegate void PumpMessageEvent(WinMsg% message);

public ref class Win32UpdateTimer : UpdateTimer
{
private:
	HACCEL hAccel;
	HWND hWnd;
	HINSTANCE hInstance;

public:
	event PumpMessageEvent^ MessageReceived;

	Win32UpdateTimer(SystemTimer^ systemTimer);

	virtual ~Win32UpdateTimer(void);

	virtual bool startLoop() override;

	void setWindowHandle(OSWindow^ window);
};

}

}