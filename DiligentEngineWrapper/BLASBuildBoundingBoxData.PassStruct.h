#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

namespace Diligent 
{
struct BLASBuildBoundingBoxDataPassStruct
{
        char* GeometryName;
        IBuffer* pBoxBuffer;
        Uint32 BoxOffset;
        Uint32 BoxStride;
        Uint32 BoxCount;
        RAYTRACING_GEOMETRY_FLAGS Flags;
};
}