using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class HardwareVertexBuffer : HardwareBuffer
    {
        internal static HardwareVertexBuffer createWrapper(IntPtr hardwareVertexBuffer)
        {
            return new HardwareVertexBuffer(hardwareVertexBuffer);
        }

        internal HardwareVertexBuffer(IntPtr hardVertexBuffer)
            :base(hardVertexBuffer)
        {

        }

        /// <summary>
        /// Gets the size in bytes of a single vertex in this buffer.
        /// </summary>
        /// <returns>The size in bytes of a vertex.</returns>
        public IntPtr getVertexSize()
        {
            return HardwareVertexBuffer_getVertexSize(hardwareBuffer);
        }

        /// <summary>
        /// Get the number of vertices in this buffer. 
        /// </summary>
        /// <returns>The number of vertices.</returns>
        public IntPtr getNumVertices()
        {
            return HardwareVertexBuffer_getNumVertices(hardwareBuffer);
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr HardwareVertexBuffer_getVertexSize(IntPtr hardVertexBuffer);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr HardwareVertexBuffer_getNumVertices(IntPtr hardVertexBuffer);

#endregion
    }
}
