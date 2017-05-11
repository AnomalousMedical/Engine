using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace SoundPlugin
{
    unsafe class ManagedStream
    {
        const int ARRAY_SIZE = 512;
        byte[] internalBuffer = new byte[ARRAY_SIZE];

        private Stream stream;
        private IntPtr managedStream;
        private CallbackHandler callbackHandler;

        internal IntPtr Pointer
        {
            get
            {
                return managedStream;
            }
        }

        public ManagedStream(Stream stream)
        {
            this.stream = stream;
            callbackHandler = new CallbackHandler();
            managedStream = callbackHandler.create(this);
        }

        private void deleted()
        {
            close();
            callbackHandler.Dispose();
        }

        private UIntPtr read(void* buffer, int size, int count)
        {
            //Read into the managed array and copy to the unmanaged one
            int readSize = ARRAY_SIZE;
            ulong lCount = (uint)count;
            if (count < ARRAY_SIZE)
            {
                readSize = count;
            }
            byte[] arr = internalBuffer;
            byte* nativeBuf = (byte*)buffer;
            int amountRead;
            ulong i = 0;
            for (; i < lCount; i += (uint)amountRead)
            {
                amountRead = stream.Read(arr, 0, readSize);
                if (amountRead != 0)
                {
                    //memcpy(&ogreBuf[i], mBuf, amountRead);
                    Marshal.Copy(arr, 0, new IntPtr(&nativeBuf[i]), amountRead);
                }
                else
                {
                    break;
                }
            }

            return new UIntPtr(i);
        }

        private UIntPtr write(void* buffer, int size, int count)
        {
            byte* nativeBuf = (byte*)buffer;
            int length = size * count;
            byte[] bytes = new byte[length];
            Marshal.Copy(new IntPtr(&nativeBuf[0]), bytes, 0, length);
            stream.Write(bytes, 0, length);
            return new UIntPtr((uint)count);
        }

        private int seek(IntPtr offset, int origin)
        {
            SeekOrigin seekOrigin = (SeekOrigin)origin; //This maps directly to the seek given 0 = begin, 1 = current, 2 = end.
            return (int)stream.Seek(offset.ToInt64(), seekOrigin);
        }

        private void close()
        {
            stream.Dispose();
        }

        private UIntPtr tell()
        {
            return new UIntPtr((ulong)stream.Position);
        }

        private bool eof()
        {
            return stream.Length == stream.Position;
        }

        #region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate UIntPtr ReadDelegate(void* buffer, int size, int count
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SeekDelegate(IntPtr offset, int origin
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void CloseDelegate(
#if FULL_AOT_COMPILE
IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate UIntPtr TellDelegate(
#if FULL_AOT_COMPILE
IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool EofDelegate(
#if FULL_AOT_COMPILE
IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DeleteDelegate(
#if FULL_AOT_COMPILE
IntPtr instanceHandle
#endif
);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ManagedStream_create(ReadDelegate readCB, ReadDelegate writeCB, SeekDelegate seekCB, CloseDelegate closeCB, TellDelegate tellCB, EofDelegate eofCB, DeleteDelegate deleteCB
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedStream_destroy(IntPtr managedStream);

#if FULL_AOT_COMPILE
        /// <summary>
        /// Callback Hanlder, both versions use a GCHandle because this class needs one anyway.
        /// </summary>
        class CallbackHandler : IDisposable
        {
            private static ReadDelegate readCB;
            private static ReadDelegate writeCB;
            private static SeekDelegate seekCB;
            private static CloseDelegate closeCB;
            private static TellDelegate tellCB;
            private static EofDelegate eofCB;
            private static DeleteDelegate deleteCB;

            static CallbackHandler()
            {
                readCB = new ReadDelegate(read);
                writeCB = new ReadDelegate(write);
                seekCB = new SeekDelegate(seek);
                closeCB = new CloseDelegate(close);
                tellCB = new TellDelegate(tell);
                eofCB = new EofDelegate(eof);
                deleteCB = new DeleteDelegate(deleted);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(ReadDelegate))]
            private static UIntPtr read(void* buffer, int size, int count, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as ManagedStream).read(buffer, size, count);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(ReadDelegate))]
            private static UIntPtr write(void* buffer, int size, int count, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as ManagedStream).write(buffer, size, count);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(SeekDelegate))]
            private static int seek(IntPtr offset, int origin, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as ManagedStream).seek(offset, origin);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(CloseDelegate))]
            private static void close(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as ManagedStream).close();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(TellDelegate))]
            private static UIntPtr tell(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as ManagedStream).tell();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(EofDelegate))]
            private static bool eof(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as ManagedStream).eof();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(DeleteDelegate))]
            private static void deleted(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as ManagedStream).deleted();
            }

            private GCHandle handle;

            public IntPtr create(ManagedStream obj)
            {
                handle = GCHandle.Alloc(obj, GCHandleType.Normal);
                return ManagedStream_create(readCB, writeCB, seekCB, closeCB, tellCB, eofCB, deleteCB, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        /// <summary>
        /// Callback Hanlder, both versions use a GCHandle because this class needs one anyway.
        /// </summary>
        class CallbackHandler : IDisposable
        {
            private ReadDelegate readCB;
            private ReadDelegate writeCB;
            private SeekDelegate seekCB;
            private CloseDelegate closeCB;
            private TellDelegate tellCB;
            private EofDelegate eofCB;
            private DeleteDelegate deleteCB;

            private GCHandle handle;

            public IntPtr create(ManagedStream obj)
            {
                readCB = new ReadDelegate(obj.read);
                writeCB = new ReadDelegate(obj.write);
                seekCB = new SeekDelegate(obj.seek);
                closeCB = new CloseDelegate(obj.close);
                tellCB = new TellDelegate(obj.tell);
                eofCB = new EofDelegate(obj.eof);
                deleteCB = new DeleteDelegate(obj.deleted);

                handle = GCHandle.Alloc(obj, GCHandleType.Normal);

                return ManagedStream_create(readCB, writeCB, seekCB, closeCB, tellCB, eofCB, deleteCB);
            }

            public void Dispose()
            {
                readCB = null;
                writeCB = null;
                seekCB = null;
                closeCB = null;
                tellCB = null;
                eofCB = null;
                deleteCB = null;

                handle.Free();
            }
        }
#endif

        #endregion
    }
}
