#include "StdAfx.h"
#include "NativeKeyboard.h"
#include "NativeOSWindow.h"

NativeKeyboard::NativeKeyboard(NativeOSWindow* osWindow, KeyDownDelegate keyDownCB, KeyUpDelegate keyUpCB HANDLE_ARG)
:osWindow(osWindow),
keyDownCB(keyDownCB),
keyUpCB(keyUpCB)
ASSIGN_HANDLE_INITIALIZER
{
    osWindow->setKeyboard(this);
}

NativeKeyboard::~NativeKeyboard(void)
{
    
}

//PInvoke
extern "C" _AnomalousExport NativeKeyboard* NativeKeyboard_new(NativeOSWindow* osWindow, NativeKeyboard::KeyDownDelegate keyDownCB, NativeKeyboard::KeyUpDelegate keyUpCB HANDLE_ARG)
{
	return new NativeKeyboard(osWindow, keyDownCB, keyUpCB PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void NativeKeyboard_delete(NativeKeyboard* keyboard)
{
	delete keyboard;
}