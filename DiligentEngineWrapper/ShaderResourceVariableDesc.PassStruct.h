#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

namespace Diligent 
{
struct ShaderResourceVariableDescPassStruct
{
        SHADER_TYPE ShaderStages;
        Char* Name;
        SHADER_RESOURCE_VARIABLE_TYPE Type;
};
}