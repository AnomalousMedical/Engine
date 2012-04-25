using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    public class VirtualFileSystemFileInterface : ManagedFileInterface
    {
        public override System.IO.Stream Open(string path)
        {
            return new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        }
    }
}
