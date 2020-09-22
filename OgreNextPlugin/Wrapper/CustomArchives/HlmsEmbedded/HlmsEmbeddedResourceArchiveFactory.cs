using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreNextPlugin
{
    class HlmsEmbeddedResourceArchiveFactory : OgreManagedArchiveFactory
    {
        public HlmsEmbeddedResourceArchiveFactory()
            : base("HlmsEmbeddedResource")
        {

        }

        protected override OgreManagedArchive doCreateInstance(string name)
        {
            return new HlmsEmbeddedResourceArchive(name, "HlmsEmbeddedResource");
        }
    }
}
