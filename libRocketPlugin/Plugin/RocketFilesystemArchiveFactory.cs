using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgreWrapper;

namespace libRocketPlugin
{
    class RocketFilesystemArchiveFactory : OgreManagedArchiveFactory
    {
        public RocketFilesystemArchiveFactory()
            :base(RocketFilesystemArchive.ArchiveName)
        {

        }

        protected override OgreManagedArchive doCreateInstance(string name)
        {
            return new RocketFilesystemArchive(name, RocketFilesystemArchive.ArchiveName);
        }
    }
}
