#include "stdafx.h"
#include "KeyFrame.h"
#include "OgreKeyFrame.h"

namespace Engine
{

namespace Rendering
{

KeyFrame::KeyFrame(Ogre::KeyFrame* ogreFrame)
:ogreFrame(ogreFrame)
{

}

float KeyFrame::getTime()
{
	return ogreFrame->getTime();
}

}

}