#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::v1::KeyFrame* NodeAnimationTrack_getKeyFrame(Ogre::v1::NodeAnimationTrack* animTrack, ushort index)
{
	return animTrack->getKeyFrame(index);
}

extern "C" _AnomalousExport float NodeAnimationTrack_getKeyFramesAtTime1(Ogre::v1::NodeAnimationTrack* animTrack, float timePos, uint keyIndex, Ogre::v1::KeyFrame* keyFrame1, Ogre::v1::KeyFrame* keyFrame2)
{
	Ogre::v1::TimeIndex timeIndex(timePos, keyIndex);
	return animTrack->getKeyFramesAtTime(timeIndex, &keyFrame1, &keyFrame2);
}

extern "C" _AnomalousExport float NodeAnimationTrack_getKeyFramesAtTime2(Ogre::v1::NodeAnimationTrack* animTrack, float timePos, uint keyIndex, Ogre::v1::KeyFrame* keyFrame1, Ogre::v1::KeyFrame* keyFrame2, ushort* firstKeyIndex)
{
	Ogre::v1::TimeIndex timeIndex(timePos, keyIndex);
	return animTrack->getKeyFramesAtTime(timeIndex, &keyFrame1, &keyFrame2, firstKeyIndex);
}

extern "C" _AnomalousExport Ogre::v1::KeyFrame* NodeAnimationTrack_createKeyFrame(Ogre::v1::NodeAnimationTrack* animTrack, float timePos)
{
	return animTrack->createKeyFrame(timePos);
}

extern "C" _AnomalousExport void NodeAnimationTrack_removeKeyFrame(Ogre::v1::NodeAnimationTrack* animTrack, ushort index)
{
	animTrack->removeKeyFrame(index);
}

extern "C" _AnomalousExport void NodeAnimationTrack_removeAllKeyFrames(Ogre::v1::NodeAnimationTrack* animTrack)
{
	animTrack->removeAllKeyFrames();
}