using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

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
    public partial class BLASBuildBoundingBoxData
    {

        public BLASBuildBoundingBoxData()
        {
            
        }
        public String GeometryName { get; set; }
        public IBuffer pBoxBuffer { get; set; }
        public Uint32 BoxOffset { get; set; } = 0;
        public Uint32 BoxStride { get; set; } = 0;
        public Uint32 BoxCount { get; set; } = 0;
        public RAYTRACING_GEOMETRY_FLAGS Flags { get; set; } = RAYTRACING_GEOMETRY_FLAGS.RAYTRACING_GEOMETRY_FLAG_NONE;


    }
}
