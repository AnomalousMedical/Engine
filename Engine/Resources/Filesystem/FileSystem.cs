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
        }

        internal static String fixPathFile(String path)
        {
            //Fix up any ../ sections to point to the upper directory.
            String[] splitPath = path.Split(SEPS, StringSplitOptions.RemoveEmptyEntries);
            int lenMinusOne = splitPath.Length - 1;
            StringBuilder pathString = new StringBuilder(path.Length);
            for (int i = 0; i < lenMinusOne; ++i)
            {
                if (splitPath[i + 1] != "..")
                {
                    pathString.Append(splitPath[i]);
                    pathString.Append("/");
                }
                else
                {
                    ++i;
                }
            }
            if (lenMinusOne == -1)
            {
                return "";
            }
            if (splitPath[lenMinusOne] != "..")
            {
                pathString.Append(splitPath[lenMinusOne]);
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
