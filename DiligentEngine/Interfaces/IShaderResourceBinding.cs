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
using float4 = Engine.Vector4;
using float3 = Engine.Vector3;

namespace DiligentEngine
{
    /// <summary>
    /// Shader resource binding interface
    /// </summary>
    public partial class IShaderResourceBinding :  IObject
    {
        public IShaderResourceBinding(IntPtr objPtr)
            : base(objPtr)
        {

        }
        /// <summary>
        /// Returns variable
        /// \param [in] ShaderType - Type of the shader to look up the variable.
        /// Must be one of Diligent::SHADER_TYPE.
        /// \param [in] Name       - Variable name
        /// 
        /// \note  This operation may potentially be expensive. If the variable will be used often, it is
        /// recommended to store and reuse the pointer as it never changes.
        /// </summary>
        public IShaderResourceVariable GetVariableByName(SHADER_TYPE ShaderType, String Name)
        {
            var theReturnValue = 
            IShaderResourceBinding_GetVariableByName(
                this.objPtr
                , ShaderType
                , Name
            );
            return new IShaderResourceVariable(theReturnValue);
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr IShaderResourceBinding_GetVariableByName(
            IntPtr objPtr
            , SHADER_TYPE ShaderType
            , String Name
        );
    }
}
