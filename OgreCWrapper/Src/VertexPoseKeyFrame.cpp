#include "Stdafx.h"

extern "C" __declspec(dllexport) void VertexPoseKeyFrame_addPoseReference(Ogre::VertexPoseKeyFrame* vpKeyFrame, ushort poseIndex, float influence)
{
	vpKeyFrame->addPoseReference(poseIndex, influence);
}

extern "C" __declspec(dllexport) void VertexPoseKeyFrame_updatePoseReference(Ogre::VertexPoseKeyFrame* vpKeyFrame, ushort poseIndex, float influence)
{
	vpKeyFrame->updatePoseReference(poseIndex, influence);
}

extern "C" __declspec(dllexport) void VertexPoseKeyFrame_removePoseReference(Ogre::VertexPoseKeyFrame* vpKeyFrame, ushort poseIndex)
{
	vpKeyFrame->removePoseReference(poseIndex);
}

extern "C" __declspec(dllexport) void VertexPoseKeyFrame_removeAllPoseReferences(Ogre::VertexPoseKeyFrame* vpKeyFrame)
{
	vpKeyFrame->removeAllPoseReferences();
}