using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DiligentEngine
{
    public class IObject
    {
        internal protected IntPtr objPtr;

        public IntPtr ObjPtr => objPtr;

        public IObject(IntPtr objPtr)
        {
            this.objPtr = objPtr;
        }
        internal void AddRef()
        {
            IObject_AddRef(objPtr);
        }

        internal void Release()
        {
            IObject_Release(objPtr);
        }

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IObject_AddRef(IntPtr obj);

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IObject_Release(IntPtr obj);
    }
}
