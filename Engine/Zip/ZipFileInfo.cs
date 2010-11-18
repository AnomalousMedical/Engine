using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZipAccess
{
    public class ZipFileInfo
    {
        public ZipFileInfo(String fullName, long compressedSize, long uncompressedSize)
        {
            FullName = fullName;
            CompressedSize = compressedSize;
            UncompressedSize = uncompressedSize;
            String path = fullName;
            // Replace \ with / first
            path.Replace('\\', '/');

	        //If we end with a / then this is a directory
	        if(path.EndsWith("/"))
	        {
		        IsDirectory = true;
		        path = path.Substring(0, path.Length - 1);
		        compressedSize = -1;
		        uncompressedSize = -1;
	        }

            // split based on final /
	        int i = path.LastIndexOf('/');

	        if (i == -1)
            {
		        DirectoryName = String.Empty;
		        Name = fullName;
            }
            else
            {
		        Name = path.Substring(i+1, path.Length - i - 1);
                DirectoryName = path.Substring(0, i+1);
            }
        }

        public String Name { get; private set; }

        public String DirectoryName { get; private set; }

        public String FullName { get; private set; }

        public long CompressedSize { get; private set; }

        public long UncompressedSize { get; private set; }

        public bool IsDirectory { get; private set; }
    }
}
