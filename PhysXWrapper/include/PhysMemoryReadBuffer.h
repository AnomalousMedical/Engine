#pragma once

#include "AutoPtr.h"
#include "Stream.h"

namespace PhysXWrapper
{

ref class PhysMemoryWriteBuffer;

/// <summary>
/// A wrapper for the read buffer.  Doesn't contain any real functionality
/// of its own, but it allows the user to pass information to the cooking API.
/// </summary>
public ref class PhysMemoryReadBuffer
{
internal:
	AutoPtr<MemoryReadBuffer> readBuffer;

public:
	PhysMemoryReadBuffer(PhysMemoryWriteBuffer^ buffer);

	~PhysMemoryReadBuffer();
};

}