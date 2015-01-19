#include "Stdafx.h"

//MeshPtr
extern "C" _AnomalousExport Ogre::SkeletonPtr* SkeletonPtr_createHeapPtr(Ogre::SkeletonPtr* stackSharedPtr)
{
	return new Ogre::SkeletonPtr(*stackSharedPtr);
}

extern "C" _AnomalousExport void SkeletonPtr_Delete(Ogre::SkeletonPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}