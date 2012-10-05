using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Engine.Resources
{
    /// <summary>
    /// This class links one virtual file system folder to another. So if you
    /// have a bunch of files in a folder Original and some more in a folder
    /// called Override and you link override into Original with this class the
    /// files from Override will appear to be a part of Original and any
    /// duplicate files will be replaced by their versions from Override.
    /// </summary>
    class VirtualFolderLinkArchive : Archive
    {
        private String realRootPath;
        private String overrideRootPath;

        public VirtualFolderLinkArchive(String realRootPath, String overrideRootPath)
        {
            this.realRootPath = realRootPath;
            this.overrideRootPath = overrideRootPath;
        }

        public override void Dispose()
        {

        }

        public override bool containsRealAbsolutePath(string path)
        {
            return false;
        }

        public override IEnumerable<String> listFiles(bool recursive)
        {
            foreach (String file in VirtualFileSystem.Instance.listFiles(realRootPath, recursive))
            {
                yield return fixOutgoingFileString(file);
            }
        }

        public override IEnumerable<String> listFiles(string url, bool recursive)
        {
            foreach (String file in VirtualFileSystem.Instance.listFiles(fixIncomingURL(url), recursive))
            {
                yield return fixOutgoingFileString(file);
            }
        }

        public override IEnumerable<String> listFiles(string url, string searchPattern, bool recursive)
        {
            foreach (String file in VirtualFileSystem.Instance.listFiles(fixIncomingURL(url), searchPattern, recursive))
            {
                yield return fixOutgoingFileString(file);
            }
        }

        public override IEnumerable<String> listDirectories(bool recursive)
        {
            foreach (String dir in VirtualFileSystem.Instance.listDirectories(realRootPath, recursive))
            {
                yield return fixOutgoingDirectoryString(dir);
            }
        }

        public override IEnumerable<String> listDirectories(string url, bool recursive)
        {
            foreach (String dir in VirtualFileSystem.Instance.listDirectories(fixIncomingURL(url), recursive))
            {
                yield return fixOutgoingDirectoryString(dir);
            }
        }

        public override IEnumerable<String> listDirectories(string url, bool recursive, bool includeHidden)
        {
            foreach (String dir in VirtualFileSystem.Instance.listDirectories(fixIncomingURL(url), recursive))
            {
                yield return fixOutgoingDirectoryString(dir);
            }
        }

        public override IEnumerable<String> listDirectories(string url, string searchPattern, bool recursive)
        {
            foreach (String dir in VirtualFileSystem.Instance.listDirectories(fixIncomingURL(url), recursive))
            {
                yield return fixOutgoingDirectoryString(dir);
            }
        }

        public override IEnumerable<String> listDirectories(string url, string searchPattern, bool recursive, bool includeHidden)
        {
            foreach (String dir in VirtualFileSystem.Instance.listDirectories(fixIncomingURL(url), recursive))
            {
                yield return fixOutgoingDirectoryString(dir);
            }
        }

        public override Stream openStream(string url, FileMode mode)
        {
            return VirtualFileSystem.Instance.openStream(fixIncomingURL(url), mode);
        }

        public override Stream openStream(string url, FileMode mode, FileAccess access)
        {
            return VirtualFileSystem.Instance.openStream(fixIncomingURL(url), mode, access);
        }

        public override bool isDirectory(string url)
        {
            return VirtualFileSystem.Instance.isDirectory(fixIncomingURL(url));
        }

        public override bool exists(string filename)
        {
            return VirtualFileSystem.Instance.exists(fixIncomingURL(filename));
        }

        public override VirtualFileInfo getFileInfo(string filename)
        {
            VirtualFileInfo originalInfo = VirtualFileSystem.Instance.getFileInfo(fixIncomingURL(filename));
            return new VirtualFileInfo(originalInfo.Name, 
                fixOutgoingDirectoryString(originalInfo.DirectoryName), 
                fixOutgoingFileString(originalInfo.FullName), 
                originalInfo.RealLocation, 
                originalInfo.CompressedSize, 
                originalInfo.UncompressedSize);
        }

        private String fixOutgoingFileString(String url)
        {
            return FileSystem.fixPathFile(url).Replace(realRootPath, overrideRootPath);
        }

        private String fixOutgoingDirectoryString(String url)
        {
            return FileSystem.fixPathDir(url).Replace(realRootPath, overrideRootPath);
        }

        private String fixIncomingURL(String url)
        {
            return url.Replace(overrideRootPath, realRootPath);
        }
    }
}
