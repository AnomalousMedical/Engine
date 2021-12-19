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
    [StructLayout(LayoutKind.Sequential)]
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
        public float2 Padding2;

        // Reflection sphere properties
        public float3 SphereReflectionColorMask;
        public int SphereReflectionBlur;

        // Refraction cube properties
        public float3 GlassReflectionColorMask;
        public float GlassAbsorption;
        public float4 GlassMaterialColor;
        public float2 GlassIndexOfRefraction;  // min and max IOR
        public int GlassEnableDispersion;
        public uint DispersionSampleCount; // 1..16
        public float4 DispersionSamples_0; // [rgb color] [IOR scale]
        public float4 DispersionSamples_1;
        public float4 DispersionSamples_2;
        public float4 DispersionSamples_3;
        public float4 DispersionSamples_4;
        public float4 DispersionSamples_5;
        public float4 DispersionSamples_6;
        public float4 DispersionSamples_7;
        public float4 DispersionSamples_8;
        public float4 DispersionSamples_9;
        public float4 DispersionSamples_10;
        public float4 DispersionSamples_11;
        public float4 DispersionSamples_12;
        public float4 DispersionSamples_13;
        public float4 DispersionSamples_14;
        public float4 DispersionSamples_15;

        public float4 DiscPoints_0; // packed float2[16]
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

                // Sphere constants.
                SphereReflectionColorMask = new Vector3(0.81f, 1.0f, 0.45f),
                SphereReflectionBlur = 1,

                // Glass cube constants.
                GlassReflectionColorMask = new Vector3(0.22f, 0.83f, 0.93f),
                GlassAbsorption = 0.5f,
                GlassMaterialColor = new Vector4(0.33f, 0.93f, 0.29f, 0f),
                GlassIndexOfRefraction = new Vector2(1.5f, 1.02f),
                GlassEnableDispersion = 0,

                // Wavelength to RGB and index of refraction interpolation factor.
                DispersionSamples_0 = new Vector4(0.140000f, 0.000000f, 0.266667f, 0.53f),
                DispersionSamples_1 = new Vector4(0.130031f, 0.037556f, 0.612267f, 0.25f),
                DispersionSamples_2 = new Vector4(0.100123f, 0.213556f, 0.785067f, 0.16f),
                DispersionSamples_3 = new Vector4(0.050277f, 0.533556f, 0.785067f, 0.00f),
                DispersionSamples_4 = new Vector4(0.000000f, 0.843297f, 0.619682f, 0.13f),
                DispersionSamples_5 = new Vector4(0.000000f, 0.927410f, 0.431834f, 0.38f),
                DispersionSamples_6 = new Vector4(0.000000f, 0.972325f, 0.270893f, 0.27f),
                DispersionSamples_7 = new Vector4(0.000000f, 0.978042f, 0.136858f, 0.19f),
                DispersionSamples_8 = new Vector4(0.324000f, 0.944560f, 0.029730f, 0.47f),
                DispersionSamples_9 = new Vector4(0.777600f, 0.871879f, 0.000000f, 0.64f),
                DispersionSamples_10 = new Vector4(0.972000f, 0.762222f, 0.000000f, 0.77f),
                DispersionSamples_11 = new Vector4(0.971835f, 0.482222f, 0.000000f, 0.62f),
                DispersionSamples_12 = new Vector4(0.886744f, 0.202222f, 0.000000f, 0.73f),
                DispersionSamples_13 = new Vector4(0.715967f, 0.000000f, 0.000000f, 0.68f),
                DispersionSamples_14 = new Vector4(0.459920f, 0.000000f, 0.000000f, 0.91f),
                DispersionSamples_15 = new Vector4(0.218000f, 0.000000f, 0.000000f, 0.99f),
                DispersionSampleCount = 4,

                AmbientColor = new Vector4(1f, 1f, 1f, 0f) * 0.015f,
                LightPos_0 = new Vector4(8.00f, -8.0f, +0.00f, 0f),
                LightColor_0 = new Vector4(1.00f, +0.8f, +0.80f, 0f),
                LightPos_1 = new Vector4(0.00f, -4.0f, -5.00f, 0f),
                LightColor_1 = new Vector4(0, +0, +0, 0f),

                // Random points on disc.
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
