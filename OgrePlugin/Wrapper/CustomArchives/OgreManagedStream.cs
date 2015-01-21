using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    /// <summary>
    /// This class exposes a managed stream through PInvoke to Ogre. This class
    /// is tricky in how it is allocated. It will be created by the managed
    /// archives, which will allocate a native version of this class that is
    /// handed off to ogre by the archive. When ogre is done it will delete the
    /// class and this will call back into the OgreManagedStream through its
    /// delete function. Overall the class can be used as expected, but this is
    /// strange behavior to get what we want.
    /// </summary>
    unsafe class OgreManagedStream
    {
        enum AccessMode
        {
            READ = 1,
            WRITE = 2
        };

        const int ARRAY_SIZE = 4096;

        Stream stream;
	    byte[] internalBuffer = new byte[ARRAY_SIZE];

        IntPtr nativeStream;
        private CallbackHandler callbackHandler;

        public IntPtr NativeStream
        {
            get
            {
                return nativeStream;
            }
        }

        public OgreManagedStream(String name, Stream stream)
        {
            this.stream = stream;

            callbackHandler = new CallbackHandler();

            AccessMode accessMode = 0;
            if (stream.CanRead)
            {
                accessMode |= AccessMode.READ;
            }
            if (stream.CanWrite)
            {
                accessMode |= AccessMode.WRITE;
            }

            nativeStream = callbackHandler.create(this, name, new IntPtr(stream.Length), accessMode);
        }

        private void deleted()
        {
            close();
            callbackHandler.Dispose();
        }

        IntPtr read(void* buf, IntPtr count)
        {
            //Read into the managed array and copy to the unmanaged one
	        int readSize = ARRAY_SIZE;
            long lCount = count.ToInt64();
	        if(lCount < ARRAY_SIZE)
	        {
                readSize = count.ToInt32();
	        }
	        byte[] arr = internalBuffer;
            byte* ogreBuf = (byte*)buf;
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
		        if(amountRead != 0)
		        {
                    Marshal.Copy(arr, 0, new IntPtr(&ogreBuf[i]), amountRead);
		        }
		        else
		        {
			        break;
		        }
	        }

	        return new IntPtr(i);
        }

        IntPtr write(void* buf, IntPtr count)
        {
            //Read into the managed array and copy to the unmanaged one
            int writeSize = ARRAY_SIZE;
            long lCount = count.ToInt64();
            if (lCount < ARRAY_SIZE)
            {
                writeSize = count.ToInt32();
            }
            byte[] arr = internalBuffer;
            byte* ogreBuf = (byte*)buf;
            long i = 0;
            for (; i < lCount; i += writeSize)
            {
                //Make sure we are not writing past the end of where we are suppost to.
                if (lCount - i < writeSize)
                {
                    writeSize = (int)(lCount - i);
                }
                if (writeSize != 0)
                {
                    Marshal.Copy(new IntPtr(&ogreBuf[i]), arr, 0, writeSize);
                    stream.Write(arr, 0, writeSize);
                }
                else
                {
                    break;
                }
            }

            return new IntPtr(i);
        }

        private void skip(IntPtr count)
        {
            stream.Seek(count.ToInt64(), SeekOrigin.Current);
        }

        private void seek(IntPtr pos)
        {
            stream.Seek(pos.ToInt64(), SeekOrigin.Begin);
        }

        private IntPtr tell()
        {
            return new IntPtr(stream.Position);
        }

        private bool eof()
        {
            return stream.Length == stream.Position;
        }

        private void close()
        {
            stream.Dispose();
        }

#region PInvoke
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr ReadDelegate(void* buf, IntPtr count
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr WriteDelegate(void* buf, IntPtr count
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SkipDelegate(IntPtr count
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SeekDelegate(IntPtr pos
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr TellDelegate(
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
        private delegate void CloseDelegate(
#if FULL_AOT_COMPILE
        IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DeletedDelegate(
#if FULL_AOT_COMPILE
        IntPtr instanceHandle
#endif
);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OgreManagedStream_Create(String name, IntPtr size, AccessMode accessMode, ReadDelegate read, WriteDelegate write, SkipDelegate skip, SeekDelegate seek, TellDelegate tell, EofDelegate eof, CloseDelegate close, DeletedDelegate deleted
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static ReadDelegate readCallback;
            private static WriteDelegate writeCallback;
            private static SkipDelegate skipCallback;
            private static SeekDelegate seekCallback;
            private static TellDelegate tellCallback;
            private static EofDelegate eofCallback;
            private static CloseDelegate closeCallback;
            private static DeletedDelegate deletedCallback;

            static CallbackHandler()
            {
                readCallback = new ReadDelegate(read);
                writeCallback = new WriteDelegate(write);
                skipCallback = new SkipDelegate(skip);
                seekCallback = new SeekDelegate(seek);
                tellCallback = new TellDelegate(tell);
                eofCallback = new EofDelegate(eof);
                closeCallback = new CloseDelegate(close);
                deletedCallback = new DeletedDelegate(deleted);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(ReadDelegate))]
            private static IntPtr read(void* buf, IntPtr count, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as OgreManagedStream).read(buf, count);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(WriteDelegate))]
            private static IntPtr write(void* buf, IntPtr count, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as OgreManagedStream).write(buf, count);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(SkipDelegate))]
            private static void skip(IntPtr count, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as OgreManagedStream).skip(count);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(SeekDelegate))]
            private static void seek(IntPtr pos, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as OgreManagedStream).seek(pos);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(TellDelegate))]
            private static IntPtr tell(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as OgreManagedStream).tell();
            }
            
            [MonoTouch.MonoPInvokeCallback(typeof(EofDelegate))]
            private static bool eof(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as OgreManagedStream).eof();
            }

            [MonoTouch.MonoPInvokeCallback(typeof(CloseDelegate))]
            private static void close(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as OgreManagedStream).close();
            }

            [MonoTouch.MonoPInvokeCallback(typeof(DeletedDelegate))]
            private static void deleted(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as OgreManagedStream).deleted();
            }

            GCHandle handle; //This class keeps itself alive with a gchandle, so we put both of them in the CallbackHandler. This is needed for the class to not get collected.

            public IntPtr create(OgreManagedStream obj, String name, IntPtr size, AccessMode accessMode)
            {
                handle = GCHandle.Alloc(this, GCHandleType.Normal);
                return OgreManagedStream_Create(name, size, accessMode, readCallback, writeCallback, skipCallback, seekCallback, tellCallback, eofCallback, closeCallback, deletedCallback, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            ReadDelegate readCallback;
            WriteDelegate writeCallback;
            SkipDelegate skipCallback;
            SeekDelegate seekCallback;
            TellDelegate tellCallback;
            EofDelegate eofCallback;
            CloseDelegate closeCallback;
            DeletedDelegate deletedCallback;

            GCHandle handle; //This class keeps itself alive with a gchandle, so we put both of them in the CallbackHandler. This is needed for the class to not get collected.

            public IntPtr create(OgreManagedStream obj, String name, IntPtr size, AccessMode accessMode)
            {
                handle = GCHandle.Alloc(this, GCHandleType.Normal);

                readCallback = new ReadDelegate(obj.read);
                writeCallback = new WriteDelegate(obj.write);
                skipCallback = new SkipDelegate(obj.skip);
                seekCallback = new SeekDelegate(obj.seek);
                tellCallback = new TellDelegate(obj.tell);
                eofCallback = new EofDelegate(obj.eof);
                closeCallback = new CloseDelegate(obj.close);
                deletedCallback = new DeletedDelegate(obj.deleted);

                return OgreManagedStream_Create(name, size, accessMode, readCallback, writeCallback, skipCallback, seekCallback, tellCallback, eofCallback, closeCallback, deletedCallback);
            }

            public void Dispose()
            {
                handle.Free();

                readCallback = null;
                skipCallback = null;
                seekCallback = null;
                tellCallback = null;
                eofCallback = null;
                closeCallback = null;
            }
        }
#endif

#endregion
    }
}
