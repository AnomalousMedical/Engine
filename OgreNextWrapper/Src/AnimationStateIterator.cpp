#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::v1::AnimationState* AnimationStateIterator_peekNextValue(Ogre::v1::AnimationStateIterator* iter)
{
	return iter->peekNextValue();
}

extern "C" _AnomalousExport void AnimationStateIterator_moveNext(Ogre::v1::AnimationStateIterator* iter)
{
	iter->moveNext();
}

extern "C" _AnomalousExport bool AnimationStateIterator_hasMoreElements(Ogre::v1::AnimationStateIterator* iter)
{
	return iter->hasMoreElements();
}