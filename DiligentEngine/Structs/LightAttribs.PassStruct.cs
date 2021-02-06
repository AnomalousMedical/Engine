using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

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
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct LightAttribsPassStruct
    {
        public float4 f4Direction;
        public float4 f4AmbientLight;
        public float4 f4Intensity;
        public float4x4 ShadowAttribs_mWorldToLightViewT;
        public float4 ShadowAttribs_f4ShadowMapDim;
        public int ShadowAttribs_iNumCascades;
        public float ShadowAttribs_fNumCascades;
        public BOOL ShadowAttribs_bVisualizeCascades;
        public BOOL ShadowAttribs_bVisualizeShadowing;
        public float ShadowAttribs_fReceiverPlaneDepthBiasClamp;
        public float ShadowAttribs_fFixedDepthBias;
        public float ShadowAttribs_fCascadeTransitionRegion;
        public int ShadowAttribs_iMaxAnisotropy;
        public float ShadowAttribs_fVSMBias;
        public float ShadowAttribs_fVSMLightBleedingReduction;
        public float ShadowAttribs_fEVSMPositiveExponent;
        public float ShadowAttribs_fEVSMNegativeExponent;
        public BOOL ShadowAttribs_bIs32BitEVSM;
        public int ShadowAttribs_iFixedFilterSize;
        public float ShadowAttribs_fFilterWorldSize;
        public Uint32 ShadowAttribs_fDummy;
        public static LightAttribsPassStruct[] ToStruct(IEnumerable<LightAttribs> vals)
        {
            if(vals == null)
            {
                return null;
            }

            return vals.Select(i => new LightAttribsPassStruct
            {
                f4Direction = i.f4Direction,
                f4AmbientLight = i.f4AmbientLight,
                f4Intensity = i.f4Intensity,
                ShadowAttribs_mWorldToLightViewT = i.ShadowAttribs.mWorldToLightViewT,
                ShadowAttribs_f4ShadowMapDim = i.ShadowAttribs.f4ShadowMapDim,
                ShadowAttribs_iNumCascades = i.ShadowAttribs.iNumCascades,
                ShadowAttribs_fNumCascades = i.ShadowAttribs.fNumCascades,
                ShadowAttribs_bVisualizeCascades = i.ShadowAttribs.bVisualizeCascades,
                ShadowAttribs_bVisualizeShadowing = i.ShadowAttribs.bVisualizeShadowing,
                ShadowAttribs_fReceiverPlaneDepthBiasClamp = i.ShadowAttribs.fReceiverPlaneDepthBiasClamp,
                ShadowAttribs_fFixedDepthBias = i.ShadowAttribs.fFixedDepthBias,
                ShadowAttribs_fCascadeTransitionRegion = i.ShadowAttribs.fCascadeTransitionRegion,
                ShadowAttribs_iMaxAnisotropy = i.ShadowAttribs.iMaxAnisotropy,
                ShadowAttribs_fVSMBias = i.ShadowAttribs.fVSMBias,
                ShadowAttribs_fVSMLightBleedingReduction = i.ShadowAttribs.fVSMLightBleedingReduction,
                ShadowAttribs_fEVSMPositiveExponent = i.ShadowAttribs.fEVSMPositiveExponent,
                ShadowAttribs_fEVSMNegativeExponent = i.ShadowAttribs.fEVSMNegativeExponent,
                ShadowAttribs_bIs32BitEVSM = i.ShadowAttribs.bIs32BitEVSM,
                ShadowAttribs_iFixedFilterSize = i.ShadowAttribs.iFixedFilterSize,
                ShadowAttribs_fFilterWorldSize = i.ShadowAttribs.fFilterWorldSize,
                ShadowAttribs_fDummy = Convert.ToUInt32(i.ShadowAttribs.fDummy),
            }).ToArray();
        }
    }
}
