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
            return defaultArchive;
        }

        public static void CloseArchive(Archive archive)
        {
            archive.close();
        }
    }
}
