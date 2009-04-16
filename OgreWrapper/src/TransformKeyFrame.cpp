#include "stdafx.h"
#include "TransformKeyFrame.h"
#include "Ogre.h"

namespace Engine
{

namespace Rendering
{

TransformKeyFrame::TransformKeyFrame(Ogre::TransformKeyFrame* ogreAnimation)
:ogreAnimation(ogreAnimation)
{

}

TransformKeyFrame::~TransformKeyFrame()
{
	ogreAnimation = 0;
}

}

}