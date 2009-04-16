#include "stdafx.h"
#include "VertexMorphKeyFrame.h"
#include "OgreKeyFrame.h"

namespace OgreWrapper
{

VertexMorphKeyFrame::VertexMorphKeyFrame(Ogre::VertexMorphKeyFrame* ogreFrame)
:KeyFrame(ogreFrame), ogreFrame(ogreFrame)
{

}

VertexMorphKeyFrame::~VertexMorphKeyFrame()
{
	ogreFrame = 0;
}

}