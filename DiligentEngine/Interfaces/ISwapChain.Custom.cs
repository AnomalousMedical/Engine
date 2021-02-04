using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Engine;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;
using Float32 = System.Single;
using Uint16 = System.UInt16;

namespace DiligentEngine
{
    public partial class ISwapChain
    {
        public TEXTURE_FORMAT GetDesc_ColorBufferFormat => ISwapChain_GetDesc_ColorBufferFormat(objPtr);

        public TEXTURE_FORMAT GetDesc_DepthBufferFormat => ISwapChain_GetDesc_DepthBufferFormat(objPtr);


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern TEXTURE_FORMAT ISwapChain_GetDesc_ColorBufferFormat(
            IntPtr objPtr
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern TEXTURE_FORMAT ISwapChain_GetDesc_DepthBufferFormat(
            IntPtr objPtr
        );
    }
}
