#include "Stdafx.h"

//HardwareIndexBufferPtr
extern "C" _AnomalousExport Ogre::v1::HardwareIndexBufferSharedPtr* HardwareIndexBufferPtr_createHeapPtr(Ogre::v1::HardwareIndexBufferSharedPtr* stackSharedPtr)
{
	return new Ogre::v1::HardwareIndexBufferSharedPtr(*stackSharedPtr);
}

extern "C" _AnomalousExport void HardwareIndexBufferPtr_Delete(Ogre::v1::HardwareIndexBufferSharedPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}

//HardwareVertexBufferPtr
extern "C" _AnomalousExport Ogre::v1::HardwareVertexBufferSharedPtr* HardwareVertexBufferPtr_createHeapPtr(Ogre::v1::HardwareVertexBufferSharedPtr* stackSharedPtr)
{
	return new Ogre::v1::HardwareVertexBufferSharedPtr(*stackSharedPtr);
}

extern "C" _AnomalousExport void HardwareVertexBufferPtr_Delete(Ogre::v1::HardwareVertexBufferSharedPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}

//HardwarePixelBufferPtr
extern "C" _AnomalousExport Ogre::v1::HardwarePixelBufferSharedPtr* HardwarePixelBufferPtr_createHeapPtr(Ogre::v1::HardwarePixelBufferSharedPtr* stackSharedPtr)
{
	return new Ogre::v1::HardwarePixelBufferSharedPtr(*stackSharedPtr);
}

extern "C" _AnomalousExport void HardwarePixelBufferPtr_Delete(Ogre::v1::HardwarePixelBufferSharedPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}