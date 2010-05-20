using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace ZipAccess
{
    class ZipStream : Stream
    {
        //Seek method constants (stdio.h)
        private const int SEEK_CUR = 1;
        private const int SEEK_END = 2;
        private const int  SEEK_SET = 0;

        IntPtr zzipFile;
        long uncompressedSize;

        internal ZipStream(IntPtr zzipFile, long uncompressedSize)
        {
            this.zzipFile = zzipFile;
            this.uncompressedSize = uncompressedSize;
        }

        public override void Close()
        {
            base.Close();
            if(zzipFile != IntPtr.Zero)
            {
                ZipStream_FileClose(zzipFile);
                zzipFile = IntPtr.Zero;
            }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            
        }

        public override long Length
        {
            get { return uncompressedSize; }
        }

        public override long Position
        {
            get
            {
                return ZipStream_Tell(zzipFile);
            }
            set
            {
                ZipStream_Seek(zzipFile, (Int32)value, SEEK_SET);
            }
        }

        public override unsafe int Read(byte[] buffer, int offset, int count)
        {
            fixed(void* buf = &buffer[offset])
            {
                return ZipStream_FileRead(zzipFile, buf, count);
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            int whence = 0;
	        switch(origin)
	        {
		        case SeekOrigin.Begin:
			        whence = SEEK_SET;
			        break;
		        case SeekOrigin.Current:
			        whence = SEEK_CUR;
			        break;
		        case SeekOrigin.End:
			        whence = SEEK_END;
			        break;
	        }
            return ZipStream_Seek(zzipFile, (Int32)offset, whence);
        }

        public override void SetLength(long value)
        {
            
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            
        }

        [DllImport("Zip")]
        private static extern void ZipStream_FileClose(IntPtr zzipFile);

        [DllImport("Zip")]
        private static extern Int32 ZipStream_Seek(IntPtr zzipFile, Int32 offset, int whence);

        [DllImport("Zip")]
        private static unsafe extern int ZipStream_FileRead(IntPtr zzipFile, void* buf, int count);

        [DllImport("Zip")]
        private static extern Int32 ZipStream_Tell(IntPtr zzipFile);
    }
}
