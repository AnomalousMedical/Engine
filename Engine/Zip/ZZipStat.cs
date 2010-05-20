using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ZipAccess
{
    [StructLayout(LayoutKind.Sequential)]
    struct ZZipStat
    {
        int d_compr;	/* compression method */
        int d_csize;        /* compressed size */
        int st_size;	/* file size / decompressed size */
        IntPtr d_name;		/* file name / strdupped name */

        public int CompressedSize
        {
            get
            {
                return d_csize;
            }
        }

        public int UncompressedSize
        {
            get
            {
                return st_size;
            }
        }

        public int CompressionMethod
        {
            get
            {
                return d_compr;
            }
        }

        public String Name
        {
            get
            {
                return Marshal.PtrToStringAnsi(d_name);
            }
        }
    }
}
