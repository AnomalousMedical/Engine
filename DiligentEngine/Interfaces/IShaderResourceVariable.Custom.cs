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
    /// Shader resource variable
    /// </summary>
    public partial class IShaderResourceVariable :  IObject
    {
        /// <summary>
        /// Binds resource array to the variable
        /// \param [in] ppObjects    - pointer to the array of objects
        /// \param [in] FirstElement - first array element to set
        /// \param [in] NumElements  - number of objects in ppObjects array
        /// 
        /// \remark The method performs run-time correctness checks.
        /// For instance, shader resource view cannot
        /// be assigned to a constant buffer variable.
        /// </summary>
        public void SetArray(List<IDeviceObject> ppObjects, Uint32 FirstElement = 0)
        {
            IShaderResourceVariable_SetArray(
                this.objPtr
                , ppObjects.Select(i => i.objPtr).ToArray()
                , FirstElement
                , (Uint32)ppObjects.Count
            );
        }

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IShaderResourceVariable_SetArray(
            IntPtr objPtr
            , IntPtr[] ppObjects
            , Uint32 FirstElement
            , Uint32 NumElements
        );
    }
}
