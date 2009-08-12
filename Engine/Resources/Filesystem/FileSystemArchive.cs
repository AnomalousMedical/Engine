using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Engine.Resources
{
    class FileSystemArchive : Archive
    {
        internal override void open()
        {

        }

        internal override void close()
        {

        }

        public override String[] listFiles(String url)
        {
            return Directory.GetFiles(url);
        }

        public override String[] listFiles(String url, String searchPattern)
        {
            return Directory.GetFiles(url, searchPattern);
        }

        public override String[] listFiles(String url, String searchPattern, SearchType searchType)
        {
            return Directory.GetFiles(url, searchPattern, searchType == SearchType.AllDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        public override String[] listDirectories(String url)
        {
            return Directory.GetFiles(url);
        }

        public override String[] listDirectories(String url, String searchPattern)
        {
            return Directory.GetFiles(url, searchPattern);
        }

        public override String[] listDirectories(String url, String searchPattern, SearchType searchType)
        {
            return Directory.GetDirectories(url, searchPattern, searchType == SearchType.AllDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
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
    }
}
