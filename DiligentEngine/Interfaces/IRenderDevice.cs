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

namespace DiligentEngine
{
    public partial class IRenderDevice :  IObject
    {
        public IRenderDevice(IntPtr objPtr)
            : base(objPtr)
        {

        }
        public IShader CreateShader(ShaderCreateInfo ShaderCI)
        {
            return new IShader(IRenderDevice_CreateShader(
                this.objPtr
                , ShaderCI.FilePath
                , ShaderCI.Source
                , ShaderCI.EntryPoint
                , ShaderCI.Desc
            ));
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr IRenderDevice_CreateShader(
            IntPtr objPtr
            , String ShaderCI_FilePath
            , String ShaderCI_Source
            , String ShaderCI_EntryPoint
            , ShaderDesc ShaderCI_Desc
        );
    }
}
