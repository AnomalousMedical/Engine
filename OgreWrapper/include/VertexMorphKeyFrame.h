#pragma once

#include "KeyFrame.h"

namespace Ogre
{
	class VertexMorphKeyFrame;
}

namespace Rendering
{

//This class is incomplete it needs its vertex buffer functions.

/// <summary>
/// Specialised KeyFrame which stores absolute vertex positions for a complete
/// buffer, designed to be interpolated with other keys in the same track. 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class VertexMorphKeyFrame : public KeyFrame
{
private:
	Ogre::VertexMorphKeyFrame* ogreFrame;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="ogreFrame">The Ogre::VertexMorphKeyFrame to wrap.</param>
	VertexMorphKeyFrame(Ogre::VertexMorphKeyFrame* ogreFrame);

public:
	/// <summary>
	/// Destructor.
	/// </summary>
	~VertexMorphKeyFrame();
};

}