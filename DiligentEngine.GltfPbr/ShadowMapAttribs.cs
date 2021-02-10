using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using BOOL = System.Int32; //The bools for these structs are defined as 4 byte see BasicStructures.fxh for more

namespace DiligentEngine.GltfPbr
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ShadowMapAttribs
    {
        public static readonly ShadowMapAttribs Default = new ShadowMapAttribs()
        {
            iNumCascades = 0,
            fNumCascades = 0,
            bVisualizeCascades = 0,
            bVisualizeShadowing = 0,
            fReceiverPlaneDepthBiasClamp = 10,
            fFixedDepthBias = 1e-5f,
            fCascadeTransitionRegion = 0.1f,
            iMaxAnisotropy = 4,
            fVSMBias = 1e-4f,
            fVSMLightBleedingReduction = 0,
            fEVSMPositiveExponent = 40,
            fEVSMNegativeExponent = 5,
            bIs32BitEVSM = 1,
            iFixedFilterSize = 3,
            fFilterWorldSize = 0,
        };

        public Matrix4x4 mWorldToLightViewT;
        public CascadeAttribs Cascades_0;
        public CascadeAttribs Cascades_1;
        public CascadeAttribs Cascades_2;
        public CascadeAttribs Cascades_3;
        public CascadeAttribs Cascades_4;
        public CascadeAttribs Cascades_5;
        public CascadeAttribs Cascades_6;
        public CascadeAttribs Cascades_7;
        public Matrix4x4 mWorldToShadowMapUVDepthT_0;
        public Matrix4x4 mWorldToShadowMapUVDepthT_1;
        public Matrix4x4 mWorldToShadowMapUVDepthT_2;
        public Matrix4x4 mWorldToShadowMapUVDepthT_3;
        public Matrix4x4 mWorldToShadowMapUVDepthT_4;
        public Matrix4x4 mWorldToShadowMapUVDepthT_5;
        public Matrix4x4 mWorldToShadowMapUVDepthT_6;
        public Matrix4x4 mWorldToShadowMapUVDepthT_7;
        public float fCascadeCamSpaceZEnd_0;
        public float fCascadeCamSpaceZEnd_1;
        public float fCascadeCamSpaceZEnd_2;
        public float fCascadeCamSpaceZEnd_3;
        public float fCascadeCamSpaceZEnd_4;
        public float fCascadeCamSpaceZEnd_5;
        public float fCascadeCamSpaceZEnd_6;
        public float fCascadeCamSpaceZEnd_7;
        public Vector4 f4ShadowMapDim;
        public int iNumCascades;
        public float fNumCascades;
        public BOOL bVisualizeCascades;
        public BOOL bVisualizeShadowing;
        public float fReceiverPlaneDepthBiasClamp;
        public float fFixedDepthBias;
        public float fCascadeTransitionRegion;
        public int iMaxAnisotropy;
        public float fVSMBias;
        public float fVSMLightBleedingReduction;
        public float fEVSMPositiveExponent;
        public float fEVSMNegativeExponent;
        public BOOL bIs32BitEVSM;
        public int iFixedFilterSize;
        public float fFilterWorldSize;
        public BOOL fDummy;
    }
}
