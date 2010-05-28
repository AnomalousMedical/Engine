using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    public class Pass : IDisposable
    {
        internal static Pass createWrapper(IntPtr nativePtr, object[] args)
        {
            return new Pass(nativePtr, args[0] as Technique);
        }

        IntPtr pass;
        Technique parent;

        private Pass(IntPtr pass, Technique parent)
        {
            this.pass = pass;
            this.parent = parent;
        }

        public void Dispose()
        {
            pass = IntPtr.Zero;
        }
    }
}
