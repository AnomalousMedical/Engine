#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::HardwareIndexBuffer::IndexType HardwareIndexBuffer_getType(Ogre::HardwareIndexBuffer* hardwareBuffer)
{
	return hardwareBuffer->getType();
}

extern "C" _AnomalousExport size_t HardwareIndexBuffer_getNumIndexes(Ogre::HardwareIndexBuffer* hardwareBuffer)
{
	return hardwareBuffer->getNumIndexes();
}

extern "C" _AnomalousExport size_t HardwareIndexBuffer_getIndexSize(Ogre::HardwareIndexBuffer* hardwareBuffer)
{
	return hardwareBuffer->getIndexSize();
}