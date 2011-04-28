using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Anomaly
{
    class DirectoryUtility
    {
        private DirectoryUtility() { }

        public static void ForceDirectoryDelete(String path)
        {
            //Turn off read only on any files that have it.
            String[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            foreach (String file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            Directory.Delete(path, true);
        }
    }
}
