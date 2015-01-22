#pragma once

#include "KeyboardButtonCode.h"

class NativeOSWindow;

class NativeKeyboard
{
public:
	typedef void (*KeyDownDelegate)(KeyboardButtonCode keyCode, uint character);
	typedef void (*KeyUpDelegate)(KeyboardButtonCode keyCode);
    
	NativeKeyboard(NativeOSWindow* osWindow, KeyDownDelegate keyDownCB, KeyUpDelegate keyUpCB);
    
	virtual ~NativeKeyboard(void);

	void fireKeyDown(KeyboardButtonCode keyCode, uint character)
	{
		keyDownCB(keyCode, character);
	}

	void fireKeyUp(KeyboardButtonCode keyCode)
	{
		keyUpCB(keyCode);
	}
    
private:
	KeyDownDelegate keyDownCB;
	KeyUpDelegate keyUpCB;
	NativeOSWindow* osWindow;
};