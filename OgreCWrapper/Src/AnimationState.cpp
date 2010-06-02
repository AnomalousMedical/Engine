#include "Stdafx.h"

extern "C" __declspec(dllexport) String AnimationState_getAnimationName(Ogre::AnimationState* animationState)
{
	return animationState->getAnimationName().c_str();
}

extern "C" __declspec(dllexport) float AnimationState_getTimePosition(Ogre::AnimationState* animationState)
{
	return animationState->getTimePosition();
}

extern "C" __declspec(dllexport) void AnimationState_setTimePosition(Ogre::AnimationState* animationState, float timePos)
{
	animationState->setTimePosition(timePos);
}

extern "C" __declspec(dllexport) float AnimationState_getLength(Ogre::AnimationState* animationState)
{
	return animationState->getLength();
}

extern "C" __declspec(dllexport) void AnimationState_setLength(Ogre::AnimationState* animationState, float length)
{
	animationState->setLength(length);
}

extern "C" __declspec(dllexport) float AnimationState_getWeight(Ogre::AnimationState* animationState)
{
	return animationState->getWeight();
}

extern "C" __declspec(dllexport) void AnimationState_setWeight(Ogre::AnimationState* animationState, float weight)
{
	animationState->setWeight(weight);
}

extern "C" __declspec(dllexport) void AnimationState_addTime(Ogre::AnimationState* animationState, float offset)
{
	animationState->addTime(offset);
}

extern "C" __declspec(dllexport) bool AnimationState_hasEnded(Ogre::AnimationState* animationState)
{
	return animationState->hasEnded();
}

extern "C" __declspec(dllexport) bool AnimationState_getEnabled(Ogre::AnimationState* animationState)
{
	return animationState->getEnabled();
}

extern "C" __declspec(dllexport) void AnimationState_setEnabled(Ogre::AnimationState* animationState, bool enabled)
{
	animationState->setEnabled(enabled);
}

extern "C" __declspec(dllexport) void AnimationState_setLoop(Ogre::AnimationState* animationState, bool loop)
{
	animationState->setLoop(loop);
}

extern "C" __declspec(dllexport) bool AnimationState_getLoop(Ogre::AnimationState* animationState)
{
	return animationState->getLoop();
}

extern "C" __declspec(dllexport) void AnimationState_copyStateFrom(Ogre::AnimationState* animationState, Ogre::AnimationState* copyState)
{
	animationState->copyStateFrom(*copyState);
}

extern "C" __declspec(dllexport) void AnimationState_createBlendMask(Ogre::AnimationState* animationState, uint blendMaskSizeHint, float initialWeight)
{
	animationState->createBlendMask(blendMaskSizeHint, initialWeight);
}

extern "C" __declspec(dllexport) void AnimationState_destroyBlendMask(Ogre::AnimationState* animationState)
{
	animationState->destroyBlendMask();
}

extern "C" __declspec(dllexport) bool AnimationState_hasBlendMask(Ogre::AnimationState* animationState)
{
	return animationState->hasBlendMask();
}

extern "C" __declspec(dllexport) void AnimationState_setBlendMaskEntry(Ogre::AnimationState* animationState, uint boneHandle, float weight)
{
	animationState->setBlendMaskEntry(boneHandle, weight);
}

extern "C" __declspec(dllexport) float AnimationState_getBlendMaskEntry(Ogre::AnimationState* animationState, uint boneHandle)
{
	return animationState->getBlendMaskEntry(boneHandle);
}