using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    class OgreEngineArchiveFactory : OgreManagedArchiveFactory
    {
        public OgreEngineArchiveFactory()
            :base("EngineArchive")
        {

        }

        protected override OgreManagedArchive doCreateInstance(string name)
        {
            return new OgreEngineArchive(name, "EngineArchive");
        }
    }
}
