using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class VertexBufferBinding : IDisposable
    {
        private IntPtr vertexBufferBinding;

        internal VertexBufferBinding(IntPtr vertexBufferBinding)
        {
            this.vertexBufferBinding = vertexBufferBinding;
        }

        public void Dispose()
        {
            vertexBufferBinding = IntPtr.Zero;
        }

        internal IntPtr OgreObject
        {
            get
            {
                return vertexBufferBinding;
            }
        }

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
        public void setBinding(ushort index, HardwareVertexBufferSharedPtr buffer)
        {
            VertexBufferBinding_setBinding(vertexBufferBinding, index, buffer.HeapSharedPtr);
        }

	    /// <summary>
	    /// Removes an existing binding. 
	    /// </summary>
	    /// <param name="index">The index of the binding to remove.</param>
        public void unsetBinding(ushort index)
        {
            VertexBufferBinding_unsetBinding(vertexBufferBinding, index);
        }

	    /// <summary>
	    /// Removes all the bindings. 
	    /// </summary>
        public void unsetAllBindings()
        {
            VertexBufferBinding_unsetAllBindings(vertexBufferBinding);
        }

	    /// <summary>
	    /// Gets the buffer bound to the given source index.
	    /// </summary>
	    /// <param name="index">The index of the buffer to retrieve.</param>
	    /// <returns>The buffer bound to index.</returns>
        public HardwareVertexBufferSharedPtr getBuffer(ushort index)
        {
            HardwareBufferManager bufferManager = HardwareBufferManager.getInstance();
            return bufferManager.getVertexBufferObject(VertexBufferBinding_getBuffer(vertexBufferBinding, index, bufferManager.ProcessVertexBufferCallback));
        }

	    /// <summary>
	    /// Gets whether a buffer is bound to the given source index. 
	    /// </summary>
	    /// <param name="index">The index to check for binding.</param>
	    /// <returns>True if a buffer is bound to the index.</returns>
        public bool isBufferBound(ushort index)
        {
            return VertexBufferBinding_isBufferBound(vertexBufferBinding, index);
        }

	    /// <summary>
	    /// Get the number of bindings.
	    /// </summary>
	    /// <returns>The number of bindings.</returns>
        public IntPtr getBufferCount()
        {
            return VertexBufferBinding_getBufferCount(vertexBufferBinding);
        }

	    /// <summary>
	    /// Gets the highest index which has already been set, plus 1.
        /// This is to assist in binding the vertex buffers such that there are not gaps in the list. 
	    /// </summary>
	    /// <returns>The highest index which has already been set, plus 1</returns>
        public ushort getNextIndex()
        {
            return VertexBufferBinding_getNextIndex(vertexBufferBinding);
        }

	    /// <summary>
	    /// Gets the last bound index. 
	    /// </summary>
	    /// <returns>The last index to have data bound to it.</returns>
        public ushort getLastBoundIndex()
        {
            return VertexBufferBinding_getLastBoundIndex(vertexBufferBinding);
        }

	    /// <summary>
	    /// Check whether any gaps in the bindings. 
	    /// </summary>
	    /// <returns>True if there are gaps in the bindings.</returns>
        public bool hasGaps()
        {
            return VertexBufferBinding_hasGaps(vertexBufferBinding);
        }

#region PInvoke

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexBufferBinding_setBinding(IntPtr vertexBinding, ushort index, IntPtr buffer);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexBufferBinding_unsetBinding(IntPtr vertexBinding, ushort index);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexBufferBinding_unsetAllBindings(IntPtr vertexBinding);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexBufferBinding_getBuffer(IntPtr vertexBinding, ushort index, ProcessWrapperObjectDelegate processCallback);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool VertexBufferBinding_isBufferBound(IntPtr vertexBinding, ushort index);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VertexBufferBinding_getBufferCount(IntPtr vertexBinding);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort VertexBufferBinding_getNextIndex(IntPtr vertexBinding);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort VertexBufferBinding_getLastBoundIndex(IntPtr vertexBinding);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool VertexBufferBinding_hasGaps(IntPtr vertexBinding);

#endregion
    }
}
