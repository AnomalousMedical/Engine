using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class VirtualFileInfo
    {
        internal VirtualFileInfo(String name, String directoryName, String fullName, String realLocation, Int64 compressedSize, Int64 uncompressedSize)
        {
            Name = name;
            DirectoryName = directoryName;
            FullName = fullName;
            RealLocation = realLocation;
            CompressedSize = compressedSize;
            UncompressedSize = uncompressedSize;
        }

        public Int64 CompressedSize { get; private set; }

        public Int64 UncompressedSize { get; private set; }

        public String Name { get; private set; }

        public String DirectoryName { get; private set; }

        public String FullName { get; private set; }

        public String RealLocation { get; private set; }
    }
}
