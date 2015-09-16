using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// A block of memory that can be used with multiple streams at once. This is
    /// basically a giant byte array and a way to get streams that point into the
    /// array. It is thread safe to read from multiple streams at once (not one stream
    /// in many threads).
    /// </summary>
    public class MemoryBlock : IDisposable
    {
        private byte[] bytes;

        public MemoryBlock(Stream source)
        {
            bytes = new byte[source.Length];
            int read;
            long readAmount;
            do
            {
                readAmount = 16384;
                if (source.Position + readAmount > source.Length)
                {
                    readAmount = source.Length - source.Position;
                }
            }
            while ((read = source.Read(bytes, (int)source.Position, (int)readAmount)) > 0);
        }

        public void Dispose()
        {
            bytes = null;
        }

        public Stream getSubStream(long begin, long end)
        {
            return new MemoryBlockSubStream(bytes, begin, end);
        }

        public long Length
        {
            get
            {
                return bytes.Length;
            }
        }
    }
}
