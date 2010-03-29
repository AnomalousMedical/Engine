using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Engine.Resources
{
    public enum FileMode
    {
        // Summary:
        //     Specifies that the operating system should create a new file. This requires
        //     System.Security.Permissions.FileIOPermissionAccess.Write. If the file already
        //     exists, an System.IO.IOException is thrown.
        CreateNew = System.IO.FileMode.CreateNew,
        //
        // Summary:
        //     Specifies that the operating system should create a new file. If the file
        //     already exists, it will be overwritten. This requires System.Security.Permissions.FileIOPermissionAccess.Write.
        //     System.IO.FileMode.Create is equivalent to requesting that if the file does
        //     not exist, use System.IO.FileMode.CreateNew; otherwise, use System.IO.FileMode.Truncate.
        Create = System.IO.FileMode.Create,
        //
        // Summary:
        //     Specifies that the operating system should open an existing file. The ability
        //     to open the file is dependent on the value specified by System.IO.FileAccess.
        //     A System.IO.FileNotFoundException is thrown if the file does not exist.
        Open = System.IO.FileMode.Open,
        //
        // Summary:
        //     Specifies that the operating system should open a file if it exists; otherwise,
        //     a new file should be created. If the file is opened with FileAccess.Read,
        //     System.Security.Permissions.FileIOPermissionAccess.Read is required. If the
        //     file access is FileAccess.Write then System.Security.Permissions.FileIOPermissionAccess.Write
        //     is required. If the file is opened with FileAccess.ReadWrite, both System.Security.Permissions.FileIOPermissionAccess.Read
        //     and System.Security.Permissions.FileIOPermissionAccess.Write are required.
        //     If the file access is FileAccess.Append, then System.Security.Permissions.FileIOPermissionAccess.Append
        //     is required.
        OpenOrCreate = System.IO.FileMode.OpenOrCreate,
        //
        // Summary:
        //     Specifies that the operating system should open an existing file. Once opened,
        //     the file should be truncated so that its size is zero bytes. This requires
        //     System.Security.Permissions.FileIOPermissionAccess.Write. Attempts to read
        //     from a file opened with Truncate cause an exception.
        Truncate = System.IO.FileMode.Truncate,
        //
        // Summary:
        //     Opens the file if it exists and seeks to the end of the file, or creates
        //     a new file. FileMode.Append can only be used in conjunction with FileAccess.Write.
        //     Attempting to seek to a position before the end of the file will throw an
        //     System.IO.IOException and any attempt to read fails and throws an System.NotSupportedException.
        Append = System.IO.FileMode.Append,
    }

    public enum FileAccess
    {
        // Summary:
        //     Read access to the file. Data can be read from the file. Combine with Write
        //     for read/write access.
        Read = System.IO.FileAccess.Read,
        //
        // Summary:
        //     Write access to the file. Data can be written to the file. Combine with Read
        //     for read/write access.
        Write = System.IO.FileAccess.Write,
        //
        // Summary:
        //     Read and write access to the file. Data can be written to and read from the
        //     file.
        ReadWrite = System.IO.FileAccess.ReadWrite,
    }

    abstract class Archive : IDisposable
    {
        public abstract void Dispose();

        public abstract String[] listFiles(bool recursive);

        public abstract String[] listFiles(String url, bool recursive);

        public abstract String[] listFiles(String url, String searchPattern, bool recursive);

        public abstract String[] listDirectories(bool recursive);

        public abstract String[] listDirectories(String url, bool recursive);

        public abstract String[] listDirectories(String url, bool recursive, bool includeHidden);

        public abstract String[] listDirectories(String url, String searchPattern, bool recursive);

        public abstract String[] listDirectories(String url, String searchPattern, bool recursive, bool includeHidden);

        public abstract Stream openStream(String url, FileMode mode);

        public abstract Stream openStream(String url, FileMode mode, FileAccess access);

        public abstract bool isDirectory(String url);

        public abstract bool exists(String filename);

        public abstract VirtualFileInfo getFileInfo(String filename);
    }
}
