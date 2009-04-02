#pragma once

#include "AutoPtr.h"
#include "Stream.h"

namespace Physics
{

/// <summary>
/// A wrapper for the write buffer.  Doesn't contain any real functionality
/// of its own, but it allows the user to get information from the cooking API.
/// </summary>
public ref class PhysMemoryWriteBuffer
{
internal:
	AutoPtr<MemoryWriteBuffer> writeBuffer;

public:
	PhysMemoryWriteBuffer(void);

	~PhysMemoryWriteBuffer();
};

}