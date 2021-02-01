using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DilligentEngine
{
    public class IDeviceContext : DilligentObject
    {
        public IDeviceContext(IntPtr objPtr) : base(objPtr)
        {
        }

        public void Flush()
        {
            IDeviceContext_Flush(objPtr);
        }

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_Flush(IntPtr objPtr);
    }
}
