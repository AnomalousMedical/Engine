#pragma once

#include "HardwareVertexBuffer.h"

namespace Ogre
{
	class VertexElement;
}

namespace OgreWrapper{

/// <summary>
/// Vertex element semantics, used to identify the meaning of vertex buffer contents
/// </summary>
[Engine::Attributes::SingleEnum]
public enum class VertexElementSemantic
{
	/// <summary>
	/// Position, 3 reals per vertex
	/// </summary>
	VES_POSITION = 1,

	/// <summary>
	/// Blending weights
	/// </summary>
	VES_BLEND_WEIGHTS = 2,

	/// <summary>
    /// Blending indices
	/// </summary>
    VES_BLEND_INDICES = 3,

	/// <summary>
	/// Normal, 3 reals per vertex
	/// </summary>
	VES_NORMAL = 4,

	/// <summary>
	/// Diffuse colours
	/// </summary>
	VES_DIFFUSE = 5,

	/// <summary>
	/// Specular colours
	/// </summary>
	VES_SPECULAR = 6,

	/// <summary>
	/// Texture coordinates
	/// </summary>
	VES_TEXTURE_COORDINATES = 7,

	/// <summary>
    /// Binormal (Y axis if normal is Z)
	/// </summary>
    VES_BINORMAL = 8,

	/// <summary>
    /// Tangent (X axis if normal is Z)
	/// </summary>
    VES_TANGENT = 9
};

/// <summary>
/// Vertex element type, used to identify the base types of the vertex contents
/// </summary>
[Engine::Attributes::SingleEnum]
public enum class VertexElementType
{
    VET_FLOAT1 = 0,
    VET_FLOAT2 = 1,
    VET_FLOAT3 = 2,
    VET_FLOAT4 = 3,
    /// alias to more specific colour type - use the current rendersystem's colour packing
	VET_COLOUR = 4,
	VET_SHORT1 = 5,
	VET_SHORT2 = 6,
	VET_SHORT3 = 7,
	VET_SHORT4 = 8,
    VET_UBYTE4 = 9,
    /// D3D style compact colour
    VET_COLOUR_ARGB = 10,
    /// GL style compact colour
    VET_COLOUR_ABGR = 11
};

/// <summary>
/// This class declares the usage of a single vertex buffer as a component of a
/// complete VertexDeclaration.
/// <para>
/// Several vertex buffers can be used to supply the input geometry for a
/// rendering operation, and in each case a vertex buffer can be used in
/// different ways for different operations; the buffer itself does not define
/// the semantics (position, normal etc), the VertexElement class does. 
/// </para>
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class VertexElement
{
private:
	const Ogre::VertexElement* vertexElement;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="vertexElement">The VertexElement to wrap.</param>
	VertexElement(const Ogre::VertexElement* vertexElement);

public:
	/// <summary>
	/// Destructor.
	/// </summary>
	virtual ~VertexElement(void);

	/// <summary>
	/// Gets the vertex buffer index from where this element draws it's values. 
	/// </summary>
	/// <returns>The index of the source vertex buffer.</returns>
	unsigned short getSource();

	/// <summary>
	/// Gets the offset into the buffer where this element starts. 
	/// </summary>
	/// <returns>The offset in bytes to the start of this element.</returns>
	size_t getOffset();

	/// <summary>
	/// Gets the data format of this element. 
	/// </summary>
	/// <returns>The data format of this element.</returns>
	VertexElementType getType();

	/// <summary>
	/// Gets the meaning of this element. 
	/// </summary>
	/// <returns>The meaning of the element.</returns>
	VertexElementSemantic getSemantic();

	/// <summary>
	/// Gets the index of this element, only applicable for repeating elements. 
	/// </summary>
	/// <returns>The index of the element.</returns>
	unsigned short getIndex();

	/// <summary>
	/// Gets the size of this element in bytes. 
	/// </summary>
	/// <returns>The size of this element in bytes.</returns>
	size_t getSize();

	/// <summary>
	/// Adjusts a pointer to the base of a vertex to point at this element. 
	/// </summary>
	/// <param name="base">Pointer to the start of a vertex in this buffer.</param>
	/// <param name="elem">Pointer to a pointer which will be set to the start of this element.</param>
	void baseVertexPointerToElement(void* base, void** elem);

	/// <summary>
	/// Adjusts a pointer to the base of a vertex to point at this element. 
	/// </summary>
	/// <param name="base">Pointer to the start of a vertex in this buffer.</param>
	/// <param name="elem">Pointer to a pointer which will be set to the start of this element.</param>
	void baseVertexPointerToElement(void* base, float** elem);

	/// <summary>
	/// Adjusts a pointer to the base of a vertex to point at this element. 
	/// </summary>
	/// <param name="base">Pointer to the start of a vertex in this buffer.</param>
	/// <param name="elem">Pointer to a pointer which will be set to the start of this element.</param>
	void baseVertexPointerToElement(void* base, unsigned int** elem);

	/// <summary>
	/// Adjusts a pointer to the base of a vertex to point at this element. 
	/// </summary>
	/// <param name="base">Pointer to the start of a vertex in this buffer.</param>
	/// <param name="elem">Pointer to a pointer which will be set to the start of this element.</param>
	void baseVertexPointerToElement(void* base, unsigned char** elem);

	/// <summary>
	/// Adjusts a pointer to the base of a vertex to point at this element. 
	/// </summary>
	/// <param name="base">Pointer to the start of a vertex in this buffer.</param>
	/// <param name="elem">Pointer to a pointer which will be set to the start of this element.</param>
	void baseVertexPointerToElement(void* base, unsigned short** elem);

	/// <summary>
	/// Utility method for helping to calculate offsets. 
	/// </summary>
	/// <param name="eType">The type to get the size of.</param>
	/// <returns>The size of the element in bytes.</returns>
	static size_t getTypeSize(VertexElementType eType);

	/// <summary>
	/// Utility method which returns the count of values in a given type. 
	/// </summary>
	/// <param name="eType">The type to get the value count of.</param>
	/// <returns>The number of values in the type.</returns>
	static size_t getTypeCount(VertexElementType eType);

	/// <summary>
	/// Simple converter function which will turn a single-value type into a
    /// multi-value type based on a parameter. 
	/// </summary>
	/// <param name="baseType">The base type to make multi.</param>
	/// <param name="count">The number of elements in the multi type.</param>
	/// <returns>The VertexElementType for the multi type.</returns>
	static VertexElementType multiplyTypeCount(VertexElementType baseType, unsigned short count);

	/// <summary>
	/// Simple converter function which will a type into it's single-value
    /// equivalent - makes switches on type easier. 
	/// </summary>
	/// <param name="multiType">The type to get the base of.</param>
	/// <returns>The base VertexElementType.</returns>
	static VertexElementType getBaseType(VertexElementType multiType);

	/// <summary>
	/// Utility method for converting colour from one packed 32-bit colour type
    /// to another. 
	/// </summary>
	/// <param name="srcType">The source type.</param>
	/// <param name="dstType">The destination type.</param>
	/// <param name="ptr">Read / write value to change.</param>
	static void convertColorValue(VertexElementType srcType, VertexElementType dstType, unsigned int* ptr);

	/// <summary>
	/// Utility method for converting colour to a packed 32-bit colour type. 
	/// </summary>
	/// <param name="src">Source colour.</param>
	/// <param name="dst">The destination type.</param>
	/// <returns>The converted color.</returns>
	static unsigned int convertColorValue(Engine::Color src, VertexElementType dst);

	/// <summary>
	/// Utility method to get the most appropriate packed colour vertex element format.
	/// </summary>
	/// <returns>The VertexElementType of the best format.</returns>
	static VertexElementType getBestColorVertexElementType();

	/// <summary>
	/// Comparison operator.
	/// </summary>
	/// <param name="p1">The lhs.</param>
	/// <param name="p2">The rhs.</param>
	/// <returns>True if p1 == p2 according to Ogre.</returns>
	static bool operator == (VertexElement^ p1,  VertexElement^ p2) 
	{
		return p1->vertexElement == p2->vertexElement;
	}
};

}