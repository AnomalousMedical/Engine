#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

namespace Diligent 
{
struct BLASBoundingBoxDescPassStruct
{
        char* GeometryName;
        Uint32 MaxBoxCount;
};
}