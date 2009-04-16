#include "StdAfx.h"
#include "..\include\Bone.h"
#include "Ogre.h"
#include "MathUtils.h"
#include "MarshalUtils.h"

namespace OgreWrapper{

Bone::Bone(Ogre::Bone* bone)
:Node(bone),
bone(bone)
{

}

Bone::~Bone(void)
{
	
}

Bone^ Bone::createChild(unsigned short handle, EngineMath::Vector3 translation, EngineMath::Quaternion rotate)
{
	Ogre::Bone* ogreBone = bone->createChild(handle, MathUtils::copyVector3(translation), MathUtils::copyQuaternion(rotate));
	return bones.getObject(ogreBone);
}

unsigned short Bone::getHandle()
{
	return bone->getHandle();
}

void Bone::setBindingPose()
{
	bone->setBindingPose();
}

void Bone::reset()
{
	bone->reset();
}

void Bone::setManuallyControlled(bool manuallyControlled)
{
	bone->setManuallyControlled(manuallyControlled);
}

bool Bone::isManuallyControlled()
{
	return bone->isManuallyControlled();
}

void Bone::needUpdate(bool forceParentUpdate)
{
	bone->needUpdate(forceParentUpdate);
}

System::String^ Bone::getName()
{
	return MarshalUtils::convertString(bone->getName());
}

}