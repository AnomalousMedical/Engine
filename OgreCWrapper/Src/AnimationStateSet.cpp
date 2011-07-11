#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::AnimationState* AnimationStateSet_createAnimationState(Ogre::AnimationStateSet* animationStateSet, String animName, float timePos, float length, float weight, bool enabled)
{
	return animationStateSet->createAnimationState(animName, timePos, length, weight, enabled);
}

extern "C" _AnomalousExport Ogre::AnimationState* AnimationStateSet_getAnimationState(Ogre::AnimationStateSet* animationStateSet, String name)
{
	return animationStateSet->getAnimationState(name);
}

extern "C" _AnomalousExport bool AnimationStateSet_hasAnimationState(Ogre::AnimationStateSet* animationStateSet, String name)
{
	return animationStateSet->hasAnimationState(name);
}

extern "C" _AnomalousExport void AnimationStateSet_removeAnimationState(Ogre::AnimationStateSet* animationStateSet, String name)
{
	animationStateSet->removeAnimationState(name);
}

extern "C" _AnomalousExport void AnimationStateSet_removeAllAnimationStates(Ogre::AnimationStateSet* animationStateSet)
{
	animationStateSet->removeAllAnimationStates();
}

extern "C" _AnomalousExport void AnimationStateSet_copyMatchingState(Ogre::AnimationStateSet* animationStateSet, Ogre::AnimationStateSet* target)
{
	animationStateSet->copyMatchingState(target);
}

extern "C" _AnomalousExport uint AnimationStateSet_getDirtyFrameNumber(Ogre::AnimationStateSet* animationStateSet)
{
	return animationStateSet->getDirtyFrameNumber();
}

extern "C" _AnomalousExport bool AnimationStateSet_hasEnabledAnimationState(Ogre::AnimationStateSet* animationStateSet)
{
	return animationStateSet->hasEnabledAnimationState();
}

extern "C" _AnomalousExport void AnimationStateSet_notifyDirty(Ogre::AnimationStateSet* animationStateSet)
{
	animationStateSet->_notifyDirty();
}

extern "C" _AnomalousExport Ogre::AnimationStateIterator* AnimationStateSet_getAnimationStateIterator(Ogre::AnimationStateSet* animationStateSet)
{
	return new Ogre::AnimationStateIterator(animationStateSet->getAnimationStateIterator());
}

extern "C" _AnomalousExport void AnimationStateSet_iteratorDelete(Ogre::AnimationStateIterator* iter)
{
	//The lifecycle for these starts out by copying the iterator returned by ogre into a new pointer. 
	//This is done in the class that returns the iterator.
	//It is deleted here. This is called by the c# dispose method.
	delete iter;
}