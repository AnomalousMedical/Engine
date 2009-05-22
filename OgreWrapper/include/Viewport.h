/// <file>Viewport.h</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#pragma once

namespace Ogre
{
	class Viewport;
}

namespace OgreWrapper
{

/// <summary>
/// A viewport is where a camera places its output which is then put onto the
/// viewport's render target.
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class Viewport
{
private:
	Ogre::Viewport* viewport;

internal:
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="viewport">The native viewport to wrap.</param>
	/// <param name="name">The name of the viewport.</param>
	Viewport(Ogre::Viewport* viewport);

	/// <summary>
	/// Gets the wrapped native viewport.
	/// </summary>
	/// <returns>The wrapped native viewport.</returns>
	Ogre::Viewport* getViewport();
public:
	/// <summary>
	/// Destructor
	/// </summary>
	~Viewport();

	/// <summary>
	/// The visibility mask is a way to exclude objects from rendering for a given viewport. 
	/// For each object in the frustum, a check is made between this mask and the objects 
	/// visibility flags.
	/// </summary>
	/// <param name="mask">The mask to set.</param>
	void setVisibilityMask(unsigned int mask);

	/// <summary>
	/// Gets a per-viewport visibility mask.
	/// </summary>
	/// <returns>The visibility mask.</returns>
	unsigned int getVisibilityMask();

	void setBackgroundColor(Engine::Color color);

	Engine::Color getBackgroundColor();
};

}