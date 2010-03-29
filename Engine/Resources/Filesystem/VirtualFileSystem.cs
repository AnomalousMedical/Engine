using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Engine.Resources;
using Logging;

namespace Engine
{
    public class VirtualFileSystem : IDisposable
    {
        #region Static

        static VirtualFileSystem instance;

        public static VirtualFileSystem Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion Static

        /// <summary>
        /// A map of files to the archives that contain them
        /// </summary>
        Dictionary<String, Archive> fileMap = new Dictionary<string, Archive>(1, StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// A map of directories to the archives that have them. A single
        /// directory can be in multiple archives and there is no definition as
        /// to which is defined here. It is considered to not matter.
        /// </summary>
        Dictionary<String, DirectoryEntry> directoryMap = new Dictionary<String, DirectoryEntry>(1, StringComparer.InvariantCultureIgnoreCase);

        List<Archive> archives = new List<Archive>();

        public VirtualFileSystem()
        {
            if (instance != null)
            {
                throw new Exception("Only one VirtualFileSystem can be created at a time.");
            }
            instance = this;
            directoryMap.Add("/", new DirectoryEntry());
        }

        public void Dispose()
        {
            foreach (Archive archive in archives)
            {
                archive.Dispose();
            }
            instance = null;
        }

        public void addArchive(String path)
        {
            Log.Debug("Added resource archive {0}.", path);
            Archive archive = FileSystem.OpenArchive(path);

            directoryMap["/"].addArchive(archive);

            //Add all directory entries.
            String[] directories = archive.listDirectories(path, true, true);
            DirectoryEntry currentEntry;
            String directory;
            foreach (String directoryIter in directories)
            {
                directory = FileSystem.fixPathDir(directoryIter);
                if (!directoryMap.TryGetValue(directory, out currentEntry))
                {
                    currentEntry = new DirectoryEntry();
                    directoryMap.Add(directory, currentEntry);
                }
                currentEntry.addArchive(archive);
            }

            //Add all file entries, replacing archives for duplicate files
            String[] files = archive.listFiles(path, true);
            String file;
            foreach (String fileIter in files)
            {
                file = FileSystem.fixPathFile(fileIter);
                if (fileMap.ContainsKey(file))
                {
                    fileMap[file] = archive;
                }
                else
                {
                    fileMap.Add(file, archive);
                }
            }
        }

        public String[] listFiles(bool recursive)
        {
            return listFiles("", recursive);
        }

        public String[] listFiles(String url, bool recursive)
        {
            String asDir = FileSystem.fixPathDir(url);
            DirectoryEntry dirEntry;
            if (directoryMap.TryGetValue(asDir, out dirEntry))
            {
                return dirEntry.listFiles(asDir, recursive);
            }
            throw new FileNotFoundException("Could not find directory in virtual file system.", url);
        }

        public String[] listFiles(String url, String searchPattern, bool recursive)
        {
            String asDir = FileSystem.fixPathDir(url);
            DirectoryEntry dirEntry;
            if (directoryMap.TryGetValue(asDir, out dirEntry))
            {
                return dirEntry.listFiles(asDir, searchPattern, recursive);
            }
            throw new FileNotFoundException("Could not find directory in virtual file system.", url);
        }

        public String[] listDirectories(bool recursive)
        {
            return listDirectories("", recursive, true);
        }

        public String[] listDirectories(String url, bool recursive)
        {
            return listDirectories(url, recursive, true);
        }

        public String[] listDirectories(String url, bool recursive, bool includeHidden)
        {
            String asDir = FileSystem.fixPathDir(url);
            DirectoryEntry dirEntry;
            if (directoryMap.TryGetValue(asDir, out dirEntry))
            {
                return dirEntry.listDirectories(asDir, recursive, includeHidden);
            }
            throw new FileNotFoundException("Could not find directory in virtual file system.", url);
        }

        public String[] listDirectories(String url, String searchPattern, bool recursive)
        {
            return listDirectories(url, searchPattern, recursive, true);
        }

        public String[] listDirectories(String url, String searchPattern, bool recursive, bool includeHidden)
        {
            String asDir = FileSystem.fixPathDir(url);
            DirectoryEntry dirEntry;
            if (directoryMap.TryGetValue(asDir, out dirEntry))
            {
                return dirEntry.listDirectories(asDir, searchPattern, recursive, includeHidden);
            }
            throw new FileNotFoundException("Could not find directory in virtual file system.", url);
        }

        public Stream openStream(String url, Resources.FileMode mode)
        {
            url = FileSystem.fixPathFile(url);
            Archive targetArchive;
            if (fileMap.TryGetValue(url, out targetArchive))
            {
                return targetArchive.openStream(url, mode);
            }
            throw new FileNotFoundException("Could not find file in virtual file system.", url);
        }

        public Stream openStream(String url, Resources.FileMode mode, Resources.FileAccess access)
        {
            url = FileSystem.fixPathFile(url);
            Archive targetArchive;
            if (fileMap.TryGetValue(url, out targetArchive))
            {
                return targetArchive.openStream(url, mode, access);
            }
            throw new FileNotFoundException("Could not find file in virtual file system.", url);
        }

        public bool isDirectory(String url)
        {
            url = FileSystem.fixPathDir(url);
            return directoryMap.ContainsKey(url);
        }

        public bool exists(String filename)
        {
            String asFile = FileSystem.fixPathFile(filename);
            if (fileMap.ContainsKey(asFile))
            {
                return true;
            }
            String asDir = FileSystem.fixPathDir(filename);
            return directoryMap.ContainsKey(asDir);
        }

        public VirtualFileInfo getFileInfo(String filename)
        {
            Archive targetArchive;
            String asFile = FileSystem.fixPathFile(filename);
            if (fileMap.TryGetValue(asFile, out targetArchive))
            {
                return targetArchive.getFileInfo(asFile);
            }
            String asDir = FileSystem.fixPathDir(filename);
            DirectoryEntry dirEntry;
            if (directoryMap.TryGetValue(asDir, out dirEntry))
            {
                return dirEntry.getFileInfo(asDir);
            }
            throw new FileNotFoundException("Could not find file in virtual file system.", filename);
        }

        public String getFullPath(String filename)
        {
            Archive targetArchive;
            String asFile = FileSystem.fixPathFile(filename);
            if (fileMap.TryGetValue(asFile, out targetArchive))
            {
                return targetArchive.getFullPath(asFile);
            }
            String asDir = FileSystem.fixPathDir(filename);
            DirectoryEntry dirEntry;
            if (directoryMap.TryGetValue(asDir, out dirEntry))
            {
                return dirEntry.getFullPath(asDir);
            }
            throw new FileNotFoundException("Could not find file in virtual file system.", filename);
        }

        /// <summary>
        /// A container that holds all archives that contain a particular
        /// directory. NOTE that this is all the information that is stored. The
        /// functions must be called as if they were being called on the base
        /// archives themselves. The way these classes are designed will handle
        /// this naturally, but it is important to realize that this entry knows
        /// nothing about the actual directory it represents merely what
        /// archives contain it.
        /// </summary>
        class DirectoryEntry
        {
            private List<Archive> archives = new List<Archive>();

            public void addArchive(Archive archive)
            {
                archives.Add(archive);
            }

            public VirtualFileInfo getFileInfo(String filename)
            {
                return archives[archives.Count - 1].getFileInfo(filename);
            }

            public String getFullPath(String filename)
            {
                return archives[archives.Count - 1].getFullPath(filename);
            }

            public String[] listFiles(String url, bool recursive)
            {
                List<String> files = new List<string>();
                foreach (Archive archive in archives)
                {
                    files.AddRange(archive.listFiles(url, recursive));
                }
                return files.Distinct().ToArray();
            }

            public String[] listFiles(String url, String searchPattern, bool recursive)
            {
                List<String> files = new List<string>();
                foreach (Archive archive in archives)
                {
                    files.AddRange(archive.listFiles(url, searchPattern, recursive));
                }
                return files.Distinct().ToArray();
            }

            public String[] listDirectories(String url, bool recursive, bool includeHidden)
            {
                List<String> directories = new List<string>();
                foreach (Archive archive in archives)
                {
                    directories.AddRange(archive.listDirectories(url, recursive, includeHidden));
                }
                return directories.Distinct().ToArray();
            }

            public String[] listDirectories(String url, String searchPattern, bool recursive, bool includeHidden)
            {
                List<String> directories = new List<string>();
                foreach (Archive archive in archives)
                {
                    directories.AddRange(archive.listDirectories(url, searchPattern, recursive, includeHidden));
                }
                return directories.Distinct().ToArray();
            }
        }
    }
}
