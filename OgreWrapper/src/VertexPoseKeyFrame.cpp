#include "stdafx.h"
#include "VertexPoseKeyFrame.h"
#include "OgreKeyFrame.h"

namespace OgreWrapper
{

VertexPoseKeyFrame::VertexPoseKeyFrame(Ogre::VertexPoseKeyFrame* ogreFrame)
:KeyFrame(ogreFrame), ogreFrame(ogreFrame)
{

}

VertexPoseKeyFrame::~VertexPoseKeyFrame()
{
	ogreFrame = 0;
}

void VertexPoseKeyFrame::addPoseReference(unsigned short poseIndex, float influence)
{
	ogreFrame->addPoseReference(poseIndex, influence);
}

void VertexPoseKeyFrame::updatePoseReference(unsigned short poseIndex, float influence)
{
	ogreFrame->updatePoseReference(poseIndex, influence);
}

void VertexPoseKeyFrame::removePoseReference(unsigned short poseIndex)
{
	ogreFrame->removePoseReference(poseIndex);
}

void VertexPoseKeyFrame::removeAllPoseReferences()
{
	ogreFrame->removeAllPoseReferences();
}

}