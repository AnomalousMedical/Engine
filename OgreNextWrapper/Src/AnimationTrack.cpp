#include "Stdafx.h"

extern "C" _AnomalousExport bool AnimationTrack_hasNonZeroKeyFrames(Ogre::v1::AnimationTrack* animationTrack)
{
	return animationTrack->hasNonZeroKeyFrames();
}

extern "C" _AnomalousExport void AnimationTrack_optimize(Ogre::v1::AnimationTrack* animationTrack)
{
	animationTrack->optimise();
}

extern "C" _AnomalousExport ushort AnimationTrack_getHandle(Ogre::v1::AnimationTrack* animationTrack)
{
	return animationTrack->getHandle();
}

extern "C" _AnomalousExport ushort AnimationTrack_getNumKeyFrames(Ogre::v1::AnimationTrack* animationTrack)
{
	return animationTrack->getNumKeyFrames();
}