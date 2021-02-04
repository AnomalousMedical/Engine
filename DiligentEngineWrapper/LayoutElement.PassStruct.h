#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

namespace Diligent 
{
struct LayoutElementPassStruct
{
        char* HLSLSemantic;
        Uint32 InputIndex;
        Uint32 BufferSlot;
        Uint32 NumComponents;
        VALUE_TYPE ValueType;
        Uint32 IsNormalized;
        Uint32 RelativeOffset;
        Uint32 Stride;
        INPUT_ELEMENT_FREQUENCY Frequency;
        Uint32 InstanceDataStepRate;
};
}