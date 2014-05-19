using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if ENABLE_LEGACY_SHIMS
namespace System.IO
{
    public static class Directory
    {
        private static DirectoryImpl dir;

        public static DirectoryImpl DirImpl
        {
            get
            {
                return dir;
            }

            set
            {
                dir = value;
            }
        }

        public static bool Exists(String path)
        {
            return dir.Exists(path);
        }

        public static void CreateDirectory(String path)
        {
            dir.CreateDirectory(path);
        }
    }

    public interface DirectoryImpl
    {
        bool Exists(String path);

        void CreateDirectory(String path);
    }
}
#endif