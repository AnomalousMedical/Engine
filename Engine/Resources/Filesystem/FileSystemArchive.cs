using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Logging;

namespace Engine.Resources
{
    class FileSystemArchive : Archive
    {
        private String baseDirectory;

        public FileSystemArchive(String baseDirectory)
        {
            this.baseDirectory = FileSystem.fixPathDir(Path.GetFullPath(baseDirectory));
            //Temp mac os fix
            if (!Directory.Exists(this.baseDirectory))
            {
                this.baseDirectory = "/" + this.baseDirectory;
            }
        }

        public override void Dispose()
        {

        }

        public override bool containsRealAbsolutePath(String path)
        {
            if (File.Exists(path))
            {
                return FileSystem.fixPathFile(path).StartsWith(baseDirectory, StringComparison.OrdinalIgnoreCase);
            }
            else if (Directory.Exists(path))
            {
                return FileSystem.fixPathDir(path).StartsWith(baseDirectory, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override IEnumerable<String> listFiles(bool recursive)
        {
            return listFiles(baseDirectory, recursive);
        }

        public override IEnumerable<String> listFiles(String url, bool recursive)
        {
            return listFiles(url, "*", recursive);
        }

        public override IEnumerable<String> listFiles(String url, String searchPattern, bool recursive)
        {
            foreach (String file in Directory.GetFiles(fixIncomingDirectoryURL(url), searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                yield return fixOutgoingFileString(file);
            }
        }

        public override IEnumerable<String> listDirectories(bool recursive)
        {
            return listDirectories(baseDirectory, recursive);
        }

        public override IEnumerable<String> listDirectories(String url, bool recursive)
        {
            foreach(String dir in Directory.GetDirectories(fixIncomingDirectoryURL(url), "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                yield return fixOutgoingDirectoryString(dir);
            }
        }

        public override IEnumerable<String> listDirectories(String url, bool recursive, bool includeHidden)
        {
            foreach(String dir in listDirectories(fixIncomingDirectoryURL(url), "*", recursive, includeHidden))
            {
                yield return fixOutgoingDirectoryString(dir);
            }
        }

        public override IEnumerable<String> listDirectories(String url, String searchPattern, bool recursive)
        {
            foreach(String dir in Directory.GetDirectories(fixIncomingDirectoryURL(url), searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                yield return fixOutgoingDirectoryString(dir);
            }
        }

        public override IEnumerable<String> listDirectories(String url, String searchPattern, bool recursive, bool includeHidden)
        {
            String[] directories = Directory.GetDirectories(fixIncomingDirectoryURL(url), searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            if (!includeHidden)
            {
                foreach (String dir in directories)
                {
                    FileAttributes attr = File.GetAttributes(dir);
                    if ((attr & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        yield return fixOutgoingDirectoryString(dir);
                    }
                }
            }
            else
            {
                foreach (String dir in directories)
                {
                    yield return fixOutgoingDirectoryString(dir);
                }
            }
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
            bool isDirectory;
            FileAttributes attr = File.GetAttributes(fixIncomingURL(url, out isDirectory));
            return isDirectory;
        }

        public override VirtualFileInfo getFileInfo(String filename)
        {
            bool isDirectory;
            String fixedUrl = fixIncomingURL(filename, out isDirectory);
            FileInfo info = new FileInfo(fixedUrl);
            if (isDirectory)
            {
                return new VirtualFileInfo(info.Name, fixOutgoingDirectoryString(info.DirectoryName), fixOutgoingDirectoryString(info.FullName), info.FullName, 0, 0);
            }
            else
            {
                return new VirtualFileInfo(info.Name, fixOutgoingDirectoryString(info.DirectoryName), fixOutgoingFileString(info.FullName), info.FullName, info.Length, info.Length);
            }
        }

        private String fixOutgoingFileString(String url)
        {
            String fixedUrl = FileSystem.fixPathFile(url);
            if(fixedUrl.StartsWith(baseDirectory))
            {
                fixedUrl = fixedUrl.Substring(baseDirectory.Length);
            }
                //OMG LOOK HERE
            else if (fixedUrl.StartsWith(baseDirectory.Substring(1))) //store this string in a buffer and reuse that
            {
                fixedUrl = fixedUrl.Substring(baseDirectory.Length - 1);
            }
            return fixedUrl;
        }

        private String fixOutgoingDirectoryString(String url)
        {
            String fixedUrl = FileSystem.fixPathDir(url).Replace(baseDirectory, "");
            if (fixedUrl.StartsWith(baseDirectory))
            {
                fixedUrl = fixedUrl.Substring(baseDirectory.Length);
            }
            //OMG LOOK HERE
            else if (fixedUrl.StartsWith(baseDirectory.Substring(1)))
            {
                fixedUrl = fixedUrl.Substring(baseDirectory.Length - 1);
            }
            return fixedUrl;
        }

        private String fixIncomingURL(String url, out bool isDirectory)
        {
            String asFile = fixIncomingFileURL(url);
            if (File.Exists(asFile))
            {
                isDirectory = false;
                return asFile;
            }
            String asDirectory = fixIncomingDirectoryURL(url);
            if (Directory.Exists(asDirectory))
            {
                isDirectory = true;
                return asDirectory;
            }
            throw new FileNotFoundException("Could not convert url.", url);
        }

        private String fixIncomingDirectoryURL(String url)
        {
            url = FileSystem.fixPathDir(url);

            //temp osx fix
            if (!Directory.Exists(url))
            {
                url = "/" + url;
            }
            //end

            if (url.StartsWith(baseDirectory))
            {
                url = url.Substring(baseDirectory.Length - 1);
            }
            if (url.StartsWith("/"))
            {
                url = url.Substring(1);
            }
            if (url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }
            return baseDirectory + url;
        }

        private String fixIncomingFileURL(String url)
        {
            url = FileSystem.fixPathFile(url);

            //temp osx fix
            if (!File.Exists(url))
            {
                url = "/" + url;
            }
            //end

            if (url.StartsWith(baseDirectory))
            {
                url = url.Substring(baseDirectory.Length - 1);
            }
            if (url.StartsWith("/"))
            {
                url = url.Substring(1);
            }			
			
            String finalPath = baseDirectory + url;
			
			//Check to see if the file exists, case sensitivity fix
			if(!File.Exists(finalPath))
			{
				String searchFolder = Path.GetDirectoryName(finalPath);
				String searchFile = Path.GetFileName(finalPath).ToUpperInvariant();
				String[] files = Directory.GetFiles(searchFolder);
				String matchingFile = null;
				foreach(String file in files)
				{
					String checkFile = Path.GetFileName(file);
					if(checkFile.ToUpperInvariant() == searchFile)
					{
						matchingFile = file;
						break;
					}
				}
				//If we found a matching file, update the path, otherwise use what was found earlier.
				if(matchingFile != null)
				{
					finalPath = matchingFile;	
				}
			}
			
			return finalPath;
        }
    }
}
