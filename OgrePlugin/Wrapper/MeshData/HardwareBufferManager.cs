using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class HardwareBufferManager : IDisposable
    {
        static HardwareBufferManager instance = new HardwareBufferManager();

        public static HardwareBufferManager getInstance()
        {
            return instance;
        }

        private SharedPtrCollection<HardwareIndexBuffer> indexBuffers = new SharedPtrCollection<HardwareIndexBuffer>(HardwareIndexBuffer.createWrapper, HardwareIndexBufferPtr_createHeapPtr, HardwareIndexBufferPtr_Delete);
        private SharedPtrCollection<HardwareVertexBuffer> vertexBuffers = new SharedPtrCollection<HardwareVertexBuffer>(HardwareVertexBuffer.createWrapper, HardwareVertexBufferPtr_createHeapPtr, HardwareVertexBufferPtr_Delete);
        private SharedPtrCollection<HardwarePixelBuffer> pixelBuffers = new SharedPtrCollection<HardwarePixelBuffer>(HardwarePixelBuffer.createWrapper, HardwarePixelBufferPtr_createHeapPtr, HardwarePixelBufferPtr_Delete);

        public void Dispose()
        {
            indexBuffers.Dispose();
            vertexBuffers.Dispose();
            pixelBuffers.Dispose();
        }

        internal HardwareIndexBufferSharedPtr getIndexBufferObject(IntPtr hardwareIndexBuffer)
        {
            return new HardwareIndexBufferSharedPtr(indexBuffers.getObject(hardwareIndexBuffer));
        }

        internal ProcessWrapperObjectDelegate ProcessIndexBufferCallback
        {
            get
            {
                return indexBuffers.ProcessWrapperCallback;
            }
        }

        internal HardwareVertexBufferSharedPtr getVertexBufferObject(IntPtr hardwareVertexBuffer)
        {
            return new HardwareVertexBufferSharedPtr(vertexBuffers.getObject(hardwareVertexBuffer));
        }

        internal ProcessWrapperObjectDelegate ProcessVertexBufferCallback
        {
            get
            {
                return vertexBuffers.ProcessWrapperCallback;
            }
        }

        internal HardwarePixelBufferSharedPtr getPixelBufferObject(IntPtr hardwarePixelBuffer)
        {
            return new HardwarePixelBufferSharedPtr(pixelBuffers.getObject(hardwarePixelBuffer));
        }

        internal ProcessWrapperObjectDelegate ProcessPixelBufferCallback
        {
            get
            {
                return pixelBuffers.ProcessWrapperCallback;
            }
        }

#region PInvoke

        //HardwareIndexBufferPtr
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr HardwareIndexBufferPtr_createHeapPtr(IntPtr stackSharedPtr);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void HardwareIndexBufferPtr_Delete(IntPtr heapSharedPtr);

        //HardwareVertexBufferPtr
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr HardwareVertexBufferPtr_createHeapPtr(IntPtr stackSharedPtr);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void HardwareVertexBufferPtr_Delete(IntPtr heapSharedPtr);

        //HardwarePixelBufferPtr
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr HardwarePixelBufferPtr_createHeapPtr(IntPtr stackSharedPtr);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void HardwarePixelBufferPtr_Delete(IntPtr heapSharedPtr);

#endregion
    }
}
