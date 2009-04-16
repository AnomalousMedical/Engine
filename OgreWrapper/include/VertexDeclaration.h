#pragma once

#include "VertexElement.h"
#include "VertexElementCollection.h"

namespace Ogre
{
	class VertexDeclaration;
}

namespace Rendering{

ref class VertexElement;

typedef System::Collections::Generic::IEnumerable<VertexElement^> VertexElementEnum;
typedef System::Collections::Generic::List<VertexElement^> VertexElementList;

/// <remarks>
/// This class declares the format of a set of vertex inputs, which can be
/// issued to the rendering API through a RenderOperation.
/// <para>
/// You should be aware that the ordering and structure of the VertexDeclaration
/// can be very important on DirectX with older cards,so if you want to maintain
/// maximum compatibility with all render systems and all cards you should be
/// careful to follow these rules:
/// </para>
/// <list type="number">
/// <item>VertexElements should be added in the following order, and the order of the elements within a shared buffer should be as follows: position, blending weights, normals, diffuse colours, specular colours, texture coordinates (in order, with no gaps).</item>
/// <item>You must not have unused gaps in your buffers which are not referenced by any VertexElement</item>
/// <item>You must not cause the buffer and offset settings of 2 VertexElements to overlap</item>
/// </list>
/// <para>
/// Whilst GL and more modern graphics cards in D3D will allow you to defy these
/// rules, sticking to them will ensure that your buffers have the maximum
/// compatibility. 
/// </para>
/// <para>
/// Like the other classes in this functional area, these declarations should be
/// created and destroyed using the HardwareBufferManager. 
/// </para>
/// </remarks>
[Engine::Attributes::DoNotSaveAttribute]
public ref class VertexDeclaration
{
private:
	Ogre::VertexDeclaration* vertexDeclaration;
	VertexElementCollection vertexElements;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="vertexDeclaration"></param>
	VertexDeclaration(Ogre::VertexDeclaration* vertexDeclaration);

	/// <summary>
	/// Get the wrapped ogre object.
	/// </summary>
	/// <returns>The wrapped ogre object.</returns>
	Ogre::VertexDeclaration* getOgreVertexDeclaration();

public:
	/// <summary>
	/// Destructor.
	/// </summary>
	virtual ~VertexDeclaration(void);

	/// <summary>
	/// Get the number of elements in the declaration. 
	/// </summary>
	/// <returns>The number of elements in the declaration.</returns>
	size_t getElementCount();

	/// <summary>
	/// Gets read-only access to the list of vertex elements. 
	/// </summary>
	/// <returns>A list of vertex elements.  This is a copy of the elements in the declaration.</returns>
	VertexElementEnum^ getElements();

	/// <summary>
	/// Get a single element. 
	/// </summary>
	/// <param name="index">The index of the element to retrieve.</param>
	/// <returns>The element at the specified index.</returns>
	VertexElement^ getElement(unsigned short index);

	/// <summary>
	/// Sorts the elements in this list to be compatible with the maximum number
    /// of rendering APIs / graphics cards.
	/// <para>
	/// Older graphics cards require vertex data to be presented in a more rigid
    /// way, as defined in the main documentation for this class. As well as the
    /// ordering being important, where shared source buffers are used, the
    /// declaration must list all the elements for each source in turn. 
	/// </para>
	/// </summary>
	void sort();

	/// <summary>
	/// Remove any gaps in the source buffer list used by this declaration.
	/// <para>
	/// This is useful if you've modified a declaration and want to remove any
    /// gaps in the list of buffers being used. Note, however, that if this
    /// declaration is already being used with a VertexBufferBinding, you will
    /// need to alter that too. This method is mainly useful when reorganising
    /// buffers based on an altered declaration. 
	/// </para>
	/// <para>
	/// This will cause the vertex declaration to be re-sorted. 
	/// </para>
	/// </summary>
	void closeGapsInSource();

	/// <summary>
	/// Generates a new VertexDeclaration for optimal usage based on the current
    /// vertex declaration, which can be used with VertexData::reorganiseBuffers
    /// later if you wish, or simply used as a template.
	/// <para>
    /// Different buffer organisations and buffer usages will be returned
    /// depending on the parameters passed to this method. 
	/// </para>
	/// </summary>
	/// <param name="skeletalAnimation">Whether this vertex data is going to be skeletally animated.</param>
	/// <param name="vertexAnimation">Whether this vertex data is going to be vertex animated.</param>
	/// <returns>A new VertexDeclaration.</returns>
	VertexDeclaration^ getAutoOrganizedDeclaration(bool skeletalAnimation, bool vertexAnimation);

	/// <summary>
	/// Gets the index of the highest source value referenced by this declaration. 
	/// </summary>
	/// <returns>The index of the highest source value.</returns>
	unsigned short getMaxSource();

	/// <summary>
	/// Adds a new VertexElement to this declaration.
	/// <para>
    /// This method adds a single element (positions, normals etc) to the end of
    /// the vertex declaration. Please read the information in VertexDeclaration
    /// about the importance of ordering and structure for compatibility with
    /// older D3D drivers. 
	/// </para>
	/// </summary>
	/// <param name="source">The binding index of HardwareVertexBuffer which will provide the source for this element. See VertexBufferBindingState for full information.</param>
	/// <param name="offset">The offset in bytes where this element is located in the buffer.</param>
	/// <param name="theType">The data format of the element (3 floats, a colour etc).</param>
	/// <param name="semantic">The meaning of the data (position, normal, diffuse colour etc).</param>
	/// <returns>A new VertexElement as specified.</returns>
	VertexElement^ addElement(unsigned short source, size_t offset, VertexElementType theType, VertexElementSemantic semantic);

	/// <summary>
	/// Adds a new VertexElement to this declaration.
	/// <para>
    /// This method adds a single element (positions, normals etc) to the end of
    /// the vertex declaration. Please read the information in VertexDeclaration
    /// about the importance of ordering and structure for compatibility with
    /// older D3D drivers. 
	/// </para>
	/// </summary>
	/// <param name="source">The binding index of HardwareVertexBuffer which will provide the source for this element. See VertexBufferBindingState for full information.</param>
	/// <param name="offset">The offset in bytes where this element is located in the buffer.</param>
	/// <param name="theType">The data format of the element (3 floats, a colour etc).</param>
	/// <param name="semantic">The meaning of the data (position, normal, diffuse colour etc).</param>
	/// <param name="index">Optional index for multi-input elements like texture coordinates </param>
	/// <returns>A new VertexElement as specified.</returns>
	VertexElement^ addElement(unsigned short source, size_t offset, VertexElementType theType, VertexElementSemantic semantic, unsigned short index);

	/// <summary>
	/// Inserts a new VertexElement at a given position in this declaration.
	/// <para>
	/// This method adds a single element (positions, normals etc) at a given
    /// position in this vertex declaration. Please read the information in
    /// VertexDeclaration about the importance of ordering and structure for
    /// compatibility with older D3D drivers. 
	/// </para>
	/// </summary>
	/// <param name="atPosition">The binding index of HardwareVertexBuffer which will provide the source for this element. See VertexBufferBindingState for full information.</param>
	/// <param name="source">The offset in bytes where this element is located in the buffer </param>
	/// <param name="offset">The data format of the element (3 floats, a colour etc) </param>
	/// <param name="theType"></param>
	/// <param name="semantic">The meaning of the data (position, normal, diffuse colour etc) </param>
	/// <returns>A reference to the VertexElement added. </returns>
	VertexElement^ insertElement(unsigned short atPosition, unsigned short source, size_t offset, VertexElementType theType, VertexElementSemantic semantic);

	/// <summary>
	/// Inserts a new VertexElement at a given position in this declaration.
	/// <para>
	/// This method adds a single element (positions, normals etc) at a given
    /// position in this vertex declaration. Please read the information in
    /// VertexDeclaration about the importance of ordering and structure for
    /// compatibility with older D3D drivers. 
	/// </para>
	/// </summary>
	/// <param name="atPosition">The binding index of HardwareVertexBuffer which will provide the source for this element. See VertexBufferBindingState for full information.</param>
	/// <param name="source">The offset in bytes where this element is located in the buffer </param>
	/// <param name="offset">The data format of the element (3 floats, a colour etc) </param>
	/// <param name="theType"></param>
	/// <param name="semantic">The meaning of the data (position, normal, diffuse colour etc) </param>
	/// <param name="index">Optional index for multi-input elements like texture coordinates </param>
	/// <returns>A reference to the VertexElement added. </returns>
	VertexElement^ insertElement(unsigned short atPosition, unsigned short source, size_t offset, VertexElementType theType, VertexElementSemantic semantic, unsigned short index);

	/// <summary>
	/// Remove the element at the given index from this declaration. 
	/// </summary>
	/// <param name="elemIndex">The element to remove.</param>
	void removeElement(unsigned short elemIndex);

	/// <summary>
	/// Remove the element with the given semantic.
	/// </summary>
	/// <param name="semantic">The semantic of the element to remove.</param>
	void removeElement(VertexElementSemantic semantic);

	/// <summary>
	/// Remove the element with the given semantic and usage index. In this case
    /// 'index' means the usage index for repeating elements such as texture
    /// coordinates. For other elements this will always be 0 and does not refer
    /// to the index in the vector.
	/// </summary>
	/// <param name="semantic">The semantic of the element to remove.</param>
	/// <param name="index">The usage index of repeating elements.</param>
	void removeElement(VertexElementSemantic semantic, unsigned short index);

	/// <summary>
	/// Remove all elements. 
	/// </summary>
	void removeAllElements();

	/// <summary>
	/// Modify an element in-place.
	/// </summary>
	/// <param name="elemIndex">The index of the element.</param>
	/// <param name="source">The binding index of HardwareVertexBuffer which will provide the source for this element. See VertexBufferBindingState for full information.</param>
	/// <param name="offset">The offset in bytes where this element is located in the buffer.</param>
	/// <param name="theType">The data format of the element (3 floats, a colour etc).</param>
	/// <param name="semantic">The meaning of the data (position, normal, diffuse colour etc).</param>
	void modifyElement(unsigned short elemIndex, unsigned short source, size_t offset, VertexElementType theType, VertexElementSemantic semantic);

	/// <summary>
	/// Modify an element in-place.
	/// </summary>
	/// <param name="elemIndex">The index of the element.</param>
	/// <param name="source">The binding index of HardwareVertexBuffer which will provide the source for this element. See VertexBufferBindingState for full information.</param>
	/// <param name="offset">The offset in bytes where this element is located in the buffer.</param>
	/// <param name="theType">The data format of the element (3 floats, a colour etc).</param>
	/// <param name="semantic">The meaning of the data (position, normal, diffuse colour etc).</param>
	/// <param name="index">Optional index for multi-input elements like texture coordinates </param>
	void modifyElement(unsigned short elemIndex, unsigned short source, size_t offset, VertexElementType theType, VertexElementSemantic semantic, unsigned short index);

	/// <summary>
	/// Finds a VertexElement with the given semantic, and index if there is more than one element with the same semantic.
	/// </summary>
	/// <param name="sem">The semantic to search for.</param>
	/// <returns>The specified element or null if it is not found.</returns>
	VertexElement^ findElementBySemantic(VertexElementSemantic sem);

	/// <summary>
	/// Finds a VertexElement with the given semantic, and index if there is more than one element with the same semantic.
	/// </summary>
	/// <param name="sem">The semantic to search for.</param>
	/// <param name="index">The index of the element.</param>
	/// <returns>The specified element or null if it is not found.</returns>
	VertexElement^ findElementBySemantic(VertexElementSemantic sem, unsigned short index);

	/// <summary>
	/// Based on the current elements, gets the size of the vertex for a given buffer source. 
	/// </summary>
	/// <param name="source">The buffer binding index for which to get the vertex size. Gets a list of elements which use a given source. </param>
	/// <returns>A list of elements for the source.  The list contains copies of the elements.</returns>
	VertexElementEnum^ findElementsBySource(unsigned short source);

	/// <summary>
	/// Gets the vertex size defined by this declaration for a given source. 
	/// </summary>
	/// <param name="source">The buffer binding index for which to get the vertex size.</param>
	/// <returns>The size of source's vertices.</returns>
	size_t getVertexSize(unsigned short source);

	/// <summary>
	/// Clones this declaration. 
	/// </summary>
	/// <returns>A new declaration identical to this one.</returns>
	VertexDeclaration^ clone();
};

}