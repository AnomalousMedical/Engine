using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace OgreWrapper
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
        const int ARRAY_SIZE = 512;

        Stream stream;
	    byte[] internalBuffer = new byte[ARRAY_SIZE];

        ReadDelegate readCallback;
        SkipDelegate skipCallback;
        SeekDelegate seekCallback;
        TellDelegate tellCallback;
        EofDelegate eofCallback;
        CloseDelegate closeCallback;
        DeletedDelegate deletedCallback;

        GCHandle handle;

        IntPtr nativeStream;

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

            readCallback = new ReadDelegate(read);
            skipCallback = new SkipDelegate(skip);
            seekCallback = new SeekDelegate(seek);
            tellCallback = new TellDelegate(tell);
            eofCallback = new EofDelegate(eof);
            closeCallback = new CloseDelegate(close);
            deletedCallback = new DeletedDelegate(deleted);

            nativeStream = OgreManagedStream_Create(name, new IntPtr(stream.Length), readCallback, skipCallback, seekCallback, tellCallback, eofCallback, closeCallback, deletedCallback);

            handle = GCHandle.Alloc(this, GCHandleType.Normal);
        }

        private void deleted()
        {
            close();

            readCallback = null;
            skipCallback = null;
            seekCallback = null;
            tellCallback = null;
            eofCallback = null;
            closeCallback = null;

            handle.Free();
        }

        IntPtr read(void* buf, IntPtr count)
        {
            //Read into the managed array and copy to the unmanaged one
	        int readSize = ARRAY_SIZE;
            long lCount = count.ToInt64();
	        if(count.ToInt32() < ARRAY_SIZE)
	        {
                readSize = count.ToInt32();
	        }
	        byte[] arr = internalBuffer;
            byte* ogreBuf = (byte*)buf;
            int amountRead;
            long i = 0;
            for (; i < lCount; i += amountRead)
	        {
		        amountRead = stream.Read(arr, 0, readSize);
		        if(amountRead != 0)
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
            stream.Close();
        }

#region PInvoke
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr ReadDelegate(void* buf, IntPtr count);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SkipDelegate(IntPtr count);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SeekDelegate(IntPtr pos);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr TellDelegate();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool EofDelegate();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void CloseDelegate();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DeletedDelegate();

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OgreManagedStream_Create(String name, IntPtr size, ReadDelegate read, SkipDelegate skip, SeekDelegate seek, TellDelegate tell, EofDelegate eof, CloseDelegate close, DeletedDelegate deleted);

#endregion
    }
}
