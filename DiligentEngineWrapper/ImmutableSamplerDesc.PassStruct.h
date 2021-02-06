#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

namespace Diligent 
{
struct ImmutableSamplerDescPassStruct
{
        SHADER_TYPE ShaderStages;
        Char* SamplerOrTextureName;
        FILTER_TYPE Desc_MinFilter;
        FILTER_TYPE Desc_MagFilter;
        FILTER_TYPE Desc_MipFilter;
        TEXTURE_ADDRESS_MODE Desc_AddressU;
        TEXTURE_ADDRESS_MODE Desc_AddressV;
        TEXTURE_ADDRESS_MODE Desc_AddressW;
        Float32 Desc_MipLODBias;
        Uint32 Desc_MaxAnisotropy;
        COMPARISON_FUNCTION Desc_ComparisonFunc;
        Float32 Desc_BorderColor[4];
        float Desc_MinLOD;
        float Desc_MaxLOD;
};
}