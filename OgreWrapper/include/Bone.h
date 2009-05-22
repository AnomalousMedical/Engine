#pragma once

#include "Enums.h"
#include "Node.h"
#include "BoneCollection.h"

namespace Ogre
{
	class Bone;
}

namespace OgreWrapper{

ref class Bone;

/// <summary>
/// A bone in a skeleton.
/// 
/// See Skeleton for more information about the principles behind skeletal animation. This class is 
/// a node in the joint hierarchy. Mesh vertices also have assignments to bones to define how they 
/// move in relation to the skeleton. 
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class Bone : public Node
{
private:
	Ogre::Bone* bone;
	BoneCollection bones;

internal:
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="bone">The bone to wrap.</param>
	Bone(Ogre::Bone* bone);

	/// <summary>
	/// Destructor.
	/// </summary>
	virtual ~Bone(void);

public:
	/// <summary>
	/// Creates a new Bone as a child of this bone.
	/// 
    /// This method creates a new bone which will inherit the transforms of this bone, with the 
	/// handle specified. 
	/// </summary>
	/// <param name="handle">The numeric handle to give the new bone; must be unique within the Skeleton.</param>
	/// <param name="translation">Initial translation offset of child relative to parent.</param>
	/// <param name="rotate">Initial rotation relative to parent.</param>
	/// <returns></returns>
	Bone^ createChild(unsigned short handle, Engine::Vector3 translation, Engine::Quaternion rotate);

	/// <summary>
	/// Gets the numeric handle for this bone (unique within the skeleton).
	/// </summary>
	/// <returns>The numeric handle for the bone.</returns>
	unsigned short getHandle();

	/// <summary>
	/// Sets the current position / orientation to be the 'binding pose' ie the layout in which 
	/// bones were originally bound to a mesh.
	/// </summary>
	void setBindingPose();

	/// <summary>
	/// Resets the position and orientation of this Bone to the original binding position.
	/// 
    /// Bones are bound to the mesh in a binding pose. They are then modified from this position 
	/// during animation. This method returns the bone to it's original position and orientation. 
	/// </summary>
	void reset();

	/// <summary>
	/// Sets whether or not this bone is manually controlled.
	/// 
    /// Manually controlled bones can be altered by the application at runtime, and their positions 
	/// will not be reset by the animation routines. Note that you should also make sure that there 
	/// are no AnimationTrack objects referencing this bone, or if there are, you should disable 
	/// them using pAnimation->destroyTrack(pBone->getHandle()); 
	/// </summary>
	/// <param name="manuallyControlled">True to enable manual control.  False to disable it.</param>
	void setManuallyControlled(bool manuallyControlled);

	/// <summary>
	/// Determine if this bone is manually controlled.
	/// </summary>
	/// <returns>True if the bone is manually controlled.</returns>
	bool isManuallyControlled();

	/// <summary>
	/// To be called in the event of transform changes to this node that require it's recalculation. 
	/// </summary>
	/// <param name="forceParentUpdate">Even if the node thinks it has already told it's parent, tell it anyway.</param>
	void needUpdate(bool forceParentUpdate);

	/// <summary>
	/// Returns the name of the bone. 
	/// </summary>
	/// <returns>The name of the bone.</returns>
	System::String^ getName();
};

}