using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Engine.Resources
{
    class FileSystemArchive : Archive
    {
        public override void Dispose()
        {

        }

        public override String[] listFiles(String url, bool recursive)
        {
            return Directory.GetFiles(url, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        public override String[] listFiles(String url, String searchPattern, bool recursive)
        {
            return Directory.GetFiles(url, searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        public override String[] listDirectories(String url, bool recursive)
        {
            return Directory.GetDirectories(url, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        public override String[] listDirectories(String url, String searchPattern, bool recursive)
        {
            return Directory.GetDirectories(url, searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        public override Stream openStream(String url, FileMode mode)
        {
            return File.Open(url, (System.IO.FileMode)mode);
        }

        public override Stream openStream(String url, FileMode mode, FileAccess access)
        {
            return File.Open(url, (System.IO.FileMode)mode, (System.IO.FileAccess)access);
        }

        public override bool isDirectory(String url)
        {
            FileAttributes attr = File.GetAttributes(url);
            return (attr & FileAttributes.Directory) == FileAttributes.Directory;
        }

        public override bool exists(String filename)
        {
            return File.Exists(filename);
        }

        public override ArchiveFileInfo getFileInfo(String filename)
        {
            return new FileSystemFileInfo(filename);
        }

        public override String getFullPath(String filename)
        {
            return Path.GetFullPath(filename);
        }
    }
}
