using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    public class RocketNativeObject
    {
        protected IntPtr ptr;

        protected RocketNativeObject(IntPtr ptr)
        {
            this.ptr = ptr;
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
