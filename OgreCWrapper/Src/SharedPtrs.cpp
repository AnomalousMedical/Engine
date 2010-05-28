#include "Stdafx.h"

extern "C" __declspec(dllexport) Ogre::MaterialPtr* MaterialPtr_createHeapPtr(Ogre::MaterialPtr* stackSharedPtr)
{
	return new Ogre::MaterialPtr(*stackSharedPtr);
}

extern "C" __declspec(dllexport) void MaterialPtr_Delete(Ogre::MaterialPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}