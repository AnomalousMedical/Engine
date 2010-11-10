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
        private GCHandle handle;

        private ReadDelegate readCB;
        private SeekDelegate seekCB;
        private CloseDelegate closeCB;
        private TellDelegate tellCB;
        private EofDelegate eofCB;
        private DeleteDelegate deleteCB;

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

            readCB = new ReadDelegate(read);
            seekCB = new SeekDelegate(seek);
            closeCB = new CloseDelegate(close);
            tellCB = new TellDelegate(tell);
            eofCB = new EofDelegate(eof);
            deleteCB = new DeleteDelegate(deleted);

            managedStream = ManagedStream_create(readCB, seekCB, closeCB, tellCB, eofCB, deleteCB);

            handle = GCHandle.Alloc(this, GCHandleType.Normal);
        }

        private void deleted()
        {
            close();

            readCB = null;
            seekCB = null;
            closeCB = null;
            tellCB = null;
            eofCB = null;
            deleteCB = null;

            handle.Free();
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

        private int seek(IntPtr offset, int origin)
        {
            SeekOrigin seekOrigin = (SeekOrigin)origin; //This maps directly to the seek given 0 = begin, 1 = current, 2 = end.
            return (int)stream.Seek(offset.ToInt64(), seekOrigin);
        }

        private void close()
        {
            stream.Close();
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
        private delegate UIntPtr ReadDelegate(void* buffer, int size, int count);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SeekDelegate(IntPtr offset, int origin);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void CloseDelegate();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate UIntPtr TellDelegate();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool EofDelegate();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DeleteDelegate();

        [DllImport("SoundWrapper")]
        private static extern IntPtr ManagedStream_create(ReadDelegate readCB, SeekDelegate seekCB, CloseDelegate closeCB, TellDelegate tellCB, EofDelegate eofCB, DeleteDelegate deleteCB);

        [DllImport("SoundWrapper")]
        private static extern void ManagedStream_destroy(IntPtr managedStream);

        #endregion
    }
}
