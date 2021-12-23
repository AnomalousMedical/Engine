using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
using System.Runtime.InteropServices;

namespace DiligentEngine.RT.HLSL
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PrimaryRayPayload
    {
        float3 Color;
        float Depth;
        uint Recursion;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ShadowRayPayload
    {
        float Shading;   // 0 - fully shadowed, 1 - fully in light, 0..1 - for semi-transparent objects
        uint Recursion; // Current recusrsion depth
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct SpriteFrame
    {
        public float u1;
        public float v1;
        public float u2;
        public float v2;
        public float u3;
        public float v3;
        public float u4;
        public float v4;
    };
}
