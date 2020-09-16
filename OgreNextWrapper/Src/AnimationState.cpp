#include "Stdafx.h"

extern "C" _AnomalousExport String AnimationState_getAnimationName(Ogre::v1::AnimationState* animationState)
{
	return animationState->getAnimationName().c_str();
}

extern "C" _AnomalousExport float AnimationState_getTimePosition(Ogre::v1::AnimationState* animationState)
{
	return animationState->getTimePosition();
}

extern "C" _AnomalousExport void AnimationState_setTimePosition(Ogre::v1::AnimationState* animationState, float timePos)
{
	animationState->setTimePosition(timePos);
}

extern "C" _AnomalousExport float AnimationState_getLength(Ogre::v1::AnimationState* animationState)
{
	return animationState->getLength();
}

extern "C" _AnomalousExport void AnimationState_setLength(Ogre::v1::AnimationState* animationState, float length)
{
	animationState->setLength(length);
}

extern "C" _AnomalousExport float AnimationState_getWeight(Ogre::v1::AnimationState* animationState)
{
	return animationState->getWeight();
}

extern "C" _AnomalousExport void AnimationState_setWeight(Ogre::v1::AnimationState* animationState, float weight)
{
	animationState->setWeight(weight);
}

extern "C" _AnomalousExport void AnimationState_addTime(Ogre::v1::AnimationState* animationState, float offset)
{
	animationState->addTime(offset);
}

extern "C" _AnomalousExport bool AnimationState_hasEnded(Ogre::v1::AnimationState* animationState)
{
	return animationState->hasEnded();
}

extern "C" _AnomalousExport bool AnimationState_getEnabled(Ogre::v1::AnimationState* animationState)
{
	return animationState->getEnabled();
}

extern "C" _AnomalousExport void AnimationState_setEnabled(Ogre::v1::AnimationState* animationState, bool enabled)
{
	animationState->setEnabled(enabled);
}

extern "C" _AnomalousExport void AnimationState_setLoop(Ogre::v1::AnimationState* animationState, bool loop)
{
	animationState->setLoop(loop);
}

extern "C" _AnomalousExport bool AnimationState_getLoop(Ogre::v1::AnimationState* animationState)
{
	return animationState->getLoop();
}

extern "C" _AnomalousExport void AnimationState_copyStateFrom(Ogre::v1::AnimationState* animationState, Ogre::v1::AnimationState* copyState)
{
	animationState->copyStateFrom(*copyState);
}

extern "C" _AnomalousExport void AnimationState_createBlendMask(Ogre::v1::AnimationState* animationState, uint blendMaskSizeHint, float initialWeight)
{
	animationState->createBlendMask(blendMaskSizeHint, initialWeight);
}

extern "C" _AnomalousExport void AnimationState_destroyBlendMask(Ogre::v1::AnimationState* animationState)
{
	animationState->destroyBlendMask();
}

extern "C" _AnomalousExport bool AnimationState_hasBlendMask(Ogre::v1::AnimationState* animationState)
{
	return animationState->hasBlendMask();
}

extern "C" _AnomalousExport void AnimationState_setBlendMaskEntry(Ogre::v1::AnimationState* animationState, uint boneHandle, float weight)
{
	animationState->setBlendMaskEntry(boneHandle, weight);
}

extern "C" _AnomalousExport float AnimationState_getBlendMaskEntry(Ogre::v1::AnimationState* animationState, uint boneHandle)
{
	return animationState->getBlendMaskEntry(boneHandle);
}