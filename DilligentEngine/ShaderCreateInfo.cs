using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DilligentEngine
{
    public class ShaderCreateInfo : IDisposable
    {
        private IntPtr ptr;

        public IntPtr ObjPtr => ptr;

        public ShaderCreateInfo()
        {
            this.ptr = ShaderCreateInfo_Create();
        }

        public void Dispose()
        {
            ShaderCreateInfo_Delete(this.ptr);
        }

        public void Lazy_VS()
        {
            Lazy_VS(this.ptr);
        }

        public void Lazy_PS()
        {
            Lazy_PS(this.ptr);
        }

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ShaderCreateInfo_Create();

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ShaderCreateInfo_Delete(IntPtr obj);

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Lazy_VS(IntPtr ShaderCI);

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Lazy_PS(IntPtr ShaderCI);
    }
}
