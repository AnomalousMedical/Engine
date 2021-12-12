#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

namespace Diligent 
{
struct TLASBuildInstanceDataPassStruct
{
        char* InstanceName;
        IBottomLevelAS* pBLAS;
        InstanceMatrix Transform;
        Uint32 CustomId;
        RAYTRACING_INSTANCE_FLAGS Flags;
        Uint8 Mask;
        Uint32 ContributionToHitGroupIndex;
};
}