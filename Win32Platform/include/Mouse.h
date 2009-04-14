/// <file>OISMouse.h</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#pragma once

#include "AutoPtr.h"
#include "NMouseListener.h"

namespace OIS
{
	class Mouse;
	enum MouseButtonID;
}

namespace Engine
{

namespace Platform
{

/// <summary>
/// This class allows access to the state of a mouse.
/// </summary>
public ref class OISMouse : public Mouse
{
private:
	OIS::Mouse* oisMouse;
	AutoPtr<NMouseListener> mouseListener;
	float sensitivity;

internal:
	/// <summary>
	/// Callback from the native listener for when the mouse moves.
	/// </summary>
	void moved();

	/// <summary>
	/// Callback from the native listener for when a mouse button is pressed.
	/// </summary>
	void buttonPressed(OIS::MouseButtonID id);

	/// <summary>
	/// Callback from the native listener for when a mouse button is released.
	/// </summary>
	void buttonReleased(OIS::MouseButtonID id);

	/// <summary>
	/// Gets the wrapped native mouse.
	/// </summary>
	/// <returns>The wrapped native mouse.</returns>
	OIS::Mouse* getMouse();

	/// <summary>
	/// Callback if the window the mouse is in changes size.
	/// </summary>
	void windowResized(OSWindow^ window);

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="oisMouse">The native mouse class to wrap.</param>
	/// <param name="width">The width of the area the mouse is on.</param>
	/// <param name="height">The height of the area the mouse is on.</param>
	OISMouse(OIS::Mouse* oisMouse, int width, int height);

public:
	/// <summary>
	/// Fired when the mouse moves.
	/// </summary>
	event MouseMovedEvent^ MouseMoved;
	
	/// <summary>
	/// Fired when a mouse button is pressed.
	/// </summary>
	event MousePressedEvent^ MousePressed;

	/// <summary>
	/// Fired when a mouse button is released.
	/// </summary>
	event MouseReleasedEvent^ MouseReleased;

	/// <summary>
	/// Destructor
	/// </summary>
	~OISMouse();

	/// <summary>
	/// Returns the absolute mouse position on the screen bounded by the mouse area
	/// and 0, 0.
	/// </summary>
	/// <returns>The position of the mouse.</returns>
	virtual EngineMath::Vector3 getAbsMouse() override;

	/// <summary>
	/// Returns the relative mouse position from the last time capture was called.
	/// </summary>
	/// <returns>The relative mouse position.</returns>
	virtual EngineMath::Vector3 getRelMouse() override;

	/// <summary>
	/// Determines if the specified button is pressed.
	/// </summary>
	/// <returns>True if the button is pressed.  False if it is not.</returns>
	virtual bool buttonDown( MouseButtonCode button ) override;

	/// <summary>
	/// Captures the current state of the mouse.
	/// </summary>
	virtual void capture() override;

	/// <summary>
	/// Set the sensitivity of the mouse.
	/// </summary>
	/// <param name="sensitivity">The sensitivity to set.</param>
	virtual void setSensitivity(float sensitivity) override;

	/// <summary>
	/// Get the width that the mouse produces input for.
	/// </summary>
	/// <returns>The width of the mouse area.</returns>
	virtual float getMouseAreaWidth() override;

	/// <summary>
	/// Get the height that the mouse produces input for.
	/// </summary>
	/// <returns>The height of the mouse area.</returns>
	virtual float getMouseAreaHeight() override;
};

}

}