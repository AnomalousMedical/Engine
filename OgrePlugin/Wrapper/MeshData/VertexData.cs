using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    public class VertexData : IDisposable
    {
        IntPtr vertexData;
        VertexBufferBinding vertexBinding;
        VertexDeclaration vertexDecl;

        internal VertexData(IntPtr vertexData)
        {
            this.vertexData = vertexData;
            vertexBinding = new VertexBufferBinding(VertexData_getVertexBufferBinding(vertexData));
            vertexDecl = new VertexDeclaration(VertexData_getVertexDeclaration(vertexData));
        }

        public void Dispose()
        {
            vertexData = IntPtr.Zero;
            vertexBinding.Dispose();
            vertexDecl.Dispose();
        }

        /// <summary>
	    /// Modifies the vertex data to be suitable for use for rendering shadow
        /// geometry.
	    /// <para>
	    /// Preparing vertex data to generate a shadow volume involves firstly
        /// ensuring that the vertex buffer containing the positions is a standalone
        /// vertex buffer, with no other components in it. This method will
        /// therefore break apart any existing vertex buffers if position is sharing
        /// a vertex buffer. Secondly, it will double the size of this vertex buffer
        /// so that there are 2 copies of the position data for the mesh. The first
        /// half is used for the original, and the second half is used for the
        /// 'extruded' version. The vertex count used to render will remain the same
        /// though, so as not to add any overhead to regular rendering of the
        /// object. Both copies of the position are required in one buffer because
        /// shadow volumes stretch from the original mesh to the extruded version. 
	    /// </para>
	    /// <para>
	    ///  It's important to appreciate that this method can fundamentally change
        ///  the structure of your vertex buffers, although in reality they will be
        ///  new buffers. As it happens, if other objects are using the original
        ///  buffers then they will be unaffected because the reference counting
        ///  will keep them intact. However, if you have made any assumptions about
        ///  the structure of the vertex data in the buffers of this object, you may
        ///  have to rethink them. 
	    /// </para>
	    /// </summary>
        public void prepareForShadowVolume()
        {
            VertexData_prepareForShadowVolume(vertexData);
        }

	    /// <summary>
	    /// Reorganises the data in the vertex buffers according to the new vertex
        /// declaration passed in.
	    /// <para>
	    /// Note that new vertex buffers are created and written to, so if the
        /// buffers being referenced by this vertex data object are also used by
        /// others, then the original buffers will not be damaged by this operation.
        /// Once this operation has completed, the new declaration passed in will
        /// overwrite the current one. 
	    /// </para>
	    /// </summary>
	    /// <param name="newDeclaration">The vertex declaration which will be used for the reorganised buffer state. Note that the new declaration must not include any elements which do not already exist in the current declaration; you can drop elements by excluding them from the declaration if you wish, however.</param>
        public void reorganizeBuffers(VertexDeclaration newDeclaration)
        {
            VertexData_reorganizeBuffers(vertexData, newDeclaration.OgreObject);
        }

	    /// <summary>
	    /// Remove any gaps in the vertex buffer bindings.
	    /// <para>
	    /// This is useful if you've removed elements and buffers from this vertex
        /// data and want to remove any gaps in the vertex buffer bindings. This
        /// method is mainly useful when reorganising vertex data manually. 
	    /// </para>
	    /// <para>
	    /// This will cause binding index of the elements in the vertex declaration
        /// to be altered to new binding index. 
	    /// </para>
	    /// </summary>
        public void closeGapsInBindings()
        {
            VertexData_closeGapsInBindings(vertexData);
        }

	    /// <summary>
	    /// Remove all vertex buffers that never used by the vertex declaration.
	    /// <para>
        /// This is useful if you've removed elements from the vertex declaration
        /// and want to unreference buffers that never used any more. This method is
        /// mainly useful when reorganising vertex data manually. 
	    /// </para>
	    /// <para>
        /// This also remove any gaps in the vertex buffer bindings. 
	    /// </para>
	    /// </summary>
        public void removeUnusedBuffers()
        {
            VertexData_removeUnusedBuffers(vertexData);
        }

	    /// <summary>
	    /// Convert all packed colour values (VET_COLOUR_*) in buffers used to another type. 
	    /// </summary>
	    /// <param name="srcType">The source colour type to assume if the ambiguous VET_COLOUR is encountered.</param>
	    /// <param name="destType">The destination colour type, must be VET_COLOUR_ABGR or VET_COLOUR_ARGB.</param>
        public void convertPackedColor(VertexElementType srcType, VertexElementType destType)
        {
            VertexData_convertPackedColor(vertexData, srcType, destType);
        }

	    /// <summary>
	    /// Allocate elements to serve a holder of morph / pose target data for hardware morphing / pose blending.
	    /// <para>
        /// This method will allocate the given number of 3D texture coordinate sets for use as a morph target or target pose offset (3D position). These elements will be saved in hwAnimationDataList. It will also assume that the source of these new elements will be new buffers which are not bound at this time, so will start the sources to 1 higher than the current highest binding source. The caller is expected to bind these new buffers when appropriate. For morph animation the original position buffer will be the 'from' keyframe data, whilst for pose animation it will be the original vertex data. 
	    /// </para>
	    /// </summary>
	    /// <param name="count"></param>
        public void allocateHardwareAnimationElements(ushort count)
        {
            VertexData_allocateHardwareAnimationElements(vertexData, count);
        }

	    /// <summary>
	    /// Declaration of the vertex to be used in this operation. Note that this
        /// is created for you on construction. 
	    /// </summary>
        public VertexDeclaration vertexDeclaration 
	    {
		    get
            {
                return vertexDecl;
            }
	    }

	    /// <summary>
	    /// The vertex buffer bindings to be used. Note that this is created for you
        /// on construction. 
	    /// </summary>
        public VertexBufferBinding vertexBufferBinding 
	    {
		    get
            {
                return vertexBinding;
            }
	    }

	    /// <summary>
	    /// The base vertex index to start from.
	    /// </summary>
        public IntPtr vertexStart 
	    {
		    get
            {
                return VertexData_getVertexStart(vertexData);
            }
            set
            {
                VertexData_setVertexStart(vertexData, value);
            }
	    }

	    /// <summary>
	    /// The number of vertices used in this operation.
	    /// </summary>
        public IntPtr vertexCount 
	    {
		    get
            {
                return VertexData_getVertexCount(vertexData);
            }
            set
            {
                VertexData_setVertexCount(vertexData, value);
            }
	    }

#region PInvoke

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexData_prepareForShadowVolume(IntPtr vertexData);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexData_reorganizeBuffers(IntPtr vertexData, IntPtr newDeclaration);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexData_closeGapsInBindings(IntPtr vertexData);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexData_removeUnusedBuffers(IntPtr vertexData);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexData_convertPackedColor(IntPtr vertexData, VertexElementType srcType, VertexElementType destType);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexData_allocateHardwareAnimationElements(IntPtr vertexData, ushort count);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexData_getVertexBufferBinding(IntPtr vertexData);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexData_getVertexDeclaration(IntPtr vertexData);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexData_setVertexStart(IntPtr vertexData, IntPtr vertexStart);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexData_getVertexStart(IntPtr vertexData);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexData_setVertexCount(IntPtr vertexData, IntPtr vertexCount);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexData_getVertexCount(IntPtr vertexData);

#endregion
    }
}
