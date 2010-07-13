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

extern "C" _AnomalousExport void oisKeyboard_capture(OIS::Keyboard* keyboard, char* keys)
{
	keyboard->capture();
	keyboard->copyKeyStates(keys);
}

typedef void (*KeyCallback) (OIS::KeyCode key, unsigned int text);

class BufferedKeyListener : public OIS::KeyListener
{
private:
	KeyCallback keyPressedCb;
	KeyCallback keyReleasedCb;

public:
	BufferedKeyListener(KeyCallback keyPressedCb, KeyCallback keyReleasedCb)
		:keyPressedCb(keyPressedCb),
		keyReleasedCb(keyReleasedCb)
	{

	}

	virtual ~BufferedKeyListener()
	{

	}
	
	virtual bool keyPressed( const OIS::KeyEvent &arg )
	{
		keyPressedCb(arg.key, arg.text);
		return true;
	}
	
	virtual bool keyReleased( const OIS::KeyEvent &arg )
	{
		keyReleasedCb(arg.key, arg.text);
		return true;
	}
};

extern "C" _AnomalousExport BufferedKeyListener* oisKeyboard_createListener(OIS::Keyboard* keyboard, KeyCallback keyPressedCb, KeyCallback keyReleasedCb)
{
	BufferedKeyListener* keyListener = new BufferedKeyListener(keyPressedCb, keyReleasedCb);
	keyboard->setEventCallback(keyListener);
	return keyListener;
}

extern "C" _AnomalousExport void oisKeyboard_destroyListener(OIS::Keyboard* keyboard, BufferedKeyListener* listener)
{
	keyboard->setEventCallback(0);
	delete listener;
}


extern "C" _AnomalousExport void oisKeyboard_setTextTranslationMode(OIS::Keyboard* keyboard, OIS::Keyboard::TextTranslationMode mode)
{
	keyboard->setTextTranslation(mode);
}

extern "C" _AnomalousExport OIS::Keyboard::TextTranslationMode oisKeyboard_getTextTranslationMode(OIS::Keyboard* keyboard)
{
	return keyboard->getTextTranslation();
}