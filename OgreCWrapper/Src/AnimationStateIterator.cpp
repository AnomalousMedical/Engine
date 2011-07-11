#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::AnimationState* AnimationStateIterator_peekNextValue(Ogre::AnimationStateIterator* iter)
{
	return iter->peekNextValue();
}

extern "C" _AnomalousExport void AnimationStateIterator_moveNext(Ogre::AnimationStateIterator* iter)
{
	iter->moveNext();
}

extern "C" _AnomalousExport bool AnimationStateIterator_hasMoreElements(Ogre::AnimationStateIterator* iter)
{
	return iter->hasMoreElements();
}