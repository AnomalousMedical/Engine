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
    public partial class ShadowMapAttribs
    {

        public ShadowMapAttribs()
        {
            
        }
        public float4x4 mWorldToLightViewT { get; set; }
        public float4 f4ShadowMapDim { get; set; }
        public int iNumCascades { get; set; } = 0;
        public float fNumCascades { get; set; } = 0;
        public BOOL bVisualizeCascades { get; set; } = false;
        public BOOL bVisualizeShadowing { get; set; } = false;
        public float fReceiverPlaneDepthBiasClamp { get; set; } = 10;
        public float fFixedDepthBias { get; set; } = 1e-5f;
        public float fCascadeTransitionRegion { get; set; } = 0.1f;
        public int iMaxAnisotropy { get; set; } = 4;
        public float fVSMBias { get; set; } = 1e-4f;
        public float fVSMLightBleedingReduction { get; set; } = 0;
        public float fEVSMPositiveExponent { get; set; } = 40;
        public float fEVSMNegativeExponent { get; set; } = 5;
        public BOOL bIs32BitEVSM { get; set; } = true;
        public int iFixedFilterSize { get; set; } = 3;
        public float fFilterWorldSize { get; set; } = 0;
        public bool fDummy { get; set; }


    }
}
