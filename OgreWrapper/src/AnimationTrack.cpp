#include "stdafx.h"
#include "AnimationTrack.h"
#include "OgreAnimationTrack.h"

namespace Rendering
{

AnimationTrack::AnimationTrack(Ogre::AnimationTrack* ogreTrack, Animation^ parent)
:ogreTrack(ogreTrack), parent(parent)
{

}

bool AnimationTrack::hasNonZeroKeyFrames()
{
	return ogreTrack->hasNonZeroKeyFrames();
}

void AnimationTrack::optimize()
{
	ogreTrack->optimise();
}

unsigned short AnimationTrack::getHandle()
{
	return ogreTrack->getHandle();
}

unsigned short AnimationTrack::getNumKeyFrames()
{
	return ogreTrack->getNumKeyFrames();
}

Animation^ AnimationTrack::getParent()
{
	return parent;
}

}