#include "Stdafx.h"

//MeshPtr
extern "C" _AnomalousExport Ogre::v1::MeshPtr* MeshPtr_createHeapPtr(Ogre::v1::MeshPtr* stackSharedPtr)
{
	return new Ogre::v1::MeshPtr(*stackSharedPtr);
}

extern "C" _AnomalousExport void MeshPtr_Delete(Ogre::v1::MeshPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}

extern "C" _AnomalousExport size_t MeshManager_getMemoryUsage()
{
	return Ogre::v1::MeshManager::getSingleton().getMemoryUsage();
}