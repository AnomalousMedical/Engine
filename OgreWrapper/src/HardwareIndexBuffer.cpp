#include "StdAfx.h"
#include "..\include\HardwareIndexBuffer.h"

namespace Engine{

namespace Rendering{

HardwareIndexBuffer::HardwareIndexBuffer(const Ogre::HardwareIndexBufferSharedPtr& hardwareIndexBuffer)
:HardwareBuffer(hardwareIndexBuffer.get()),
hardwareIndexBuffer(hardwareIndexBuffer.get()),
autoSharedPtr(new Ogre::HardwareIndexBufferSharedPtr(hardwareIndexBuffer))
{
}

HardwareIndexBuffer::~HardwareIndexBuffer(void)
{

}

HardwareIndexBuffer::IndexType HardwareIndexBuffer::getType()
{
	return (HardwareIndexBuffer::IndexType)hardwareIndexBuffer->getType();
}

size_t HardwareIndexBuffer::getNumIndexes()
{
	return hardwareIndexBuffer->getNumIndexes();
}

size_t HardwareIndexBuffer::getIndexSize()
{
	return hardwareIndexBuffer->getIndexSize();
}

}

}