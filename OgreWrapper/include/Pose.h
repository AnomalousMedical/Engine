#pragma once

namespace Ogre
{
	class Pose;
}

namespace OgreWrapper
{

/// <summary>
/// A pose is a linked set of vertex offsets applying to one set of vertex data.
/// <para>
/// The target index referred to by the pose has a meaning set by the user of
/// this class; but for example when used by Mesh it refers to either the Mesh
/// shared geometry (0) or a SubMesh dedicated geometry (1+). Pose instances can
/// be referred to by keyframes in VertexAnimationTrack in order to animate
/// based on blending poses together. 
/// </para>
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class Pose
{
private:
	Ogre::Pose* pose;

internal:
	/// <summary>
	/// Constructor, internal use only.
	/// </summary>
	/// <param name="pose">The pose to wrap.</param>
	Pose(Ogre::Pose* pose);

	/// <summary>
	/// Get the ogre pose wrapped by this class.
	/// </summary>
	/// <returns></returns>
	Ogre::Pose* getOgrePose();

public:
	/// <summary>
	/// Return the name of the pose (may be blank). 
	/// </summary>
	/// <returns>The name of the pose.</returns>
	System::String^ getName();

	/// <summary>
	/// Return the target geometry index of the pose. 
	/// </summary>
	/// <returns>The index of the target geometry.</returns>
	unsigned short getTarget();

	/// <summary>
	/// Adds an offset to a vertex for this pose.
	/// </summary>
	/// <param name="index">The vertex index.</param>
	/// <param name="offset">The position offset for this pose.</param>
	void addVertex(size_t index, EngineMath::Vector3 offset);

	/// <summary>
	/// Adds an offset to a vertex for this pose by reference.
	/// </summary>
	/// <param name="index">The vertex index.</param>
	/// <param name="offset">The position offset for this pose.</param>
	void addVertex(size_t index, EngineMath::Vector3% offset);

	/// <summary>
	/// Remove a vertex offset. 
	/// </summary>
	/// <param name="index">The index of the offset to remove.</param>
	void removeVertex(size_t index);

	/// <summary>
	/// Clear all vertex offsets. 
	/// </summary>
	void clearVertexOffsets();

	/// <summary>
	/// Get the offset at the specified index.
	/// </summary>
	/// <param name="index">The index of the offset to get.</param>
	/// <returns></returns>
	EngineMath::Vector3 getOffset(size_t index);
};

}