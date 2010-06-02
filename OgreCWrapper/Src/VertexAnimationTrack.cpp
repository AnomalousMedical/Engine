#include "Stdafx.h"

extern "C" __declspec(dllexport) Ogre::VertexAnimationType VertexAnimationTrack_getAnimationType(Ogre::VertexAnimationTrack* animTrack)
{
	return animTrack->getAnimationType();
}

extern "C" __declspec(dllexport) Ogre::VertexMorphKeyFrame* VertexAnimationTrack_createVertexMorphKeyFrame(Ogre::VertexAnimationTrack* animTrack, float timePos)
{
	return animTrack->createVertexMorphKeyFrame(timePos);
}

extern "C" __declspec(dllexport) Ogre::VertexPoseKeyFrame* VertexAnimationTrack_createVertexPoseKeyFrame(Ogre::VertexAnimationTrack* animTrack, float timePos)
{
	return animTrack->createVertexPoseKeyFrame(timePos);
}

extern "C" __declspec(dllexport) void VertexAnimationTrack_apply1(Ogre::VertexAnimationTrack* animTrack, float timePos, uint keyIndex)
{
	animTrack->apply(Ogre::TimeIndex(timePos, keyIndex));
}

extern "C" __declspec(dllexport) void VertexAnimationTrack_apply2(Ogre::VertexAnimationTrack* animTrack, float timePos, uint keyIndex, float weight)
{
	animTrack->apply(Ogre::TimeIndex(timePos, keyIndex), weight);
}

extern "C" __declspec(dllexport) void VertexAnimationTrack_apply3(Ogre::VertexAnimationTrack* animTrack, float timePos, uint keyIndex, float weight, float scale)
{
	animTrack->apply(Ogre::TimeIndex(timePos, keyIndex), weight, scale);
}

extern "C" __declspec(dllexport) Ogre::VertexMorphKeyFrame* VertexAnimationTrack_getVertexMorphKeyFrame(Ogre::VertexAnimationTrack* animTrack, ushort index)
{
	return animTrack->getVertexMorphKeyFrame(index);
}

extern "C" __declspec(dllexport) Ogre::VertexPoseKeyFrame* VertexAnimationTrack_getVertexPoseKeyFrame(Ogre::VertexAnimationTrack* animTrack, ushort index)
{
	return animTrack->getVertexPoseKeyFrame(index);
}

extern "C" __declspec(dllexport) void VertexAnimationTrack_setTargetMode(Ogre::VertexAnimationTrack* animTrack, Ogre::VertexAnimationTrack::TargetMode m)
{
	animTrack->setTargetMode(m);
}

extern "C" __declspec(dllexport) Ogre::VertexAnimationTrack::TargetMode VertexAnimationTrack_getTargetMode(Ogre::VertexAnimationTrack* animTrack)
{
	return animTrack->getTargetMode();
}

extern "C" __declspec(dllexport) Ogre::KeyFrame* VertexAnimationTrack_getKeyFrame(Ogre::VertexAnimationTrack* animTrack, ushort index)
{
	return animTrack->getKeyFrame(index);
}

extern "C" __declspec(dllexport) float VertexAnimationTrack_getKeyFramesAtTime1(Ogre::VertexAnimationTrack* animTrack, float timePos, uint keyIndex, Ogre::KeyFrame* keyFrame1, Ogre::KeyFrame* keyFrame2)
{
	Ogre::TimeIndex timeIndex(timePos, keyIndex);
	return animTrack->getKeyFramesAtTime(timeIndex, &keyFrame1, &keyFrame2);
}

extern "C" __declspec(dllexport) float VertexAnimationTrack_getKeyFramesAtTime2(Ogre::VertexAnimationTrack* animTrack, float timePos, uint keyIndex, Ogre::KeyFrame* keyFrame1, Ogre::KeyFrame* keyFrame2, ushort* firstKeyIndex)
{
	Ogre::TimeIndex timeIndex(timePos, keyIndex);
	return animTrack->getKeyFramesAtTime(timeIndex, &keyFrame1, &keyFrame2, firstKeyIndex);
}

extern "C" __declspec(dllexport) Ogre::KeyFrame* VertexAnimationTrack_createKeyFrame(Ogre::VertexAnimationTrack* animTrack, float timePos)
{
	return animTrack->createKeyFrame(timePos);
}

extern "C" __declspec(dllexport) void VertexAnimationTrack_removeKeyFrame(Ogre::VertexAnimationTrack* animTrack, ushort index)
{
	animTrack->removeKeyFrame(index);
}

extern "C" __declspec(dllexport) void VertexAnimationTrack_removeAllKeyFrames(Ogre::VertexAnimationTrack* animTrack)
{
	animTrack->removeAllKeyFrames();
}