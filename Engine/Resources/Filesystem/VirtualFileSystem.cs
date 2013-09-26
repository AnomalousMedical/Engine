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
                Log.Info("Added resource archive {0}.", path);
                directoryMap["/"].addArchive(archive);
                addAndScanArchive(path, archive);
                return true;
            }

            try
            {
                Log.Warning("Could not find archive '{0}'. Archive not loaded into virtual file system", Path.GetFullPath(path));
            }
            catch (Exception)
            {
                Log.Warning("Could not find archive '{0}'. Archive not loaded into virtual file system", path);
            }
            return false;
        }

        public void createVirtualFolderLink(String realRootPath, String overrideRootPath)
        {
            realRootPath = FileSystem.fixPathDir(realRootPath);
            overrideRootPath = FileSystem.fixPathDir(overrideRootPath);
            Log.Info("Created Virtual Folder Link from '{0}' to '{1}'", realRootPath, overrideRootPath);
            Archive archive = new VirtualFolderLinkArchive(realRootPath, overrideRootPath);
            DirectoryEntry currentEntry;
            if (!directoryMap.TryGetValue(overrideRootPath, out currentEntry))
            {
                currentEntry = new DirectoryEntry();
                directoryMap.Add(overrideRootPath, currentEntry);
            }
            currentEntry.addArchive(archive);
            addAndScanArchive(overrideRootPath, archive);
        }

        public IEnumerable<String> listFiles(bool recursive)
        {
            return listFiles("", recursive);
        }

        public IEnumerable<String> listFiles(String url, bool recursive)
        {
            String asDir = FileSystem.fixPathDir(url);
            DirectoryEntry dirEntry;
            if (directoryMap.TryGetValue(asDir, out dirEntry))
            {
                return dirEntry.listFiles(asDir, recursive);
            }
            throw new FileNotFoundException(String.Format("Could not find directory \"{0}\" in virtual file system.", url), url);
        }

        public IEnumerable<String> listFiles(String url, String searchPattern, bool recursive)
        {
            String asDir = FileSystem.fixPathDir(url);
            DirectoryEntry dirEntry;
            if (directoryMap.TryGetValue(asDir, out dirEntry))
            {
                return dirEntry.listFiles(asDir, searchPattern, recursive);
            }
            throw new FileNotFoundException(String.Format("Could not find directory \"{0}\" in virtual file system.", url), url);
        }

        public IEnumerable<String> listDirectories(bool recursive)
        {
            return listDirectories("", recursive, true);
        }

        public IEnumerable<String> listDirectories(String url, bool recursive)
        {
            return listDirectories(url, recursive, true);
        }

        public IEnumerable<String> listDirectories(String url, bool recursive, bool includeHidden)
        {
            String asDir = FileSystem.fixPathDir(url);
            DirectoryEntry dirEntry;
            if (directoryMap.TryGetValue(asDir, out dirEntry))
            {
                return dirEntry.listDirectories(asDir, recursive, includeHidden);
            }
            throw new FileNotFoundException(String.Format("Could not find directory \"{0}\" in virtual file system.", url), url);
        }

        public IEnumerable<String> listDirectories(String url, String searchPattern, bool recursive)
        {
            return listDirectories(url, searchPattern, recursive, true);
        }

        public IEnumerable<String> listDirectories(String url, String searchPattern, bool recursive, bool includeHidden)
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

        private void addAndScanArchive(String path, Archive archive)
        {
            archives.Add(archive);

            //Add all directory entries.
            IEnumerable<String> directories = archive.listDirectories(path, true, true);
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
            IEnumerable<String> files = archive.listFiles(path, true);
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

            public IEnumerable<String> listFiles(String url, bool recursive)
            {
                return listFilesImpl(url, recursive).Distinct();
            }

            private IEnumerable<String> listFilesImpl(String url, bool recursive)
            {
                foreach (Archive archive in archives)
                {
                    foreach (String file in archive.listFiles(url, recursive))
                    {
                        yield return file;
                    }
                }
            }

            public IEnumerable<String> listFiles(String url, String searchPattern, bool recursive)
            {
                return listFilesImpl(url, searchPattern, recursive).Distinct();
            }

            private IEnumerable<String> listFilesImpl(String url, String searchPattern, bool recursive)
            {
                foreach (Archive archive in archives)
                {
                    foreach(String file in archive.listFiles(url, searchPattern, recursive))
                    {
                        yield return file;
                    }
                }
            }

            public IEnumerable<String> listDirectories(String url, bool recursive, bool includeHidden)
            {
                return listDirectoriesImpl(url, recursive, includeHidden).Distinct();
            }

            private IEnumerable<String> listDirectoriesImpl(String url, bool recursive, bool includeHidden)
            {
                List<String> directories = new List<string>();
                foreach (Archive archive in archives)
                {
                    foreach (String file in archive.listDirectories(url, recursive, includeHidden))
                    {
                        yield return file;
                    }
                }
            }

            public IEnumerable<String> listDirectories(String url, String searchPattern, bool recursive, bool includeHidden)
            {
                return listDirectoriesImpl(url, searchPattern, recursive, includeHidden).Distinct();
            }

            public IEnumerable<String> listDirectoriesImpl(String url, String searchPattern, bool recursive, bool includeHidden)
            {
                foreach (Archive archive in archives)
                {
                    foreach(String file in archive.listDirectories(url, searchPattern, recursive, includeHidden))
                    {
                        yield return file;
                    }
                }
            }
        }
    }
}
