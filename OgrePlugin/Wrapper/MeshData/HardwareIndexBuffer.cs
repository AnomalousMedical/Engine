using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class HardwareIndexBuffer : HardwareBuffer
    {
        /// <summary>
	    /// The type of index.
	    /// </summary>
	    enum IndexType : uint
	    {
		    /// <summary>
		    /// 16 bit indices.
		    /// </summary>
		    IT_16BIT,
		    /// <summary>
		    /// 32 bit indices.
		    /// </summary>
		    IT_32BIT
        };

        internal static HardwareIndexBuffer createWrapper(IntPtr hardwareBuffer)
        {
            return new HardwareIndexBuffer(hardwareBuffer);
        }

        internal HardwareIndexBuffer(IntPtr hardIndexBuffer)
            :base(hardIndexBuffer)
        {

        }

        /// <summary>
        /// Get the type of indexes used in this buffer. 
        /// </summary>
        /// <returns>The IndexType of the buffer.</returns>
        IndexType getType()
        {
            return HardwareIndexBuffer_getType(hardwareBuffer);
        }

        /// <summary>
        /// Get the number of indexes in this buffer.
        /// </summary>
        /// <returns>The number of indexes in the buffer.</returns>
        IntPtr getNumIndexes()
        {
            return HardwareIndexBuffer_getNumIndexes(hardwareBuffer);
        }

        /// <summary>
        /// Get the size in bytes of each index.
        /// </summary>
        /// <returns>The size of a single index in bytes.</returns>
        IntPtr getIndexSize()
        {
            return HardwareIndexBuffer_getIndexSize(hardwareBuffer);
        }

#region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern IndexType HardwareIndexBuffer_getType(IntPtr hardwareBuffer);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr HardwareIndexBuffer_getNumIndexes(IntPtr hardwareBuffer);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr HardwareIndexBuffer_getIndexSize(IntPtr hardwareBuffer);

#endregion
    }
}
