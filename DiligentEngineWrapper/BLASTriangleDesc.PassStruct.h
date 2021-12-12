#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

namespace Diligent 
{
struct BLASTriangleDescPassStruct
{
        char* GeometryName;
        Uint32 MaxVertexCount;
        VALUE_TYPE VertexValueType;
        Uint8 VertexComponentCount;
        Uint32 MaxPrimitiveCount;
        VALUE_TYPE IndexType;
        Uint32 AllowsTransforms;
};
}