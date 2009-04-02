#include "StdAfx.h"
#include "..\include\PhysMemoryWriteBuffer.h"

namespace PhysXWrapper
{

PhysMemoryWriteBuffer::PhysMemoryWriteBuffer(void)
:writeBuffer(new MemoryWriteBuffer())
{

}

PhysMemoryWriteBuffer::~PhysMemoryWriteBuffer()
{
	writeBuffer.Reset();
}

}