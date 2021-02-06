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

namespace DiligentEngine
{
    public enum SHADER_TYPE :  Uint32
    {
        SHADER_TYPE_UNKNOWN = 0x0000,
        SHADER_TYPE_VERTEX = 0x0001,
        SHADER_TYPE_PIXEL = 0x0002,
        SHADER_TYPE_GEOMETRY = 0x0004,
        SHADER_TYPE_HULL = 0x0008,
        SHADER_TYPE_DOMAIN = 0x0010,
        SHADER_TYPE_COMPUTE = 0x0020,
        SHADER_TYPE_AMPLIFICATION = 0x0040,
        SHADER_TYPE_MESH = 0x0080,
        SHADER_TYPE_RAY_GEN = 0x0100,
        SHADER_TYPE_RAY_MISS = 0x0200,
        SHADER_TYPE_RAY_CLOSEST_HIT = 0x0400,
        SHADER_TYPE_RAY_ANY_HIT = 0x0800,
        SHADER_TYPE_RAY_INTERSECTION = 0x1000,
        SHADER_TYPE_CALLABLE = 0x2000,
        SHADER_TYPE_LAST = SHADER_TYPE_CALLABLE,
    }
}
