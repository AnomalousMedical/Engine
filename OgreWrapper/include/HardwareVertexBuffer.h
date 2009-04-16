#pragma once

#include "HardwareBuffer.h"
#include "AutoPtr.h"

namespace Ogre
{
	class HardwareVertexBuffer;
	class HardwareVertexBufferSharedPtr;
}

namespace OgreWrapper{

/// <summary>
/// Specialisation of HardwareBuffer for vertex buffers.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class HardwareVertexBuffer : public HardwareBuffer
{
private:
	Ogre::HardwareVertexBuffer* hardwareVertexBuffer;
	AutoPtr<Ogre::HardwareVertexBufferSharedPtr> autoSharedPtr;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="hardwareVertexBuffer">The HardwareVertexBuffer to wrap.</param>
	HardwareVertexBuffer(const Ogre::HardwareVertexBufferSharedPtr& hardwareVertexBuffer);

	/// <summary>
	/// Get the wrapped buffer SharedPtr.
	/// </summary>
	/// <returns>The wrapped buffer SharedPtr.</returns>
	const Ogre::HardwareVertexBufferSharedPtr& getOgreHardwareVertexBufferPtr();

	Ogre::HardwareVertexBuffer* getOgrePtr()
	{
		return hardwareVertexBuffer;
	}

public:
	/// <summary>
	/// Destructor.
	/// </summary>
	virtual ~HardwareVertexBuffer(void);

	/// <summary>
	/// Gets the size in bytes of a single vertex in this buffer.
	/// </summary>
	/// <returns>The size in bytes of a vertex.</returns>
	size_t getVertexSize();

	/// <summary>
	/// Get the number of vertices in this buffer. 
	/// </summary>
	/// <returns>The number of vertices.</returns>
	size_t getNumVertices();
};

}