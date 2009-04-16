#pragma once

#include "Enums.h"
#include "AnimationTrack.h"
#include "VertexMorphKeyFrameCollection.h"
#include "VertexPoseKeyFrameCollection.h"

namespace Ogre
{
	class VertexAnimationTrack;
	class VertexMorphKeyFrame;
	class VertexPoseKeyFrame;
}

namespace OgreWrapper
{

ref class VertexMorphKeyFrame;
ref class VertexPoseKeyFrame;
value class TimeIndex;
ref class VertexData;

//Incomplete class, missing vertex data functions

/// <summary>
/// Specialised AnimationTrack for dealing with changing vertex position information. 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class VertexAnimationTrack : public AnimationTrack
{
public:
/// <summary>
/// The target animation mode for vertex animation.
/// </summary>
[Engine::Attributes::SingleEnum]
enum class TargetMode : unsigned int
{
	/// <summary>
	/// Interpolate vertex positions in software.
	/// </summary>
	TM_SOFTWARE, 
	/// <summary>
	/// Bind keyframe 1 to position, and keyframe 2 to a texture coordinate
	/// for interpolation in hardware.
	/// </summary>
	TM_HARDWARE
};

private:
	Ogre::VertexAnimationTrack* ogreAnimation;

	VertexMorphKeyFrameCollection morphs;
	VertexPoseKeyFrameCollection poses;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="ogreAnimation">The Ogre::VertexAnimationTrack to wrap.</param>
	/// <param name="parent">The parent animation wrapper.</param>
	VertexAnimationTrack(Ogre::VertexAnimationTrack* ogreAnimation, Animation^ parent);

	/// <summary>
	/// Destructor.
	/// </summary>
	~VertexAnimationTrack();

public:
	/// <summary>
	/// Get the type of vertex animation we're performing. 
	/// </summary>
	/// <returns>The animation type enum.</returns>
	VertexAnimationType getAnimationType();

	/// <summary>
	/// Creates a new morph KeyFrame and adds it to this animation at the given
    /// time index.
	/// <para>
    /// It is better to create KeyFrames in time order. Creating them out of
    /// order can result in expensive reordering processing. Note that a
    /// KeyFrame at time index 0.0 is always created for you, so you don't need
    /// to create this one, just access it using getKeyFrame(0); 
	/// </para>
	/// </summary>
	/// <param name="timePos">The time from which this KeyFrame will apply. </param>
	/// <returns>The new VertexMorphKeyFrame.</returns>
	VertexMorphKeyFrame^ createVertexMorphKeyFrame(float timePos);

	/// <summary>
	/// Creates the single pose KeyFrame and adds it to this animation.
	/// </summary>
	/// <param name="timePos">The time from which this KeyFrame will apply. </param>
	/// <returns>The new VertexPoseKeyFrame.</returns>
	VertexPoseKeyFrame^ createVertexPoseKeyFrame(float timePos);

	/// <summary>
	/// Applies an animation track to the designated target. 
	/// </summary>
	/// <param name="timeIndex">The time position in the animation to apply. </param>
	void apply(TimeIndex timeIndex);

	/// <summary>
	/// Applies an animation track to the designated target. 
	/// </summary>
	/// <param name="timeIndex">The time position in the animation to apply. </param>
	/// <param name="weight">The influence to give to this track, 1.0 for full influence, less to blend with other animations.</param>
	void apply(TimeIndex timeIndex, float weight);

	/// <summary>
	/// Applies an animation track to the designated target. 
	/// </summary>
	/// <param name="timeIndex">The time position in the animation to apply. </param>
	/// <param name="weight">The influence to give to this track, 1.0 for full influence, less to blend with other animations.</param>
	/// <param name="scale">The scale to apply to translations and scalings, useful for adapting an animation to a different size target.</param>
	void apply(TimeIndex timeIndex, float weight, float scale);

	/// <summary>
	/// Applies an animation track to the designated target. 
	/// </summary>
	/// <param name="timeIndex">The time position in the animation to apply. </param>
	void apply(TimeIndex% timeIndex);

	/// <summary>
	/// Applies an animation track to the designated target. 
	/// </summary>
	/// <param name="timeIndex">The time position in the animation to apply. </param>
	/// <param name="weight">The influence to give to this track, 1.0 for full influence, less to blend with other animations.</param>
	void apply(TimeIndex% timeIndex, float weight);

	/// <summary>
	/// Applies an animation track to the designated target. 
	/// </summary>
	/// <param name="timeIndex">The time position in the animation to apply. </param>
	/// <param name="weight">The influence to give to this track, 1.0 for full influence, less to blend with other animations.</param>
	/// <param name="scale">The scale to apply to translations and scalings, useful for adapting an animation to a different size target.</param>
	void apply(TimeIndex% timeIndex, float weight, float scale);

	/// <summary>
	/// Returns the morph KeyFrame at the specified index. 
	/// </summary>
	/// <param name="index">The time index of the frame.</param>
	/// <returns>The key frame at the given position.</returns>
	VertexMorphKeyFrame^ getVertexMorphKeyFrame(unsigned short index);

	/// <summary>
	/// Returns the pose KeyFrame at the specified index. 
	/// </summary>
	/// <param name="index">The time index of the frame.</param>
	/// <returns></returns>
	VertexPoseKeyFrame^ getVertexPoseKeyFrame(unsigned short index);

	/// <summary>
	/// Set the target mode. 
	/// </summary>
	/// <param name="m">The mode to set.</param>
	void setTargetMode(TargetMode m);

	/// <summary>
	/// Get the target mode. 
	/// </summary>
	/// <returns>The current mode.</returns>
	TargetMode getTargetMode();

	/// <summary>
	/// Returns the KeyFrame at the specified index. 
	/// </summary>
	/// <param name="index">The index of the key frame.</param>
	/// <returns>The key frame at the given index, or null if the index does not exist.</returns>
	virtual KeyFrame^ getKeyFrame(unsigned short index) override;

	/// <summary>
	/// Gets the 2 KeyFrame objects which are active at the time given, and the
    /// blend value between them.
	/// <para>
    /// At any point in time in an animation, there are either 1 or 2 keyframes
    /// which are 'active', 1 if the time index is exactly on a keyframe, 2 at
    /// all other times i.e. the keyframe before and the keyframe after. 
	/// </para>
	/// <para>
    /// This method returns those keyframes given a time index, and also returns
    /// a parametric value indicating the value of 't' representing where the
    /// time index falls between them. E.g. if it returns 0, the time index is
    /// exactly on keyFrame1, if it returns 0.5 it is half way between keyFrame1
    /// and keyFrame2 etc. 
	/// </para>
	/// </summary>
	/// <param name="timeIndex">The time index.</param>
	/// <param name="keyFrame1">Pointer to a KeyFrame pointer which will receive the pointer to the keyframe just before or at this time index.</param>
	/// <param name="keyFrame2">Pointer to a KeyFrame pointer which will receive the pointer to the keyframe just after this time index.</param>
	/// <returns>Parametric value indicating how far along the gap between the 2 keyframes the timeIndex value is, e.g. 0.0 for exactly at 1, 0.25 for a quarter etc. By definition the range of this value is: 0.0 &lt;= returnValue &lt; 1.0 .</returns>
	virtual float getKeyFramesAtTime(TimeIndex% timeIndex, _OUT KeyFrame^ keyFrame1, _OUT KeyFrame^ keyFrame2) override;

	/// <summary>
	/// Gets the 2 KeyFrame objects which are active at the time given, and the
    /// blend value between them.
	/// <para>
    /// At any point in time in an animation, there are either 1 or 2 keyframes
    /// which are 'active', 1 if the time index is exactly on a keyframe, 2 at
    /// all other times i.e. the keyframe before and the keyframe after. 
	/// </para>
	/// <para>
    /// This method returns those keyframes given a time index, and also returns
    /// a parametric value indicating the value of 't' representing where the
    /// time index falls between them. E.g. if it returns 0, the time index is
    /// exactly on keyFrame1, if it returns 0.5 it is half way between keyFrame1
    /// and keyFrame2 etc. 
	/// </para>
	/// </summary>
	/// <param name="timeIndex">The time index.</param>
	/// <param name="keyFrame1">Pointer to a KeyFrame pointer which will receive the pointer to the keyframe just before or at this time index.</param>
	/// <param name="keyFrame2">Pointer to a KeyFrame pointer which will receive the pointer to the keyframe just after this time index.</param>
	/// <returns>Parametric value indicating how far along the gap between the 2 keyframes the timeIndex value is, e.g. 0.0 for exactly at 1, 0.25 for a quarter etc. By definition the range of this value is: 0.0 &lt;= returnValue &lt; 1.0 .</returns>
	virtual float getKeyFramesAtTime(TimeIndex% timeIndex, _OUT KeyFrame^ keyFrame1, _OUT KeyFrame^ keyFrame2, _OUT unsigned short% firstKeyIndex) override;

	/// <summary>
	/// Creates a new KeyFrame and adds it to this animation at the given time
    /// index.
	/// <para>
    /// It is better to create KeyFrames in time order. Creating them out of
    /// order can result in expensive reordering processing. Note that a
    /// KeyFrame at time index 0.0 is always created for you, so you don't need
    /// to create this one, just access it using getKeyFrame(0); 
	/// </para>
	/// </summary>
	/// <param name="timePos">The time from which this KeyFrame will apply.</param>
	/// <returns>The new KeyFrame.</returns>
	virtual KeyFrame^ createKeyFrame(float timePos) override;

	/// <summary>
	/// Removes a KeyFrame by it's index. 
	/// </summary>
	/// <param name="index">The index of the key frame to remove.</param>
	virtual void removeKeyFrame(unsigned short index) override;

	/// <summary>
	/// Removes all the KeyFrames from this track. 
	/// </summary>
	virtual void removeAllKeyFrames() override;
};

}