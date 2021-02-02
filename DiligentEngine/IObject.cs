using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DiligentEngine
{
    public class IObject : IDisposable
    {
        internal protected IntPtr objPtr;

        public IObject(IntPtr objPtr)
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
