using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    public abstract class SystemInterface : IDisposable
    {
        protected IntPtr systemInterfacePtr;

        public abstract void Dispose();

        internal IntPtr Ptr
        {
            get
            {
                return systemInterfacePtr;
            }
        }
    }
}
