#include "Stdafx.h"

extern "C" _AnomalousExport void VertexPoseKeyFrame_addPoseReference(Ogre::v1::VertexPoseKeyFrame* vpKeyFrame, ushort poseIndex, float influence)
{
	vpKeyFrame->addPoseReference(poseIndex, influence);
}

extern "C" _AnomalousExport void VertexPoseKeyFrame_updatePoseReference(Ogre::v1::VertexPoseKeyFrame* vpKeyFrame, ushort poseIndex, float influence)
{
	vpKeyFrame->updatePoseReference(poseIndex, influence);
}

extern "C" _AnomalousExport void VertexPoseKeyFrame_removePoseReference(Ogre::v1::VertexPoseKeyFrame* vpKeyFrame, ushort poseIndex)
{
	vpKeyFrame->removePoseReference(poseIndex);
}

extern "C" _AnomalousExport void VertexPoseKeyFrame_removeAllPoseReferences(Ogre::v1::VertexPoseKeyFrame* vpKeyFrame)
{
	vpKeyFrame->removeAllPoseReferences();
}