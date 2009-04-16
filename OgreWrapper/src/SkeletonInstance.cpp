#include "StdAfx.h"
#include "..\include\SkeletonInstance.h"
#include "Ogre.h"
#include "Bone.h"
#include "MarshalUtils.h"

namespace Rendering{

SkeletonInstance::SkeletonInstance(Ogre::SkeletonInstance* skeletonInstance)
:Skeleton(skeletonInstance), skeletonInstance(skeletonInstance)
{

}

SkeletonInstance::~SkeletonInstance(void)
{

}

}