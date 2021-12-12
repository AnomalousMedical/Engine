using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

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
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct BLASBuildBoundingBoxDataPassStruct
    {
        public String GeometryName;
        public IntPtr pBoxBuffer;
        public Uint32 BoxOffset;
        public Uint32 BoxStride;
        public Uint32 BoxCount;
        public RAYTRACING_GEOMETRY_FLAGS Flags;
        public static BLASBuildBoundingBoxDataPassStruct[] ToStruct(IEnumerable<BLASBuildBoundingBoxData> vals)
        {
            if(vals == null)
            {
                return null;
            }

            return vals.Select(i => new BLASBuildBoundingBoxDataPassStruct
            {
                GeometryName = i.GeometryName,
                pBoxBuffer = i.pBoxBuffer == null ? IntPtr.Zero : i.pBoxBuffer.objPtr,
                BoxOffset = i.BoxOffset,
                BoxStride = i.BoxStride,
                BoxCount = i.BoxCount,
                Flags = i.Flags,
            }).ToArray();
        }
    }
}
