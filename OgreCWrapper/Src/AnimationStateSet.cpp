#include "Stdafx.h"

extern "C" __declspec(dllexport) Ogre::AnimationState* AnimationStateSet_createAnimationState(Ogre::AnimationStateSet* animationStateSet, String animName, float timePos, float length, float weight, bool enabled)
{
	return animationStateSet->createAnimationState(animName, timePos, length, weight, enabled);
}

extern "C" __declspec(dllexport) Ogre::AnimationState* AnimationStateSet_getAnimationState(Ogre::AnimationStateSet* animationStateSet, String name)
{
	return animationStateSet->getAnimationState(name);
}

extern "C" __declspec(dllexport) bool AnimationStateSet_hasAnimationState(Ogre::AnimationStateSet* animationStateSet, String name)
{
	return animationStateSet->hasAnimationState(name);
}

extern "C" __declspec(dllexport) void AnimationStateSet_removeAnimationState(Ogre::AnimationStateSet* animationStateSet, String name)
{
	animationStateSet->removeAnimationState(name);
}

extern "C" __declspec(dllexport) void AnimationStateSet_removeAllAnimationStates(Ogre::AnimationStateSet* animationStateSet)
{
	animationStateSet->removeAllAnimationStates();
}

extern "C" __declspec(dllexport) void AnimationStateSet_copyMatchingState(Ogre::AnimationStateSet* animationStateSet, Ogre::AnimationStateSet* target)
{
	animationStateSet->copyMatchingState(target);
}

extern "C" __declspec(dllexport) uint AnimationStateSet_getDirtyFrameNumber(Ogre::AnimationStateSet* animationStateSet)
{
	return animationStateSet->getDirtyFrameNumber();
}

extern "C" __declspec(dllexport) bool AnimationStateSet_hasEnabledAnimationState(Ogre::AnimationStateSet* animationStateSet)
{
	return animationStateSet->hasEnabledAnimationState();
}

extern "C" __declspec(dllexport) void AnimationStateSet_notifyDirty(Ogre::AnimationStateSet* animationStateSet)
{
	animationStateSet->_notifyDirty();
}