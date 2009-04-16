#include "stdafx.h"
#include "KeyFrame.h"
#include "OgreKeyFrame.h"

namespace OgreWrapper
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