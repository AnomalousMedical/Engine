#pragma once

namespace Ogre
{
	class VertexBufferBinding;
}

namespace OgreWrapper{

ref class HardwareVertexBufferSharedPtr;

typedef System::Collections::Generic::Dictionary<unsigned short, unsigned short> BindingIndexMap;

/// <summary>
/// Records the state of all the vertex buffer bindings required to provide a
/// vertex declaration with the input data it needs for the vertex elements.
/// <para>
/// Why do we have this binding list rather than just have VertexElement
/// referring to the vertex buffers direct? Well, in the underlying APIs,
/// binding the vertex buffers to an index (or 'stream') is the way that vertex
/// data is linked, so this structure better reflects the realities of that. In
/// addition, by separating the vertex declaration from the list of vertex
/// buffer bindings, it becomes possible to reuse bindings between declarations
/// and vice versa, giving opportunities to reduce the state changes required to
/// perform rendering. 
/// </para>
/// <para>
/// Like the other classes in this functional area, these binding maps should be
/// created and destroyed using the HardwareBufferManager. 
/// </para>
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class VertexBufferBinding
{
private:
	Ogre::VertexBufferBinding* vertexBufferBinding;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="vertexBufferBinding">The Ogre::VertexBufferBinding to wrap.</param>
	VertexBufferBinding(Ogre::VertexBufferBinding* vertexBufferBinding);

	Ogre::VertexBufferBinding* getVertexBufferBinding()
	{
		return vertexBufferBinding;
	}

public:
	/// <summary>
	/// Destructor.
	/// </summary>
	virtual ~VertexBufferBinding(void);

	/// <summary>
	/// Set a binding, associating a vertex buffer with a given index.
	/// <para>
	/// If the index is already associated with a vertex buffer, the association
    /// will be replaced. This may cause the old buffer to be destroyed if
    /// nothing else is referring to it. You should assign bindings from 0 and
    /// not leave gaps, although you can bind them in any order. 
	/// </para>
	/// </summary>
	/// <param name="index">The index to set.</param>
	/// <param name="buffer">The buffer to set.</param>
	void setBinding(unsigned short index, HardwareVertexBufferSharedPtr^ buffer);

	/// <summary>
	/// Removes an existing binding. 
	/// </summary>
	/// <param name="index">The index of the binding to remove.</param>
	void unsetBinding(unsigned short index);

	/// <summary>
	/// Removes all the bindings. 
	/// </summary>
	void unsetAllBindings();

	/// <summary>
	/// Gets the buffer bound to the given source index.
	/// </summary>
	/// <param name="index">The index of the buffer to retrieve.</param>
	/// <returns>The buffer bound to index.</returns>
	HardwareVertexBufferSharedPtr^ getBuffer(unsigned short index);

	/// <summary>
	/// Gets whether a buffer is bound to the given source index. 
	/// </summary>
	/// <param name="index">The index to check for binding.</param>
	/// <returns>True if a buffer is bound to the index.</returns>
	bool isBufferBound(unsigned short index);

	/// <summary>
	/// Get the number of bindings.
	/// </summary>
	/// <returns>The number of bindings.</returns>
	size_t getBufferCount();

	/// <summary>
	/// Gets the highest index which has already been set, plus 1.
    /// This is to assist in binding the vertex buffers such that there are not gaps in the list. 
	/// </summary>
	/// <returns>The highest index which has already been set, plus 1</returns>
	unsigned short getNextIndex();

	/// <summary>
	/// Gets the last bound index. 
	/// </summary>
	/// <returns>The last index to have data bound to it.</returns>
	unsigned short getLastBoundIndex();

	/// <summary>
	/// Check whether any gaps in the bindings. 
	/// </summary>
	/// <returns>True if there are gaps in the bindings.</returns>
	bool hasGaps();

	/// <summary>
	/// Remove any gaps in the bindings.
	/// <para>
    /// This is useful if you've removed vertex buffer from this vertex buffer
    /// bindings and want to remove any gaps in the bindings. Note, however,
    /// that if this bindings is already being used with a VertexDeclaration,
    /// you will need to alter that too. This method is mainly useful when
    /// reorganising buffers manually. 
	/// </para>c
	/// </summary>
	/// <param name="indexMap">To be retrieve the binding index map that used to translation old index to new index; will be cleared by this method before fill-in.</param>
	void closeGaps(BindingIndexMap^ indexMap);
};

}