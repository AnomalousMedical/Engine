#include "StdAfx.h"
#include "..\include\Skeleton.h"
#include "OgreSkeleton.h"
#include "MarshalUtils.h"
#include "Bone.h"
#include "Animation.h"
#include "AnimationStateSet.h"

namespace OgreWrapper
{

using namespace System::Collections::Generic;

Skeleton::Skeleton(const Ogre::SkeletonPtr& ogreSkeleton)
:Resource(ogreSkeleton.get()),
skeleton(ogreSkeleton.get()),
autoSharedPtr(new Ogre::SkeletonPtr(ogreSkeleton))
{
}

Skeleton::Skeleton(Ogre::Skeleton* ogreSkeleton)
:Resource(ogreSkeleton), skeleton(ogreSkeleton)
{

}

const Ogre::SkeletonPtr& Skeleton::getSkeletonPtr()
{
	return *autoSharedPtr.Get();
}

Ogre::Skeleton* Skeleton::getSkeleton()
{
	return skeleton;
}

Skeleton::~Skeleton(void)
{
	skeleton = 0;
}

Bone^ Skeleton::createBone()
{
	Ogre::Bone* ogreBone = skeleton->createBone();
	return bones.getObject(ogreBone);
}

Bone^ Skeleton::createBone(unsigned short handle)
{
	Ogre::Bone* ogreBone = skeleton->createBone(handle);
	return bones.getObject(ogreBone);
}

Bone^ Skeleton::createBone(System::String^ name)
{
	Ogre::Bone* ogreBone = skeleton->createBone(MarshalUtils::convertString(name));
	return bones.getObject(ogreBone);
}

Bone^ Skeleton::createBone(System::String^ name, unsigned short handle)
{
	Ogre::Bone* ogreBone = skeleton->createBone(MarshalUtils::convertString(name), handle);
	return bones.getObject(ogreBone);
}

unsigned short Skeleton::getNumBones()
{
	return skeleton->getNumBones();
}

BoneIterator^ Skeleton::getRootBoneIterator()
{
	Ogre::Skeleton::BoneIterator boneIter = skeleton->getRootBoneIterator();
	List<Bone^>^ bones = gcnew List<Bone^>();
	while(boneIter.hasMoreElements())
	{
		bones->Add(this->bones.getObject(boneIter.getNext()));
	}
	return bones->GetEnumerator();
}

BoneIterator^ Skeleton::getBoneIterator()
{
	Ogre::Skeleton::BoneIterator boneIter = skeleton->getBoneIterator();
	List<Bone^>^ bones = gcnew List<Bone^>();
	while(boneIter.hasMoreElements())
	{
		bones->Add(this->bones.getObject(boneIter.getNext()));
	}
	return bones->GetEnumerator();
}

Bone^ Skeleton::getBone(unsigned short handle)
{
	return bones.getObject(skeleton->getBone(handle));
}

Bone^ Skeleton::getBone(System::String^ name)
{
	return bones.getObject(skeleton->getBone(MarshalUtils::convertString(name)));
}

bool Skeleton::hasBone(System::String^ name)
{
	return skeleton->hasBone(MarshalUtils::convertString(name));
}

void Skeleton::setBindingPose()
{
	skeleton->setBindingPose();
}

void Skeleton::reset()
{
	skeleton->reset();
}

void Skeleton::reset(bool resetManualBones)
{
	skeleton->reset(resetManualBones);
}

Animation^ Skeleton::createAnimation(System::String^ name, float length)
{
	return animations.getObject(skeleton->createAnimation(MarshalUtils::convertString(name), length));
}

Animation^ Skeleton::getAnimation(System::String^ name)
{
	return animations.getObject(skeleton->getAnimation(MarshalUtils::convertString(name)));
}

void Skeleton::removeAnimation(System::String^ name)
{
	Ogre::String ogreString = MarshalUtils::convertString(name);
	animations.destroyObject(skeleton->getAnimation(ogreString));
	skeleton->removeAnimation(ogreString);
}

bool Skeleton::hasAnimation(System::String^ name)
{
	return skeleton->hasAnimation(MarshalUtils::convertString(name));
}

void Skeleton::setAnimationState(AnimationStateSet^ animSet)
{
	skeleton->setAnimationState(*animSet->getOgreAnimationStateSet());
}

unsigned short Skeleton::getNumAnimations()
{
	return skeleton->getNumAnimations();
}

SkeletonAnimationBlendMode Skeleton::getBlendMode()
{
	return (SkeletonAnimationBlendMode)skeleton->getBlendMode();
}

void Skeleton::setBlendMode(SkeletonAnimationBlendMode blendMode)
{
	skeleton->setBlendMode((Ogre::SkeletonAnimationBlendMode)blendMode);
}

void Skeleton::optimizeAllAnimations(bool preservingIdentityNodeTracks)
{
	skeleton->optimiseAllAnimations(preservingIdentityNodeTracks);
}

bool Skeleton::getManualBonesDirty()
{
	return skeleton->getManualBonesDirty();
}

bool Skeleton::hasManualBones()
{
	return skeleton->hasManualBones();
}

}