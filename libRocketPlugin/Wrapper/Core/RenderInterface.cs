using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    public abstract class RenderInterface : IDisposable
    {
        protected IntPtr renderInterface;

        public abstract void Dispose();

        internal IntPtr Ptr
        {
            get
            {
                return renderInterface;
            }
        }
    }
}
