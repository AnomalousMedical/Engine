#include "Stdafx.h"

//Windows
#define WINVER 0x0500
#define WIN32_LEAN_AND_MEAN
#define NOMINMAX
#include <windows.h>
#include "Windowsx.h"
#include "KeyboardButtonCode.h"

//Taken from CEGUI wiki
//http://www.cegui.org.uk/wiki/index.php/DirectInput_to_CEGUI_utf32
uint keycodeToUTF32(unsigned int scanCode)
{
	uint utf = 0;

	BYTE keyboardState[256];
	unsigned char ucBuffer[3];
	static WCHAR deadKey = '\0';

	// Retrieve the keyboard layout in order to perform the necessary convertions
	HKL hklKeyboardLayout = GetKeyboardLayout(0); // 0 means current thread 
	// This seemingly cannot fail 
	// If this value is cached then the application must respond to WM_INPUTLANGCHANGE 

	// Retrieve the keyboard state
	// Handles CAPS-lock and SHIFT states
	if (GetKeyboardState(keyboardState) == FALSE)
		return utf;

	/* 0. Convert virtual-key code into a scan code
	1. Convert scan code into a virtual-key code
	Does not distinguish between left- and right-hand keys.
	2. Convert virtual-key code into an unshifted character value
	in the low order word of the return value. Dead keys (diacritics)
	are indicated by setting the top bit of the return value.
	3. Windows NT/2000/XP: Convert scan code into a virtual-key
	Distinguishes between left- and right-hand keys.*/
	UINT virtualKey = MapVirtualKeyEx(scanCode, 3, hklKeyboardLayout);
	if (virtualKey == 0) // No translation possible
		return utf;

	/* Parameter 5:
	0. No menu is active
	1. A menu is active
	Return values:
	Negative. Returned a dead key
	0. No translation available
	1. A translation exists
	2. Dead-key could not be combined with character 	*/
	int ascii = ToAsciiEx(virtualKey, scanCode, keyboardState, (LPWORD)ucBuffer, 0, hklKeyboardLayout);
	if (deadKey != '\0' && ascii == 1)
	{
		// A dead key is stored and we have just converted a character key
		// Combine the two into a single character
		WCHAR wcBuffer[3];
		WCHAR out[3];
		wcBuffer[0] = ucBuffer[0];
		wcBuffer[1] = deadKey;
		wcBuffer[2] = '\0';
		if (FoldStringW(MAP_PRECOMPOSED, (LPWSTR)wcBuffer, 3, (LPWSTR)out, 3))
			utf = out[0];
		else
		{
			// FoldStringW failed
			DWORD dw = GetLastError();
			switch (dw)
			{
			case ERROR_INSUFFICIENT_BUFFER:
			case ERROR_INVALID_FLAGS:
			case ERROR_INVALID_PARAMETER:
				break;
			}
		}
		deadKey = '\0';
	}
	else if (ascii == 1)
	{
		// We have a single character
		utf = ucBuffer[0];
		deadKey = '\0';
	}
	else
	{
		// Convert a non-combining diacritical mark into a combining diacritical mark
		switch (ucBuffer[0])
		{
		case 0x5E: // Circumflex accent: â
			deadKey = 0x302;
			break;
		case 0x60: // Grave accent: � 
			deadKey = 0x300;
			break;
		case 0xA8: // Diaeresis: ü
			deadKey = 0x308;
			break;
		case 0xB4: // Acute accent: é
			deadKey = 0x301;
			break;
		case 0xB8: // Cedilla: ç
			deadKey = 0x327;
			break;
		default:
			deadKey = ucBuffer[0];
		}
	}

	return utf;
}

uint getUtf32WithSpecial(WPARAM virtualKey, unsigned int scanCode)
{
	switch (virtualKey)
	{
	case VK_NUMPAD0:
		return 48;
	case VK_NUMPAD1:
		return 49;
	case VK_NUMPAD2:
		return 50;
	case VK_NUMPAD3:
		return 51;
	case VK_NUMPAD4:
		return 52;
	case VK_NUMPAD5:
		return 53;
	case VK_NUMPAD6:
		return 54;
	case VK_NUMPAD7:
		return 55;
	case VK_NUMPAD8:
		return 56;
	case VK_NUMPAD9:
		return 57;
	case VK_DIVIDE:
		return 47;
	case VK_DECIMAL:
		return 46;
	default:
		return keycodeToUTF32(scanCode);
	}
}

extern "C" _AnomalousExport uint InputManager_getUtf32(WPARAM wParam, LPARAM lParam)
{
	return getUtf32WithSpecial(wParam, (lParam & 0x01FF0000) >> 16);
}

#include "Win32KeyMap.h"

extern "C" _AnomalousExport KeyboardButtonCode InputManager_virtualKeyToKeyboardButtonCode(WPARAM wParam)
{
	if (wParam < KEY_MAP_MAX)
	{
		return keyMap[wParam];
	}
	return KC_UNASSIGNED;
}

extern "C" _AnomalousExport POINT GetDesktopCoordinates(HWND parent, HWND child, POINT point)
{
	MapWindowPoints(child, parent, (LPPOINT)&point, 1);
	return point;
}