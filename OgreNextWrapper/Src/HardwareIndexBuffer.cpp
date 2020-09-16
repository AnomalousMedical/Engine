#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::v1::HardwareIndexBuffer::IndexType HardwareIndexBuffer_getType(Ogre::v1::HardwareIndexBuffer* hardwareBuffer)
{
	return hardwareBuffer->getType();
}

extern "C" _AnomalousExport size_t HardwareIndexBuffer_getNumIndexes(Ogre::v1::HardwareIndexBuffer* hardwareBuffer)
{
	return hardwareBuffer->getNumIndexes();
}

extern "C" _AnomalousExport size_t HardwareIndexBuffer_getIndexSize(Ogre::v1::HardwareIndexBuffer* hardwareBuffer)
{
	return hardwareBuffer->getIndexSize();
}