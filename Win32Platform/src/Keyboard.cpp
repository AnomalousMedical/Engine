/// <file>OISKeyboard.cpp</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "StdAfx.h"
#include "..\include\Keyboard.h"

#include "OIS.h"

namespace Engine{

namespace Platform{

OISKeyboard::OISKeyboard(OIS::Keyboard* oisKeyboard)
:oisKeyboard( oisKeyboard ), keys( new char[256] )
{
	keyboardListener.Reset(new NKeyboardListener(this));
	oisKeyboard->setEventCallback(keyboardListener.Get());
}

OISKeyboard::~OISKeyboard()
{
	delete[] keys;
	keys = 0;
}

void OISKeyboard::keyPressed(OIS::KeyCode button, unsigned int text)
{
	
}

void OISKeyboard::keyReleased(OIS::KeyCode button, unsigned int text)
{
	
}

OIS::Keyboard* OISKeyboard::getKeyboard()
{
	return oisKeyboard;
}

bool OISKeyboard::isKeyDown( KeyboardButtonCode keyCode )
{
	return keys[static_cast<int>(keyCode)];
}

bool OISKeyboard::isModifierDown( Modifier keyCode )
{
	return oisKeyboard->isModifierDown((OIS::Keyboard::Modifier)keyCode);
}

System::String^ OISKeyboard::getAsString( KeyboardButtonCode code )
{
	return gcnew System::String( oisKeyboard->getAsString((OIS::KeyCode)code).c_str() );
}

void OISKeyboard::capture()
{
	oisKeyboard->capture();
	oisKeyboard->copyKeyStates(keys);
}

}

}