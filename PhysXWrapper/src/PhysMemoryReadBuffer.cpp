#include "StdAfx.h"
#include "..\include\PhysMemoryReadBuffer.h"
#include "PhysMemoryWriteBuffer.h"

namespace Physics
{

PhysMemoryReadBuffer::PhysMemoryReadBuffer(PhysMemoryWriteBuffer^ buffer)
:readBuffer(new MemoryReadBuffer(buffer->writeBuffer->data))
{

}

PhysMemoryReadBuffer::~PhysMemoryReadBuffer()
{
	readBuffer.Reset();
}

}