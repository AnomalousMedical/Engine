using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgrePlugin
{
    class OgreScalableEngineArchiveFactory : OgreManagedArchiveFactory
    {
        public OgreScalableEngineArchiveFactory()
            :base("ScalableEngineArchive")
        {

        }

        protected override OgreManagedArchive doCreateInstance(string name)
        {
            return new ScalableResourceArchive(new OgreEngineArchive(name, "EngineArchive"), name, "ScalableEngineArchive");
        }
    }
}
