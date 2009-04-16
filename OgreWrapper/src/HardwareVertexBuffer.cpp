#include "StdAfx.h"
#include "..\include\HardwareVertexBuffer.h"

namespace Engine{

namespace Rendering{

HardwareVertexBuffer::HardwareVertexBuffer(const Ogre::HardwareVertexBufferSharedPtr& hardwareVertexBuffer)
:HardwareBuffer(hardwareVertexBuffer.get()),
hardwareVertexBuffer(hardwareVertexBuffer.get()),
autoSharedPtr(new Ogre::HardwareVertexBufferSharedPtr(hardwareVertexBuffer))
{
}

HardwareVertexBuffer::~HardwareVertexBuffer(void)
{

}

const Ogre::HardwareVertexBufferSharedPtr& HardwareVertexBuffer::getOgreHardwareVertexBufferPtr()
{
	return *autoSharedPtr.Get();
}

size_t HardwareVertexBuffer::getVertexSize()
{
	return hardwareVertexBuffer->getVertexSize();
}

size_t HardwareVertexBuffer::getNumVertices()
{
	return hardwareVertexBuffer->getNumVertices();
}

}

}