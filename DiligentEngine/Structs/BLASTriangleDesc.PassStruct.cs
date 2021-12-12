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
    struct BLASTriangleDescPassStruct
    {
        public String GeometryName;
        public Uint32 MaxVertexCount;
        public VALUE_TYPE VertexValueType;
        public Uint8 VertexComponentCount;
        public Uint32 MaxPrimitiveCount;
        public VALUE_TYPE IndexType;
        public Uint32 AllowsTransforms;
        public static BLASTriangleDescPassStruct[] ToStruct(IEnumerable<BLASTriangleDesc> vals)
        {
            if(vals == null)
            {
                return null;
            }

            return vals.Select(i => new BLASTriangleDescPassStruct
            {
                GeometryName = i.GeometryName,
                MaxVertexCount = i.MaxVertexCount,
                VertexValueType = i.VertexValueType,
                VertexComponentCount = i.VertexComponentCount,
                MaxPrimitiveCount = i.MaxPrimitiveCount,
                IndexType = i.IndexType,
                AllowsTransforms = Convert.ToUInt32(i.AllowsTransforms),
            }).ToArray();
        }
    }
}
