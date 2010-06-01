#include "Stdafx.h"

extern "C" __declspec(dllexport) void* HardwareBuffer_lockBuf(Ogre::HardwareBuffer* hardwareBuffer, size_t offset, size_t length, Ogre::HardwareBuffer::LockOptions options)
{
	return hardwareBuffer->lock(offset, length, options);
}

extern "C" __declspec(dllexport) void* HardwareBuffer_lockBufAll(Ogre::HardwareBuffer* hardwareBuffer, Ogre::HardwareBuffer::LockOptions options)
{
	return hardwareBuffer->lock(options);
}

extern "C" __declspec(dllexport) void HardwareBuffer_unlock(Ogre::HardwareBuffer* hardwareBuffer)
{
	hardwareBuffer->unlock();
}

extern "C" __declspec(dllexport) void HardwareBuffer_readData(Ogre::HardwareBuffer* hardwareBuffer, size_t offset, size_t length, void* dest)
{
	hardwareBuffer->readData(offset, length, dest);
}

extern "C" __declspec(dllexport) void HardwareBuffer_writeData(Ogre::HardwareBuffer* hardwareBuffer, size_t offset, size_t length, void* source)
{
	hardwareBuffer->writeData(offset, length, source);
}

extern "C" __declspec(dllexport) void HardwareBuffer_writeDataDiscard(Ogre::HardwareBuffer* hardwareBuffer, size_t offset, size_t length, void* source, bool discardWholeBuffer)
{
	hardwareBuffer->writeData(offset, length, source, discardWholeBuffer);
}

extern "C" __declspec(dllexport) size_t HardwareBuffer_getSizeInBytes(Ogre::HardwareBuffer* hardwareBuffer)
{
	return hardwareBuffer->getSizeInBytes();
}

extern "C" __declspec(dllexport) Ogre::HardwareBuffer::Usage HardwareBuffer_getUsage(Ogre::HardwareBuffer* hardwareBuffer)
{
	return hardwareBuffer->getUsage();
}

extern "C" __declspec(dllexport) bool HardwareBuffer_isSystemMemory(Ogre::HardwareBuffer* hardwareBuffer)
{
	return hardwareBuffer->isSystemMemory();
}

extern "C" __declspec(dllexport) bool HardwareBuffer_hasShadowBuffer(Ogre::HardwareBuffer* hardwareBuffer)
{
	return hardwareBuffer->hasShadowBuffer();
}

extern "C" __declspec(dllexport) bool HardwareBuffer_isLocked(Ogre::HardwareBuffer* hardwareBuffer)
{
	return hardwareBuffer->isLocked();
}

extern "C" __declspec(dllexport) void HardwareBuffer_suppressHardwareUpdate(Ogre::HardwareBuffer* hardwareBuffer, bool suppress)
{
	hardwareBuffer->suppressHardwareUpdate(suppress);
}