#pragma once

#include "ois.h"
#include "vcclr.h"

namespace Engine{

namespace Platform{

ref class OISKeyboard;

/// <summary>
/// This is the native keyboard listener that subscribes directly to the OIS::OISKeyboard
/// and forwards the events to .NET.
/// </summary>
class NKeyboardListener : public OIS::KeyListener
{
private:
	gcroot<OISKeyboard^> keyboard;

public:
	/// <summary>
	/// The keyboard listener takes the OISKeyboard to forward events to.
	/// </summary>
	/// <param name="keyboard">The keyboard to forward events to.</param>
	NKeyboardListener(gcroot<OISKeyboard^> keyboard);

	/// <summary>
	/// Destructor.
	/// </summary>
	virtual ~NKeyboardListener(void);

	/// <summary>
	/// This event is fired when a key is pressed.
	/// </summary>
	/// <param name="arg">The KeyEvent.</param>
	virtual bool keyPressed( const OIS::KeyEvent &arg );

	/// <summary>
	/// This event is fired when a key is released.
	/// </summary>
	/// <param name="arg">The KeyEvent.</param>
	virtual bool keyReleased( const OIS::KeyEvent &arg );
};

}

}