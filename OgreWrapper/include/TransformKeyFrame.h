#pragma once

namespace Ogre
{
	class TransformKeyFrame;
}

namespace Rendering
{

/// <summary>
/// Specialised KeyFrame which stores a full transform. 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class TransformKeyFrame
{
private:
	Ogre::TransformKeyFrame* ogreAnimation;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="ogreAnimation">The Ogre::TransformKeyFrame to wrap.</param>
	TransformKeyFrame(Ogre::TransformKeyFrame* ogreAnimation);

	/// <summary>
	/// Destructor.
	/// </summary>
	~TransformKeyFrame();
};

}