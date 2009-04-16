#include "StdAfx.h"
#include "..\include\HardwareBuffer.h"
#include "OgreHardwareBuffer.h"

namespace OgreWrapper{

HardwareBuffer::HardwareBuffer(Ogre::HardwareBuffer* hardwareBuffer)
:hardwareBuffer(hardwareBuffer)
{
}

HardwareBuffer::~HardwareBuffer(void)
{
}

void* HardwareBuffer::lock(size_t offset, size_t length, LockOptions options)
{
	return hardwareBuffer->lock(offset, length, (Ogre::HardwareBuffer::LockOptions)options);
}

void* HardwareBuffer::lock(LockOptions options)
{
	return hardwareBuffer->lock((Ogre::HardwareBuffer::LockOptions)options);
}

void HardwareBuffer::unlock()
{
	return hardwareBuffer->unlock();
}

void HardwareBuffer::readData(size_t offset, size_t length, void* dest)
{
	return hardwareBuffer->readData(offset, length, dest);
}

void HardwareBuffer::writeData(size_t offset, size_t length, void* source)
{
	return hardwareBuffer->writeData(offset, length, source);
}

void HardwareBuffer::writeData(size_t offset, size_t length, void* source, bool discardWholeBuffer)
{
	return hardwareBuffer->writeData(offset, length, source, discardWholeBuffer);
}

size_t HardwareBuffer::getSizeInBytes()
{
	return hardwareBuffer->getSizeInBytes();
}

HardwareBuffer::Usage HardwareBuffer::getUsage()
{
	return (HardwareBuffer::Usage)hardwareBuffer->getUsage();
}

bool HardwareBuffer::isSystemMemory()
{
	return hardwareBuffer->isSystemMemory();
}

bool HardwareBuffer::hasShadowBuffer()
{
	return hardwareBuffer->hasShadowBuffer();
}

bool HardwareBuffer::isLocked()
{
	return hardwareBuffer->isLocked();
}

void HardwareBuffer::suppressHardwareUpdate(bool suppress)
{
	return hardwareBuffer->suppressHardwareUpdate(suppress);
}

}