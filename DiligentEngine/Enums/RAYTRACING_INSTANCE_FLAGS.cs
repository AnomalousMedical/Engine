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
    public enum RAYTRACING_INSTANCE_FLAGS :  Uint8
    {
        RAYTRACING_INSTANCE_NONE = 0,
        RAYTRACING_INSTANCE_TRIANGLE_FACING_CULL_DISABLE = 0x01,
        RAYTRACING_INSTANCE_TRIANGLE_FRONT_COUNTERCLOCKWISE = 0x02,
        RAYTRACING_INSTANCE_FORCE_OPAQUE = 0x04,
        RAYTRACING_INSTANCE_FORCE_NO_OPAQUE = 0x08,
        RAYTRACING_INSTANCE_FLAGS_LAST = RAYTRACING_INSTANCE_FORCE_NO_OPAQUE,
    }
}
