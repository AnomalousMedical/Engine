using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DiligentEngine
{
    class StringManager : IDisposable
    {
        protected internal IntPtr objPtr;

        public StringManager()
        {
            this.objPtr = StringManager_Create();
        }

        public void Dispose()
        {
            StringManager_Delete(this.objPtr);
        }

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr StringManager_Create();

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void StringManager_Delete(IntPtr obj);
    }
}
