#pragma once

#include "AnimationTrack.h"
#include "TransformKeyFrameCollection.h"

namespace Ogre
{
	class NodeAnimationTrack;
}

namespace Rendering
{

ref class TransformKeyFrame;
ref class RenderNode;

/// <summary>
/// Specialised AnimationTrack for dealing with node transforms. 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class NodeAnimationTrack : public AnimationTrack
{
private:
	Ogre::NodeAnimationTrack* ogreAnimation;
	TransformKeyFrameCollection transforms;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="ogreAnimation">The Ogre::NodeAnimationTrack to wrap.</param>
	NodeAnimationTrack(Ogre::NodeAnimationTrack* ogreAnimation, Animation^ parent);

	/// <summary>
	/// Destructor.
	/// </summary>
	~NodeAnimationTrack();

public:
	TransformKeyFrame^ createNodeKeyFrame(float timePos);

	RenderNode^ getAssociatedNode();

	void setAssociatedNode(RenderNode^ node);

	void applyToNode(RenderNode^ node, TimeIndex timeIndex);

	void applyToNode(RenderNode^ node, TimeIndex timeIndex, float weight);

	void applyToNode(RenderNode^ node, TimeIndex timeIndex, float weight, float scale);

	void applyToNode(RenderNode^ node, TimeIndex% timeIndex);

	void applyToNode(RenderNode^ node, TimeIndex% timeIndex, float weight);

	void applyToNode(RenderNode^ node, TimeIndex% timeIndex, float weight, float scale);

	void setUseShortestRotationPath(bool useShortestPath);

	bool getUseShortestRotationPath();

	KeyFrame^ getInterpolatedKeyFrame(TimeIndex timeIndex);

	void apply(TimeIndex timeIndex);

	void apply(TimeIndex timeIndex, float weight);

	void apply(TimeIndex timeIndex, float weight, float scale);

	void apply(TimeIndex% timeIndex);

	void apply(TimeIndex% timeIndex, float weight);

	void apply(TimeIndex% timeIndex, float weight, float scale);

	TransformKeyFrame^ getNodeKeyFrame(unsigned short index);

	/// <summary>
	/// Returns the KeyFrame at the specified index. 
	/// </summary>
	/// <param name="index">The index of the key frame.</param>
	/// <returns>The key frame at the given index, or null if the index does not exist.</returns>
	virtual KeyFrame^ getKeyFrame(unsigned short index) override;

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
	/// <returns>Parametric value indicating how far along the gap between the 2 keyframes the timeIndex value is, e.g. 0.0 for exactly at 1, 0.25 for a quarter etc. By definition the range of this value is: 0.0 &lt;= returnValue &lt; 1.0.</returns>
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