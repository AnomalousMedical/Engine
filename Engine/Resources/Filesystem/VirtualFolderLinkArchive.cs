//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;
//using System.Text.RegularExpressions;

//namespace Engine.Resources
//{
//    /// <summary>
//    /// This class links one virtual file system folder to another. So if you
//    /// have a bunch of files in a folder Original and some more in a folder
//    /// called Override and you link override into Original with this class the
//    /// files from Override will appear to be a part of Original and any
//    /// duplicate files will be replaced by their versions from Override.
//    /// </summary>
//    class VirtualFolderLinkArchive : Archive
//    {
//        private String realRootPath;
//        private String overrideRootPath;

//        public VirtualFolderLinkArchive(String realRootPath, String overrideRootPath)
//        {
//            this.realRootPath = realRootPath;
//            this.overrideRootPath = overrideRootPath;
//        }

//        public override void Dispose()
//        {
            
//        }

//        public override bool containsRealAbsolutePath(string path)
//        {
//            return false;
//        }

//        public override string[] listFiles(bool recursive)
//        {
//            return VirtualFileSystem.Instance.listFiles(realRootPath, recursive);
//        }

//        public override string[] listFiles(string url, bool recursive)
//        {
            
//        }

//        public override string[] listFiles(string url, string searchPattern, bool recursive)
//        {
//            Regex r = new Regex(searchPattern);
//            List<String> matches = new List<string>();
//            foreach (String file in files)
//            {
//                if (r.Match(file).Success)
//                {
//                    matches.Add(file);
//                }
//            }
//            return matches.ToArray();
//        }

//        public override string[] listDirectories(bool recursive)
//        {
//            return new String[0];
//        }

//        public override string[] listDirectories(string url, bool recursive)
//        {
//            return new String[0];
//        }

//        public override string[] listDirectories(string url, bool recursive, bool includeHidden)
//        {
//            return new String[0];
//        }

//        public override string[] listDirectories(string url, string searchPattern, bool recursive)
//        {
//            return new String[0];
//        }

//        public override string[] listDirectories(string url, string searchPattern, bool recursive, bool includeHidden)
//        {
//            return new String[0];
//        }

//        public override Stream openStream(string url, FileMode mode)
//        {
//            return VirtualFileSystem.Instance.openStream(fixIncomingURL(url), mode);
//        }

//        public override Stream openStream(string url, FileMode mode, FileAccess access)
//        {
//            return VirtualFileSystem.Instance.openStream(fixIncomingURL(url), mode, access);
//        }

//        public override bool isDirectory(string url)
//        {
//            return VirtualFileSystem.Instance.isDirectory(fixIncomingURL(url));
//        }

//        public override bool exists(string filename)
//        {
//            return VirtualFileSystem.Instance.exists(fixIncomingURL(filename));
//        }

//        public override VirtualFileInfo getFileInfo(string filename)
//        {
//            return VirtualFileSystem.Instance.getFileInfo(fixIncomingURL(filename));
//        }

//        private String fixOutgoingFileString(String url)
//        {
//            return FileSystem.fixPathFile(url).Replace(realRootPath, overrideRootPath);
//        }

//        private String fixOutgoingDirectoryString(String url)
//        {
//            return FileSystem.fixPathDir(url).Replace(realRootPath, overrideRootPath);
//        }

//        private String fixIncomingURL(String url)
//        {
//            return url.Replace(overrideRootPath, realRootPath);
//        }
//    }
//}
