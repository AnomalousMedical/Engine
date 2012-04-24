using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    public class RocketNativeObject
    {
        protected IntPtr ptr;

        /// <summary>
        /// Default constructor, if you call this be sure to call setPtr at some point.
        /// </summary>
        protected RocketNativeObject()
        {

        }

        protected RocketNativeObject(IntPtr ptr)
        {
            this.ptr = ptr;
        }

        protected void setPtr(IntPtr ptr)
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
