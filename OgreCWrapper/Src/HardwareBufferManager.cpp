#include "Stdafx.h"

//HardwareIndexBufferPtr
extern "C" _AnomalousExport Ogre::HardwareIndexBufferSharedPtr* HardwareIndexBufferPtr_createHeapPtr(Ogre::HardwareIndexBufferSharedPtr* stackSharedPtr)
{
	return new Ogre::HardwareIndexBufferSharedPtr(*stackSharedPtr);
}

extern "C" _AnomalousExport void HardwareIndexBufferPtr_Delete(Ogre::HardwareIndexBufferSharedPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}

//HardwareVertexBufferPtr
extern "C" _AnomalousExport Ogre::HardwareVertexBufferSharedPtr* HardwareVertexBufferPtr_createHeapPtr(Ogre::HardwareVertexBufferSharedPtr* stackSharedPtr)
{
	return new Ogre::HardwareVertexBufferSharedPtr(*stackSharedPtr);
}

extern "C" _AnomalousExport void HardwareVertexBufferPtr_Delete(Ogre::HardwareVertexBufferSharedPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}

//HardwarePixelBufferPtr
extern "C" _AnomalousExport Ogre::HardwarePixelBufferSharedPtr* HardwarePixelBufferPtr_createHeapPtr(Ogre::HardwarePixelBufferSharedPtr* stackSharedPtr)
{
	return new Ogre::HardwarePixelBufferSharedPtr(*stackSharedPtr);
}

extern "C" _AnomalousExport void HardwarePixelBufferPtr_Delete(Ogre::HardwarePixelBufferSharedPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}