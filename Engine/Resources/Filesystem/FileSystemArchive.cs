﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Engine.Resources
{
    class FileSystemArchive : Archive
    {
        private String baseDirectory;

        public FileSystemArchive(String baseDirectory)
        {
            this.baseDirectory = FileSystem.fixPathDir(Path.GetFullPath(baseDirectory));
        }

        public override void Dispose()
        {

        }

        public override String[] listFiles(bool recursive)
        {
            return listFiles(baseDirectory, recursive);
        }

        public override String[] listFiles(String url, bool recursive)
        {
            String[] files = Directory.GetFiles(fixIncomingDirectoryURL(url), "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            for (int i = 0; i < files.Length; ++i)
            {
                files[i] = fixOutgoingFileString(files[i]);
            }
            return files;
        }

        public override String[] listFiles(String url, String searchPattern, bool recursive)
        {
            String[] files = Directory.GetFiles(fixIncomingDirectoryURL(url), searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            for (int i = 0; i < files.Length; ++i)
            {
                files[i] = fixOutgoingFileString(files[i]);
            }
            return files;
        }

        public override String[] listDirectories(bool recursive)
        {
            return listDirectories(baseDirectory, recursive);
        }

        public override String[] listDirectories(String url, bool recursive)
        {
            String[] dirs = Directory.GetDirectories(fixIncomingDirectoryURL(url), "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            for (int i = 0; i < dirs.Length; ++i)
            {
                dirs[i] = fixOutgoingDirectoryString(dirs[i]);
            }
            return dirs;
        }

        public override String[] listDirectories(String url, bool recursive, bool includeHidden)
        {
            String[] dirs = listDirectories(fixIncomingDirectoryURL(url), "*", recursive, includeHidden);
            for (int i = 0; i < dirs.Length; ++i)
            {
                dirs[i] = fixOutgoingDirectoryString(dirs[i]);
            }
            return dirs;
        }

        public override String[] listDirectories(String url, String searchPattern, bool recursive)
        {
            String[] dirs = Directory.GetDirectories(fixIncomingDirectoryURL(url), searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            for (int i = 0; i < dirs.Length; ++i)
            {
                dirs[i] = fixOutgoingDirectoryString(dirs[i]);
            }
            return dirs;
        }

        public override String[] listDirectories(String url, String searchPattern, bool recursive, bool includeHidden)
        {
            String[] directories = listDirectories(fixIncomingDirectoryURL(url), searchPattern, recursive);
            if (!includeHidden)
            {
                List<String> ret = new List<string>();
                foreach (String dir in directories)
                {
                    FileAttributes attr = File.GetAttributes(dir);
                    if ((attr & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        ret.Add(dir);
                    }
                }
                return ret.ToArray();
            }
            for (int i = 0; i < directories.Length; ++i)
            {
                directories[i] = fixOutgoingDirectoryString(directories[i]);
            }
            return directories;
        }

        public override Stream openStream(String url, FileMode mode)
        {
            return File.Open(fixIncomingFileURL(url), (System.IO.FileMode)mode);
        }

        public override Stream openStream(String url, FileMode mode, FileAccess access)
        {
            return File.Open(fixIncomingFileURL(url), (System.IO.FileMode)mode, (System.IO.FileAccess)access);
        }

        public override bool isDirectory(String url)
        {
            FileAttributes attr = File.GetAttributes(fixIncomingDirectoryURL(url));
            return (attr & FileAttributes.Directory) == FileAttributes.Directory;
        }

        public override bool exists(String filename)
        {
            return File.Exists(fixIncomingDirectoryURL(filename));
        }

        public override VirtualFileInfo getFileInfo(String filename)
        {
            FileInfo info = new FileInfo(fixIncomingDirectoryURL(filename));
            return new VirtualFileInfo(info.Name, fixOutgoingDirectoryString(info.DirectoryName), fixOutgoingDirectoryString(info.FullName), info.FullName, info.Length, info.Length);
        }

        public override String getFullPath(String filename)
        {
            return Path.GetFullPath(fixIncomingDirectoryURL(filename));
        }

        private String fixOutgoingFileString(String url)
        {
            return FileSystem.fixPathFile(url).Replace(baseDirectory, "");
        }

        private String fixOutgoingDirectoryString(String url)
        {
            return FileSystem.fixPathDir(url).Replace(baseDirectory, "");
        }

        private String fixIncomingDirectoryURL(String url)
        {
            url = FileSystem.fixPathDir(url);
            if (url.StartsWith(baseDirectory))
            {
                url = url.Substring(baseDirectory.Length - 1);
            }
            if (url.StartsWith("/"))
            {
                url = url.Substring(1);
            }
            return baseDirectory + url;
        }

        private String fixIncomingFileURL(String url)
        {
            url = FileSystem.fixPathFile(url);
            if (url.StartsWith(baseDirectory))
            {
                url = url.Substring(baseDirectory.Length - 1);
            }
            if (url.StartsWith("/"))
            {
                url = url.Substring(1);
            }
            return baseDirectory + url;
        }
    }
}
