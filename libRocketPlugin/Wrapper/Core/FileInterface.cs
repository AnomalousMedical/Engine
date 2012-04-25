using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    public abstract class FileInterface : RocketNativeObject, IDisposable
    {
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
    }
}
