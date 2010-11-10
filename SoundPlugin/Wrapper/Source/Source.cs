using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace SoundPlugin
{
    public class Source : SoundPluginObject, IDisposable
    {
        internal Source(IntPtr source)
            : base(source)
        {

        }

        public void Dispose()
        {

        }
    }
}
