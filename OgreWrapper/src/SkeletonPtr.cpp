//source
#include "stdafx.h"
#include "SkeletonPtr.h"
#include "Skeleton.h"

namespace Engine{

namespace Rendering{

Skeleton^ SkeletonPtrCollection::createWrapper(const void* sharedPtr, array<System::Object^>^ args)
{
	return gcnew Skeleton(*(const Ogre::SkeletonPtr*)sharedPtr);
}

SkeletonPtr^ SkeletonPtrCollection::getObject(const Ogre::SkeletonPtr& meshPtr)
{
	return gcnew SkeletonPtr(getObjectVoid(meshPtr.get(), &meshPtr));
}

SkeletonPtr::SkeletonPtr(SharedPtr<Skeleton^>^ sharedPtr)
:sharedPtr(sharedPtr), mesh(sharedPtr->Value)
{

}

SkeletonPtr::~SkeletonPtr()
{
	delete sharedPtr;
}

}

}