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
    public enum RAYTRACING_BUILD_AS_FLAGS :  Uint8
    {
        RAYTRACING_BUILD_AS_NONE = 0,
        RAYTRACING_BUILD_AS_ALLOW_UPDATE = 0x01,
        RAYTRACING_BUILD_AS_ALLOW_COMPACTION = 0x02,
        RAYTRACING_BUILD_AS_PREFER_FAST_TRACE = 0x04,
        RAYTRACING_BUILD_AS_PREFER_FAST_BUILD = 0x08,
        RAYTRACING_BUILD_AS_LOW_MEMORY = 0x10,
        RAYTRACING_BUILD_AS_FLAGS_LAST = RAYTRACING_BUILD_AS_LOW_MEMORY,
    }
}
