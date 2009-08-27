using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Resources
{
    public interface ArchiveFileInfo
    {
        Int64 CompressedSize { get; }

        Int64 UncompressedSize { get; }

        String Name { get; }

        String DirectoryName { get; }

        String FullName { get; }
    }
}
