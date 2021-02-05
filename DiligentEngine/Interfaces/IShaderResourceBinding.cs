using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
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
    public partial class IShaderResourceBinding :  IObject
    {
        public IShaderResourceBinding(IntPtr objPtr)
            : base(objPtr)
        {

        }
        public IShaderResourceVariable GetVariableByName(SHADER_TYPE ShaderType, String Name)
        {
            return new IShaderResourceVariable(IShaderResourceBinding_GetVariableByName(
                this.objPtr
                , ShaderType
                , Name
            ));
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr IShaderResourceBinding_GetVariableByName(
            IntPtr objPtr
            , SHADER_TYPE ShaderType
            , String Name
        );
    }
}
