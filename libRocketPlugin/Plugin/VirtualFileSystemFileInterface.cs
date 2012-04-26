using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace libRocketPlugin
{
    public class VirtualFileSystemFileInterface : ManagedFileInterface
    {
        public override System.IO.Stream Open(string path)
        {
            return VirtualFileSystem.Instance.openStream(path, Engine.Resources.FileMode.Open, Engine.Resources.FileAccess.Read);
            //return new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        }
    }
}
