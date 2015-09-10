#include "Stdafx.h"

//MeshPtr
extern "C" _AnomalousExport Ogre::MeshPtr* MeshPtr_createHeapPtr(Ogre::MeshPtr* stackSharedPtr)
{
	return new Ogre::MeshPtr(*stackSharedPtr);
}

extern "C" _AnomalousExport void MeshPtr_Delete(Ogre::MeshPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}

extern "C" _AnomalousExport size_t MeshManager_getMemoryUsage()
{
	return Ogre::MeshManager::getSingleton().getMemoryUsage();
}