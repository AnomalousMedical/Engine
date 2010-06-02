#include "Stdafx.h"

typedef void (*BoneFoundCallback)(Ogre::Bone* bone);

extern "C" __declspec(dllexport) Ogre::Bone* Skeleton_createBone(Ogre::Skeleton* skeleton)
{
	return skeleton->createBone();
}

extern "C" __declspec(dllexport) Ogre::Bone* Skeleton_createBoneHandle(Ogre::Skeleton* skeleton, ushort handle)
{
	return skeleton->createBone(handle);
}

extern "C" __declspec(dllexport) Ogre::Bone* Skeleton_createBoneName(Ogre::Skeleton* skeleton, String name)
{
	return skeleton->createBone(name);
}

extern "C" __declspec(dllexport) Ogre::Bone* Skeleton_createBoneNameHandle(Ogre::Skeleton* skeleton, String name, ushort handle)
{
	return skeleton->createBone(name, handle);
}

extern "C" __declspec(dllexport) ushort Skeleton_getNumBones(Ogre::Skeleton* skeleton)
{
	return skeleton->getNumBones();
}

extern "C" __declspec(dllexport) void Skeleton_getRootBoneIterator(Ogre::Skeleton* skeleton, BoneFoundCallback boneFound)
{
	Ogre::Skeleton::BoneIterator boneIter = skeleton->getRootBoneIterator();
	while(boneIter.hasMoreElements())
	{
		boneFound(boneIter.getNext());
	}
}

extern "C" __declspec(dllexport) void Skeleton_getBoneIterator(Ogre::Skeleton* skeleton, BoneFoundCallback boneFound)
{
	Ogre::Skeleton::BoneIterator boneIter = skeleton->getBoneIterator();
	while(boneIter.hasMoreElements())
	{
		boneFound(boneIter.getNext());
	}
}

extern "C" __declspec(dllexport) Ogre::Bone* Skeleton_getBoneHandle(Ogre::Skeleton* skeleton, ushort handle)
{
	return skeleton->getBone(handle);
}

extern "C" __declspec(dllexport) Ogre::Bone* Skeleton_getBoneName(Ogre::Skeleton* skeleton, String name)
{
	return skeleton->getBone(name);
}

extern "C" __declspec(dllexport) bool Skeleton_hasBone(Ogre::Skeleton* skeleton, String name)
{
	return skeleton->hasBone(name);
}

extern "C" __declspec(dllexport) void Skeleton_setBindingPose(Ogre::Skeleton* skeleton)
{
	skeleton->setBindingPose();
}

extern "C" __declspec(dllexport) void Skeleton_reset(Ogre::Skeleton* skeleton)
{
	skeleton->reset();
}

extern "C" __declspec(dllexport) void Skeleton_resetResetManual(Ogre::Skeleton* skeleton, bool resetManualBones)
{
	skeleton->reset(resetManualBones);
}

extern "C" __declspec(dllexport) Ogre::Animation* Skeleton_createAnimation(Ogre::Skeleton* skeleton, String name, float length)
{
	return skeleton->createAnimation(name, length);
}

extern "C" __declspec(dllexport) Ogre::Animation* Skeleton_getAnimation(Ogre::Skeleton* skeleton, String name)
{
	return skeleton->getAnimation(name);
}

extern "C" __declspec(dllexport) bool Skeleton_hasAnimation(Ogre::Skeleton* skeleton, String name)
{
	return skeleton->hasAnimation(name);
}

extern "C" __declspec(dllexport) void Skeleton_removeAnimation(Ogre::Skeleton* skeleton, String name)
{
	skeleton->removeAnimation(name);
}

extern "C" __declspec(dllexport) void Skeleton_setAnimationState(Ogre::Skeleton* skeleton, Ogre::AnimationStateSet* animSet)
{
	skeleton->setAnimationState(*animSet);
}

extern "C" __declspec(dllexport) ushort Skeleton_getNumAnimations(Ogre::Skeleton* skeleton)
{
	return skeleton->getNumAnimations();
}

extern "C" __declspec(dllexport) Ogre::SkeletonAnimationBlendMode Skeleton_getBlendMode(Ogre::Skeleton* skeleton)
{
	return skeleton->getBlendMode();
}

extern "C" __declspec(dllexport) void Skeleton_setBlendMode(Ogre::Skeleton* skeleton, Ogre::SkeletonAnimationBlendMode blendMode)
{
	skeleton->setBlendMode(blendMode);
}

extern "C" __declspec(dllexport) void Skeleton_optimizeAllAnimations(Ogre::Skeleton* skeleton, bool preservingIdentityNodeTracks)
{
	skeleton->optimiseAllAnimations(preservingIdentityNodeTracks);
}

extern "C" __declspec(dllexport) bool Skeleton_getManualBonesDirty(Ogre::Skeleton* skeleton)
{
	return skeleton->getManualBonesDirty();
}

extern "C" __declspec(dllexport) bool Skeleton_hasManualBones(Ogre::Skeleton* skeleton)
{
	return skeleton->hasManualBones();
}