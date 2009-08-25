using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZipAccess;

namespace Engine.Resources
{
    class ZipArchive : Archive
    {
        private ZipFile zipFile = null;

        internal static bool CanOpenURL(String url)
        {
            return url.EndsWith(".zip");
        }

        public ZipArchive(String filename)
        {
            zipFile = new ZipFile(filename);
        }

        public override void Dispose()
        {
            if (zipFile != null)
            {
                zipFile.Dispose();
                zipFile = null;
            }
        }

        public override string[] listFiles(string url, bool recursive)
        {
            if (url.EndsWith(".zip"))
            {
                return zipFile.listFiles("", recursive).ToArray();
            }
            return zipFile.listFiles(url, recursive).ToArray();
        }

        public override String[] listFiles(String url, String searchPattern, bool recursive)
        {
            throw new NotImplementedException();
        }

        public override System.IO.Stream openStream(string url, FileMode mode)
        {
            return zipFile.openFile(url);
        }

        public override System.IO.Stream openStream(string url, FileMode mode, FileAccess access)
        {
            return zipFile.openFile(url);
        }

        public override bool isDirectory(string url)
        {
            return !url.Contains(".") || url.EndsWith(".zip");
        }
    }
}
