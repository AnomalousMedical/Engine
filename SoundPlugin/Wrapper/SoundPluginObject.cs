using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundPlugin
{
    /// <summary>
    /// Base class for all sound plugin objects.
    /// </summary>
    public abstract class SoundPluginObject
    {
        protected SoundPluginObject(IntPtr nativePtr)
        {
            Pointer = nativePtr;
        }

        internal virtual void delete()
        {
            Pointer = IntPtr.Zero;
        }

        internal IntPtr Pointer { get; private set; }

        protected bool IsNull
        {
            get
            {
                return Pointer == IntPtr.Zero;
            }
        }
    }
}
