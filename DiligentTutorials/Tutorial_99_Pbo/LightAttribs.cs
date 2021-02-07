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

namespace Tutorial_99_Pbo
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LightAttribs
    {
        public static readonly LightAttribs Default = new LightAttribs()
        {
            f4Direction = new float4(0, 0, -1, 0),
            f4AmbientLight = new float4(0, 0, 0, 0),
            f4Intensity = new float4(1, 1, 1, 1),
            ShadowAttribs = new ShadowMapAttribs()
        };

        public float4 f4Direction;
        public float4 f4AmbientLight;
        public float4 f4Intensity;
        public ShadowMapAttribs ShadowAttribs;
    }
}
