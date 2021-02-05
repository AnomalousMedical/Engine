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
    public partial class IPipelineState :  IDeviceObject
    {
        public IPipelineState(IntPtr objPtr)
            : base(objPtr)
        {

        }
        public IShaderResourceVariable GetStaticVariableByName(SHADER_TYPE ShaderType, String Name)
        {
            var theReturnValue = 
            IPipelineState_GetStaticVariableByName(
                this.objPtr
                , ShaderType
                , Name
            );
            return new IShaderResourceVariable(theReturnValue);
        }
        public IShaderResourceBinding CreateShaderResourceBinding(bool InitStaticResources)
        {
            var theReturnValue = 
            IPipelineState_CreateShaderResourceBinding(
                this.objPtr
                , InitStaticResources
            );
            return new IShaderResourceBinding(theReturnValue);
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr IPipelineState_GetStaticVariableByName(
            IntPtr objPtr
            , SHADER_TYPE ShaderType
            , String Name
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr IPipelineState_CreateShaderResourceBinding(
            IntPtr objPtr
            , [MarshalAs(UnmanagedType.I1)]bool InitStaticResources
        );
    }
}
