using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DilligentEngine
{
    public class DilligentStruct
    {
        internal protected IntPtr strPtr;

        public DilligentStruct(IntPtr strPtr)
        {
            this.strPtr = strPtr;
        }
        public virtual void Dispose()
        {
            DilligentStruct_Delete(strPtr);
        }

        public IntPtr ObjPtr => strPtr;

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void DilligentStruct_Delete(IntPtr obj);
    }
}
