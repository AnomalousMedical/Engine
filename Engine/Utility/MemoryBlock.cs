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
        private IntPtr bytes;
        private long length;

        public MemoryBlock(int length)
        {
            this.length = length;
            bytes = Marshal.AllocHGlobal(length);
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(bytes);
            bytes = IntPtr.Zero;
        }

        public void loadStream(Stream source)
        {
            loadStream(source, 0, (int)source.Length);
        }

        public void loadStream(Stream source, int destOffset, int length)
        {
            if(destOffset + length > this.length)
            {
                throw new IOException(String.Format("Memory block is not large enough to load a stream to position {0} of length {1}", destOffset, source.Length));
            }
            using (UnmanagedMemoryStream stream = new UnmanagedMemoryStream((byte*)bytes.ToPointer() + destOffset, length, length, FileAccess.Write))
            {
                int leftToRead = length;
                byte[] buffer = new byte[8 * 1024];
                int len;
                while ((len = source.Read(buffer, 0, Math.Min(buffer.Length, leftToRead))) > 0)
                {
                    stream.Write(buffer, 0, len);
                    leftToRead -= len;
                }
            }
        }

        public Stream getSubStream(long begin, long end)
        {
            return new UnmanagedMemoryStream((byte*)bytes.ToPointer() + begin, end - begin);
        }

        public long Length
        {
            get
            {
                return length;
            }
        }
    }
}
