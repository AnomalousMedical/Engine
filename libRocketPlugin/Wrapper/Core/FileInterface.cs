using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    public abstract class FileInterface : RocketNativeObject, IDisposable
    {
        private List<RocketFileSystemExtension> extensions = new List<RocketFileSystemExtension>();

        protected FileInterface()
        {

        }

        protected FileInterface(IntPtr ptr)
            :base(ptr)
        {

        }

        public virtual void Dispose()
        {

        }

        public abstract Stream Open(String path);

        public abstract bool Exists(String path);

        public abstract void addExtension(RocketFileSystemExtension extension);

        public abstract void removeExtension(RocketFileSystemExtension extension);
    }
}
