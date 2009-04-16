#pragma once

namespace Ogre
{
	class AnimationState;
}

namespace OgreWrapper
{

ref class AnimationStateSet;

/// <summary>
/// Represents the state of an animation and the weight of it's influence. 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class AnimationState
{
private:
	Ogre::AnimationState* animationState;
	AnimationStateSet^ parent;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="animationState">The AnimationState to wrap.</param>
	AnimationState(Ogre::AnimationState* animationState, AnimationStateSet^ parent);

public:
	/// <summary>
	/// Destructor.
	/// </summary>
	virtual ~AnimationState(void);

	/// <summary>
	/// Get the name of the animation.
	/// </summary>
	/// <returns>The name of the animation.</returns>
	System::String^ getAnimationName();

	/// <summary>
	/// Gets the time position for this animation. 
	/// </summary>
	/// <returns>The time position.</returns>
	float getTimePosition();

	/// <summary>
	/// Sets the time position for this animation. 
	/// </summary>
	/// <param name="timePos">The time to set.</param>
	void setTimePosition(float timePos);

	/// <summary>
	/// Gets the total length of this animation (may be shorter than whole animation). 
	/// </summary>
	/// <returns>The length of the animation.</returns>
	float getLength();

	/// <summary>
	/// Sets the total length of this animation (may be shorter than whole animation).
	/// </summary>
	/// <param name="length">The length to set.</param>
	void setLength(float length);

	/// <summary>
	/// Gets the weight (influence) of this animation.
	/// </summary>
	/// <returns>The weight of the animation.</returns>
	float getWeight();

	/// <summary>
	/// Sets the weight (influence) of this animation.
	/// </summary>
	/// <param name="weight">The weight to set.</param>
	void setWeight(float weight);

	/// <summary>
	/// Modifies the time position, adjusting for animation length. 
	/// </summary>
	/// <param name="offset">The offset to add.</param>
	void addTime(float offset);

	/// <summary>
	/// Returns true if the animation has reached the end and is not looping. 
	/// </summary>
	/// <returns>True if the animation is over and not looping, otherwise false.</returns>
	bool hasEnded();

	/// <summary>
	/// Returns true if this animation is currently enabled. 
	/// </summary>
	/// <returns>True if the animation is enabled.  False if it is disabled.</returns>
	bool getEnabled();

	/// <summary>
	/// Sets whether this animation is enabled. 
	/// </summary>
	/// <param name="enabled">True to enable the animation.  False to disable it.</param>
	void setEnabled(bool enabled);

	/// <summary>
	/// Sets whether or not an animation loops at the start and end of the animation if the time 
	/// continues to be altered. 
	/// </summary>
	/// <param name="loop">True if the animation loops.  False if it does not.</param>
	void setLoop(bool loop);

	/// <summary>
	/// Gets whether or not this animation loops. 
	/// </summary>
	/// <returns>True if the animation loops false if it does not.</returns>
	bool getLoop();

	/// <summary>
	/// Copies the states from another animation state, preserving the animation name (unlike 
	/// operator=) but copying everything else.
	/// </summary>
	/// <param name="animationState">The state to copy from.</param>
	void copyStateFrom(AnimationState^ animationState);

	/// <summary>
	/// Create a new blend mask with the given number of entries In addition to assigning a single 
	/// weight value to a skeletal animation, it may be desirable to assign animation weights per 
	/// bone using a 'blend mask'.
	/// </summary>
	/// <param name="blendMaskSizeHint">The number of bones of the skeleton owning this AnimationState.</param>
	/// <param name="initialWeight">The value all the blend mask entries will be initialised with (negative to skip initialisation).</param>
	void createBlendMask(unsigned int blendMaskSizeHint, float initialWeight);

	/// <summary>
	/// Destroy the currently set blend mask.
	/// </summary>
	void destroyBlendMask();

	/// <summary>
	/// Determine if this animation has a blend mask.
	/// </summary>
	/// <returns>True if the animation has a blend mask set.  False if it does not.</returns>
	bool hasBlendMask();

	/// <summary>
	/// Set the weight for the bone identified by the given handle.
	/// </summary>
	/// <param name="boneHandle">The bone handle to set a blend mask for.</param>
	/// <param name="weight">The weight of the blend mask.</param>
	void setBlendMaskEntry(unsigned int boneHandle, float weight);

	/// <summary>
	/// Get the current weight for the given bone.
	/// </summary>
	/// <param name="boneHandle">The bone handle to get a blend mask for.</param>
	/// <returns>The weight of the bone.</returns>
	float getBlendMaskEntry(unsigned int boneHandle);

	/// <summary>
	/// Get the AnimationStateSet this AnimationState belongs to.
	/// </summary>
	/// <returns>The parent AnimationStateSet.</returns>
	AnimationStateSet^ getParent();
};

}