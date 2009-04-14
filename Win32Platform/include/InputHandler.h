/// <file>OISInputHandler.h</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#pragma once

using namespace System::Collections::Generic;

namespace OIS
{
	class InputManager;
}

namespace Engine
{

namespace Platform
{

ref class OISKeyboard;
ref class OISMouse;

/// <summary>
/// This class creates the input devices.
/// </summary>
public ref class OISInputHandler : public InputHandler
{
private:
	OIS::InputManager* nInputManager;
	int numKeyboards;
	int numMice;
	int numJoysticks;
	OISKeyboard^ createdKeyboard;
	OISMouse^ createdMouse;
	OSWindow^ window;
	ResizeEvent^ mouseResizeEvent;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="windowHandle">The handle of the window to listen to.</param>
	/// <param name="foreground">True if the mouse should be in foreground mode.</param>
	/// <param name="exclusive">True if the mouse should be in exclusive mode.</param>
	/// <param name="exclusive">True to disable the windows key.</param>
	OISInputHandler(OSWindow^ windowHandle, bool foreground, bool exclusive, bool noWinKey);

	/// <summary>
	/// Destructor
	/// </summary>
	~OISInputHandler();

	/// <summary>
	/// Creates a OISKeyboard object linked to the system keyboard.  This keyboard is valid
	/// until the OISInputHandler is destroyed.
	/// </summary>
	/// <param name="buffered">True if the keyboard should be buffered, which allows the keyboard events to fire.</param>
	/// <returns>The new keyboard.</returns>
	virtual Keyboard^ createKeyboard(bool buffered) override;

	/// <summary>
	/// Destroys the given keyboard.  The keyboard will be disposed after this function
	/// call and you will no longer be able to use it.
	/// </summary>
	/// <param name="keyboard">The keyboard to destroy.</param>
	virtual void destroyKeyboard(Keyboard^ keyboard) override;

	/// <summary>
	/// Creates a OISMouse object linked to the system mouse.  This mouse is valid
	/// until the OISInputHandler is destroyed.
	/// </summary>
	/// <param name="buffered">True if the mouse should be buffered, which allows the mouse events to fire.</param>
	/// <returns>The new mouse.</returns>
	virtual Mouse^ createMouse(bool buffered) override;

	/// <summary>
	/// Destroys the given mouse.  The mouse will be disposed after this function
	/// call and you will no longer be able to use it.
	/// </summary>
	/// <param name="mouse">The mouse to destroy.</param>
	virtual void destroyMouse(Mouse^ mouse) override;
};

}

}