#include "Stdafx.h"

extern "C" _AnomalousExport void* HardwareBuffer_lockBuf(Ogre::v1::HardwareBuffer* hardwareBuffer, size_t offset, size_t length, Ogre::v1::HardwareBuffer::LockOptions options)
{
	return hardwareBuffer->lock(offset, length, options);
}

extern "C" _AnomalousExport void* HardwareBuffer_lockBufAll(Ogre::v1::HardwareBuffer* hardwareBuffer, Ogre::v1::HardwareBuffer::LockOptions options)
{
	return hardwareBuffer->lock(options);
}

extern "C" _AnomalousExport void HardwareBuffer_unlock(Ogre::v1::HardwareBuffer* hardwareBuffer)
{
	hardwareBuffer->unlock();
}

extern "C" _AnomalousExport void HardwareBuffer_readData(Ogre::v1::HardwareBuffer* hardwareBuffer, size_t offset, size_t length, void* dest)
{
	hardwareBuffer->readData(offset, length, dest);
}

extern "C" _AnomalousExport void HardwareBuffer_writeData(Ogre::v1::HardwareBuffer* hardwareBuffer, size_t offset, size_t length, void* source)
{
	hardwareBuffer->writeData(offset, length, source);
}

extern "C" _AnomalousExport void HardwareBuffer_writeDataDiscard(Ogre::v1::HardwareBuffer* hardwareBuffer, size_t offset, size_t length, void* source, bool discardWholeBuffer)
{
	hardwareBuffer->writeData(offset, length, source, discardWholeBuffer);
}

extern "C" _AnomalousExport size_t HardwareBuffer_getSizeInBytes(Ogre::v1::HardwareBuffer* hardwareBuffer)
{
	return hardwareBuffer->getSizeInBytes();
}

extern "C" _AnomalousExport Ogre::v1::HardwareBuffer::Usage HardwareBuffer_getUsage(Ogre::v1::HardwareBuffer* hardwareBuffer)
{
	return hardwareBuffer->getUsage();
}

extern "C" _AnomalousExport bool HardwareBuffer_isSystemMemory(Ogre::v1::HardwareBuffer* hardwareBuffer)
{
	return hardwareBuffer->isSystemMemory();
}

extern "C" _AnomalousExport bool HardwareBuffer_hasShadowBuffer(Ogre::v1::HardwareBuffer* hardwareBuffer)
{
	return hardwareBuffer->hasShadowBuffer();
}

extern "C" _AnomalousExport bool HardwareBuffer_isLocked(Ogre::v1::HardwareBuffer* hardwareBuffer)
{
	return hardwareBuffer->isLocked();
}

extern "C" _AnomalousExport void HardwareBuffer_suppressHardwareUpdate(Ogre::v1::HardwareBuffer* hardwareBuffer, bool suppress)
{
	hardwareBuffer->suppressHardwareUpdate(suppress);
}