using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace libRocketPlugin
{
    /// <summary>
    /// A version of the Rocket FileInterface that uses managed streams.
    /// Subclasses need only open the relevant stream.
    /// </summary>
    public abstract unsafe class ManagedFileInterface : FileInterface
    {
        //This is majorly assuming that the file access is only in 1 thread
        const int ARRAY_SIZE = 4096;
        byte[] internalBuffer = new byte[ARRAY_SIZE];

        private const int SEEK_CUR = 1;
        private const int SEEK_END = 2;
        private const int SEEK_SET = 0;

        private CallbackHandler callbackHandler;

        public ManagedFileInterface()
        {
            callbackHandler = new CallbackHandler();
            setPtr(callbackHandler.create(this));
        }

        public override void Dispose()
        {
            ManagedFileInterface_Delete(ptr);
            callbackHandler.Dispose();
        }

        private IntPtr OpenCbImpl(String path)
        {
            Stream stream = Open(path);
            if (stream != null)
            {
                GCHandle handle = GCHandle.Alloc(stream, GCHandleType.Normal);
                return GCHandle.ToIntPtr(handle);
            }
            return IntPtr.Zero;
        }

        private void CloseCbImpl(IntPtr file)
        {
            GCHandle handle = GCHandle.FromIntPtr(file);
            Stream stream = (Stream)handle.Target;
            stream.Dispose();
            handle.Free();
        }

        private IntPtr ReadCbImpl(void* buffer, IntPtr size, IntPtr file)
        {
            Stream stream = getStream(file);

            //Read into the managed array and copy to the unmanaged one
            int readSize = ARRAY_SIZE;
            long lCount = size.ToInt64();
            if (size.ToInt32() < ARRAY_SIZE)
            {
                readSize = size.ToInt32();
            }
            byte[] arr = internalBuffer;
            byte* ogreBuf = (byte*)buffer;
            int amountRead;
            long i = 0;
            for (; i < lCount; i += amountRead)
            {
                //Make sure we are not reading past the end of where we are suppost to.
                if (lCount - i < readSize)
                {
                    readSize = (int)(lCount - i);
                }
                amountRead = stream.Read(arr, 0, readSize);
                if (amountRead != 0)
                {
                    //memcpy(&ogreBuf[i], mBuf, amountRead);
                    Marshal.Copy(arr, 0, new IntPtr(&ogreBuf[i]), amountRead);
                }
                else
                {
                    break;
                }
            }

            return new IntPtr(i);
        }

        private bool SeekCbImpl(IntPtr file, Int32 offset, int origin)
        {
            try
            {
                Stream stream = getStream(file);

                switch (origin)
                {
                    case SEEK_SET:
                        stream.Seek(offset, SeekOrigin.Begin);
                        break;
                    case SEEK_CUR:
                        stream.Seek(offset, SeekOrigin.Current);
                        break;
                    case SEEK_END:
                        stream.Seek(offset, SeekOrigin.End);
                        break;
                    default:
                        return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private IntPtr TellCbImpl(IntPtr file)
        {
            Stream stream = getStream(file);
            return new IntPtr(stream.Position);
        }

        private void ReleaseCbImpl()
        {
            Dispose();
        }

        private static Stream getStream(IntPtr file)
        {
            GCHandle handle = GCHandle.FromIntPtr(file);
            return (Stream)handle.Target;
        }

        #region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr OpenCb(String path
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	    delegate void CloseCb(IntPtr file
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	    delegate IntPtr ReadCb(void* buffer, IntPtr size, IntPtr file
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        delegate bool SeekCb(IntPtr file, Int32 offset, int origin
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	    delegate IntPtr TellCb(IntPtr file
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	    delegate void ReleaseCb(
#if FULL_AOT_COMPILE
        IntPtr instanceHandle
#endif
);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ManagedFileInterface_Create(OpenCb open, CloseCb close, ReadCb read, SeekCb seek, TellCb tell, ReleaseCb release
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedFileInterface_Delete(IntPtr fileInterface);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static OpenCb open;
            private static CloseCb close;
            private static ReadCb read;
            private static SeekCb seek;
            private static TellCb tell;
            private static ReleaseCb release;

            static CallbackHandler()
            {
                open = new OpenCb(OpenCbImpl);
                close = new CloseCb(CloseCbImpl);
                read = new ReadCb(ReadCbImpl);
                seek = new SeekCb(SeekCbImpl);
                tell = new TellCb(TellCbImpl);
                release = new ReleaseCb(ReleaseCbImpl);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(OpenCb))]
            private static IntPtr OpenCbImpl(string path, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as ManagedFileInterface).OpenCbImpl(path);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(CloseCb))]
            private static void CloseCbImpl(IntPtr file, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as ManagedFileInterface).CloseCbImpl(file);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(ReadCb))]
            private static IntPtr ReadCbImpl(void* buffer, IntPtr size, IntPtr file, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as ManagedFileInterface).ReadCbImpl(buffer, size, file);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(SeekCb))]
            private static bool SeekCbImpl(IntPtr file, int offset, int origin, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as ManagedFileInterface).SeekCbImpl(file, offset, origin);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(TellCb))]
            private static IntPtr TellCbImpl(IntPtr file, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as ManagedFileInterface).TellCbImpl(file);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(ReleaseCb))]
            private static void ReleaseCbImpl(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as ManagedFileInterface).ReleaseCbImpl();
            }

            private GCHandle handle;

            public IntPtr create(ManagedFileInterface obj)
            {
                handle = GCHandle.Alloc(obj);
                return ManagedFileInterface_Create(open, close, read, seek, tell, release, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            private OpenCb open;
            private CloseCb close;
            private ReadCb read;
            private SeekCb seek;
            private TellCb tell;
            private ReleaseCb release;

            public IntPtr create(ManagedFileInterface obj)
            {
                open = new OpenCb(obj.OpenCbImpl);
                close = new CloseCb(obj.CloseCbImpl);
                read = new ReadCb(obj.ReadCbImpl);
                seek = new SeekCb(obj.SeekCbImpl);
                tell = new TellCb(obj.TellCbImpl);
                release = new ReleaseCb(obj.ReleaseCbImpl);

                return ManagedFileInterface_Create(open, close, read, seek, tell, release);
            }

            public void Dispose()
            {

            }
        }
#endif

        #endregion
    }
}
