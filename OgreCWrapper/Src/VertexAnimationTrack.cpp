#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::VertexAnimationType VertexAnimationTrack_getAnimationType(Ogre::VertexAnimationTrack* animTrack)
{
	return animTrack->getAnimationType();
}

extern "C" _AnomalousExport Ogre::VertexMorphKeyFrame* VertexAnimationTrack_createVertexMorphKeyFrame(Ogre::VertexAnimationTrack* animTrack, float timePos)
{
	return animTrack->createVertexMorphKeyFrame(timePos);
}

extern "C" _AnomalousExport Ogre::VertexPoseKeyFrame* VertexAnimationTrack_createVertexPoseKeyFrame(Ogre::VertexAnimationTrack* animTrack, float timePos)
{
	return animTrack->createVertexPoseKeyFrame(timePos);
}

extern "C" _AnomalousExport void VertexAnimationTrack_apply1(Ogre::VertexAnimationTrack* animTrack, float timePos, uint keyIndex)
{
	animTrack->apply(Ogre::TimeIndex(timePos, keyIndex));
}

extern "C" _AnomalousExport void VertexAnimationTrack_apply2(Ogre::VertexAnimationTrack* animTrack, float timePos, uint keyIndex, float weight)
{
	animTrack->apply(Ogre::TimeIndex(timePos, keyIndex), weight);
}

extern "C" _AnomalousExport void VertexAnimationTrack_apply3(Ogre::VertexAnimationTrack* animTrack, float timePos, uint keyIndex, float weight, float scale)
{
	animTrack->apply(Ogre::TimeIndex(timePos, keyIndex), weight, scale);
}

extern "C" _AnomalousExport Ogre::VertexMorphKeyFrame* VertexAnimationTrack_getVertexMorphKeyFrame(Ogre::VertexAnimationTrack* animTrack, ushort index)
{
	return animTrack->getVertexMorphKeyFrame(index);
}

extern "C" _AnomalousExport Ogre::VertexPoseKeyFrame* VertexAnimationTrack_getVertexPoseKeyFrame(Ogre::VertexAnimationTrack* animTrack, ushort index)
{
	return animTrack->getVertexPoseKeyFrame(index);
}

extern "C" _AnomalousExport void VertexAnimationTrack_setTargetMode(Ogre::VertexAnimationTrack* animTrack, Ogre::VertexAnimationTrack::TargetMode m)
{
	animTrack->setTargetMode(m);
}

extern "C" _AnomalousExport Ogre::VertexAnimationTrack::TargetMode VertexAnimationTrack_getTargetMode(Ogre::VertexAnimationTrack* animTrack)
{
	return animTrack->getTargetMode();
}

extern "C" _AnomalousExport Ogre::KeyFrame* VertexAnimationTrack_getKeyFrame(Ogre::VertexAnimationTrack* animTrack, ushort index)
{
	return animTrack->getKeyFrame(index);
}

extern "C" _AnomalousExport float VertexAnimationTrack_getKeyFramesAtTime1(Ogre::VertexAnimationTrack* animTrack, float timePos, uint keyIndex, Ogre::KeyFrame* keyFrame1, Ogre::KeyFrame* keyFrame2)
{
	Ogre::TimeIndex timeIndex(timePos, keyIndex);
	return animTrack->getKeyFramesAtTime(timeIndex, &keyFrame1, &keyFrame2);
}

extern "C" _AnomalousExport float VertexAnimationTrack_getKeyFramesAtTime2(Ogre::VertexAnimationTrack* animTrack, float timePos, uint keyIndex, Ogre::KeyFrame* keyFrame1, Ogre::KeyFrame* keyFrame2, ushort* firstKeyIndex)
{
	Ogre::TimeIndex timeIndex(timePos, keyIndex);
	return animTrack->getKeyFramesAtTime(timeIndex, &keyFrame1, &keyFrame2, firstKeyIndex);
}

extern "C" _AnomalousExport Ogre::KeyFrame* VertexAnimationTrack_createKeyFrame(Ogre::VertexAnimationTrack* animTrack, float timePos)
{
	return animTrack->createKeyFrame(timePos);
}

extern "C" _AnomalousExport void VertexAnimationTrack_removeKeyFrame(Ogre::VertexAnimationTrack* animTrack, ushort index)
{
	animTrack->removeKeyFrame(index);
}

extern "C" _AnomalousExport void VertexAnimationTrack_removeAllKeyFrames(Ogre::VertexAnimationTrack* animTrack)
{
	animTrack->removeAllKeyFrames();
}