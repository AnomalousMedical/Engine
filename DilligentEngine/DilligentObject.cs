using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DilligentEngine
{
    public class DilligentObject : IDisposable
    {
        internal protected IntPtr objPtr;

        public DilligentObject(IntPtr objPtr)
        {
            this.objPtr = objPtr;
        }
        public virtual void Dispose()
        {
            DilligentObject_Release(objPtr);
        }

        public IntPtr ObjPtr => objPtr;

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void DilligentObject_Release(IntPtr obj);
    }
}
