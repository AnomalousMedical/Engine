using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace OgreWrapper
{
    public class VertexDeclaration : IDisposable
    {
        private IntPtr vertexDeclaration;
        private WrapperCollection<VertexElement> vertexElements = new WrapperCollection<VertexElement>(VertexElement.createWrapper);

        internal VertexDeclaration(IntPtr vertexDeclaration)
        {
            this.vertexDeclaration = vertexDeclaration;
        }

        public void Dispose()
        {
            vertexElements.Dispose();
            vertexDeclaration = IntPtr.Zero;
        }

        internal IntPtr OgreObject
        {
            get
            {
                return vertexDeclaration;
            }
        }

        /// <summary>
	    /// Get the number of elements in the declaration. 
	    /// </summary>
	    /// <returns>The number of elements in the declaration.</returns>
        public IntPtr getElementCount()
        {
            return VertexDeclaration_getElementCount(vertexDeclaration);
        }

	    /// <summary>
	    /// Get a single element. 
	    /// </summary>
	    /// <param name="index">The index of the element to retrieve.</param>
	    /// <returns>The element at the specified index.</returns>
        public VertexElement getElement(ushort index)
        {
            return vertexElements.getObject(VertexDeclaration_getElement(vertexDeclaration, index));
        }

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
        public void sort()
        {
            VertexDeclaration_sort(vertexDeclaration);
        }

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
        public void closeGapsInSource()
        {
            VertexDeclaration_closeGapsInSource(vertexDeclaration);
        }

	    /// <summary>
	    /// Gets the index of the highest source value referenced by this declaration. 
	    /// </summary>
	    /// <returns>The index of the highest source value.</returns>
        public ushort getMaxSource()
        {
            return VertexDeclaration_getMaxSource(vertexDeclaration);
        }

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
        public VertexElement addElement(ushort source, IntPtr offset, VertexElementType theType, VertexElementSemantic semantic)
        {
            return vertexElements.getObject(VertexDeclaration_addElement(vertexDeclaration, source, offset, theType, semantic));
        }

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
        public VertexElement addElement(ushort source, IntPtr offset, VertexElementType theType, VertexElementSemantic semantic, ushort index)
        {
            return vertexElements.getObject(VertexDeclaration_addElement2(vertexDeclaration, source, offset, theType, semantic, index));
        }

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
        public VertexElement insertElement(ushort atPosition, ushort source, IntPtr offset, VertexElementType theType, VertexElementSemantic semantic)
        {
            return vertexElements.getObject(VertexDeclaration_insertElement(vertexDeclaration, atPosition, source, offset, theType, semantic));
        }

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
        public VertexElement insertElement(ushort atPosition, ushort source, IntPtr offset, VertexElementType theType, VertexElementSemantic semantic, ushort index)
        {
            return vertexElements.getObject(VertexDeclaration_insertElement2(vertexDeclaration, atPosition, source, offset, theType, semantic, index));
        }

	    /// <summary>
	    /// Remove the element at the given index from this declaration. 
	    /// </summary>
	    /// <param name="elemIndex">The element to remove.</param>
        public void removeElement(ushort elemIndex)
        {
            vertexElements.destroyObject(VertexDeclaration_getElement(vertexDeclaration, elemIndex));
            VertexDeclaration_removeElement(vertexDeclaration, elemIndex);
        }

	    /// <summary>
	    /// Remove the element with the given semantic.
	    /// </summary>
	    /// <param name="semantic">The semantic of the element to remove.</param>
        public void removeElement(VertexElementSemantic semantic)
        {
            vertexElements.destroyObject(VertexDeclaration_findElementBySemantic(vertexDeclaration, semantic));
            VertexDeclaration_removeElement2(vertexDeclaration, semantic);
        }

	    /// <summary>
	    /// Remove the element with the given semantic and usage index. In this case
        /// 'index' means the usage index for repeating elements such as texture
        /// coordinates. For other elements this will always be 0 and does not refer
        /// to the index in the vector.
	    /// </summary>
	    /// <param name="semantic">The semantic of the element to remove.</param>
	    /// <param name="index">The usage index of repeating elements.</param>
        public void removeElement(VertexElementSemantic semantic, ushort index)
        {
            vertexElements.destroyObject(VertexDeclaration_findElementBySemantic2(vertexDeclaration, semantic, index));
            VertexDeclaration_removeElement3(vertexDeclaration, semantic, index);
        }

	    /// <summary>
	    /// Remove all elements. 
	    /// </summary>
        public void removeAllElements()
        {
            vertexElements.clearObjects();
            VertexDeclaration_removeAllElements(vertexDeclaration);
        }

	    /// <summary>
	    /// Modify an element in-place.
	    /// </summary>
	    /// <param name="elemIndex">The index of the element.</param>
	    /// <param name="source">The binding index of HardwareVertexBuffer which will provide the source for this element. See VertexBufferBindingState for full information.</param>
	    /// <param name="offset">The offset in bytes where this element is located in the buffer.</param>
	    /// <param name="theType">The data format of the element (3 floats, a colour etc).</param>
	    /// <param name="semantic">The meaning of the data (position, normal, diffuse colour etc).</param>
        public void modifyElement(ushort elemIndex, ushort source, IntPtr offset, VertexElementType theType, VertexElementSemantic semantic)
        {
            VertexDeclaration_modifyElement(vertexDeclaration, elemIndex, source, offset, theType, semantic);
        }

	    /// <summary>
	    /// Modify an element in-place.
	    /// </summary>
	    /// <param name="elemIndex">The index of the element.</param>
	    /// <param name="source">The binding index of HardwareVertexBuffer which will provide the source for this element. See VertexBufferBindingState for full information.</param>
	    /// <param name="offset">The offset in bytes where this element is located in the buffer.</param>
	    /// <param name="theType">The data format of the element (3 floats, a colour etc).</param>
	    /// <param name="semantic">The meaning of the data (position, normal, diffuse colour etc).</param>
	    /// <param name="index">Optional index for multi-input elements like texture coordinates </param>
        public void modifyElement(ushort elemIndex, ushort source, IntPtr offset, VertexElementType theType, VertexElementSemantic semantic, ushort index)
        {
            VertexDeclaration_modifyElement2(vertexDeclaration, elemIndex, source, offset, theType, semantic, index);
        }

	    /// <summary>
	    /// Finds a VertexElement with the given semantic, and index if there is more than one element with the same semantic.
	    /// </summary>
	    /// <param name="sem">The semantic to search for.</param>
	    /// <returns>The specified element or null if it is not found.</returns>
        public VertexElement findElementBySemantic(VertexElementSemantic sem)
        {
            return vertexElements.getObject(VertexDeclaration_findElementBySemantic(vertexDeclaration, sem));
        }

	    /// <summary>
	    /// Finds a VertexElement with the given semantic, and index if there is more than one element with the same semantic.
	    /// </summary>
	    /// <param name="sem">The semantic to search for.</param>
	    /// <param name="index">The index of the element.</param>
	    /// <returns>The specified element or null if it is not found.</returns>
        public VertexElement findElementBySemantic(VertexElementSemantic sem, ushort index)
        {
            return vertexElements.getObject(VertexDeclaration_findElementBySemantic2(vertexDeclaration, sem, index));
        }

	    /// <summary>
	    /// Gets the vertex size defined by this declaration for a given source. 
	    /// </summary>
	    /// <param name="source">The buffer binding index for which to get the vertex size.</param>
	    /// <returns>The size of source's vertices.</returns>
        public IntPtr getVertexSize(ushort source)
        {
            return VertexDeclaration_getVertexSize(vertexDeclaration, source);
        }

#region PInvoke

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexDeclaration_getElementCount(IntPtr vertexDeclaration);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexDeclaration_getElement(IntPtr vertexDeclaration, ushort index);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexDeclaration_sort(IntPtr vertexDeclaration);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexDeclaration_closeGapsInSource(IntPtr vertexDeclaration);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort VertexDeclaration_getMaxSource(IntPtr vertexDeclaration);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexDeclaration_addElement(IntPtr vertexDeclaration, ushort source, IntPtr offset, VertexElementType theType, VertexElementSemantic semantic);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexDeclaration_addElement2(IntPtr vertexDeclaration, ushort source, IntPtr offset, VertexElementType theType, VertexElementSemantic semantic, ushort index);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexDeclaration_insertElement(IntPtr vertexDeclaration, ushort atPosition, ushort source, IntPtr offset, VertexElementType theType, VertexElementSemantic semantic);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexDeclaration_insertElement2(IntPtr vertexDeclaration, ushort atPosition, ushort source, IntPtr offset, VertexElementType theType, VertexElementSemantic semantic, ushort index);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexDeclaration_removeElement(IntPtr vertexDeclaration, ushort elemIndex);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexDeclaration_removeElement2(IntPtr vertexDeclaration, VertexElementSemantic semantic);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexDeclaration_removeElement3(IntPtr vertexDeclaration, VertexElementSemantic semantic, ushort index);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexDeclaration_removeAllElements(IntPtr vertexDeclaration);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexDeclaration_modifyElement(IntPtr vertexDeclaration, ushort elemIndex, ushort source, IntPtr offset, VertexElementType theType, VertexElementSemantic semantic);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexDeclaration_modifyElement2(IntPtr vertexDeclaration, ushort elemIndex, ushort source, IntPtr offset, VertexElementType theType, VertexElementSemantic semantic, ushort index);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexDeclaration_findElementBySemantic(IntPtr vertexDeclaration, VertexElementSemantic sem);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexDeclaration_findElementBySemantic2(IntPtr vertexDeclaration, VertexElementSemantic sem, ushort index);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexDeclaration_getVertexSize(IntPtr vertexDeclaration, ushort source);

#endregion
    }
}
