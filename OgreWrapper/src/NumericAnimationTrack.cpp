#include "stdafx.h"
#include "NumericAnimationTrack.h"
#include "Ogre.h"

namespace OgreWrapper
{

NumericAnimationTrack::NumericAnimationTrack(Ogre::NumericAnimationTrack* ogreAnimation)
:ogreAnimation(ogreAnimation)
{

}

NumericAnimationTrack::~NumericAnimationTrack()
{
	ogreAnimation = 0;
}

}