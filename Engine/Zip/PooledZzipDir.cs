using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipAccess
{
    class PooledZzipDir : PooledObject, IDisposable
    {
        public PooledZzipDir(String zipFile)
        {
            ZZipError zzipError = ZZipError.ZZIP_NO_ERROR;
            Ptr = ZipFile.ZipFile_OpenDir(zipFile, ref zzipError);

            if (zzipError != ZZipError.ZZIP_NO_ERROR)
            {
                String errorMessage;
                switch (zzipError)
                {
                    case ZZipError.ZZIP_OUTOFMEM:
                        errorMessage = "Out of memory";
                        break;
                    case ZZipError.ZZIP_DIR_OPEN:
                    case ZZipError.ZZIP_DIR_STAT:
                    case ZZipError.ZZIP_DIR_SEEK:
                    case ZZipError.ZZIP_DIR_READ:
                        errorMessage = "Unable to read zip file";
                        break;
                    case ZZipError.ZZIP_UNSUPP_COMPR:
                        errorMessage = "Unsupported compression format";
                        break;
                    case ZZipError.ZZIP_CORRUPTED:
                        errorMessage = "Archive corrupted";
                        break;
                    default:
                        errorMessage = "Unknown ZZIP error number";
                        break;
                };
                throw new ZipIOException("Could not open zip file {0} because of {1}", zipFile, errorMessage);
            }

        }

        public void finished()
        {
            returnToPool();
        }

        public void Dispose()
        {
            if (Ptr != IntPtr.Zero)
            {
                ZipFile.ZipFile_CloseDir(Ptr);
                Ptr = IntPtr.Zero;
            }
        }

        public IntPtr Ptr { get; private set; }

        protected override void reset()
        {
            //Nothing to reset
        }
    }
}
