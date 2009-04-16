#include "stdafx.h"
#include "VertexAnimationTrack.h"
#include "Ogre.h"
#include "VertexMorphKeyFrame.h"
#include "VertexPoseKeyFrame.h"

namespace Rendering
{

VertexAnimationTrack::VertexAnimationTrack(Ogre::VertexAnimationTrack* ogreAnimation, Animation^ parent)
:AnimationTrack(ogreAnimation, parent), 
ogreAnimation(ogreAnimation)
{

}

VertexAnimationTrack::~VertexAnimationTrack()
{
	ogreAnimation = 0;
}

VertexAnimationType VertexAnimationTrack::getAnimationType()
{
	return (VertexAnimationType)ogreAnimation->getAnimationType();
}

VertexMorphKeyFrame^ VertexAnimationTrack::createVertexMorphKeyFrame(float timePos)
{
	return morphs.getObject(ogreAnimation->createVertexMorphKeyFrame(timePos));
}

VertexPoseKeyFrame^ VertexAnimationTrack::createVertexPoseKeyFrame(float timePos)
{
	return poses.getObject(ogreAnimation->createVertexPoseKeyFrame(timePos));
}

void VertexAnimationTrack::apply(TimeIndex timeIndex)
{
	return ogreAnimation->apply(Ogre::TimeIndex(timeIndex.getTimePos(), timeIndex.getKeyIndex()));
}

void VertexAnimationTrack::apply(TimeIndex timeIndex, float weight)
{
	return ogreAnimation->apply(Ogre::TimeIndex(timeIndex.getTimePos(), timeIndex.getKeyIndex()), weight);
}

void VertexAnimationTrack::apply(TimeIndex timeIndex, float weight, float scale)
{
	return ogreAnimation->apply(Ogre::TimeIndex(timeIndex.getTimePos(), timeIndex.getKeyIndex()), weight, scale);
}

void VertexAnimationTrack::apply(TimeIndex% timeIndex)
{
	return ogreAnimation->apply(Ogre::TimeIndex(timeIndex.getTimePos(), timeIndex.getKeyIndex()));
}

void VertexAnimationTrack::apply(TimeIndex% timeIndex, float weight)
{
	return ogreAnimation->apply(Ogre::TimeIndex(timeIndex.getTimePos(), timeIndex.getKeyIndex()), weight);
}

void VertexAnimationTrack::apply(TimeIndex% timeIndex, float weight, float scale)
{
	return ogreAnimation->apply(Ogre::TimeIndex(timeIndex.getTimePos(), timeIndex.getKeyIndex()), weight, scale);
}

VertexMorphKeyFrame^ VertexAnimationTrack::getVertexMorphKeyFrame(unsigned short index)
{
	return morphs.getObject(ogreAnimation->getVertexMorphKeyFrame(index));
}

VertexPoseKeyFrame^ VertexAnimationTrack::getVertexPoseKeyFrame(unsigned short index)
{
	return poses.getObject(ogreAnimation->getVertexPoseKeyFrame(index));
}

void VertexAnimationTrack::setTargetMode(VertexAnimationTrack::TargetMode m)
{
	return ogreAnimation->setTargetMode((Ogre::VertexAnimationTrack::TargetMode)m);
}

VertexAnimationTrack::TargetMode VertexAnimationTrack::getTargetMode()
{
	return (TargetMode)ogreAnimation->getTargetMode();
}

KeyFrame^ VertexAnimationTrack::getKeyFrame(unsigned short index)
{
	switch (ogreAnimation->getAnimationType())
	{
		case Ogre::VertexAnimationType::VAT_MORPH:
			return getVertexMorphKeyFrame(index);
			break;
		case Ogre::VertexAnimationType::VAT_POSE:
			return getVertexPoseKeyFrame(index);
			break;
		default:
			return nullptr;
			break;
	}
}

float VertexAnimationTrack::getKeyFramesAtTime(TimeIndex% timeIndex, _OUT KeyFrame^ keyFrame1, _OUT KeyFrame^ keyFrame2)
{
	unsigned short foo = 0;
	return getKeyFramesAtTime(timeIndex, keyFrame1, keyFrame2, foo);
}

float VertexAnimationTrack::getKeyFramesAtTime(TimeIndex% timeIndex, _OUT KeyFrame^ keyFrame1, _OUT KeyFrame^ keyFrame2, _OUT unsigned short% firstKeyIndex)
{
	Ogre::KeyFrame *ogreFrame1, *ogreFrame2;
	unsigned short foo;
	float ret = ogreAnimation->getKeyFramesAtTime(Ogre::TimeIndex(timeIndex.getTimePos(), timeIndex.getKeyIndex()), &ogreFrame1, &ogreFrame2, &foo);
	firstKeyIndex = foo;
		switch (ogreAnimation->getAnimationType())
	{
		case Ogre::VertexAnimationType::VAT_MORPH:
			keyFrame1 = morphs.getObject((Ogre::VertexMorphKeyFrame*)ogreFrame1);
			keyFrame2 = morphs.getObject((Ogre::VertexMorphKeyFrame*)ogreFrame2);
			break;
		case Ogre::VertexAnimationType::VAT_POSE:
			keyFrame1 = poses.getObject((Ogre::VertexPoseKeyFrame*)ogreFrame1);
			keyFrame2 = poses.getObject((Ogre::VertexPoseKeyFrame*)ogreFrame2);
			break;
		default:
			keyFrame1 = nullptr;
			keyFrame2 = nullptr;
	}
	return ret;
}

KeyFrame^ VertexAnimationTrack::createKeyFrame(float timePos)
{
	switch (ogreAnimation->getAnimationType())
	{
		case Ogre::VertexAnimationType::VAT_MORPH:
			return createVertexMorphKeyFrame(timePos);
			break;
		case Ogre::VertexAnimationType::VAT_POSE:
			return createVertexPoseKeyFrame(timePos);
			break;
		default:
			return nullptr;
			break;
	}
}

void VertexAnimationTrack::removeKeyFrame(unsigned short index)
{
	switch (ogreAnimation->getAnimationType())
	{
		case Ogre::VertexAnimationType::VAT_MORPH:
			morphs.destroyObject(ogreAnimation->getVertexMorphKeyFrame(index));
			break;
		case Ogre::VertexAnimationType::VAT_POSE:
			poses.destroyObject(ogreAnimation->getVertexPoseKeyFrame(index));
			break;
	}
	ogreAnimation->removeKeyFrame(index);
}

void VertexAnimationTrack::removeAllKeyFrames()
{
	morphs.clearObjects();
	poses.clearObjects();
	ogreAnimation->removeAllKeyFrames();
}

}