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
        private static String[] splitPattern = { ".zip", ".dat" };

        internal static bool CanOpenURL(String url)
        {
            return url.Contains(".zip") || url.Contains(".dat");
        }

        public ZipArchive(String filename)
        {
            String zipName = parseZipName(filename);
            String subDir = parseURLInZip(filename);
            if (subDir == "")
            {
                zipFile = new ZipFile(zipName);
            }
            else
            {
                zipFile = new ZipFile(zipName, subDir);
            }
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
            return getArray(zipFile.listFiles(parseURLInZip(url), recursive));
        }

        public override String[] listFiles(String url, String searchPattern, bool recursive)
        {
            return getArray(zipFile.listFiles(parseURLInZip(url), searchPattern, recursive));
        }

        public override String[] listDirectories(String url, bool recursive)
        {
            return getArray(zipFile.listDirectories(parseURLInZip(url), recursive));
        }

        public override String[] listDirectories(String url, String searchPattern, bool recursive)
        {
            return getArray(zipFile.listDirectories(parseURLInZip(url), searchPattern, recursive));
        }

        public override System.IO.Stream openStream(string url, FileMode mode)
        {
            return zipFile.openFile(parseURLInZip(url));
        }

        public override System.IO.Stream openStream(string url, FileMode mode, FileAccess access)
        {
            return zipFile.openFile(parseURLInZip(url));
        }

        public override bool isDirectory(string url)
        {
            ZipFileInfo info = zipFile.getFileInfo(parseURLInZip(url));
            return info != null && info.IsDirectory;
        }

        public override bool exists(String filename)
        {
            return zipFile.exists(parseURLInZip(filename));
        }

        public override ArchiveFileInfo getFileInfo(String filename)
        {
            return new ZipArchiveFileInfo(zipFile.getFileInfo(parseURLInZip(filename)));
        }

        private String parseURLInZip(String url)
        {
            String searchDirectory = url;
            if (url.Contains(".zip") || url.Contains(".dat"))
            {
                if (url.EndsWith(".zip") || url.EndsWith(".dat"))
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
            return searchDirectory;
        }

        private String parseZipName(String url)
        {
            String searchDirectory = url;
            if (url.Contains(".zip") || url.Contains(".dat"))
            {
                if (url.EndsWith(".zip") || url.EndsWith(".dat"))
                {
                    searchDirectory = url;
                }
                else
                {
                    String[] split = url.Split(splitPattern, StringSplitOptions.None);
                    if (split.Length > 0)
                    {
                        searchDirectory = split[0] + ".zip";
                    }
                }
            }
            return searchDirectory;
        }

        private String[] getArray(List<ZipFileInfo> zipFiles)
        {
            String[] ret = new String[zipFiles.Count];
            int i = 0;
            foreach (ZipFileInfo info in zipFiles)
            {
                ret[i++] = info.FullName;
            }
            return ret;
        }
    }
}
