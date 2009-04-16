#pragma once

namespace Ogre
{
	class IndexData;
}

namespace OgreWrapper{

ref class HardwareIndexBufferSharedPtr;

/// <summary>
/// Class collecting index data source information. 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class IndexData
{
private:
	Ogre::IndexData* indexData;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="indexData">The Ogre::IndexData to wrap.</param>
	IndexData(Ogre::IndexData* indexData);

public:
	/// <summary>
	/// Destructor.
	/// </summary>
	virtual ~IndexData(void);

	/// <summary>
	/// Clones this index data, potentially including replicating the index buffer.
	/// The caller is expected to dispose the returned object when ready.
	/// </summary>
	/// <returns>A clone of this IndexBuffer.</returns>
	IndexData^ clone();

	/// <summary>
	/// Re-order the indexes in this index data structure to be more vertex
    /// cache friendly; that is to re-use the same vertices as close together as
    /// possible. Can only be used for index data which consists of triangle
    /// lists. It would in fact be pointless to use it on triangle strips or
    /// fans in any case. 
	/// </summary>
	void optimizeVertexCacheTriList();

	/// <summary>
	/// The HardwareIndexBuffer to use, must be specified if useIndexes = true 
	/// </summary>
	property HardwareIndexBufferSharedPtr^ IndexBuffer 
	{
		HardwareIndexBufferSharedPtr^ get();
	}

	/// <summary>
	/// index in the buffer to start from for this operation.
	/// </summary>
	property size_t IndexStart 
	{
		size_t get();
		void set(size_t value);
	}

	/// <summary>
	/// The number of indexes to use from the buffer.
	/// </summary>
	property size_t IndexCount 
	{
		size_t get();
		void set(size_t value);
	}
};

}