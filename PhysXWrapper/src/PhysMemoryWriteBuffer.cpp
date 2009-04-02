#include "StdAfx.h"
#include "..\include\PhysMemoryWriteBuffer.h"

namespace Physics
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