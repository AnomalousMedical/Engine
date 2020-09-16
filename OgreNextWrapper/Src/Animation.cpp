#include "Stdafx.h"

extern "C" _AnomalousExport String Animation_getName(Ogre::v1::Animation* animation)
{
	return animation->getName().c_str();
}

extern "C" _AnomalousExport float Animation_getLength(Ogre::v1::Animation* animation)
{
	return animation->getLength();
}

extern "C" _AnomalousExport Ogre::v1::NodeAnimationTrack* Animation_createNodeTrack(Ogre::v1::Animation* animation, ushort handle)
{
	return animation->createNodeTrack(handle);
}

extern "C" _AnomalousExport Ogre::v1::NumericAnimationTrack* Animation_createNumericTrack(Ogre::v1::Animation* animation, ushort handle)
{
	return animation->createNumericTrack(handle);
}

extern "C" _AnomalousExport Ogre::v1::VertexAnimationTrack* Animation_createVertexTrack(Ogre::v1::Animation* animation, ushort handle, Ogre::v1::VertexAnimationType animType)
{
	return animation->createVertexTrack(handle, animType);
}

extern "C" _AnomalousExport ushort Animation_getNumNodeTracks(Ogre::v1::Animation* animation)
{
	return animation->getNumNodeTracks();
}

extern "C" _AnomalousExport Ogre::v1::NodeAnimationTrack* Animation_getNodeTrack(Ogre::v1::Animation* animation, ushort handle)
{
	return animation->getNodeTrack(handle);
}

extern "C" _AnomalousExport bool Animation_hasNodeTrack(Ogre::v1::Animation* animation, ushort handle)
{
	return animation->hasNodeTrack(handle);
}

extern "C" _AnomalousExport ushort Animation_getNumNumericTracks(Ogre::v1::Animation* animation)
{
	return animation->getNumNumericTracks();
}

extern "C" _AnomalousExport Ogre::v1::NumericAnimationTrack* Animation_getNumericTrack(Ogre::v1::Animation* animation, ushort handle)
{
	return animation->getNumericTrack(handle);
}

extern "C" _AnomalousExport bool Animation_hasNumericTrack(Ogre::v1::Animation* animation, ushort handle)
{
	return animation->hasNumericTrack(handle);
}

extern "C" _AnomalousExport ushort Animation_getNumVertexTracks(Ogre::v1::Animation* animation)
{
	return animation->getNumVertexTracks();
}

extern "C" _AnomalousExport Ogre::v1::VertexAnimationTrack* Animation_getVertexTrack(Ogre::v1::Animation* animation, ushort handle)
{
	return animation->getVertexTrack(handle);
}

extern "C" _AnomalousExport bool Animation_hasVertexTrack(Ogre::v1::Animation* animation, ushort handle)
{
	return animation->hasVertexTrack(handle);
}

extern "C" _AnomalousExport void Animation_destroyNodeTrack(Ogre::v1::Animation* animation, ushort handle)
{
	animation->destroyNodeTrack(handle);
}

extern "C" _AnomalousExport void Animation_destroyNumericTrack(Ogre::v1::Animation* animation, ushort handle)
{
	animation->destroyNumericTrack(handle);
}

extern "C" _AnomalousExport void Animation_destroyVertexTrack(Ogre::v1::Animation* animation, ushort handle)
{
	animation->destroyVertexTrack(handle);
}

extern "C" _AnomalousExport void Animation_destroyAllTracks(Ogre::v1::Animation* animation)
{
	animation->destroyAllTracks();
}

extern "C" _AnomalousExport void Animation_destroyAllNodeTracks(Ogre::v1::Animation* animation)
{
	animation->destroyAllNodeTracks();
}

extern "C" _AnomalousExport void Animation_destroyAllNumericTracks(Ogre::v1::Animation* animation)
{
	animation->destroyAllNumericTracks();
}

extern "C" _AnomalousExport void Animation_destroyAllVertexTracks(Ogre::v1::Animation* animation)
{
	animation->destroyAllVertexTracks();
}

extern "C" _AnomalousExport void Animation_apply1(Ogre::v1::Animation* animation, float timePos)
{
	animation->apply(timePos);
}

extern "C" _AnomalousExport void Animation_apply2(Ogre::v1::Animation* animation, float timePos, float wieght)
{
	animation->apply(timePos, wieght);
}

extern "C" _AnomalousExport void Animation_apply3(Ogre::v1::Animation* animation, float timePos, float wieght, float scale)
{
	animation->apply(timePos, wieght, scale);
}

extern "C" _AnomalousExport void Animation_apply4(Ogre::v1::Animation* animation, Ogre::v1::Skeleton* skeleton, float timePos)
{
	animation->apply(skeleton, timePos);
}

extern "C" _AnomalousExport void Animation_apply5(Ogre::v1::Animation* animation, Ogre::v1::Skeleton* skeleton, float timePos, float weight)
{
	animation->apply(skeleton, timePos, weight);
}

extern "C" _AnomalousExport void Animation_apply6(Ogre::v1::Animation* animation, Ogre::v1::Skeleton* skeleton, float timePos, float weight, float scale)
{
	animation->apply(skeleton, timePos, weight, scale);
}

extern "C" _AnomalousExport void Animation_apply7(Ogre::v1::Animation* animation, Ogre::v1::Skeleton* skeleton, float timePos, float weight, float* blendMask, int blendMaskSize, float scale)
{
	Ogre::v1::AnimationState::BoneBlendMask boneMask;
	for(int i = 0; i < blendMaskSize; ++i)
	{
		boneMask.push_back(blendMask[i]);
	}
	animation->apply(skeleton, timePos, weight, &boneMask, scale);
}

extern "C" _AnomalousExport void Animation_apply8(Ogre::v1::Animation* animation, Ogre::v1::Entity* entity, float timePos, float weight, bool software, bool hardware)
{
	animation->apply(entity, timePos, weight, software, hardware);
}

extern "C" _AnomalousExport void Animation_setInterpolationMode(Ogre::v1::Animation* animation, Ogre::v1::Animation::InterpolationMode im)
{
	animation->setInterpolationMode(im);
}

extern "C" _AnomalousExport Ogre::v1::Animation::InterpolationMode Animation_getInterpolationMode(Ogre::v1::Animation* animation)
{
	return animation->getInterpolationMode();
}

extern "C" _AnomalousExport void Animation_setRotationInterpolationMode(Ogre::v1::Animation* animation, Ogre::v1::Animation::RotationInterpolationMode im)
{
	animation->setRotationInterpolationMode(im);
}

extern "C" _AnomalousExport Ogre::v1::Animation::RotationInterpolationMode Animation_getRotationInterpolationMode(Ogre::v1::Animation* animation)
{
	return animation->getRotationInterpolationMode();
}

extern "C" _AnomalousExport void Animation_optimize1(Ogre::v1::Animation* animation)
{
	animation->optimise();
}

extern "C" _AnomalousExport void Animation_optimize2(Ogre::v1::Animation* animation, bool discardIdentityNodeTracks)
{
	animation->optimise(discardIdentityNodeTracks);
}