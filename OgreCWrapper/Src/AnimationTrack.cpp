#include "Stdafx.h"

extern "C" __declspec(dllexport) bool AnimationTrack_hasNonZeroKeyFrames(Ogre::AnimationTrack* animationTrack)
{
	return animationTrack->hasNonZeroKeyFrames();
}

extern "C" __declspec(dllexport) void AnimationTrack_optimize(Ogre::AnimationTrack* animationTrack)
{
	animationTrack->optimise();
}

extern "C" __declspec(dllexport) ushort AnimationTrack_getHandle(Ogre::AnimationTrack* animationTrack)
{
	return animationTrack->getHandle();
}

extern "C" __declspec(dllexport) ushort AnimationTrack_getNumKeyFrames(Ogre::AnimationTrack* animationTrack)
{
	return animationTrack->getNumKeyFrames();
}