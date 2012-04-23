using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    public class Context : IDisposable
    {
        private IntPtr ptr;

        public Context(IntPtr ptr)
        {
            this.ptr = ptr;
        }

        public void Dispose()
        {
            //Need to make mem management strategy for contexts
        }

        internal IntPtr Ptr
        {
            get
            {
                return ptr;
            }
        }
    }
}
