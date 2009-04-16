#pragma once

namespace Ogre
{
	class AnimationTrack;
}

namespace Rendering
{

ref class KeyFrame;
ref class Animation;

/// <summary>
/// Time index object used to search keyframe at the given position. 
/// </summary>
public value class TimeIndex
{
private:
	float timePos;
	unsigned int keyIndex;

public:
	/// <summary>
	/// Construct time index object by the given time position. 
	/// </summary>
	/// <param name="timePos"></param>
	TimeIndex(float timePos)
		:timePos(timePos), keyIndex(0)
	{

	}

	/// <summary>
	/// Construct time index object by the given time position and global
    /// keyframe index. 
	/// 
	/// Normally, you don't need to use this constructor directly, use
    /// Animation::_getTimeIndex instead. 
	/// </summary>
	/// <param name="timePos"></param>
	/// <param name="keyIndex"></param>
	TimeIndex(float timePos, unsigned int keyIndex)
			:timePos(timePos), keyIndex(keyIndex)
	{

	}

	/// <summary>
	/// Get the time position (in relation to the whole animation sequence)
	/// </summary>
	/// <returns>The time position.</returns>
	float getTimePos()
	{
		return timePos;
	}

	/// <summary>
	/// Get the global keyframe index (in relation to the whole animation
    /// sequence) that used to convert to local keyframe index, or
    /// INVALID_KEY_INDEX which means global keyframe index unavailable, and
    /// then slight slow method will used to search local keyframe index. 
	/// </summary>
	/// <returns>The key index.</returns>
	unsigned int getKeyIndex()
	{
		return keyIndex;
	}
};

/// <summary>
/// <para>
/// Type of vertex animation.
/// </para>
/// <para>
/// Vertex animation comes in 2 types, morph and pose. The reason
/// for the 2 types is that we have 2 different potential goals - to encapsulate
/// a complete, flowing morph animation with multiple keyframes (a typical animation,
/// but implemented by having snapshots of the vertex data at each keyframe), 
/// or to represent a single pose change, for example a facial expression. 
/// Whilst both could in fact be implemented using the same system, we choose
/// to separate them since the requirements and limitations of each are quite
/// different.
/// </para>
/// <para>
/// Morph animation is a simple approach where we have a whole series of 
/// snapshots of vertex data which must be interpolated, e.g. a running 
/// animation implemented as morph targets. Because this is based on simple
/// snapshots, it's quite fast to use when animating an entire mesh because 
/// it's a simple linear change between keyframes. However, this simplistic 
/// approach does not support blending between multiple morph animations. 
/// If you need animation blending, you are advised to use skeletal animation
/// for full-mesh animation, and pose animation for animation of subsets of 
/// meshes or where skeletal animation doesn't fit - for example facial animation.
/// For animating in a vertex shader, morph animation is quite simple and 
/// just requires the 2 vertex buffers (one the original position buffer) 
/// of absolute position data, and an interpolation factor. Each track in 
/// a morph animation refrences a unique set of vertex data.
/// </para>
/// <para>
/// Pose animation is more complex. Like morph animation each track references
/// a single unique set of vertex data, but unlike morph animation, each 
/// keyframe references 1 or more 'poses', each with an influence level. 
/// A pose is a series of offsets to the base vertex data, and may be sparse - ie it
/// may not reference every vertex. Because they're offsets, they can be 
/// blended - both within a track and between animations. This set of features
/// is very well suited to facial animation.
/// </para>
/// <para>
/// For example, let's say you modelled a face (one set of vertex data), and 
/// defined a set of poses which represented the various phonetic positions 
/// of the face. You could then define an animation called 'SayHello', containing
/// a single track which referenced the face vertex data, and which included 
/// a series of keyframes, each of which referenced one or more of the facial 
/// positions at different influence levels - the combination of which over
/// time made the face form the shapes required to say the word 'hello'. Since
/// the poses are only stored once, but can be referenced may times in 
/// many animations, this is a very powerful way to build up a speech system.
/// </para>
/// <para>
/// The downside of pose animation is that it can be more difficult to set up.
/// Also, since it uses more buffers (one for the base data, and one for each
/// active pose), if you're animating in hardware using vertex shaders you need
/// to keep an eye on how many poses you're blending at once. You define a
/// maximum supported number in your vertex program definition, see the 
/// includes_pose_animation material script entry. 
/// </para>
/// <para>
/// So, by partitioning the vertex animation approaches into 2, we keep the
/// simple morph technique easy to use, whilst still allowing all 
/// the powerful techniques to be used. Note that morph animation cannot
/// be blended with other types of vertex animation (pose animation or other
/// morph animation); pose animation can be blended with other pose animation
/// though, and both types can be combined with skeletal animation. Also note
/// that all morph animation can be expressed as pose animation, but not vice
/// versa.
/// </para>
/// </summary>
public enum class VertexAnimationType : unsigned int
{
	/// <summary>
	/// No animation
	/// </summary>
	VAT_NONE = 0,
	/// <summary>
	/// Morph animation is made up of many interpolated snapshot keyframes
	/// </summary>
	VAT_MORPH = 1,
	/// <summary>
	/// Pose animation is made up of a single delta pose keyframe
	/// </summary>
	VAT_POSE = 2
};

//Incomplete class, missing listener function.

/// <summary>
/// A 'track' in an animation sequence, i.e. a sequence of keyframes which
/// affect a certain type of animable object.
/// <para>
/// This class is intended as a base for more complete classes which will
/// actually animate specific types of object, e.g. a bone in a skeleton to
/// affect skeletal animation. An animation will likely include multiple tracks
/// each of which can be made up of many KeyFrame instances. Note that the use
/// of tracks allows each animable object to have it's own number of keyframes,
/// i.e. you do not have to have the maximum number of keyframes for all
/// animable objects just to cope with the most animated one.
/// </para>
/// <para>
/// Since the most common animable object is a Node, there are options in this
/// class for associating the track with a Node which will receive keyframe
/// updates automatically when the 'apply' method is called.
/// </para>
/// <para>
/// By default rotation is done using shortest-path algorithm. It is possible to
/// change this behaviour using setUseShortestRotationPath() method. 
/// </para>
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class AnimationTrack abstract
{
private:
	Ogre::AnimationTrack* ogreTrack;
	Animation^ parent;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="ogreTrack">The ogretrack to wrap.</param>
	/// <param name="parent">The parent animation.</param>
	AnimationTrack(Ogre::AnimationTrack* ogreTrack, Animation^ parent);

public:
	/// <summary>
	/// Method to determine if this track has any KeyFrames which are doing
    /// anything useful - can be used to determine if this track can be
    /// optimised out.
	/// </summary>
	/// <returns>True if non zero key frames exist.</returns>
	bool hasNonZeroKeyFrames();

	/// <summary>
	/// Optimise the current track by removing any duplicate keyframes. 
	/// </summary>
	void optimize();

	/// <summary>
	/// Get the handle associated with this track. 
	/// </summary>
	/// <returns>This track's handle.</returns>
	unsigned short getHandle();

	/// <summary>
	/// Returns the number of keyframes in this animation. 
	/// </summary>
	/// <returns>The number of key frames.</returns>
	unsigned short getNumKeyFrames();

	/// <summary>
	/// Returns the KeyFrame at the specified index. 
	/// </summary>
	/// <param name="index">The index of the key frame.</param>
	/// <returns>The key frame at the given index, or null if the index does not exist.</returns>
	virtual KeyFrame^ getKeyFrame(unsigned short index) = 0;

	/// <summary>
	/// <para>
	/// Gets the 2 KeyFrame objects which are active at the time given, and the
    /// blend value between them.
	/// </para>
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
	/// <returns>Parametric value indicating how far along the gap between the 2 keyframes the timeIndex value is, e.g. 0.0 for exactly at 1, 0.25 for a quarter etc. By definition the range of this value is: 0.0 &lt;= returnValue &lt; 1.0.</returns>
	virtual float getKeyFramesAtTime(TimeIndex% timeIndex, _OUT KeyFrame^ keyFrame1, _OUT KeyFrame^ keyFrame2) = 0;

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
	/// <returns>Parametric value indicating how far along the gap between the 2 keyframes the timeIndex value is, e.g. 0.0 for exactly at 1, 0.25 for a quarter etc. By definition the range of this value is: 0.0 &lt;= returnValue &lt; 1.0.</returns>
	virtual float getKeyFramesAtTime(TimeIndex% timeIndex, _OUT KeyFrame^ keyFrame1, _OUT KeyFrame^ keyFrame2, _OUT unsigned short% firstKeyIndex) = 0;

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
	virtual KeyFrame^ createKeyFrame(float timePos) = 0;

	/// <summary>
	/// Removes a KeyFrame by it's index. 
	/// </summary>
	/// <param name="index">The index of the key frame to remove.</param>
	virtual void removeKeyFrame(unsigned short index) = 0;

	/// <summary>
	/// Removes all the KeyFrames from this track. 
	/// </summary>
	virtual void removeAllKeyFrames() = 0;

	/// <summary>
	/// Returns the parent Animation object for this track. 
	/// </summary>
	/// <returns>The parent animation.</returns>
	Animation^ getParent();
};

}