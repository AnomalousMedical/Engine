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
    struct BLASBuildTriangleDataPassStruct
    {
        public String GeometryName;
        public IntPtr pVertexBuffer;
        public Uint32 VertexOffset;
        public Uint32 VertexStride;
        public Uint32 VertexCount;
        public VALUE_TYPE VertexValueType;
        public Uint8 VertexComponentCount;
        public Uint32 PrimitiveCount;
        public IntPtr pIndexBuffer;
        public Uint32 IndexOffset;
        public VALUE_TYPE IndexType;
        public IntPtr pTransformBuffer;
        public Uint32 TransformBufferOffset;
        public RAYTRACING_GEOMETRY_FLAGS Flags;
        public static BLASBuildTriangleDataPassStruct[] ToStruct(IEnumerable<BLASBuildTriangleData> vals)
        {
            if(vals == null)
            {
                return null;
            }

            return vals.Select(i => new BLASBuildTriangleDataPassStruct
            {
                GeometryName = i.GeometryName,
                pVertexBuffer = i.pVertexBuffer == null ? IntPtr.Zero : i.pVertexBuffer.objPtr,
                VertexOffset = i.VertexOffset,
                VertexStride = i.VertexStride,
                VertexCount = i.VertexCount,
                VertexValueType = i.VertexValueType,
                VertexComponentCount = i.VertexComponentCount,
                PrimitiveCount = i.PrimitiveCount,
                pIndexBuffer = i.pIndexBuffer == null ? IntPtr.Zero : i.pIndexBuffer.objPtr,
                IndexOffset = i.IndexOffset,
                IndexType = i.IndexType,
                pTransformBuffer = i.pTransformBuffer == null ? IntPtr.Zero : i.pTransformBuffer.objPtr,
                TransformBufferOffset = i.TransformBufferOffset,
                Flags = i.Flags,
            }).ToArray();
        }
    }
}
