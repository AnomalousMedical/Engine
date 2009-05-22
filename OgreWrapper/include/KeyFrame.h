#pragma once

namespace Ogre
{
	class KeyFrame;
}

namespace OgreWrapper
{

/// <summary>
/// A key frame in an animation sequence defined by an AnimationTrack.
/// <para>
/// This class can be used as a basis for all kinds of key frames. The unifying
/// principle is that multiple KeyFrames define an animation sequence, with the
/// exact state of the animation being an interpolation between these key
/// frames. 
/// </para>
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class KeyFrame abstract
{
private:
	Ogre::KeyFrame* ogreFrame;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="ogreFrame"></param>
	KeyFrame(Ogre::KeyFrame* ogreFrame);

public:
	/// <summary>
	/// Gets the time of this keyframe in the animation sequence.
	/// </summary>
	/// <returns>The time of this frame in the sequence.</returns>
	float getTime();

};

}