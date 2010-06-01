#include "Stdafx.h"

extern "C" __declspec(dllexport) Ogre::HardwareIndexBuffer::IndexType HardwareIndexBuffer_getType(Ogre::HardwareIndexBuffer* hardwareBuffer)
{
	return hardwareBuffer->getType();
}

extern "C" __declspec(dllexport) size_t HardwareIndexBuffer_getNumIndexes(Ogre::HardwareIndexBuffer* hardwareBuffer)
{
	return hardwareBuffer->getNumIndexes();
}

extern "C" __declspec(dllexport) size_t HardwareIndexBuffer_getIndexSize(Ogre::HardwareIndexBuffer* hardwareBuffer)
{
	return hardwareBuffer->getIndexSize();
}