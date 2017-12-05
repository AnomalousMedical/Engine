using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Engine.Utility
{
    public class NoCloseStream : Stream
    {
        private Stream wrapped;

        public NoCloseStream(Stream wrapped)
        {
            this.wrapped = wrapped;
        }

        protected override void Dispose(bool disposing)
        {
            
        }

        public override void Close()
        {
            
        }

        public override bool CanRead => wrapped.CanRead;

        public override bool CanSeek => wrapped.CanSeek;

        public override bool CanWrite => wrapped.CanWrite;

        public override long Length => wrapped.Length;

        public override long Position { get => wrapped.Position; set => wrapped.Position = value; }

        public override void Flush()
        {
            wrapped.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return wrapped.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return wrapped.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            wrapped.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            wrapped.Write(buffer, offset, count);
        }
    }
}
