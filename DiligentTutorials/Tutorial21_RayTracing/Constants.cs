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

namespace DiligentEngineRayTracing
{
    [StructLayout(LayoutKind.Sequential)]
    struct Constants
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
    }
}
