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
        public String FilePath
        {
            get
            {
                return ShaderCreateInfo_Get_FilePath(this.objPtr);
            }
            set
            {
                ShaderCreateInfo_Set_FilePath(this.objPtr, value);
            }
        }
        public String Source
        {
            get
            {
                return ShaderCreateInfo_Get_Source(this.objPtr);
            }
            set
            {
                ShaderCreateInfo_Set_Source(this.objPtr, value);
            }
        }
        public String EntryPoint
        {
            get
            {
                return ShaderCreateInfo_Get_EntryPoint(this.objPtr);
            }
            set
            {
                ShaderCreateInfo_Set_EntryPoint(this.objPtr, value);
            }
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ShaderCreateInfo_Create();

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ShaderCreateInfo_Delete(IntPtr obj);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern String ShaderCreateInfo_Get_FilePath(IntPtr objPtr);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ShaderCreateInfo_Set_FilePath(IntPtr objPtr, String value);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern String ShaderCreateInfo_Get_Source(IntPtr objPtr);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ShaderCreateInfo_Set_Source(IntPtr objPtr, String value);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern String ShaderCreateInfo_Get_EntryPoint(IntPtr objPtr);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ShaderCreateInfo_Set_EntryPoint(IntPtr objPtr, String value);
    }
}
