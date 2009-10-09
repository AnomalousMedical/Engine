#pragma once

#define WIN32_LEAN_AND_MEAN
#include "windows.h"

namespace Engine
{

namespace Platform
{

public ref class Win32UpdateTimer : UpdateTimer
{
private:
	HACCEL hAccel;
	HWND hWnd;
	HINSTANCE hInstance;

public:
	Win32UpdateTimer(SystemTimer^ systemTimer);

	virtual ~Win32UpdateTimer(void);

	virtual bool startLoop() override;

	void setWindowHandle(OSWindow^ window);
};

}

}