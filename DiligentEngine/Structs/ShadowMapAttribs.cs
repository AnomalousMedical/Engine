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
        public int iNumCascades { get; set; }
        public float fNumCascades { get; set; }
        public BOOL bVisualizeCascades { get; set; }
        public BOOL bVisualizeShadowing { get; set; }
        public float fReceiverPlaneDepthBiasClamp { get; set; }
        public float fFixedDepthBias { get; set; }
        public float fCascadeTransitionRegion { get; set; }
        public int iMaxAnisotropy { get; set; }
        public float fVSMBias { get; set; }
        public float fVSMLightBleedingReduction { get; set; }
        public float fEVSMPositiveExponent { get; set; }
        public float fEVSMNegativeExponent { get; set; }
        public BOOL bIs32BitEVSM { get; set; }
        public int iFixedFilterSize { get; set; }
        public float fFilterWorldSize { get; set; }
        public bool fDummy { get; set; }


    }
}
