#pragma once

namespace Ogre
{
	class NumericAnimationTrack;
}

namespace OgreWrapper
{

/// <summary>
/// Specialised AnimationTrack for dealing with generic animable values. 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class NumericAnimationTrack
{
private:
	Ogre::NumericAnimationTrack* ogreAnimation;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="ogreAnimation">The Ogre::NumericAnimationTrack to wrap.</param>
	NumericAnimationTrack(Ogre::NumericAnimationTrack* ogreAnimation);

	/// <summary>
	/// Destructor.
	/// </summary>
	~NumericAnimationTrack();
};

}