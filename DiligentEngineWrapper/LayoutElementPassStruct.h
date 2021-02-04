#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "AdditionalTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

struct LayoutElementPassStruct
{
    char* HLSLSemantic;
    Diligent::Uint32 InputIndex;
    Diligent::Uint32 BufferSlot;
    Diligent::Uint32 NumComponents;
    Diligent::VALUE_TYPE ValueType;
    Diligent::Uint32 IsNormalized;
    Diligent::Uint32 RelativeOffset;
    Diligent::Uint32 Stride;
    Diligent::INPUT_ELEMENT_FREQUENCY Frequency;
    Diligent::Uint32 InstanceDataStepRate;
};