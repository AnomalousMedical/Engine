#include "stdafx.h"
#include "NumericAnimationTrack.h"
#include "Ogre.h"

namespace Engine
{

namespace Rendering
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

}