#include "StdAfx.h"
#include "..\include\NKeyboardListener.h"
#include "Keyboard.h"

namespace Engine{

namespace Platform{

NKeyboardListener::NKeyboardListener(gcroot<OISKeyboard^> keyboard)
:keyboard(keyboard)
{

}

NKeyboardListener::~NKeyboardListener(void)
{

}

bool NKeyboardListener::keyPressed( const OIS::KeyEvent &arg )
{
	keyboard->keyPressed(arg.key, arg.text);
	return true;
}

bool NKeyboardListener::keyReleased( const OIS::KeyEvent &arg )
{
	keyboard->keyReleased(arg.key, arg.text);
	return true;
}

}

}