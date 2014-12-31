using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgrePlugin;
using Engine;
using System.IO;

namespace libRocketPlugin
{
    public class RocketFilesystemArchive : OgreManagedArchive
    {
        public const String ArchiveName = "RocketFilesystemArchive";

        public RocketFilesystemArchive(String name, String archType)
            :base(name, archType)
        {
            
        }

        protected override void load()
        {
            
        }

        protected override void unload()
        {
            
        }

        protected override Stream doOpen(string filename)
        {
            return RocketInterface.Instance.FileInterface.Open(filename);
        }

        protected override void doList(bool recursive, bool dirs, IntPtr ogreStringVector)
        {
            
        }

        protected override void doListFileInfo(bool recursive, bool dirs, IntPtr ogreFileList, IntPtr archive)
        {
            
        }

        protected override void dofind(string pattern, bool recursive, bool dirs, IntPtr ogreStringVector)
        {
            
        }

        protected override void dofindFileInfo(string pattern, bool recursive, bool dirs, IntPtr ogreFileList, IntPtr archive)
        {
            
        }

        protected override bool exists(string filename)
        {
            return RocketInterface.Instance.FileInterface.Exists(filename);
        }
    }
}
