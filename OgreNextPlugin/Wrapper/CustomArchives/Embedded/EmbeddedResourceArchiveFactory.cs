using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreNextPlugin
{
    class EmbeddedResourceArchiveFactory : OgreManagedArchiveFactory
    {
        public EmbeddedResourceArchiveFactory()
            : base("EmbeddedResource")
        {

        }

        protected override OgreManagedArchive doCreateInstance(string name)
        {
            return new EmbeddedResourceArchive(name, "EmbeddedResource");
        }
    }
}
