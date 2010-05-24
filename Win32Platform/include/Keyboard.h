/// <file>OISKeyboard.h</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#pragma once

#include "AutoPtr.h"
#include "NKeyboardListener.h"

namespace OIS
{
	class OISKeyboard;
}

namespace Engine{

namespace Platform{

ref class OISKeyboard;

/// <summary>
/// This class provides access to a keyboard plugged into the computer.
/// </summary>
ref class OISKeyboard : public Keyboard
{
private:
	OIS::Keyboard* oisKeyboard;
	char* keys;
	AutoPtr<NKeyboardListener> keyboardListener;

internal:
	/// <summary>
	/// Returns the native OISKeyboard
	/// </summary>
	OIS::Keyboard* getKeyboard();

	/// <summary>
	/// Callback from the native listener to fire the key pressed event.
	/// </summary>
	void keyPressed(OIS::KeyCode button, unsigned int text);

	/// <summary>
	/// Callback from the native listener to fire the key released event.
	/// </summary>
	void keyReleased(OIS::KeyCode button, unsigned int text);

	/// <summary>
	/// Constructor
	/// </summary>
	OISKeyboard(OIS::Keyboard* oisKeyboard);

public:
	/// <summary>
	/// Destructor
	/// </summary>
	~OISKeyboard();

	/// <summary>
	/// Checks to see if the given key is pressed.
	/// </summary>
	/// <param name="keyCode">The KeyboardButtonCode to check.</param>
	/// <returns>True if the key is pressed.  False if it is not.</returns>
	virtual bool isKeyDown( KeyboardButtonCode keyCode ) override;

	/// <summary>
	/// Checks to see if the given Modifier key is down.  This is Shift, Alt or Ctrl.
	/// </summary>
	/// <param name="keyCode">The Modifier key code to check.</param>
	/// <returns>True if the modifier is pressed down.  False if it is not.</returns>
	virtual bool isModifierDown( Modifier keyCode ) override;

	/// <summary>
	/// Returns the keyboard button as a string for the given code.  If shift is pressed
	/// the appropriate alt character will be returned.
	/// </summary>
	/// <param name="code">The code of the button to check for.</param>
	/// <returns>The button as a string.</returns>
	virtual System::String^ getAsString( KeyboardButtonCode code ) override;
	
	/// <summary>
	/// Reads the state of the keyboard.
	/// </summary>
	virtual void capture() override;
};

}

}