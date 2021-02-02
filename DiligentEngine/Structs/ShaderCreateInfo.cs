using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;

namespace DiligentEngine
{
    public partial class ShaderCreateInfo : IDisposable
    {
        internal protected IntPtr objPtr;

        public IntPtr ObjPtr => objPtr;

        public ShaderCreateInfo()
        {
            this.objPtr = ShaderCreateInfo_Create();
        }

        public void Dispose()
        {
            ShaderCreateInfo_Delete(this.objPtr);
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ShaderCreateInfo_Create();

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ShaderCreateInfo_Delete(IntPtr obj);
    }
}
