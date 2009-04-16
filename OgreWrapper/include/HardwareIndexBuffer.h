#pragma once

#include "HardwareBuffer.h"
#include "AutoPtr.h"

namespace Ogre
{
	class HardwareIndexBuffer;
}

namespace Rendering{

/// <summary>
/// Specialisation of HardwareBuffer for vertex index buffers.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class HardwareIndexBuffer : public HardwareBuffer
{
public:
	/// <summary>
	/// The type of index.
	/// </summary>
	enum class IndexType : unsigned int
	{
		/// <summary>
		/// 16 bit indices.
		/// </summary>
		IT_16BIT,
		/// <summary>
		/// 32 bit indices.
		/// </summary>
		IT_32BIT
    };

private:
	Ogre::HardwareIndexBuffer* hardwareIndexBuffer;
	AutoPtr<Ogre::HardwareIndexBufferSharedPtr> autoSharedPtr;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="hardwareIndexBuffer">The hardwareIndexBuffer to wrap.</param>
	HardwareIndexBuffer(const Ogre::HardwareIndexBufferSharedPtr& hardwareIndexBuffer);

	Ogre::HardwareIndexBuffer* getOgrePtr()
	{
		return hardwareIndexBuffer;
	}

public:
	/// <summary>
	/// Destructor.
	/// </summary>
	virtual ~HardwareIndexBuffer(void);

	/// <summary>
	/// Get the type of indexes used in this buffer. 
	/// </summary>
	/// <returns>The IndexType of the buffer.</returns>
	IndexType getType();

	/// <summary>
	/// Get the number of indexes in this buffer.
	/// </summary>
	/// <returns>The number of indexes in the buffer.</returns>
	size_t getNumIndexes();

	/// <summary>
	/// Get the size in bytes of each index.
	/// </summary>
	/// <returns>The size of a single index in bytes.</returns>
	size_t getIndexSize();
};

}