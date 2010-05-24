#include "Stdafx.h"
#include "OIS.h"

extern "C" __declspec(dllexport) bool oisKeyboard_isModifierDown(OIS::Keyboard* keyboard, OIS::Keyboard::Modifier keyCode)
{
	return keyboard->isModifierDown(keyCode);
}

extern "C" __declspec(dllexport) const char* oisKeyboard_getAsString(OIS::Keyboard* keyboard, OIS::KeyCode code)
{
	return keyboard->getAsString(code).c_str();
}

extern "C" __declspec(dllexport) void oisKeyboard_capture(OIS::Keyboard* keyboard, char* keys)
{
	keyboard->capture();
	keyboard->copyKeyStates(keys);
}