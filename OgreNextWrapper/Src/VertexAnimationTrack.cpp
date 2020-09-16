#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::v1::VertexAnimationType VertexAnimationTrack_getAnimationType(Ogre::v1::VertexAnimationTrack* animTrack)
{
	return animTrack->getAnimationType();
}

extern "C" _AnomalousExport Ogre::v1::VertexMorphKeyFrame* VertexAnimationTrack_createVertexMorphKeyFrame(Ogre::v1::VertexAnimationTrack* animTrack, float timePos)
{
	return animTrack->createVertexMorphKeyFrame(timePos);
}

extern "C" _AnomalousExport Ogre::v1::VertexPoseKeyFrame* VertexAnimationTrack_createVertexPoseKeyFrame(Ogre::v1::VertexAnimationTrack* animTrack, float timePos)
{
	return animTrack->createVertexPoseKeyFrame(timePos);
}

extern "C" _AnomalousExport void VertexAnimationTrack_apply1(Ogre::v1::VertexAnimationTrack* animTrack, float timePos, uint keyIndex)
{
	animTrack->apply(Ogre::v1::TimeIndex(timePos, keyIndex));
}

extern "C" _AnomalousExport void VertexAnimationTrack_apply2(Ogre::v1::VertexAnimationTrack* animTrack, float timePos, uint keyIndex, float weight)
{
	animTrack->apply(Ogre::v1::TimeIndex(timePos, keyIndex), weight);
}

extern "C" _AnomalousExport void VertexAnimationTrack_apply3(Ogre::v1::VertexAnimationTrack* animTrack, float timePos, uint keyIndex, float weight, float scale)
{
	animTrack->apply(Ogre::v1::TimeIndex(timePos, keyIndex), weight, scale);
}

extern "C" _AnomalousExport Ogre::v1::VertexMorphKeyFrame* VertexAnimationTrack_getVertexMorphKeyFrame(Ogre::v1::VertexAnimationTrack* animTrack, ushort index)
{
	return animTrack->getVertexMorphKeyFrame(index);
}

extern "C" _AnomalousExport Ogre::v1::VertexPoseKeyFrame* VertexAnimationTrack_getVertexPoseKeyFrame(Ogre::v1::VertexAnimationTrack* animTrack, ushort index)
{
	return animTrack->getVertexPoseKeyFrame(index);
}

extern "C" _AnomalousExport void VertexAnimationTrack_setTargetMode(Ogre::v1::VertexAnimationTrack* animTrack, Ogre::v1::VertexAnimationTrack::TargetMode m)
{
	animTrack->setTargetMode(m);
}

extern "C" _AnomalousExport Ogre::v1::VertexAnimationTrack::TargetMode VertexAnimationTrack_getTargetMode(Ogre::v1::VertexAnimationTrack* animTrack)
{
	return animTrack->getTargetMode();
}

extern "C" _AnomalousExport Ogre::v1::KeyFrame* VertexAnimationTrack_getKeyFrame(Ogre::v1::VertexAnimationTrack* animTrack, ushort index)
{
	return animTrack->getKeyFrame(index);
}

extern "C" _AnomalousExport float VertexAnimationTrack_getKeyFramesAtTime1(Ogre::v1::VertexAnimationTrack* animTrack, float timePos, uint keyIndex, Ogre::v1::KeyFrame* keyFrame1, Ogre::v1::KeyFrame* keyFrame2)
{
	Ogre::v1::TimeIndex timeIndex(timePos, keyIndex);
	return animTrack->getKeyFramesAtTime(timeIndex, &keyFrame1, &keyFrame2);
}

extern "C" _AnomalousExport float VertexAnimationTrack_getKeyFramesAtTime2(Ogre::v1::VertexAnimationTrack* animTrack, float timePos, uint keyIndex, Ogre::v1::KeyFrame* keyFrame1, Ogre::v1::KeyFrame* keyFrame2, ushort* firstKeyIndex)
{
	Ogre::v1::TimeIndex timeIndex(timePos, keyIndex);
	return animTrack->getKeyFramesAtTime(timeIndex, &keyFrame1, &keyFrame2, firstKeyIndex);
}

extern "C" _AnomalousExport Ogre::v1::KeyFrame* VertexAnimationTrack_createKeyFrame(Ogre::v1::VertexAnimationTrack* animTrack, float timePos)
{
	return animTrack->createKeyFrame(timePos);
}

extern "C" _AnomalousExport void VertexAnimationTrack_removeKeyFrame(Ogre::v1::VertexAnimationTrack* animTrack, ushort index)
{
	animTrack->removeKeyFrame(index);
}

extern "C" _AnomalousExport void VertexAnimationTrack_removeAllKeyFrames(Ogre::v1::VertexAnimationTrack* animTrack)
{
	animTrack->removeAllKeyFrames();
}