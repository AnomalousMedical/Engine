#include "Stdafx.h"

typedef void (*BoneFoundCallback)(Ogre::v1::OldBone* bone);

extern "C" _AnomalousExport Ogre::v1::OldBone* Skeleton_createBone(Ogre::v1::Skeleton* skeleton)
{
	return skeleton->createBone();
}

extern "C" _AnomalousExport Ogre::v1::OldBone* Skeleton_createBoneHandle(Ogre::v1::Skeleton* skeleton, ushort handle)
{
	return skeleton->createBone(handle);
}

extern "C" _AnomalousExport Ogre::v1::OldBone* Skeleton_createBoneName(Ogre::v1::Skeleton* skeleton, String name)
{
	return skeleton->createBone(name);
}

extern "C" _AnomalousExport Ogre::v1::OldBone* Skeleton_createBoneNameHandle(Ogre::v1::Skeleton* skeleton, String name, ushort handle)
{
	return skeleton->createBone(name, handle);
}

extern "C" _AnomalousExport ushort Skeleton_getNumBones(Ogre::v1::Skeleton* skeleton)
{
	return skeleton->getNumBones();
}

extern "C" _AnomalousExport Ogre::v1::OldBone* Skeleton_getBoneHandle(Ogre::v1::Skeleton* skeleton, ushort handle)
{
	return skeleton->getBone(handle);
}

extern "C" _AnomalousExport Ogre::v1::OldBone* Skeleton_getBoneName(Ogre::v1::Skeleton* skeleton, String name)
{
	return skeleton->getBone(name);
}

extern "C" _AnomalousExport bool Skeleton_hasBone(Ogre::v1::Skeleton* skeleton, String name)
{
	return skeleton->hasBone(name);
}

extern "C" _AnomalousExport void Skeleton_setBindingPose(Ogre::v1::Skeleton* skeleton)
{
	skeleton->setBindingPose();
}

extern "C" _AnomalousExport void Skeleton_reset(Ogre::v1::Skeleton* skeleton)
{
	skeleton->reset();
}

extern "C" _AnomalousExport void Skeleton_resetResetManual(Ogre::v1::Skeleton* skeleton, bool resetManualBones)
{
	skeleton->reset(resetManualBones);
}

extern "C" _AnomalousExport Ogre::v1::Animation* Skeleton_createAnimation(Ogre::v1::Skeleton* skeleton, String name, float length)
{
	return skeleton->createAnimation(name, length);
}

extern "C" _AnomalousExport Ogre::v1::Animation* Skeleton_getAnimation(Ogre::v1::Skeleton* skeleton, String name)
{
	return skeleton->getAnimation(name);
}

extern "C" _AnomalousExport bool Skeleton_hasAnimation(Ogre::v1::Skeleton* skeleton, String name)
{
	return skeleton->hasAnimation(name);
}

extern "C" _AnomalousExport void Skeleton_removeAnimation(Ogre::v1::Skeleton* skeleton, String name)
{
	skeleton->removeAnimation(name);
}

extern "C" _AnomalousExport void Skeleton_setAnimationState(Ogre::v1::Skeleton* skeleton, Ogre::v1::AnimationStateSet* animSet)
{
	skeleton->setAnimationState(*animSet);
}

extern "C" _AnomalousExport ushort Skeleton_getNumAnimations(Ogre::v1::Skeleton* skeleton)
{
	return skeleton->getNumAnimations();
}

extern "C" _AnomalousExport Ogre::v1::SkeletonAnimationBlendMode Skeleton_getBlendMode(Ogre::v1::Skeleton* skeleton)
{
	return skeleton->getBlendMode();
}

extern "C" _AnomalousExport void Skeleton_setBlendMode(Ogre::v1::Skeleton* skeleton, Ogre::v1::SkeletonAnimationBlendMode blendMode)
{
	skeleton->setBlendMode(blendMode);
}

extern "C" _AnomalousExport void Skeleton_optimizeAllAnimations(Ogre::v1::Skeleton* skeleton, bool preservingIdentityNodeTracks)
{
	skeleton->optimiseAllAnimations(preservingIdentityNodeTracks);
}

extern "C" _AnomalousExport bool Skeleton_getManualBonesDirty(Ogre::v1::Skeleton* skeleton)
{
	return skeleton->getManualBonesDirty();
}

extern "C" _AnomalousExport bool Skeleton_hasManualBones(Ogre::v1::Skeleton* skeleton)
{
	return skeleton->hasManualBones();
}