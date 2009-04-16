#include "stdafx.h"
#include "VertexMorphKeyFrame.h"
#include "OgreKeyFrame.h"

namespace Rendering
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