/// <file>RenderWindow.h</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#pragma once

#include "RenderTarget.h"

namespace Ogre
{
	class RenderWindow;
}

namespace OgreWrapper
{

/// <summary>
/// A subclass of RenderTarget that is specific to rendering into a window.
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class RenderWindow : public RenderTarget
{
private:
	Ogre::RenderWindow* ogreRenderWindow;

internal:
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="ogreRenderWindow">The wrapped RenderWindow.</param>
	RenderWindow(Ogre::RenderWindow* ogreRenderWindow);

public:
	/// <summary>
	/// Destructor
	/// </summary>
	~RenderWindow();

	/// <summary>
	/// Returns the native RenderWindow wrapped by this class.
	/// </summary>
	/// <returns>The native RenderWindow.</returns>
	Ogre::RenderWindow* getRenderWindow();

	/// <summary>
	/// Call this function if the render window moves or is resized.
	/// </summary>
	void windowMovedOrResized();
};

}