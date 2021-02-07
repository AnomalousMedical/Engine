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
using float2 = Engine.Vector2;
using float4x4 = Engine.Matrix4x4;
using BOOL = System.Boolean;

namespace DiligentEngine
{
    /// <summary>
    /// Pipeline state interface
    /// </summary>
    public partial class IPipelineState :  IDeviceObject
    {
        public IPipelineState(IntPtr objPtr)
            : base(objPtr)
        {

        }
        /// <summary>
        /// Returns static shader resource variable. If the variable is not found,
        /// returns nullptr.
        /// \param [in] ShaderType - Type of the shader to look up the variable.
        /// Must be one of Diligent::SHADER_TYPE.
        /// \param [in] Name - Name of the variable.
        /// \remark The method does not increment the reference counter
        /// of the returned interface.
        /// </summary>
        public IShaderResourceVariable GetStaticVariableByName(SHADER_TYPE ShaderType, String Name)
        {
            var theReturnValue = 
            IPipelineState_GetStaticVariableByName(
                this.objPtr
                , ShaderType
                , Name
            );
            return theReturnValue != IntPtr.Zero ? new IShaderResourceVariable(theReturnValue) : null;
        }
        /// <summary>
        /// Creates a shader resource binding object
        /// \param [out] ppShaderResourceBinding - memory location where pointer to the new shader resource
        /// binding object is written.
        /// \param [in] InitStaticResources      - if set to true, the method will initialize static resources in
        /// the created object, which has the exact same effect as calling
        /// IShaderResourceBinding::InitializeStaticResources().
        /// </summary>
        public AutoPtr<IShaderResourceBinding> CreateShaderResourceBinding(bool InitStaticResources)
        {
            var theReturnValue = 
            IPipelineState_CreateShaderResourceBinding(
                this.objPtr
                , InitStaticResources
            );
            return theReturnValue != IntPtr.Zero ? new AutoPtr<IShaderResourceBinding>(new IShaderResourceBinding(theReturnValue), false) : null;
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
