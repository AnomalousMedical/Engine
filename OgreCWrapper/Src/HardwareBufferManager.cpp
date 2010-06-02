#include "Stdafx.h"

//HardwareIndexBufferPtr
extern "C" __declspec(dllexport) Ogre::HardwareIndexBufferSharedPtr* HardwareIndexBufferPtr_createHeapPtr(Ogre::HardwareIndexBufferSharedPtr* stackSharedPtr)
{
	return new Ogre::HardwareIndexBufferSharedPtr(*stackSharedPtr);
}

extern "C" __declspec(dllexport) void HardwareIndexBufferPtr_Delete(Ogre::HardwareIndexBufferSharedPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}

//HardwareVertexBufferPtr
extern "C" __declspec(dllexport) Ogre::HardwareVertexBufferSharedPtr* HardwareVertexBufferPtr_createHeapPtr(Ogre::HardwareVertexBufferSharedPtr* stackSharedPtr)
{
	return new Ogre::HardwareVertexBufferSharedPtr(*stackSharedPtr);
}

extern "C" __declspec(dllexport) void HardwareVertexBufferPtr_Delete(Ogre::HardwareVertexBufferSharedPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}

//HardwarePixelBufferPtr
extern "C" __declspec(dllexport) Ogre::HardwarePixelBufferSharedPtr* HardwarePixelBufferPtr_createHeapPtr(Ogre::HardwarePixelBufferSharedPtr* stackSharedPtr)
{
	return new Ogre::HardwarePixelBufferSharedPtr(*stackSharedPtr);
}

extern "C" __declspec(dllexport) void HardwarePixelBufferPtr_Delete(Ogre::HardwarePixelBufferSharedPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}