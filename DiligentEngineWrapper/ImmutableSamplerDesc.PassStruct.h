#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

namespace Diligent 
{
struct ImmutableSamplerDescPassStruct
{
        SHADER_TYPE ShaderStages;
        Char* SamplerOrTextureName;
        SamplerDesc Desc;
};
}