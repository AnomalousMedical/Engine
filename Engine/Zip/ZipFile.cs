using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Engine;

namespace ZipAccess
{
    enum ZZipError
    {
        ZZIP_NO_ERROR = 0,	                /* no error, may be used if user sets it. */
        ZZIP_OUTOFMEM = ZipFile.ZZIP_ERROR - 20, /* out of memory */
        ZZIP_DIR_OPEN = ZipFile.ZZIP_ERROR - 21, /* failed to open zipfile, see errno for details */
        ZZIP_DIR_STAT = ZipFile.ZZIP_ERROR - 22, /* failed to fstat zipfile, see errno for details */
        ZZIP_DIR_SEEK = ZipFile.ZZIP_ERROR - 23, /* failed to lseek zipfile, see errno for details */
        ZZIP_DIR_READ = ZipFile.ZZIP_ERROR - 24, /* failed to read zipfile, see errno for details */
        ZZIP_DIR_TOO_SHORT = ZipFile.ZZIP_ERROR - 25,
        ZZIP_DIR_EDH_MISSING = ZipFile.ZZIP_ERROR - 26,
        ZZIP_DIRSIZE = ZipFile.ZZIP_ERROR - 27,
        ZZIP_ENOENT = ZipFile.ZZIP_ERROR - 28,
        ZZIP_UNSUPP_COMPR = ZipFile.ZZIP_ERROR - 29,
        ZZIP_CORRUPTED = ZipFile.ZZIP_ERROR - 31,
        ZZIP_UNDEF = ZipFile.ZZIP_ERROR - 32,
        ZZIP_DIR_LARGEFILE = ZipFile.ZZIP_ERROR - 33
    }

    enum ZZipFlags : int
    {
        ZZIP_CASELESS = (1 << 12), /* ignore filename case inside zips */
        ZZIP_NOPATHS = (1 << 13), /* ignore subdir paths, just filename*/
        ZZIP_PREFERZIP = (1 << 14), /* try first zipped file, then real*/
        ZZIP_ONLYZIP = (1 << 16), /* try _only_ zipped file, skip real*/
        ZZIP_FACTORY = (1 << 17), /* old file handle is not closed */
        ZZIP_ALLOWREAL = (1 << 18), /* real files use default_io (magic) */
        ZZIP_THREADED = (1 << 19), /* try to be safe for multithreading */
    }

    public class ZipFile : IDisposable
    {
        internal const int ZZIP_ERROR = -4096;

        internal const int ZZIP_CASEINSENSITIVE = 0x0008;

        static char[] SEPS = { '/', '\\' };

	    String file;
	    String fileFilter;
	    List<ZipFileInfo> files = new List<ZipFileInfo>();
	    List<ZipFileInfo> directories = new List<ZipFileInfo>();
        LifecycleObjectPool<PooledZzipDir> pooledZzipDirHandles;

        public ZipFile(String filename)
        {
            this.file = filename;
            this.fileFilter = null;
            commonLoad();
        }

        public ZipFile(String filename, String fileFilter)
        {
            this.file = filename;
            this.fileFilter = fileFilter;
            commonLoad();
        }

	    public void Dispose()
        {
            pooledZzipDirHandles.Dispose();
        }

        public unsafe ZipStream openFile(String filename)
        {
            try
            {
                return new ZipStream(pooledZzipDirHandles.getPooledObject(), fixPathFile(filename));
            }
            catch(ZipIOException)
            {
                return null;
            }
        }

	    public IEnumerable<ZipFileInfo> listFiles(String path, bool recursive)
        {
            return findMatches(files, path, "*", recursive);
        }

        public IEnumerable<ZipFileInfo> listFiles(String path, String searchPattern, bool recursive)
        {
            return findMatches(files, path, searchPattern, recursive);
        }

        public IEnumerable<ZipFileInfo> listDirectories(String path, bool recursive)
        {
            return findMatches(directories, path, "*", recursive);
        }

        public IEnumerable<ZipFileInfo> listDirectories(String path, String searchPattern, bool recursive)
        {
            return findMatches(directories, path, searchPattern, recursive);
        }

	    public unsafe bool fileExists(String filename)
        {
            String cFile = fixPathFile(filename);
            return files.FirstOrDefault(d => cFile.Equals(d.FullName, StringComparison.OrdinalIgnoreCase)) != null;
        }

        public bool directoryExists(String path)
        {
            if (path == "" || path == "/")
            {
                return true;
            }
            path = fixPathDir(path);
            return directories.FirstOrDefault(d => path.Equals(d.FullName, StringComparison.OrdinalIgnoreCase)) != null;
        }

        public ZipFileInfo getFileInfo(String filename)
        {
            String fixedFileName = fixPathFile(filename);
	        foreach(ZipFileInfo file in files)
	        {
		        if(file.FullName == fixedFileName)
		        {
			        return file;
		        }
	        }
	        fixedFileName = fixPathDir(filename);
	        foreach(ZipFileInfo file in directories)
	        {
		        if(file.FullName == fixedFileName)
		        {
			        return file;
		        }
	        }
	        return null;
        }

        /// <summary>
        /// The maximum number of dir handles to keep open in the pool. The default is 3.
        /// </summary>
        public int MaxDirHandlePoolSize
        {
            get
            {
                return pooledZzipDirHandles.MaxPoolSize.Value;
            }
            set
            {
                pooledZzipDirHandles.MaxPoolSize = value;
            }
        }

        private IEnumerable<ZipFileInfo> findMatches(List<ZipFileInfo> sourceList, String path, String searchPattern, bool recursive)
        {
            bool matchAll = searchPattern == "*";
	        searchPattern = wildcardToRegex(searchPattern);
	        Regex r = new Regex(searchPattern);
	        bool matchBaseDir = (path == "" || path == "/");
	        if(!matchBaseDir)
	        {
		        path = fixPathDir(path);
	        }

	        //recursive
	        if(recursive)
	        {
		        //looking in root folder
		        if(matchBaseDir)
		        {
			        foreach(ZipFileInfo file in sourceList)
			        {
				        if(matchAll || r.Match(file.FullName).Success)
				        {
					        yield return file;
				        }
			        }
		        }
		        //looking in a specific folder
		        else
		        {
			        foreach(ZipFileInfo file in sourceList)
			        {
				        Match match = r.Match(file.FullName);
				        if(file.DirectoryName.StartsWith(path) && (matchAll || match.Success))
				        {
					        yield return file;
				        }
			        }
		        }
	        }
	        //Non recursive
	        else
	        {
		        //looking in root folder
		        if(matchBaseDir)
		        {
			        foreach(ZipFileInfo file in sourceList)
			        {
				        if(String.IsNullOrEmpty(file.DirectoryName) && (matchAll || r.Match(file.FullName).Success))
				        {
					        yield return file;
				        }
			        }
		        }
		        //looking in specific folder
		        else
		        {
			        foreach(ZipFileInfo file in sourceList)
			        {
                        if (file.DirectoryName.StartsWith(path) && file.FullName.Length - file.Name.Length == path.Length && (matchAll || r.Match(file.FullName).Success))
				        {
					        String cut = file.FullName.Replace(path, "");
					        if(cut.EndsWith("/"))
					        {
						        cut = cut.Substring(0, cut.Length - 1);
					        }
					        if(cut != String.Empty && !cut.Contains("/"))
					        {
						        yield return file;
					        }
				        }
			        }
		        }
	        }
        }

        private String wildcardToRegex(String wildcard)
        {
            String expression = "^.*";
            for (int i = 0; i < wildcard.Length; i++)
            {
                switch (wildcard[i])
                {
                    case '?':
                        expression += '.'; // regular expression notation.
                        //mIsWild = true;
                        break;
                    case '*':
                        expression += ".*";
                        //mIsWild = true;
                        break;
                    case '.':
                        expression += "\\";
                        expression += wildcard[i];
                        break;
                    case ';':
                        expression += "|";
                        //mIsWild = true;
                        break;
                    default:
                        expression += wildcard[i];
                        break;
                }
            }
            return expression;
        }

        private String fixPathDir(String path)
        {
            path = fixPathFile(path);
            path += "/";
            return path;
        }

        private String fixPathFile(String path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return path;
            }

            //Fix up any ../ sections to point to the upper directory.
	        String[] splitPath = path.Split(SEPS, StringSplitOptions.RemoveEmptyEntries);
	        int lenMinusOne = splitPath.Length - 1;
	        StringBuilder pathString = new StringBuilder(path.Length);
	        for(int i = 0; i < lenMinusOne; ++i)
	        {
		        if(splitPath[i+1] != "..")
		        {
			        pathString.Append(splitPath[i]);
			        pathString.Append("/");
		        }
		        else
		        {
			        ++i;
		        }
	        }
	        if(splitPath[lenMinusOne] != "..")
	        {
		        pathString.Append(splitPath[lenMinusOne]);
	        }
	        return pathString.ToString();
        }

        private unsafe void commonLoad()
        {
            pooledZzipDirHandles = new LifecycleObjectPool<PooledZzipDir>(() => new PooledZzipDir(file), dir => dir.Dispose());
            pooledZzipDirHandles.MaxPoolSize = 3;
            PooledZzipDir zzipDir = pooledZzipDirHandles.getPooledObject();

            HashSet<String> foundDirectories = new HashSet<string>();

            //Read the directories and files out of the zip file
            ZZipStat zzipEntry = new ZZipStat();
            while (ZipFile_Read(zzipDir.Ptr, &zzipEntry))
            {
                String entryName = zzipEntry.Name;
                if (fileFilter == null || entryName.StartsWith(fileFilter))
                {
                    ZipFileInfo fileInfo = new ZipFileInfo(entryName, zzipEntry.CompressedSize, zzipEntry.UncompressedSize);
                    //Make sure we don't end with a /
                    if (fileInfo.IsDirectory)
                    {
                        if (!foundDirectories.Contains(fileInfo.FullName))
                        {
                            directories.Add(fileInfo);
                            foundDirectories.Add(fileInfo.FullName);
                        }
                    }
                    else
                    {
                        files.Add(fileInfo);
                    }
                    addParentDirectories(foundDirectories, fileInfo);
                }
            }

            zzipDir.finished();
        }

        private void addParentDirectories(HashSet<String> foundDirectories, ZipFileInfo fileInfo)
        {
            ZipFileInfo currentInfo = fileInfo;
            String currentDirectory = currentInfo.DirectoryName;
            int lastIndex = currentDirectory.LastIndexOf('/', currentDirectory.Length - 1);
            while (lastIndex != -1)
            {
                currentDirectory = currentDirectory.Substring(0, lastIndex + 1);
                if (!foundDirectories.Contains(currentDirectory))
                {
                    directories.Add(new ZipFileInfo(currentDirectory, 0, 0));
                    foundDirectories.Add(currentDirectory);
                    lastIndex = currentDirectory.LastIndexOf('/', currentDirectory.Length - 2);
                }
                else
                {
                    lastIndex = -1;
                }
            }
        }

        [DllImport(ZipLibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        internal static extern IntPtr ZipFile_OpenDir(String file, ref ZZipError error);

        [DllImport(ZipLibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        internal static extern void ZipFile_CloseDir(IntPtr zzipFile);

        [DllImport(ZipLibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static unsafe extern bool ZipFile_Read(IntPtr zzipDir, ZZipStat* zstat);

        [DllImport(ZipLibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        internal static unsafe extern ZZipError ZipFile_DirStat(IntPtr zzipDir, String filename, ZZipStat* zstat, int mode);

        [DllImport(ZipLibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        internal static extern IntPtr ZipFile_OpenFile(IntPtr zzipDir, String filename, ZZipFlags mode);
    }
}
