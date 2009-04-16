#pragma once

#include "AnimationTrack.h"
#include "VertexAnimationTrackCollection.h"
#include "NumericAnimationTrackCollection.h"
#include "NodeAnimationTrackCollection.h"

namespace Ogre
{
	class Animation;
}

namespace Rendering
{

/// <summary>
/// The types of animation interpolation available. 
/// </summary>
public enum class InterpolationMode : unsigned int
{
	/// <summary>
	/// Values are interpolated along straight lines. 
	/// </summary>
	IM_LINEAR,
	/// <summary>
	/// Values are interpolated along a spline, resulting in smoother changes in direction.
	/// </summary>
    IM_SPLINE
};

/// <summary>
/// The types of rotational interpolation available. 
/// </summary>
public enum class  RotationInterpolationMode : unsigned int
{
	/// <summary>
	/// Values are interpolated linearly. This is faster but does not 
    /// necessarily give a completely accurate result.
	/// </summary>
    RIM_LINEAR,
	/// <summary>
	/// Values are interpolated spherically. This is more accurate but
    /// has a higher cost.
	/// </summary>
    RIM_SPHERICAL
};

ref class NodeAnimationTrack;
ref class VertexAnimationTrack;
ref class NumericAnimationTrack;
ref class RenderNode;
ref class Skeleton;
ref class RenderEntity;

typedef System::Collections::Generic::List<float> BoneBlendMask;

//This class is incomplete it is missing the iterator functions and the second set of create functions.

/// <summary>
/// An animation sequence.
/// <para>
/// This class defines the interface for a sequence of animation, whether that
/// be animation of a mesh, a path along a spline, or possibly more than one
/// type of animation in one. An animation is made up of many 'tracks', which
/// are the more specific types of animation. 
/// </para>
/// <para>
/// You should not create these animations directly. They will be created via a
/// parent object which owns the animation, e.g. Skeleton.
/// </para>
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class Animation
{
private:
	Ogre::Animation* ogreAnimation;
	VertexAnimationTrackCollection vertexAnimations;
	NumericAnimationTrackCollection numericAnimations;
	NodeAnimationTrackCollection nodeAnimations;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="ogreAnimation">The ogre animation to wrap.</param>
	Animation(Ogre::Animation* ogreAnimation);

	/// <summary>
	/// Get the ogre animation for this wrapper.
	/// </summary>
	/// <returns>The Ogre::Animation wrapped by this class.</returns>
	Ogre::Animation* getOgreAnimation();

public:
	/// <summary>
	/// Destructor.
	/// </summary>
	~Animation();

	/// <summary>
	/// Gets the name of this animation. 
	/// </summary>
	/// <returns>The name of the animation.</returns>
	System::String^ getName();

	/// <summary>
	/// Gets the total length of the animation. 
	/// </summary>
	/// <returns>The length of the animation.</returns>
	float getLength();

	/// <summary>
	/// Creates a NodeAnimationTrack for animating a Node. 
	/// </summary>
	/// <param name="handle">Handle to give the track, used for accessing the track later. Must be unique within this Animation. </param>
	/// <returns>The newly created NodeAnimationTrack or null if an error occured such as the track already being defined.</returns>
	NodeAnimationTrack^ createNodeTrack(unsigned short handle);

	/// <summary>
	/// Creates a NumericAnimationTrack for animating any numeric value. 
	/// </summary>
	/// <param name="handle">Handle to give the track, used for accessing the track later. Must be unique within this Animation.</param>
	/// <returns>The newly created NumericAnimationTrack or null if an error occured such as the track already being defined.</returns>
	NumericAnimationTrack^ createNumericTrack(unsigned short handle);

	/// <summary>
	/// Creates a VertexAnimationTrack for animating vertex position data.
	/// </summary>
	/// <param name="handle">Handle to give the track, used for accessing the track later. Must be unique within this Animation, and is used to identify the target. For example when applied to a Mesh, the handle must reference the index of the geometry being modified; 0 for the shared geometry, and 1+ for SubMesh geometry with the same index-1.</param>
	/// <param name="animType">Either morph or pose animation.</param>
	/// <returns>The newly created VertexAnimationTrack or null if an error occured such as the track already being defined.</returns>
	VertexAnimationTrack^ createVertexTrack(unsigned short handle, VertexAnimationType animType);

	/// <summary>
	/// Gets the number of NodeAnimationTrack objects contained in this animation.
	/// </summary>
	/// <returns>The number of node animations.</returns>
	unsigned short getNumNodeTracks();

	/// <summary>
	/// Gets a node track by it's handle.
	/// </summary>
	/// <param name="handle">The handle to search for.</param>
	/// <returns>The node matching the handle or null if the node is not found.</returns>
	NodeAnimationTrack^ getNodeTrack(unsigned short handle);

	/// <summary>
	/// Check to see if a node track exists with the given handle.
	/// </summary>
	/// <param name="handle">The handle to search for.</param>
	/// <returns>True if a track exists with the given handle.</returns>
	bool hasNodeTrack(unsigned short handle);

	/// <summary>
	/// Gets the number of NumericAnimationTrack objects contained in this animation.
	/// </summary>
	/// <returns>The number of tracks.</returns>
	unsigned short getNumNumericTracks();

	/// <summary>
	/// Gets a numeric track by it's handle.
	/// </summary>
	/// <param name="handle">The handle to search for.</param>
	/// <returns>The node matching the handle or null if the node is not found.</returns>
	NumericAnimationTrack^ getNumericTrack(unsigned short handle);

	/// <summary>
	/// Check to see if a numeric track exists with the given handle.
	/// </summary>
	/// <param name="handle">The handle to search for.</param>
	/// <returns>True if a track exists with the given handle.</returns>
	bool hasNumericTrack(unsigned short handle);

	/// <summary>
	/// Gets the number of VertexAnimationTrack objects contained in this animation.
	/// </summary>
	/// <returns>The number of vertex tracks.</returns>
	unsigned short getNumVertexTracks();

	/// <summary>
	/// Gets a Vertex track by it's handle. 
	/// </summary>
	/// <param name="handle">The handle to search for.</param>
	/// <returns>The node matching the handle or null if the node is not found.</returns>
	VertexAnimationTrack^ getVertexTrack(unsigned short handle);

	/// <summary>
	/// Check to see if a vertex track exists for the given handle.
	/// </summary>
	/// <param name="handle">The handle to search for.</param>
	/// <returns>True if a track exists with the given handle.</returns>
	bool hasVertexTrack(unsigned short handle);

	/// <summary>
	/// Destroys the node track with the given handle.
	/// </summary>
	/// <param name="handle">The node track to destroy.</param>
	void destroyNodeTrack(unsigned short handle);

	/// <summary>
	/// Destroys the numeric track with the given handle. 
	/// </summary>
	/// <param name="handle">The numeric track to destroy.</param>
	void destroyNumericTrack(unsigned short handle);

	/// <summary>
	/// Destroys the Vertex track with the given handle. 
	/// </summary>
	/// <param name="handle">The vertex track to destroy.</param>
	void destroyVertexTrack(unsigned short handle);

	/// <summary>
	/// Removes and destroys all tracks making up this animation. 
	/// </summary>
	void destroyAllTracks();

	/// <summary>
	/// Removes and destroys all node tracks making up this animation. 
	/// </summary>
	void destroyAllNodeTracks();

	/// <summary>
	/// Removes and destroys all numeric tracks making up this animation. 
	/// </summary>
	void destroyAllNumericTracks();

	/// <summary>
	/// Removes and destroys all vertex tracks making up this animation. 
	/// </summary>
	void destroyAllVertexTracks();

	/// <summary>
	/// Applies an animation given a specific time point and weight. 
	/// </summary>
	/// <param name="timePos">The time position in the animation to apply. </param>
	void apply(float timePos);

	/// <summary>
	/// Applies an animation given a specific time point and weight. 
	/// </summary>
	/// <param name="timePos">The time position in the animation to apply. </param>
	/// <param name="wieght">The influence to give to this track, 1.0 for full influence, less to blend with other animations. </param>
	void apply(float timePos, float wieght);

	/// <summary>
	/// Applies an animation given a specific time point and weight. 
	/// </summary>
	/// <param name="timePos">The time position in the animation to apply. </param>
	/// <param name="wieght">The influence to give to this track, 1.0 for full influence, less to blend with other animations. </param>
	/// <param name="scale">The scale to apply to translations and scalings, useful for adapting an animation to a different size target. </param>
	void apply(float timePos, float wieght, float scale);

	/// <summary>
	/// Applies all node tracks given a specific time point and weight to a
    /// given skeleton.
	/// <para>
	/// Where you have associated animation tracks with Node objects, you can
    /// easily apply an animation to those nodes by calling this method. 
	/// </para>
	/// </summary>
	/// <param name="skeleton">The skeleton to apply the animation to.</param>
	/// <param name="timePos">The time position in the animation to apply. </param>
	void apply(Skeleton^ skeleton, float timePos);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="skeleton">The skeleton to apply the animation to.</param>
	/// <param name="timePos">The time position in the animation to apply. </param>
	/// <param name="weight">The influence to give to this track, 1.0 for full influence, less to blend with other animations.</param>
	void apply(Skeleton^ skeleton, float timePos, float weight);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="skeleton">The skeleton to apply the animation to.</param>
	/// <param name="timePos">The time position in the animation to apply. </param>
	/// <param name="weight">The influence to give to this track, 1.0 for full influence, less to blend with other animations.</param>
	/// <param name="scale">The scale to apply to translations and scalings, useful for adapting an animation to a different size target. </param>
	void apply(Skeleton^ skeleton, float timePos, float weight, float scale);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="skeleton">The skeleton to apply the animation to.</param>
	/// <param name="timePos">The time position in the animation to apply.</param>
	/// <param name="weight">The influence to give to this track, 1.0 for full influence, less to blend with other animations.</param>
	/// <param name="blendMask">The influence array defining additional per bone weights. These will be modulated with the weight factor.</param>
	/// <param name="scale">The scale to apply to translations and scalings, useful for adapting an animation to a different size target. </param>
	void apply(Skeleton^ skeleton, float timePos, float weight, BoneBlendMask^ blendMask, float scale);

	/// <summary>
	/// Applies all vertex tracks given a specific time point and weight to a given entity.
	/// </summary>
	/// <param name="entity">The Entity to which this animation should be applied.</param>
	/// <param name="timePos">The time position in the animation to apply.</param>
	/// <param name="weight">The weight at which the animation should be applied (only affects pose animation).</param>
	/// <param name="software">Whether to populate the software morph vertex data.</param>
	/// <param name="hardware">Whether to populate the hardware morph vertex data.</param>
	void apply(RenderEntity^ entity, float timePos, float weight, bool software, bool hardware);

	/// <summary>
	/// Tells the animation how to interpolate between keyframes.
	/// <para>
    /// By default, animations normally interpolate linearly between keyframes.
    /// This is fast, but when animations include quick changes in direction it
    /// can look a little unnatural because directions change instantly at
    /// keyframes. An alternative is to tell the animation to interpolate along
    /// a spline, which is more expensive in terms of calculation time, but
    /// looks smoother because major changes in direction are distributed around
    /// the keyframes rather than just at the keyframe. 
	/// </para>
	/// </summary>
	/// <param name="im">The mode to set.</param>
	void setInterpolationMode(InterpolationMode im);

	/// <summary>
	/// Gets the current interpolation mode of this animation. 
	/// </summary>
	/// <returns>The current interpolation mode.</returns>
	InterpolationMode getInterpolationMode();

	/// <summary>
	/// Tells the animation how to interpolate rotations.
	/// <para>
	/// By default, animations interpolate linearly between rotations. This is
    /// fast but not necessarily completely accurate. If you want more accurate
    /// interpolation, use spherical interpolation, but be aware that it will
    /// incur a higher cost. 
	/// </para>
	/// </summary>
	/// <param name="im">The interpolation mode to set.</param>
	void setRotationInterpolationMode(RotationInterpolationMode im);

	/// <summary>
	/// Gets the current rotation interpolation mode of this animation.
	/// </summary>
	/// <returns>The current rotation interpolation mode.</returns>
	RotationInterpolationMode getRotationInterpolationMode();

	/// <summary>
	/// Optimise an animation by removing unnecessary tracks and keyframes.
	/// 
    /// When you export an animation, it is possible that certain tracks have
    /// been keyframed but actually don't include anything useful - the
    /// keyframes include no transformation. These tracks can be completely
    /// eliminated from the animation and thus speed up the animation. In
    /// addition, if several keyframes in a row have the same value, then they
    /// are just adding overhead and can be removed. 
	/// 
    /// Since track-less and identity track has difference behavior for
    /// accumulate animation blending if corresponding track presenting at other
    /// animation that is non-identity, and in normally this method didn't known
    /// about the situation of other animation, it can't deciding whether or not
    /// discards identity tracks. So there have a parameter allow you choose
    /// what you want, in case you aren't sure how to do that, you should use
    /// Skeleton::optimiseAllAnimations instead. 
	/// </summary>
	void optimize();

	/// <summary>
	/// Optimise an animation by removing unnecessary tracks and keyframes.
	/// 
    /// When you export an animation, it is possible that certain tracks have
    /// been keyframed but actually don't include anything useful - the
    /// keyframes include no transformation. These tracks can be completely
    /// eliminated from the animation and thus speed up the animation. In
    /// addition, if several keyframes in a row have the same value, then they
    /// are just adding overhead and can be removed. 
	/// 
    /// Since track-less and identity track has difference behavior for
    /// accumulate animation blending if corresponding track presenting at other
    /// animation that is non-identity, and in normally this method didn't known
    /// about the situation of other animation, it can't deciding whether or not
    /// discards identity tracks. So there have a parameter allow you choose
    /// what you want, in case you aren't sure how to do that, you should use
    /// Skeleton::optimiseAllAnimations instead.
	/// </summary>
	/// <param name="discardIdentityNodeTracks">If true, discard identity node tracks.</param>
	void optimize(bool discardIdentityNodeTracks);
};

}