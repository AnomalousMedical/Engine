using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    public unsafe class OgreDataStream : Stream
    {
        private static SharedPtrCollection<OgreDataStream> dataStreamCollection;

#if FULL_AOT_COMPILE
        [Anomalous.Interop.MonoPInvokeCallback(typeof(ProcessWrapperObjectDelegate))]
        private static void processWrapperObject_AOT(IntPtr nativeObject, IntPtr stackSharedPtr)
        {
            dataStreamCollection.processWrapperObject(nativeObject, stackSharedPtr);
        }
#endif

        static OgreDataStream()
        {
            dataStreamCollection = new SharedPtrCollection<OgreDataStream>(createWrapper, DataStreamPtr_createHeapPtr, DataStreamPtr_Delete
#if FULL_AOT_COMPILE
            , processWrapperObject_AOT
#endif
                );
        }

        internal static void DisposeSharedPtrs()
        {
            dataStreamCollection.Dispose();
        }

        private static OgreDataStream createWrapper(IntPtr dataStream)
        {
            return new OgreDataStream(dataStream);
        }

        internal static OgreDataStreamPtr getObject(IntPtr dataStream)
        {
            return new OgreDataStreamPtr(dataStreamCollection.getObject(dataStream));
        }

        internal static ProcessWrapperObjectDelegate ProcessWrapperObjectCallback
        {
            get
            {
                return dataStreamCollection.ProcessWrapperCallback;
            }
        }

        private IntPtr ogreDataStream;

        private OgreDataStream(IntPtr ogreDataStream)
        {
            this.ogreDataStream = ogreDataStream;
        }

        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }

        public override bool CanRead
        {
            get
            {
                return DataStream_isReadable(ogreDataStream);
            }
        }

        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return DataStream_isWriteable(ogreDataStream);
            }
        }

        public override long Length
        {
            get
            {
                return (long)DataStream_size(ogreDataStream).ToUInt64();
            }
        }

        public override long Position
        {
            get
            {
                return (long)DataStream_tell(ogreDataStream).ToUInt64();
            }

            set
            {
                DataStream_seek(ogreDataStream, new UIntPtr((ulong)value));
            }
        }

        public override void Flush()
        {

        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            fixed (void* buf = &buffer[0])
            {
                return (int)DataStream_read(ogreDataStream, buf, new UIntPtr((uint)count)).ToUInt64();
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;
                case SeekOrigin.Current:
                    Position += offset;
                    break;
                case SeekOrigin.End:
                    Position = Length + offset;
                    break;
            }
            return Position;
        }

        public override void SetLength(long value)
        {

        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            fixed (void* buf = &buffer[0])
            {
                DataStream_write(ogreDataStream, buf, new UIntPtr((uint)count));
            }
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool DataStream_isReadable(IntPtr dataStream);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool DataStream_isWriteable(IntPtr dataStream);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern UIntPtr DataStream_tell(IntPtr dataStream);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern UIntPtr DataStream_size(IntPtr dataStream);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern UIntPtr DataStream_read(IntPtr dataStream, void* buf, UIntPtr count);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern UIntPtr DataStream_write(IntPtr dataStream, void* buf, UIntPtr count);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void DataStream_seek(IntPtr dataStream, UIntPtr pos);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr DataStreamPtr_createHeapPtr(IntPtr stackSharedPtr);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void DataStreamPtr_Delete(IntPtr heapSharedPtr);

        #endregion
    }
}
