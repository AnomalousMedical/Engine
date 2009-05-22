#pragma once

#include "KeyFrame.h"

namespace Ogre
{
	class VertexPoseKeyFrame;
}

namespace OgreWrapper
{


/// <summary>
/// Specialised KeyFrame which references a Mesh::Pose at a certain influence
/// level, which stores offsets for a subset of the vertices in a buffer to
/// provide a blendable pose.
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class VertexPoseKeyFrame : public KeyFrame
{
private:
	Ogre::VertexPoseKeyFrame* ogreFrame;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="ogreFrame">The ogre frame to wrap.</param>
	VertexPoseKeyFrame(Ogre::VertexPoseKeyFrame* ogreFrame);

public:
	/// <summary>
	/// Destructor.
	/// </summary>
	~VertexPoseKeyFrame();

	/// <summary>
	/// Add a new pose reference. 
	/// </summary>
	/// <param name="poseIndex">The index of the pose.</param>
	/// <param name="influence">The influence for this key frame.</param>
	void addPoseReference(unsigned short poseIndex, float influence);

	/// <summary>
	/// Update the influence of a pose reference.
	/// </summary>
	/// <param name="poseIndex">The index of the pose.</param>
	/// <param name="influence">The new influence for this key frame.</param>
	void updatePoseReference(unsigned short poseIndex, float influence);

	/// <summary>
	/// Remove reference to a given pose. 
	/// </summary>
	/// <param name="poseIndex">The pose index (not the index of the reference).</param>
	void removePoseReference(unsigned short poseIndex);

	/// <summary>
	/// Remove all pose references.
	/// </summary>
	void removeAllPoseReferences();
};

}