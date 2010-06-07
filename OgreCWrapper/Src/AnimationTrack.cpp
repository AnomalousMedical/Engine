#include "Stdafx.h"

extern "C" _AnomalousExport bool AnimationTrack_hasNonZeroKeyFrames(Ogre::AnimationTrack* animationTrack)
{
	return animationTrack->hasNonZeroKeyFrames();
}

extern "C" _AnomalousExport void AnimationTrack_optimize(Ogre::AnimationTrack* animationTrack)
{
	animationTrack->optimise();
}

extern "C" _AnomalousExport ushort AnimationTrack_getHandle(Ogre::AnimationTrack* animationTrack)
{
	return animationTrack->getHandle();
}

extern "C" _AnomalousExport ushort AnimationTrack_getNumKeyFrames(Ogre::AnimationTrack* animationTrack)
{
	return animationTrack->getNumKeyFrames();
}