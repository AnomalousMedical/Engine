using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    public class Viewport : IDisposable
    {
        internal static Viewport createWrapper(IntPtr nativePtr, object[] args)
        {
            return new Viewport(nativePtr);
        }

        IntPtr viewport;

        private Viewport(IntPtr viewport)
        {
            this.viewport = viewport;
        }

        public void Dispose()
        {
            viewport = IntPtr.Zero;
        }

        internal IntPtr Viewport
        {
            get
            {
                return viewport;
            }
        }
    }
}
