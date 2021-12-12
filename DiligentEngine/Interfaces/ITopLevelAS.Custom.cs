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
    /// Defines the methods to manipulate a TLAS object
    /// </summary>
    public partial class ITopLevelAS :  IDeviceObject
    {
        public const Uint32 TLAS_INSTANCE_DATA_SIZE = 64;
        public const Uint32 TLAS_INSTANCE_OFFSET_AUTO = ~0u;

        public UInt32 ScratchBufferSizes_Build => ITopLevelAS_GetScratchBufferSizes_Build(this.objPtr);

        public UInt32 ScratchBufferSizes_Update => ITopLevelAS_GetScratchBufferSizes_Update(this.objPtr);


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern UInt32 ITopLevelAS_GetScratchBufferSizes_Build(
            IntPtr objPtr
        );


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern UInt32 ITopLevelAS_GetScratchBufferSizes_Update(
            IntPtr objPtr
        );
    }
}
