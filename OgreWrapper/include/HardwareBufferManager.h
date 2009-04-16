#pragma once

#include "HardwareVertexBufferSharedPtr.h"
#include "HardwareIndexBufferSharedPtr.h"
#include "VertexDeclarationCollection.h"
#include "VertexBufferBindingCollection.h"
#include "HardwareBuffer.h"
#include "HardwareIndexBuffer.h"

namespace Ogre
{
	class HardwareBufferManager;
}

namespace Engine{

namespace Rendering{

ref class HardwareVertexBuffer;
ref class HardwareIndexBuffer;
ref class VertexDeclaration;
ref class VertexBufferBinding;


/// <summary>
/// Wrapper class for the Ogre::HardwareBufferManager.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class HardwareBufferManager
{
private:
	Ogre::HardwareBufferManager* hbManager;

	HardwareVertexBufferSharedPtrCollection vertexBuffers;
	HardwareIndexBufferSharedPtrCollection indexBuffers;

	static HardwareBufferManager^ instance = gcnew HardwareBufferManager();

	VertexDeclarationCollection vertexDeclarations;
	VertexBufferBindingCollection vertexBufferBindings;

	/// <summary>
	/// Constructor
	/// </summary>
	HardwareBufferManager();

internal:
	HardwareVertexBufferSharedPtr^ getObject(const Ogre::HardwareVertexBufferSharedPtr& vertexBuffer);

	HardwareIndexBufferSharedPtr^ getObject(const Ogre::HardwareIndexBufferSharedPtr& indexBuffer);

	/// <summary>
	/// Get the wrapper object for VertexDeclaration.
	/// </summary>
	/// <param name="ogreObject">The instance of Ogre::VertexDeclaration to get the wrapper object for.</param>
	/// <returns>The wrapper object for the input object.</returns>
	VertexDeclaration^ getObject(Ogre::VertexDeclaration* ogreObject);

	/// <summary>
	/// Destroy the wrapper for the given ogre VertexDeclaration.
	/// </summary>
	/// <param name="ogreObject">The ogre object to destroy the binding for.</param>
	void destroyObject(Ogre::VertexDeclaration* ogreObject);

	/// <summary>
	/// Get the wrapper object for VertexBufferBinding.
	/// </summary>
	/// <param name="ogreObject">The instance of Ogre::VertexBufferBinding to get the wrapper object for.</param>
	/// <returns>The wrapper object for the input object.</returns>
	VertexBufferBinding^ getObject(Ogre::VertexBufferBinding* ogreObject);

	/// <summary>
	/// Destroy the wrapper for the given ogre VertexBufferBinding.
	/// </summary>
	/// <param name="ogreObject">The ogre object to destroy the binding for.</param>
	void destroyObject(Ogre::VertexBufferBinding* ogreObject);

public:
	/// <summary>
	/// Destructor
	/// </summary>
	~HardwareBufferManager();

	/// <summary>
	/// Get the singleton instance of this HardwareBufferManager.
	/// </summary>
	/// <returns>The singleton instance of this HardwareBufferManager.</returns>
	static HardwareBufferManager^ getInstance();

	HardwareVertexBufferSharedPtr^ createVertexBuffer(size_t vertexSize, size_t numVerts, HardwareBuffer::Usage usage);

	HardwareVertexBufferSharedPtr^ createVertexBuffer(size_t vertexSize, size_t numVerts, HardwareBuffer::Usage usage, bool useShadowBuffer);

	HardwareIndexBufferSharedPtr^ createIndexBuffer(HardwareIndexBuffer::IndexType itype, size_t numIndexes, HardwareBuffer::Usage usage);

	HardwareIndexBufferSharedPtr^ createIndexBuffer(HardwareIndexBuffer::IndexType itype, size_t numIndexes, HardwareBuffer::Usage usage, bool useShadowBuffer);

	VertexDeclaration^ createVertexDeclaration();

	void destroyVertexDeclaration(VertexDeclaration^ decl);

	VertexBufferBinding^ createVertexBufferBinding();

	void destroyVertexBufferBinding(VertexBufferBinding^ binding);
};

}

}