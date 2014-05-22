using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    public class OgreArchiveManager
    {
        static OgreArchiveManager instance = null;

        public static OgreArchiveManager getInstance()
        {
            if (instance == null)
            {
                instance = new OgreArchiveManager();
            }
            return instance;
        }

        private OgreArchiveManager()
        {

        }

        public void unload(String filename)
        {
            ArchiveManager_unload(filename);
        }

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ArchiveManager_unload(String filename);
    }
}
