using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEGUIPlugin
{
    public abstract class CEGUIRenderer : IDisposable
    {
        public abstract void Dispose();

        internal abstract IntPtr Renderer { get; }

        internal abstract IntPtr ResourceProvider { get; }

        internal abstract IntPtr ImageCodec { get; }
    }
}
