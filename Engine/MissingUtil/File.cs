using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if ENABLE_LEGACY_SHIMS
namespace System.IO
{
    public enum FileMode
    {
        // Summary:
        //     Specifies that the operating system should create a new file. This requires
        //     System.Security.Permissions.FileIOPermissionAccess.Write. If the file already
        //     exists, an System.IO.IOException is thrown.
        CreateNew,
        //
        // Summary:
        //     Specifies that the operating system should create a new file. If the file
        //     already exists, it will be overwritten. This requires System.Security.Permissions.FileIOPermissionAccess.Write.
        //     System.IO.FileMode.Create is equivalent to requesting that if the file does
        //     not exist, use System.IO.FileMode.CreateNew; otherwise, use System.IO.FileMode.Truncate.
        Create,
        //
        // Summary:
        //     Specifies that the operating system should open an existing file. The ability
        //     to open the file is dependent on the value specified by System.IO.FileAccess.
        //     A System.IO.FileNotFoundException is thrown if the file does not exist.
        Open,
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
        OpenOrCreate,
        //
        // Summary:
        //     Specifies that the operating system should open an existing file. Once opened,
        //     the file should be truncated so that its size is zero bytes. This requires
        //     System.Security.Permissions.FileIOPermissionAccess.Write. Attempts to read
        //     from a file opened with Truncate cause an exception.
        Truncate,
        //
        // Summary:
        //     Opens the file if it exists and seeks to the end of the file, or creates
        //     a new file. FileMode.Append can only be used in conjunction with FileAccess.Write.
        //     Attempting to seek to a position before the end of the file will throw an
        //     System.IO.IOException and any attempt to read fails and throws an System.NotSupportedException.
        Append,
    }

    public enum FileAccess
    {
        // Summary:
        //     Read access to the file. Data can be read from the file. Combine with Write
        //     for read/write access.
        Read,
        //
        // Summary:
        //     Write access to the file. Data can be written to the file. Combine with Read
        //     for read/write access.
        Write,
        //
        // Summary:
        //     Read and write access to the file. Data can be written to and read from the
        //     file.
        ReadWrite,
    }

    public static class File
    {
        private static FileImpl fileImpl;

        public static FileImpl FileImpl
        {
            get
            {
                return fileImpl;
            }

            set
            {
                fileImpl = value;
            }
        }

        public static Stream Open(String path, FileMode mode, FileAccess access)
        {
            return fileImpl.Open(path, mode, access);
        }

        public static void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            fileImpl.Copy(sourceFileName, destFileName, overwrite);
        }
    }

    public interface FileImpl
    {
        Stream Open(String path, FileMode mode, FileAccess access);

        void Copy(string sourceFileName, string destFileName, bool overwrite);
    }
}
#endif