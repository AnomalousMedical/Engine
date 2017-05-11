using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZipAccess;
using System.IO;

namespace Engine.Resources
{
    class ZipArchive : Archive
    {
        private ZipFile zipFile = null;
        private static String[] splitPattern = { ".zip", ".dat", ".obb" };
        private String fullZipPath;

        internal static bool CanOpenURL(String url)
        {
            return url.Contains(".zip") || url.Contains(".dat") ||url.Contains(".obb");
        }

        public ZipArchive(String filename)
        {
            String zipName = parseZipName(filename);
            fullZipPath = Path.GetFullPath(zipName);
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

        public override bool containsRealAbsolutePath(String path)
        {
            return FileSystem.fixPathFile(path).StartsWith(fullZipPath, StringComparison.OrdinalIgnoreCase);
        }

        public override bool isArchiveFor(String path)
        {
            return fullZipPath == Path.GetFullPath(path);
        }

        public override IEnumerable<String> listFiles(bool recursive)
        {
            return enumerateNames(zipFile.listFiles("", recursive));
        }

        public override IEnumerable<String> listFiles(string url, bool recursive)
        {
            return enumerateNames(zipFile.listFiles(parseURLInZip(url), recursive));
        }

        public override IEnumerable<String> listFiles(String url, String searchPattern, bool recursive)
        {
            return enumerateNames(zipFile.listFiles(parseURLInZip(url), searchPattern, recursive));
        }

        public override IEnumerable<String> listDirectories(bool recursive)
        {
            return enumerateNames(zipFile.listDirectories("", recursive));
        }

        public override IEnumerable<String> listDirectories(String url, bool recursive)
        {
            return enumerateNames(zipFile.listDirectories(parseURLInZip(url), recursive));
        }

        public override IEnumerable<String> listDirectories(string url, bool recursive, bool includeHidden)
        {
            return listDirectories(url, recursive);
        }

        public override IEnumerable<String> listDirectories(String url, String searchPattern, bool recursive)
        {
            return enumerateNames(zipFile.listDirectories(parseURLInZip(url), searchPattern, recursive));
        }

        public override IEnumerable<String> listDirectories(string url, string searchPattern, bool recursive, bool includeHidden)
        {
            return listDirectories(url, searchPattern, recursive);
        }

        public override System.IO.Stream openStream(string url, FileMode mode)
        {
            return zipFile.openFile(parseURLInZip(url));
        }

        public override System.IO.Stream openStream(string url, FileMode mode, FileAccess access)
        {
            return zipFile.openFile(parseURLInZip(url));
        }

        public override Stream openStream(String url, FileMode mode, FileAccess access, FileShare share)
        {
            return zipFile.openFile(parseURLInZip(url));
        }

        public override bool isDirectory(string url)
        {
            ZipFileInfo info = zipFile.getFileInfo(parseURLInZip(url));
            return info != null && info.IsDirectory;
        }

        public override VirtualFileInfo getFileInfo(String filename)
        {
            ZipFileInfo info = zipFile.getFileInfo(parseURLInZip(filename));
            return new VirtualFileInfo(info.Name, info.DirectoryName, info.FullName, getFullPath(info.FullName), info.CompressedSize, info.UncompressedSize);
        }

        private String parseURLInZip(String url)
        {
            String searchDirectory = url;
            if (url.Contains(".zip") || url.Contains(".dat") || url.Contains(".obb"))
            {
                if (url.EndsWith(".zip") || url.EndsWith(".dat") || url.EndsWith(".obb"))
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

        public static String parseZipName(String url)
        {
            String searchDirectory = url;
            //zip file
            if (url.Contains(".zip"))
            {
                searchDirectory = extractZipName(url, searchDirectory, ".zip");
            }
            //dat file
            else if (url.Contains(".dat"))
            {
                searchDirectory = extractZipName(url, searchDirectory, ".dat");
            }
            //obb file
            else if (url.Contains(".obb"))
            {
                searchDirectory = extractZipName(url, searchDirectory, ".obb");
            }
            return searchDirectory;
        }

        /// <summary>
        /// Get the name of the zip file from the path, the zip file must have the given extension.
        /// </summary>
        private static string extractZipName(String url, String searchDirectory, String extension)
        {
            if (url.EndsWith(extension))
            {
                searchDirectory = url;
            }
            else
            {
                String[] split = url.Split(splitPattern, StringSplitOptions.None);
                if (split.Length > 0)
                {
                    searchDirectory = split[0] + extension;
                }
            }
            return searchDirectory;
        }

        private IEnumerable<String> enumerateNames(IEnumerable<ZipFileInfo> zipFiles)
        {
            foreach (ZipFileInfo info in zipFiles)
            {
                yield return info.FullName;
            }
        }

        private String getFullPath(String filename)
        {
            if (filename.Contains(".zip") || filename.Contains(".dat") || filename.Contains(".obb"))
            {
                return filename;
            }
            else
            {
                return fullZipPath + "/" + filename;
            }
        }
    }
}
