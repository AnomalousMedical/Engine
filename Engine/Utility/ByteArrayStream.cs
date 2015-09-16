using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class ByteArrayStream : Stream
    {
        private byte[] bytes;
        private long position = 0;

        public ByteArrayStream(long length)
        {
            bytes = new byte[length];
        }

        public ByteArrayStream(Stream source)
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

        public Stream getSubStream(long begin, long end)
        {
            return new ByteArraySubStream(bytes, begin, end);
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
                return bytes.Length;
            }
        }

        public override long Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
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
            if(requestedBytesEnd > bytes.Length)
            {
                readAmount = bytes.Length - position;
            }

            Buffer.BlockCopy(bytes, (int)position, buffer, offset, (int)readAmount);

            position += readAmount;

            return (int)readAmount;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch(origin)
            {
                case SeekOrigin.Begin:
                    position = offset;
                    break;
                case SeekOrigin.Current:
                    position += offset;
                    break;
                case SeekOrigin.End:
                    position = bytes.Length + offset;
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
            if (requestedBytesEnd > bytes.Length)
            {
                writeAmount = bytes.Length - position;
            }

            Buffer.BlockCopy(buffer, offset, bytes, (int)position, (int)writeAmount);

            position += writeAmount;
        }
    }
}
