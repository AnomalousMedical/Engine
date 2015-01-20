using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace ZipAccess
{
    public class ZipStream : Stream
    {
        //Seek method constants (stdio.h)
        private const int SEEK_CUR = 1;
        private const int SEEK_END = 2;
        private const int  SEEK_SET = 0;

        PooledZzipDir zzipDir;
        IntPtr zzipFile;
        long uncompressedSize;

        /// <summary>
        /// Open an new ZipStream. Each ZipStream gets its own zzipDir to allow correct multithreaded reading of files.
        /// These handles come out of a pool managed by zipFile so tons of extras are not created.
        /// </summary>
        /// <remarks>
        /// This is based on:
        /// https://bitbucket.org/sinbad/ogre/pull-request/63/made-each-zip-file-stream-have-its-own-zip/diff
        /// http://zziplib.sourceforge.net/changes.html under 2005-12-11, the zip file is opened and read with only one filehandle for
        /// multiple threads that share the same "dir" handle. Look for ->currentfp. To get around this we will just open a new zzipDir
        /// for each file. This uses the extern functions in ZipFile to do this, they are internal.
        /// </remarks>
        /// <param name="zipFile">The name of the zip file to load.</param>
        /// <param name="fileName">The name of the file to load.</param>
        internal unsafe ZipStream(PooledZzipDir zzipDir, String fileName)
        {
            this.zzipDir = zzipDir;

            //Get uncompressed size
            ZZipStat zstat = new ZZipStat();
            ZipFile.ZipFile_DirStat(zzipDir.Ptr, fileName, &zstat, ZipFile.ZZIP_CASEINSENSITIVE);
            uncompressedSize = zstat.UncompressedSize;

            //Open file
            zzipFile = ZipFile.ZipFile_OpenFile(zzipDir.Ptr, fileName, ZZipFlags.ZZIP_ONLYZIP | ZZipFlags.ZZIP_CASELESS);
            if (zzipFile == IntPtr.Zero)
            {
                //Be sure to return the directory handle
                zzipDir.finished();
                throw new ZipIOException("Cannot open file");
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing && zzipFile != IntPtr.Zero)
            {
                ZipStream_FileClose(zzipFile);
                zzipFile = IntPtr.Zero;
            }
            if (zzipDir != null)
            {
                zzipDir.finished();
                zzipDir = null;
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

        [DllImport(ZipLibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ZipStream_FileClose(IntPtr zzipFile);

        [DllImport(ZipLibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern Int32 ZipStream_Seek(IntPtr zzipFile, Int32 offset, int whence);

        [DllImport(ZipLibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static unsafe extern int ZipStream_FileRead(IntPtr zzipFile, void* buf, int count);

        [DllImport(ZipLibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern Int32 ZipStream_Tell(IntPtr zzipFile);
    }
}
