using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    public class Overlay : IDisposable
    {
        internal static Overlay createWrapper(IntPtr nativeObject, object[] args)
        {
            return new Overlay(nativeObject);
        }

        protected IntPtr overlay;

        protected Overlay(IntPtr overlay)
        {
            this.overlay = overlay;
        }

        public void Dispose()
        {
            overlay = IntPtr.Zero;
        }

        public IntPtr OgreObject
        {
            get
            {
                return overlay;
            }
        }
    }
}
