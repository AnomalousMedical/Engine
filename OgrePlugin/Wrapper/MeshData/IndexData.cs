using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class IndexData : IDisposable
    {
        IntPtr indexData;

        internal IndexData(IntPtr indexData)
        {
            this.indexData = indexData;
        }

        public void Dispose()
        {
            indexData = IntPtr.Zero;
        }

	    /// <summary>
	    /// Re-order the indexes in this index data structure to be more vertex
        /// cache friendly; that is to re-use the same vertices as close together as
        /// possible. Can only be used for index data which consists of triangle
        /// lists. It would in fact be pointless to use it on triangle strips or
        /// fans in any case. 
	    /// </summary>
        public void optimizeVertexCacheTriList()
        {
            IndexData_optimizeVertexCacheTriList(indexData);
        }

	    /// <summary>
	    /// The HardwareIndexBuffer to use, must be specified if useIndexes = true 
	    /// </summary>
        public HardwareIndexBufferSharedPtr IndexBuffer 
	    {
            get
            {
                HardwareBufferManager bufferManager = HardwareBufferManager.getInstance();
                return bufferManager.getIndexBufferObject(IndexData_getIndexBuffer(indexData, bufferManager.ProcessIndexBufferCallback));
            }
	    }

	    /// <summary>
	    /// index in the buffer to start from for this operation.
	    /// </summary>
        public IntPtr IndexStart 
	    {
		    get
            {
                return IndexData_getIndexStart(indexData);
            }
            set
            {
                IndexData_setIndexStart(indexData, value);
            }
	    }

	    /// <summary>
	    /// The number of indexes to use from the buffer.
	    /// </summary>
        public IntPtr IndexCount 
	    {
		    get
            {
                return IndexData_getIndexCount(indexData);
            }
            set
            {
                IndexData_setIndexCount(indexData, value);
            }
	    }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void IndexData_optimizeVertexCacheTriList(IntPtr indexData);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr IndexData_getIndexBuffer(IntPtr indexData, ProcessWrapperObjectDelegate processIndexBuffer);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void IndexData_setIndexStart(IntPtr indexData, IntPtr indexStart);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr IndexData_getIndexStart(IntPtr indexData);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void IndexData_setIndexCount(IntPtr indexData, IntPtr indexCount);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr IndexData_getIndexCount(IntPtr indexData);

#endregion
    }
}
