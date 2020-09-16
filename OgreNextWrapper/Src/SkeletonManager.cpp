#include "Stdafx.h"

//MeshPtr
extern "C" _AnomalousExport Ogre::v1::SkeletonPtr* SkeletonPtr_createHeapPtr(Ogre::v1::SkeletonPtr* stackSharedPtr)
{
	return new Ogre::v1::SkeletonPtr(*stackSharedPtr);
}

extern "C" _AnomalousExport void SkeletonPtr_Delete(Ogre::v1::SkeletonPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}