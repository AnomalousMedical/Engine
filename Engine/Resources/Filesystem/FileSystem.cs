using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Engine.Resources
{
    public class FileSystem
    {
        private static Archive defaultArchive = new FileSystemArchive();

        public static Archive OpenArchive(String url)
        {
            if (ZipArchive.CanOpenURL(url))
            {
                Archive zipArchive = new ZipArchive(url);
                return zipArchive;
            }
            return defaultArchive;
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
    }
}
