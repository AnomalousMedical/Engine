using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgrePlugin
{
    class EmbeddedScalableResourceArchiveFactory : OgreManagedArchiveFactory
    {
        public EmbeddedScalableResourceArchiveFactory()
            : base("EmbeddedScalableResource")
        {

        }

        protected override OgreManagedArchive doCreateInstance(string name)
        {
            return new ScalableResourceArchive(new EmbeddedResourceArchive(name, "EmbeddedResource"), name, "EmbeddedScalableResource");
        }
    }
}
