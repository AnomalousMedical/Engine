using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZipAccess;

namespace Engine.Resources
{
    class ZipArchive : Archive
    {
        private ZipFile zipFile = null;
        private static String[] splitPattern = { ".zip" };

        internal static bool CanOpenURL(String url)
        {
            return url.Contains(".zip");
        }

        public ZipArchive(String filename)
        {
            zipFile = new ZipFile(filename);
        }

        public override void Dispose()
        {
            if (zipFile != null)
            {
                zipFile.Dispose();
                zipFile = null;
            }
        }

        public override string[] listFiles(string url, bool recursive)
        {
            return zipFile.listFiles(parseURL(url), recursive).ToArray();
        }

        public override String[] listFiles(String url, String searchPattern, bool recursive)
        {
            return zipFile.listFiles(parseURL(url), searchPattern, recursive).ToArray();
        }

        public override String[] listDirectories(String url, bool recursive)
        {
            return zipFile.listDirectories(parseURL(url), recursive).ToArray();
        }

        public override String[] listDirectories(String url, String searchPattern, bool recursive)
        {
            return zipFile.listDirectories(parseURL(url), searchPattern, recursive).ToArray();
        }

        public override System.IO.Stream openStream(string url, FileMode mode)
        {
            return zipFile.openFile(url);
        }

        public override System.IO.Stream openStream(string url, FileMode mode, FileAccess access)
        {
            return zipFile.openFile(url);
        }

        public override bool isDirectory(string url)
        {
            return !url.Contains(".") || url.EndsWith(".zip");
        }

        public override bool exists(String filename)
        {
            return zipFile.exists(filename);
        }

        private String parseURL(String url)
        {
            String searchDirectory = url;
            if (url.Contains(".zip"))
            {
                if (url.EndsWith(".zip"))
                {
                    searchDirectory = "";
                }
                else
                {
                    String[] split = url.Split(splitPattern, StringSplitOptions.None);
                    if (split.Length == 1)
                    {
                        searchDirectory = "";
                    }
                    else
                    {
                        searchDirectory = split[1];
                    }
                }
            }
            return url;
        }

        public override ArchiveFileInfo getFileInfo(String filename)
        {
            throw new NotImplementedException();
        }
    }
}
