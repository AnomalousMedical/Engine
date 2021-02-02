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
    public class ISwapChain :  IObject
    {
        public ISwapChain(IntPtr objPtr)
            : base(objPtr)
        {

        }
        public void Resize(Uint32 NewWidth, Uint32 NewHeight, SURFACE_TRANSFORM NewTransform)
        {
            ISwapChain_Resize(this.objPtr, NewWidth, NewHeight, NewTransform);
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ISwapChain_Resize(IntPtr objPtr, Uint32 NewWidth, Uint32 NewHeight, SURFACE_TRANSFORM NewTransform);
    }
}
