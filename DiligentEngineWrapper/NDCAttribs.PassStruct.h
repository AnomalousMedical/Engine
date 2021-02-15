#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

namespace Diligent 
{
struct NDCAttribsPassStruct
{
        float MinZ;
        float ZtoDepthScale;
        float YtoVScale;
};
}