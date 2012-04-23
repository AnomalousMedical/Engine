using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    public abstract class FileInterface
    {
        protected IntPtr ptr;

        public abstract void Dispose();

        internal IntPtr Ptr
        {
            get
            {
                return ptr;
            }
        }
    }
}
