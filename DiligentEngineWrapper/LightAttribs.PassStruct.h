#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

namespace Diligent 
{
struct LightAttribsPassStruct
{
        float4 f4Direction;
        float4 f4AmbientLight;
        float4 f4Intensity;
        float4x4 ShadowAttribs_mWorldToLightViewT;
        CascadeAttribs ShadowAttribs_Cascades[0];
        float4x4 ShadowAttribs_mWorldToShadowMapUVDepthT[0];
        float ShadowAttribs_fCascadeCamSpaceZEnd[0];
        float4 ShadowAttribs_f4ShadowMapDim;
        int ShadowAttribs_iNumCascades;
        float ShadowAttribs_fNumCascades;
        BOOL ShadowAttribs_bVisualizeCascades;
        BOOL ShadowAttribs_bVisualizeShadowing;
        float ShadowAttribs_fReceiverPlaneDepthBiasClamp;
        float ShadowAttribs_fFixedDepthBias;
        float ShadowAttribs_fCascadeTransitionRegion;
        int ShadowAttribs_iMaxAnisotropy;
        float ShadowAttribs_fVSMBias;
        float ShadowAttribs_fVSMLightBleedingReduction;
        float ShadowAttribs_fEVSMPositiveExponent;
        float ShadowAttribs_fEVSMNegativeExponent;
        BOOL ShadowAttribs_bIs32BitEVSM;
        int ShadowAttribs_iFixedFilterSize;
        float ShadowAttribs_fFilterWorldSize;
        Uint32 ShadowAttribs_fDummy;
};
}