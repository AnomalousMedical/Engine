using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Engine.Resources
{
    class FileSystem
    {
        static char[] SEPS = { '/', '\\' };

        internal static Archive OpenArchive(String url)
        {
#if !FIXLATER_DISABLED
            if (ZipArchive.CanOpenURL(url))
            {
                if (File.Exists(ZipArchive.parseZipName(url)))
                {
                    Archive zipArchive = new ZipArchive(url);
                    return zipArchive;
                }
            }
            else if(Directory.Exists(url) || File.Exists(url))
            {
                return new FileSystemArchive(url);
            }
            return null;
#else
            throw new NotImplementedException();
#endif
        }

        internal static String fixPathFile(String path)
        {
            //Fix up any ../ sections to point to the upper directory.
            String[] splitPath = path.Split(SEPS, StringSplitOptions.RemoveEmptyEntries);
            int length = splitPath.Length;
            Stack<String> pathStack = new Stack<string>(splitPath.Length);
            for (int i = 0; i < length; ++i)
            {
                if (splitPath[i] == "..")
                {
                    pathStack.Pop();
                }
                else
                {
                    pathStack.Push(splitPath[i]); 
                }
            }
            if (length == 0)
            {
                return "";
            }

            StringBuilder pathString = new StringBuilder(path.Length);
            String currentFormat = "{0}";
            foreach(var section in pathStack.Reverse())
            {
                pathString.AppendFormat(currentFormat, section);
                currentFormat = "/{0}";
            }
            return pathString.ToString();
        }

        internal static String fixPathDir(String path)
        {
            path = fixPathFile(path);
            path += "/";
            return path;
        }
    }
}
