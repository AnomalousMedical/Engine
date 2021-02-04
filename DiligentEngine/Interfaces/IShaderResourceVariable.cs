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
using PVoid = System.IntPtr;

namespace DiligentEngine
{
    public partial class IShaderResourceVariable :  IObject
    {
        public IShaderResourceVariable(IntPtr objPtr)
            : base(objPtr)
        {

        }
        public void Set(IDeviceObject pObject)
        {
            IShaderResourceVariable_Set(
                this.objPtr
                , pObject.objPtr
            );
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IShaderResourceVariable_Set(
            IntPtr objPtr
            , IntPtr pObject
        );
    }
}
