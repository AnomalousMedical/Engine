using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class IndexData : IDisposable
    {
        IntPtr indexData;

        internal IndexData(IntPtr indexData)
        {
            this.indexData = indexData;
        }

        public void Dispose()
        {
            indexData = IntPtr.Zero;
        }
    }
}
