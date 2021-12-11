#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

namespace Diligent 
{
struct RayTracingProceduralHitShaderGroupPassStruct
{
        char* Name;
        IShader* pIntersectionShader;
        IShader* pClosestHitShader;
        IShader* pAnyHitShader;
};
}