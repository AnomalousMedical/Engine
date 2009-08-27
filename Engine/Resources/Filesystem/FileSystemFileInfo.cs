using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Engine.Resources
{
    class FileSystemFileInfo : ArchiveFileInfo
    {
        private FileInfo fileInfo;

        public FileSystemFileInfo(String filename)
        {
            this.fileInfo = new FileInfo(filename);
        }

        public long CompressedSize
        {
            get 
            {
                return fileInfo.Length;
            }
        }

        public long UncompressedSize
        {
            get
            {
                return fileInfo.Length;
            }
        }

        public string Name
        {
            get
            {
                return fileInfo.Name;
            }
        }

        public string DirectoryName
        {
            get
            {
                return fileInfo.DirectoryName;
            }
        }

        public string FullName
        {
            get
            {
                return fileInfo.FullName;
            }
        }
    }
}
