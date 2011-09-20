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

        public static String GetFileName(String url)
        {
            url = url.Replace('\\', '/');
            int lastSlash = url.LastIndexOf('/');
            lastSlash++;
            if (lastSlash > url.Length - 1)
            {
                lastSlash = url.Length - 1;
            }
            return url.Substring(lastSlash);
        }

        public static String GetDirectoryName(String url)
        {
            String repUrl = url.Replace('\\', '/');
            int lastSlash = repUrl.LastIndexOf('/');
            if (lastSlash < 0)
            {
                lastSlash = 0;
            }
            return url.Substring(0, lastSlash);
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

        public bool containsRealAbsolutePath(String path)
        {
            foreach (Archive archive in archives)
            {
                if (archive.containsRealAbsolutePath(path))
                {
                    return true;
                }
            }
            return false;
        }

        public bool addArchive(String path)
        {
            Archive archive = FileSystem.OpenArchive(path);
            if (archive != null)
            {
                archives.Add(archive);

                Log.Info("Added resource archive {0}.", path);
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
                return true;
            }

            Log.Warning("Could not find archive {0}. Archive not loaded into virtual file system", path);
            return false;
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
            throw new FileNotFoundException(String.Format("Could not find directory \"{0}\" in virtual file system.", url), url);
        }

        public String[] listFiles(String url, String searchPattern, bool recursive)
        {
            String asDir = FileSystem.fixPathDir(url);
            DirectoryEntry dirEntry;
            if (directoryMap.TryGetValue(asDir, out dirEntry))
            {
                return dirEntry.listFiles(asDir, searchPattern, recursive);
            }
            throw new FileNotFoundException(String.Format("Could not find directory \"{0}\" in virtual file system.", url), url);
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
            throw new FileNotFoundException(String.Format("Could not find directory \"{0}\" in virtual file system.", url), url);
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
            throw new FileNotFoundException(String.Format("Could not find directory \"{0}\" in virtual file system.", url), url);
        }

        public Stream openStream(String url, Resources.FileMode mode)
        {
            url = FileSystem.fixPathFile(url);
            Archive targetArchive;
            if (fileMap.TryGetValue(url, out targetArchive))
            {
                return targetArchive.openStream(url, mode);
            }
            throw new FileNotFoundException(String.Format("Could not find file \"{0}\" in virtual file system.", url), url);
        }

        public Stream openStream(String url, Resources.FileMode mode, Resources.FileAccess access)
        {
            url = FileSystem.fixPathFile(url);
            Archive targetArchive;
            if (fileMap.TryGetValue(url, out targetArchive))
            {
                return targetArchive.openStream(url, mode, access);
            }
            throw new FileNotFoundException(String.Format("Could not find file \"{0}\" in virtual file system.", url), url);
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
            throw new FileNotFoundException(String.Format("Could not find file \"{0}\" in virtual file system.", filename), filename);
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
