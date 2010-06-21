#include "Stdafx.h"
#include "OIS.h"

extern "C" _AnomalousExport bool oisKeyboard_isModifierDown(OIS::Keyboard* keyboard, OIS::Keyboard::Modifier keyCode)
{
	return keyboard->isModifierDown(keyCode);
}

extern "C" _AnomalousExport const char* oisKeyboard_getAsString(OIS::Keyboard* keyboard, OIS::KeyCode code)
{
	return keyboard->getAsString(code).c_str();
}

#ifdef WINDOWS
#include "win32/Win32KeyBoard.h"
extern "C" _AnomalousExport int oisKeyboard_translateText(OIS::KeyCode kc)
{
	static WCHAR deadKey;

	BYTE keyState[256];
	HKL  layout = GetKeyboardLayout(0);
	if( GetKeyboardState(keyState) == 0 )
		return 0;

	unsigned int vk = MapVirtualKeyEx(kc, 3, layout);
	if( vk == 0 )
		return 0;

	unsigned char buff[3] = {0,0,0};
	int ascii = ToAsciiEx(vk, kc, keyState, (LPWORD) buff, 0, layout);
	//WCHAR wide[3];
	//int ascii = ToUnicodeEx(vk, kc, keyState, wide, 3, 0, layout);
	if(ascii == 1 && deadKey != '\0' )
	{
		// A dead key is stored and we have just converted a character key
		// Combine the two into a single character
		WCHAR wcBuff[3] = {buff[0], deadKey, '\0'};
		WCHAR out[3];

		deadKey = '\0';
		if(FoldStringW(MAP_PRECOMPOSED, (LPWSTR)wcBuff, 3, (LPWSTR)out, 3))
			return out[0];
	}
	else if (ascii == 1)
	{	// We have a single character
		deadKey = '\0';
		return buff[0];
	}
	else if(ascii == 2)
	{	// Convert a non-combining diacritical mark into a combining diacritical mark
		// Combining versions range from 0x300 to 0x36F; only 5 (for French) have been mapped below
		// http://www.fileformat.info/info/unicode/block/combining_diacritical_marks/images.htm
		switch(buff[0])	{
		case 0x5E: // Circumflex accent: â
			deadKey = 0x302; break;
		case 0x60: // Grave accent: à
			deadKey = 0x300; break;
		case 0xA8: // Diaeresis: ü
			deadKey = 0x308; break;
		case 0xB4: // Acute accent: é
			deadKey = 0x301; break;
		case 0xB8: // Cedilla: ç
			deadKey = 0x327; break;
		default:
			deadKey = buff[0]; break;
		}
	}

	return 0;
}
#endif

#ifdef MAC_OSX
extern "C" _AnomalousExport int oisKeyboard_translateText(OIS::KeyCode kc)
{
	return 0;
}
#endif

extern "C" _AnomalousExport void oisKeyboard_capture(OIS::Keyboard* keyboard, char* keys)
{
	keyboard->capture();
	keyboard->copyKeyStates(keys);
}