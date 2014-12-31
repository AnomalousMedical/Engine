using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgrePlugin;

namespace libRocketPlugin
{
    class CommonResourcesArchiveFactory : OgreManagedArchiveFactory
    {
        public const String Name = "LibrocketOgreCommonResourcesArchiveFactory";

        public CommonResourcesArchiveFactory()
            : base(Name)
        {

        }

        protected override OgreManagedArchive doCreateInstance(string name)
        {
            CommonResourcesArchive archive = new CommonResourcesArchive(name, Name);
            return archive;
        }
    }
}
