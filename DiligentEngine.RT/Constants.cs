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
using Engine;

namespace DiligentEngine.RT
{
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct Constants
    {
        // Camera world position
        public float4 CameraPos;

        // Near and far clip plane distances
        public float2 ClipPlanes;
        public float2 Padding0;

        // Camera view frustum corner rays
        public float4 FrustumRayLT;
        public float4 FrustumRayLB;
        public float4 FrustumRayRT;
        public float4 FrustumRayRB;


        // The number of shadow PCF samples
        public int ShadowPCF;
        // Maximum ray recursion depth
        public uint MaxRecursion;
        public float Darkness;
        public float Padding2;

        public float4 DiscPoints_0;
        public float4 DiscPoints_1;
        public float4 DiscPoints_2;
        public float4 DiscPoints_3;
        public float4 DiscPoints_4;
        public float4 DiscPoints_5;
        public float4 DiscPoints_6;
        public float4 DiscPoints_7;

        // Light properties
        public float4 AmbientColor;
        public float4 LightPos_0;
        public float4 LightPos_1;
        public float4 LightColor_0;
        public float4 LightColor_1;

        public static Constants CreateDefault(uint maxRecursionDepth)
        {
            return new Constants
            {
                ClipPlanes = new Vector2(0.1f, 100.0f),
                ShadowPCF = 1,
                MaxRecursion = Math.Min(6, maxRecursionDepth),

                AmbientColor = new Vector4(1f, 1f, 1f, 0f) * 0f,
                Darkness = 0.125f,
                LightPos_0 = new Vector4(8.00f, -8.0f, +0.00f, 0f),
                LightColor_0 = new Vector4(1.00f, +0.8f, +0.80f, 0f),
                LightPos_1 = new Vector4(0.00f, -4.0f, -5.00f, 0f),
                LightColor_1 = new Vector4(0.4f, +0.4f, +0.6f, 0f),

                // Random points on disc. packed float2[16]
                DiscPoints_0 = new Vector4(+0.0f, +0.0f, +0.9f, -0.9f),
                DiscPoints_1 = new Vector4(-0.8f, +1.0f, -1.1f, -0.8f),
                DiscPoints_2 = new Vector4(+1.5f, +1.2f, -2.1f, +0.7f),
                DiscPoints_3 = new Vector4(+0.1f, -2.2f, -0.2f, +2.4f),
                DiscPoints_4 = new Vector4(+2.4f, -0.3f, -3.0f, +2.8f),
                DiscPoints_5 = new Vector4(+2.0f, -2.6f, +0.7f, +3.5f),
                DiscPoints_6 = new Vector4(-3.2f, -1.6f, +3.4f, +2.2f),
                DiscPoints_7 = new Vector4(-1.8f, -3.2f, -1.1f, +3.6f),
            };
        }
    }
}
