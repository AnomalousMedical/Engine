using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using Engine;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    /// <summary>
    /// Vertex element semantics, used to identify the meaning of vertex buffer contents
    /// </summary>
    [SingleEnum]
    public enum VertexElementSemantic
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
    [SingleEnum]
    public enum VertexElementType
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

    public unsafe class VertexElement : IDisposable
    {
        internal static VertexElement createWrapper(IntPtr vertexElement, object[] args)
        {
            return new VertexElement(vertexElement);
        }

        IntPtr vertexElement;

        internal VertexElement(IntPtr vertexElement)
        {
            this.vertexElement = vertexElement;
        }

        public void Dispose()
        {
            vertexElement = IntPtr.Zero;
        }

        /// <summary>
	    /// Gets the vertex buffer index from where this element draws it's values. 
	    /// </summary>
	    /// <returns>The index of the source vertex buffer.</returns>
	    public ushort getSource()
        {
            return VertexElement_getSource(vertexElement);
        }

	    /// <summary>
	    /// Gets the offset into the buffer where this element starts. 
	    /// </summary>
	    /// <returns>The offset in bytes to the start of this element.</returns>
        public IntPtr getOffset()
        {
            return VertexElement_getOffset(vertexElement);
        }

	    /// <summary>
	    /// Gets the data format of this element. 
	    /// </summary>
	    /// <returns>The data format of this element.</returns>
        public VertexElementType getType()
        {
            return VertexElement_getType(vertexElement);
        }

	    /// <summary>
	    /// Gets the meaning of this element. 
	    /// </summary>
	    /// <returns>The meaning of the element.</returns>
        public VertexElementSemantic getSemantic()
        {
            return VertexElement_getSemantic(vertexElement);
        }

	    /// <summary>
	    /// Gets the index of this element, only applicable for repeating elements. 
	    /// </summary>
	    /// <returns>The index of the element.</returns>
        public ushort getIndex()
        {
            return VertexElement_getIndex(vertexElement);
        }

	    /// <summary>
	    /// Gets the size of this element in bytes. 
	    /// </summary>
	    /// <returns>The size of this element in bytes.</returns>
        public IntPtr getSize()
        {
            return VertexElement_getSize(vertexElement);
        }

	    /// <summary>
	    /// Adjusts a pointer to the base of a vertex to point at this element. 
	    /// </summary>
	    /// <param name="base">Pointer to the start of a vertex in this buffer.</param>
	    /// <param name="elem">Pointer to a pointer which will be set to the start of this element.</param>
        public void baseVertexPointerToElement(void* basePtr, void** elem)
        {
            VertexElement_baseVertexPointerToElementVoid(vertexElement, basePtr, elem);
        }

	    /// <summary>
	    /// Adjusts a pointer to the base of a vertex to point at this element. 
	    /// </summary>
	    /// <param name="base">Pointer to the start of a vertex in this buffer.</param>
	    /// <param name="elem">Pointer to a pointer which will be set to the start of this element.</param>
        public void baseVertexPointerToElement(void* basePtr, float** elem)
        {
            VertexElement_baseVertexPointerToElementFloat(vertexElement, basePtr, elem);
        }

	    /// <summary>
	    /// Adjusts a pointer to the base of a vertex to point at this element. 
	    /// </summary>
	    /// <param name="base">Pointer to the start of a vertex in this buffer.</param>
	    /// <param name="elem">Pointer to a pointer which will be set to the start of this element.</param>
        public void baseVertexPointerToElement(void* basePtr, uint** elem)
        {
            VertexElement_baseVertexPointerToElementUInt(vertexElement, basePtr, elem);
        }

	    /// <summary>
	    /// Adjusts a pointer to the base of a vertex to point at this element. 
	    /// </summary>
	    /// <param name="base">Pointer to the start of a vertex in this buffer.</param>
	    /// <param name="elem">Pointer to a pointer which will be set to the start of this element.</param>
        public void baseVertexPointerToElement(void* basePtr, byte** elem)
        {
            VertexElement_baseVertexPointerToElementByte(vertexElement, basePtr, elem);
        }

	    /// <summary>
	    /// Adjusts a pointer to the base of a vertex to point at this element. 
	    /// </summary>
	    /// <param name="base">Pointer to the start of a vertex in this buffer.</param>
	    /// <param name="elem">Pointer to a pointer which will be set to the start of this element.</param>
        public void baseVertexPointerToElement(void* basePtr, ushort** elem)
        {
            VertexElement_baseVertexPointerToElementUShort(vertexElement, basePtr, elem);
        }

	    /// <summary>
	    /// Utility method for helping to calculate offsets. 
	    /// </summary>
	    /// <param name="eType">The type to get the size of.</param>
	    /// <returns>The size of the element in bytes.</returns>
        public static IntPtr getTypeSize(VertexElementType eType)
        {
            return VertexElement_getTypeSize(eType);
        }

	    /// <summary>
	    /// Utility method which returns the count of values in a given type. 
	    /// </summary>
	    /// <param name="eType">The type to get the value count of.</param>
	    /// <returns>The number of values in the type.</returns>
        public static IntPtr getTypeCount(VertexElementType eType)
        {
            return VertexElement_getTypeCount(eType);
        }

	    /// <summary>
	    /// Simple converter function which will turn a single-value type into a
        /// multi-value type based on a parameter. 
	    /// </summary>
	    /// <param name="baseType">The base type to make multi.</param>
	    /// <param name="count">The number of elements in the multi type.</param>
	    /// <returns>The VertexElementType for the multi type.</returns>
        public static VertexElementType multiplyTypeCount(VertexElementType baseType, ushort count)
        {
            return VertexElement_multiplyTypeCount(baseType, count);
        }

	    /// <summary>
	    /// Simple converter function which will a type into it's single-value
        /// equivalent - makes switches on type easier. 
	    /// </summary>
	    /// <param name="multiType">The type to get the base of.</param>
	    /// <returns>The base VertexElementType.</returns>
        public static VertexElementType getBaseType(VertexElementType multiType)
        {
            return VertexElement_getBaseType(multiType);
        }

	    /// <summary>
	    /// Utility method for converting colour from one packed 32-bit colour type
        /// to another. 
	    /// </summary>
	    /// <param name="srcType">The source type.</param>
	    /// <param name="dstType">The destination type.</param>
	    /// <param name="ptr">Read / write value to change.</param>
        public static void convertColorValue(VertexElementType srcType, VertexElementType dstType, uint* ptr)
        {
            VertexElement_convertColorValue(srcType, dstType, ptr);
        }

	    /// <summary>
	    /// Utility method for converting colour to a packed 32-bit colour type. 
	    /// </summary>
	    /// <param name="src">Source colour.</param>
	    /// <param name="dst">The destination type.</param>
	    /// <returns>The converted color.</returns>
        public static uint convertColorValue(Color src, VertexElementType dst)
        {
            return VertexElement_convertColorValue2(src, dst);
        }

	    /// <summary>
	    /// Utility method to get the most appropriate packed colour vertex element format.
	    /// </summary>
	    /// <returns>The VertexElementType of the best format.</returns>
        public static VertexElementType getBestColorVertexElementType()
        {
            return VertexElement_getBestColorVertexElementType();
        }

	    /// <summary>
	    /// Comparison operator.
	    /// </summary>
	    /// <param name="p1">The lhs.</param>
	    /// <param name="p2">The rhs.</param>
	    /// <returns>True if p1 == p2 according to Ogre.</returns>
	    public static bool operator == (VertexElement p1,  VertexElement p2) 
	    {
            if (Object.ReferenceEquals(p1, p2))
            {
                return true;
            }

            if (((object)p1 == null) || ((object)p2 == null))
            {
                return false;
            }

		    return p1.vertexElement == p2.vertexElement;
	    }

        /// <summary>
        /// Comparison operator.
        /// </summary>
        /// <param name="p1">The lhs.</param>
        /// <param name="p2">The rhs.</param>
        /// <returns>True if p1 == p2 according to Ogre.</returns>
        public static bool operator !=(VertexElement p1, VertexElement p2)
        {
            if (Object.ReferenceEquals(p1, p2))
            {
                return false;
            }

            if (((object)p1 == null) || ((object)p2 == null))
            {
                return true;
            }

            return p1.vertexElement != p2.vertexElement;
        }

        public override int GetHashCode()
        {
            return vertexElement.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }
            return obj is VertexElement && ((VertexElement)obj).vertexElement == vertexElement;
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort VertexElement_getSource(IntPtr vertexElement);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexElement_getOffset(IntPtr vertexElement);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern VertexElementType VertexElement_getType(IntPtr vertexElement);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern VertexElementSemantic VertexElement_getSemantic(IntPtr vertexElement);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort VertexElement_getIndex(IntPtr vertexElement);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexElement_getSize(IntPtr vertexElement);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexElement_baseVertexPointerToElementVoid(IntPtr vertexElement, void* basePtr, void** elem);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexElement_baseVertexPointerToElementFloat(IntPtr vertexElement, void* basePtr, float** elem);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexElement_baseVertexPointerToElementUInt(IntPtr vertexElement, void* basePtr, uint** elem);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexElement_baseVertexPointerToElementByte(IntPtr vertexElement, void* basePtr, byte** elem);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexElement_baseVertexPointerToElementUShort(IntPtr vertexElement, void* basePtr, ushort** elem);

        //static functions
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexElement_getTypeSize(VertexElementType eType);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexElement_getTypeCount(VertexElementType eType);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern VertexElementType VertexElement_multiplyTypeCount(VertexElementType baseType, ushort count);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern VertexElementType VertexElement_getBaseType(VertexElementType multiType);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexElement_convertColorValue(VertexElementType srcType, VertexElementType dstType, uint* ptr);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern uint VertexElement_convertColorValue2(Color src, VertexElementType dst);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern VertexElementType VertexElement_getBestColorVertexElementType();

#endregion
    }
}
