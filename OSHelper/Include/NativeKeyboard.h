#pragma once

#include "KeyboardButtonCode.h"

class NativeOSWindow;

class NativeKeyboard
{
public:
	typedef void(*KeyDownDelegate)(KeyboardButtonCode keyCode, uint character HANDLE_ARG);
	typedef void(*KeyUpDelegate)(KeyboardButtonCode keyCode HANDLE_ARG);
    
	NativeKeyboard(NativeOSWindow* osWindow, KeyDownDelegate keyDownCB, KeyUpDelegate keyUpCB HANDLE_ARG);
    
	virtual ~NativeKeyboard(void);

	void fireKeyDown(KeyboardButtonCode keyCode, uint character)
	{
		keyDownCB(keyCode, character PASS_HANDLE_ARG);
	}

	void fireKeyUp(KeyboardButtonCode keyCode)
	{
		keyUpCB(keyCode PASS_HANDLE_ARG);
	}
    
private:
	KeyDownDelegate keyDownCB;
	KeyUpDelegate keyUpCB;
	NativeOSWindow* osWindow;
	HANDLE_INSTANCE
};