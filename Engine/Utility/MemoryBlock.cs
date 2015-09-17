using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZipAccess;

namespace Engine
{
    /// <summary>
    /// A block of memory that can be used with multiple streams at once. This is
    /// basically a giant byte array and a way to get streams that point into the
    /// array. It is thread safe to read from multiple streams at once (not one stream
    /// in many threads).
    /// </summary>
    public unsafe class MemoryBlock : IDisposable
    {
        private byte* bytes;
        private long length;

        public MemoryBlock(Stream source)
        {
            length = source.Length;
            bytes = MemoryBlock_AllocateBuffer((int)length);
            using (UnmanagedMemoryStream stream = new UnmanagedMemoryStream(bytes, length, length, FileAccess.Write))
            {
                source.CopyTo(stream);
            }
        }

        public void Dispose()
        {
            MemoryBlock_DellocateBuffer(bytes);
            bytes = null;
        }

        public Stream getSubStream(long begin, long end)
        {
            return new UnmanagedMemoryStream(bytes + begin, end - begin);
        }

        public long Length
        {
            get
            {
                return length;
            }
        }

        #region PInvoke

        [DllImport(ZipLibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern byte* MemoryBlock_AllocateBuffer(int length);

        [DllImport(ZipLibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern void MemoryBlock_DellocateBuffer(byte* buffer);

        #endregion
    }
}
