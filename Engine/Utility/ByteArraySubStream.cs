using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    unsafe class ByteArraySubStream : Stream
    {
        private byte[] bytes;
        private long begin;
        private long end;
        private long position;

        public ByteArraySubStream(byte[] bytes, long begin, long end)
        {
            this.bytes = bytes;
            this.begin = begin;
            this.end = end;
            this.position = begin;
        }

        public override void Close()
        {
            bytes = null;
            base.Close();
        }

        public override bool CanRead
        {
            get
            {
                return true;
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
                return true;
            }
        }

        public override long Length
        {
            get
            {
                return end - begin;
            }
        }

        public override long Position
        {
            get
            {
                return position - begin;
            }

            set
            {
                position = begin + value;
            }
        }

        public override void Flush()
        {

        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (offset + count > buffer.Length)
            {
                throw new ArgumentException("The sum of offset and count is greater than the buffer length.");
            }

            long readAmount = count;

            long requestedBytesEnd = position + count;
            if (requestedBytesEnd > end)
            {
                readAmount = end - position;
            }

            Buffer.BlockCopy(bytes, (int)position, buffer, offset, (int)readAmount);

            position += readAmount;

            return (int)readAmount;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    position = this.begin + offset;
                    break;
                case SeekOrigin.Current:
                    position += offset;
                    break;
                case SeekOrigin.End:
                    position = end + offset;
                    break;
            }
            return position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("SetLength not supported");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (offset + count > buffer.Length)
            {
                throw new ArgumentException("The sum of offset and count is greater than the buffer length.");
            }

            long writeAmount = count;

            long requestedBytesEnd = position + count;
            if (requestedBytesEnd > end)
            {
                writeAmount = end - position;
            }

            Buffer.BlockCopy(buffer, offset, bytes, (int)position, (int)writeAmount);

            position += writeAmount;
        }
    }
}
