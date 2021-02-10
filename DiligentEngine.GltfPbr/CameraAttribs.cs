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

namespace DiligentEngine.GltfPbr
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CameraAttribs
    {
        public float4 f4Position { get; set; }
        public float4 f4ViewportSize { get; set; }
        public float2 f2ViewportOrigin { get; set; }
        public float fNearPlaneZ { get; set; }
        public float fFarPlaneZ { get; set; }
        public float4x4 mViewT { get; set; }
        public float4x4 mProjT { get; set; }
        public float4x4 mViewProjT { get; set; }
        public float4x4 mViewInvT { get; set; }
        public float4x4 mProjInvT { get; set; }
        public float4x4 mViewProjInvT { get; set; }
        public float4 f4ExtraData_0 { get; set; }
        public float4 f4ExtraData_1 { get; set; }
        public float4 f4ExtraData_2 { get; set; }
        public float4 f4ExtraData_3 { get; set; }
        public float4 f4ExtraData_4 { get; set; }


    }
}
