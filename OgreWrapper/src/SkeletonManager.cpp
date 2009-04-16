#include "StdAfx.h"
#include "..\include\SkeletonManager.h"
#include "Ogre.h"
#include "Skeleton.h"

namespace Engine{

namespace Rendering{

SkeletonManager::SkeletonManager()
:skeletonManager(Ogre::SkeletonManager::getSingletonPtr())
{
}

SkeletonPtr^ SkeletonManager::getObject(const Ogre::SkeletonPtr& skeleton)
{
	return skeletonPtrs.getObject(skeleton);
}

SkeletonManager::~SkeletonManager()
{
	skeletonPtrs.clearObjects();
}

SkeletonManager^ SkeletonManager::getInstance()
{
	return instance;
}

}

}