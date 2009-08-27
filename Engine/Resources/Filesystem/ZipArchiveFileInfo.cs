using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZipAccess;

namespace Engine.Resources
{
    class ZipArchiveFileInfo : ArchiveFileInfo
    {
        private ZipFileInfo fileInfo;

        public ZipArchiveFileInfo(ZipFileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        public long CompressedSize
        {
            get { return fileInfo.CompressedSize; }
        }

        public long UncompressedSize
        {
            get { return fileInfo.UncompressedSize; }
        }

        public string Name
        {
            get { return fileInfo.Name; }
        }

        public string DirectoryName
        {
            get { return fileInfo.DirectoryName; }
        }

        public string FullName
        {
            get { return fileInfo.FullName; }
        }
    }
}
