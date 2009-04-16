#pragma once

#include "OgreSkeleton.h"
#include "AutoPtr.h"
#include "Resource.h"
#include "BoneCollection.h"
#include "AnimationCollection.h"

namespace OgreWrapper
{

[Engine::Attributes::SingleEnum]
public enum class SkeletonAnimationBlendMode : unsigned int
{
	ANIMBLEND_AVERAGE,
	ANIMBLEND_CUMULATIVE
};

ref class Bone;
ref class Animation;
ref class AnimationStateSet;

typedef System::Collections::Generic::IEnumerator<Bone^> BoneIterator;

/// <summary>
/// A collection of Bone objects used to animate a skinned mesh.
/// <para>
/// Skeletal animation works by having a collection of 'bones' which are
/// actually just joints with a position and orientation, arranged in a tree
/// structure. For example, the wrist joint is a child of the elbow joint, which
/// in turn is a child of the shoulder joint. Rotating the shoulder
/// automatically moves the elbow and wrist as well due to this hierarchy. 
/// </para>
/// <para>
/// So how does this animate a mesh? Well every vertex in a mesh is assigned to
/// one or more bones which affects it's position when the bone is moved. If a
/// vertex is assigned to more than one bone, then weights must be assigned to
/// determine how much each bone affects the vertex (actually a weight of 1.0 is
/// used for single bone assignments). Weighted vertex assignments are
/// especially useful around the joints themselves to avoid 'pinching' of the
/// mesh in this region. 
/// </para>
/// <para>
/// Therefore by moving the skeleton using preset animations, we can animate the
/// mesh. The advantage of using skeletal animation is that you store less
/// animation data, especially as vertex counts increase. In addition, you are
/// able to blend multiple animations together (e.g. walking and looking around,
/// running and shooting) and provide smooth transitions between animations
/// without incurring as much of an overhead as would be involved if you did
/// this on the core vertex data. 
/// </para>
/// <para>
/// Skeleton definitions are loaded from datafiles, namely the .skeleton file
/// format. They are loaded on demand, especially when referenced by a Mesh. 
/// </para>
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class Skeleton : Resource
{
private:
	Ogre::Skeleton* skeleton;
	AutoPtr<Ogre::SkeletonPtr> autoSharedPtr;

	BoneCollection bones;
	AnimationCollection animations;

protected:
	/// <summary>
	/// Constructor.  Does not create a shared pointer, used by SkeletonInstance.
	/// </summary>
	/// <param name="ogreSkeleton">The Ogre::Skeleton's pointer to wrap.</param>
	Skeleton(Ogre::Skeleton* ogreSkeleton);

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="ogreSkeleton">The Ogre::Skeleton's pointer to wrap.</param>
	Skeleton(const Ogre::SkeletonPtr& ogreSkeleton);

	/// <summary>
	/// Gets the Ogre::SkeletonPtr to the skeleton.
	/// </summary>
	/// <returns>The Ogre::SkeletonPtr.</returns>
	const Ogre::SkeletonPtr& getSkeletonPtr();

	/// <summary>
	/// Gets a raw pointer to the skeleton.
	/// </summary>
	/// <returns>A raw pointer to the skeleton.</returns>
	Ogre::Skeleton* Skeleton::getSkeleton();

public:
	/// <summary>
	/// Destructor.
	/// </summary>
	virtual ~Skeleton(void);

	/// <summary>
	/// Creates a brand new Bone owned by this Skeleton. 
	/// </summary>
	/// <returns>The new bone.</returns>
	Bone^ createBone();

	/// <summary>
	/// Creates a brand new Bone owned by this Skeleton. 
	/// </summary>
	/// <param name="handle">The handle to give to this new bone - must be unique within this skeleton. You should also ensure that all bone handles are eventually contiguous (this is to simplify their compilation into an indexed array of transformation matrices). For this reason it is advised that you use the simpler createBone method which automatically assigns a sequential handle starting from 0. </param>
	/// <returns>The new bone.</returns>
	Bone^ createBone(unsigned short handle);

	/// <summary>
	/// Creates a brand new Bone owned by this Skeleton.
	/// This method creates an unattached new Bone for this skeleton. Unless this is to be a root 
	/// bone (there may be more than one of these), you must attach it to another Bone in the 
	/// skeleton using addChild for it to be any use. For this reason you will likely be better off 
	/// creating child bones using the Bone::createChild method instead, once you have created the 
	/// root bone. 
	///
    /// Note that this method automatically generates a handle for the bone, which you can retrieve 
	/// using Bone::getHandle. If you wish the new Bone to have a specific handle, use the alternate 
	/// form of this method which takes a handle as a parameter, although you should note the 
	/// restrictions. 
	/// </summary>
	/// <param name="name">The name to give to this new bone - must be unique within this skeleton. Note that the way OGRE looks up bones is via a numeric handle, so if you name a Bone this way it will be given an automatic sequential handle. The name is just for your convenience, although it is recommended that you only use the handle to retrieve the bone in performance-critical code.</param>
	/// <returns>The new bone.</returns>
	Bone^ createBone(System::String^ name);

	/// <summary>
	/// Creates a brand new Bone owned by this Skeleton. 
	/// </summary>
	/// <param name="name">The name to give to this new bone - must be unique within this skeleton. Note that the way OGRE looks up bones is via a numeric handle, so if you name a Bone this way it will be given an automatic sequential handle. The name is just for your convenience, although it is recommended that you only use the handle to retrieve the bone in performance-critical code.</param>
	/// <param name="handle">The handle to give to this new bone - must be unique within this skeleton. You should also ensure that all bone handles are eventually contiguous (this is to simplify their compilation into an indexed array of transformation matrices). For this reason it is advised that you use the simpler createBone method which automatically assigns a sequential handle starting from 0.</param>
	/// <returns>The new bone.</returns>
	Bone^ createBone(System::String^ name, unsigned short handle);

	/// <summary>
	/// Returns the number of bones in this skeleton. 
	/// </summary>
	/// <returns>The number of bones in the skeleton.</returns>
	unsigned short getNumBones();

	/// <summary>
	/// Get an iterator over the root bones in the skeleton, ie those with no parents.  Due to 
	/// the nature of a wrapper a new List instance is created every time this function is called.
	/// </summary>
	/// <returns>An enumerator over the root bones.</returns>
	BoneIterator^ getRootBoneIterator();

	/// <summary>
	/// Get an iterator over all the bones in the skeleton.  Due to the nature of a wrapper a new 
	/// List instance is created every time this function is called.
	/// </summary>
	/// <returns>An enumerator over all the bones.</returns>
	BoneIterator^ getBoneIterator();

	/// <summary>
	/// Gets a bone by it's handle. 
	/// </summary>
	/// <param name="handle">The handle to search for.</param>
	/// <returns>The bone or null if the bone is not found.</returns>
	Bone^ getBone(unsigned short handle);

	/// <summary>
	/// Gets a bone by it's name. 
	/// </summary>
	/// <param name="name">The name of the bone to search for.</param>
	/// <returns>The bone or null if it does not exist.</returns>
	Bone^ getBone(System::String^ name);

	/// <summary>
	/// Returns whether this skeleton contains the named bone.
	/// </summary>
	/// <param name="name">The name of the bone to search for.</param>
	/// <returns>True if the skeleton contains the bone. False if it does not.</returns>
	bool hasBone(System::String^ name);

	/// <summary>
	/// Sets the current position / orientation to be the 'binding pose' i.e. the layout in which 
	/// bones were originally bound to a mesh. 
	/// </summary>
	void setBindingPose();

	/// <summary>
	/// Resets the position and orientation of all bones in this skeleton to their original binding 
	/// position. 
	/// </summary>
	void reset();

	/// <summary>
	/// Resets the position and orientation of all bones in this skeleton to their original binding 
	/// position. 
	/// </summary>
	/// <param name="resetManualBones">If set to true, causes the state of manual bones to be reset too, which is normally not done to allow the manual state to persist even when keyframe animation is applied.</param>
	void reset(bool resetManualBones);

	/// <summary>
	/// Creates a new Animation object for animating this skeleton. 
	/// </summary>
	/// <param name="name">The name of this animation.</param>
	/// <param name="length">The length of the animation in seconds.</param>
	/// <returns></returns>
	Animation^ createAnimation(System::String^ name, float length);

	/// <summary>
	/// Returns the named Animation object. 
	/// </summary>
	/// <param name="name">The name of the animation </param>
	/// <returns>The animation matching the name.</returns>
	Animation^ getAnimation(System::String^ name);

	/// <summary>
	/// Returns whether this skeleton contains the named animation. 
	/// </summary>
	/// <param name="name">The name of the animation.</param>
	/// <returns>True if the skeleton has the animation.  False if it does not.</returns>
	bool hasAnimation(System::String^ name);

	/// <summary>
	/// Removes an Animation from this skeleton. 
	/// </summary>
	/// <param name="name">The name of the animation to remove.</param>
	void removeAnimation(System::String^ name);

	/// <summary>
	/// Changes the state of the skeleton to reflect the application of the
    /// passed in collection of animations.
	/// <para>
    /// Animating a skeleton involves both interpolating between keyframes of a
    /// specific animation, and blending between the animations themselves.
    /// Calling this method sets the state of the skeleton so that it reflects
    /// the combination of all the passed in animations, at the time index
    /// specified for each, using the weights specified. Note that the weights
    /// between animations do not have to sum to 1.0, because some animations
    /// may affect only subsets of the skeleton. If the weights exceed 1.0 for
    /// the same area of the skeleton, the movement will just be exaggerated. 
	/// </para>
	/// </summary>
	/// <param name="animSet">The animation state to set.</param>
	void setAnimationState(AnimationStateSet^ animSet);

	/// <summary>
	/// Gets the number of animations on this skeleton. 
	/// </summary>
	/// <returns>The number of animations.</returns>
	unsigned short getNumAnimations();

	/// <summary>
	/// Gets the animation blending mode which this skeleton will use. 
	/// </summary>
	/// <returns>The blending mode.</returns>
	SkeletonAnimationBlendMode getBlendMode();

	/// <summary>
	/// Sets the animation blending mode this skeleton will use.
	/// </summary>
	/// <param name="blendMode">The blend mode to set.</param>
	void setBlendMode(SkeletonAnimationBlendMode blendMode);

	/// <summary>
	/// Optimise all of this skeleton's animations.
	/// </summary>
	/// <param name="preservingIdentityNodeTracks">If true, don't destroy identity node tracks.</param>
	void optimizeAllAnimations(bool preservingIdentityNodeTracks);

	/// <summary>
	/// Have manual bones been modified since the skeleton was last updated?
	/// </summary>
	/// <returns>True if the bones were updated.  False if they were not.</returns>
	bool getManualBonesDirty();

	/// <summary>
	/// Are there any manually controlled bones? 
	/// </summary>
	/// <returns>True if there are manually controlled bones.  False if there are not.</returns>
	bool hasManualBones();
};

}