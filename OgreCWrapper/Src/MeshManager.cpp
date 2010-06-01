#include "Stdafx.h"

//MeshPtr
extern "C" __declspec(dllexport) Ogre::MeshPtr* MeshPtr_createHeapPtr(Ogre::MeshPtr* stackSharedPtr)
{
	return new Ogre::MeshPtr(*stackSharedPtr);
}

extern "C" __declspec(dllexport) void MeshPtr_Delete(Ogre::MeshPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}