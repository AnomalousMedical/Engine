#pragma once

#include "AnimationStateCollection.h"

namespace Ogre
{
	class AnimationStateSet;
}

namespace OgreWrapper{

ref class AnimationState;

/// <summary>
/// Wrapper for AnimationStateSets.  This is a set of pose animations for an entity.
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class AnimationStateSet
{
private:
	Ogre::AnimationStateSet* animationStateSet;
	AnimationStateCollection states;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="animationStateSet">The Ogre AnimationStateSet to wrap.</param>
	AnimationStateSet(Ogre::AnimationStateSet* animationStateSet);

	Ogre::AnimationStateSet* getOgreAnimationStateSet()
	{
		return animationStateSet;
	}

public:
	/// <summary>
	/// Destructor.
	/// </summary>
	virtual ~AnimationStateSet(void);

	/// <summary>
	/// Create a new AnimationState instance. 
	/// </summary>
	/// <param name="animName">The name of the animation.</param>
	/// <param name="timePos">Starting time position.</param>
	/// <param name="length">Length of the animation to play.</param>
	/// <param name="weight">Weight to apply the animation with.</param>
	/// <param name="enabled">Whether the animation is enabled.</param>
	/// <returns>A new AnimationState with the given parameters.</returns>
	AnimationState^ createAnimationState(System::String^ animName, float timePos, float length, float weight, bool enabled);

	/// <summary>
	/// Get an animation state by the name of the animation. 
	/// </summary>
	/// <param name="name">The name of the state to search for.</param>
	/// <returns>The animation state specified by name or null if it does not exist.</returns>
	AnimationState^ getAnimationState(System::String^ name);

	/// <summary>
	/// Tests if state for the named animation is present. 
	/// </summary>
	/// <param name="name">The state to search for.</param>
	/// <returns>True if the animation state is found.  False if it is not.</returns>
	bool hasAnimationState(System::String^ name);

	/// <summary>
	/// Remove animation state with the given name.
	/// </summary>
	/// <param name="name">The name of the state to remove.</param>
	void removeAnimationState(System::String^ name);

	/// <summary>
	/// Remove all animation states. 
	/// </summary>
	void removeAllAnimationStates();

	/// <summary>
	/// Copy the state of any matching animation states from this to another. 
	/// </summary>
	/// <param name="target">The AnimationStateSet to copy states to.</param>
	void copyMatchingState(AnimationStateSet^ target);

	/// <summary>
	/// Get the latest animation state been altered frame number.
	/// </summary>
	/// <returns>The last frame the state was modified.</returns>
	unsigned long getDirtyFrameNumber();

	/// <summary>
	/// Tests if exists enabled animation state in this set. (Ogre Comment).
	/// </summary>
	/// <returns>Uh I dunno.</returns>
	bool hasEnabledAnimationState();

	/// <summary>
	/// Notify that this animation state set is dirty.
	/// </summary>
	void notifyDirty();
};

}