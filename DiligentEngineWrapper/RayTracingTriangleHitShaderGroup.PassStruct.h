#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

namespace Diligent 
{
struct RayTracingTriangleHitShaderGroupPassStruct
{
        char* Name;
        IShader* pClosestHitShader;
        IShader* pAnyHitShader;
};
}