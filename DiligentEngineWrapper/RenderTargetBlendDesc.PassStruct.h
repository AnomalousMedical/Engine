#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

namespace Diligent 
{
struct RenderTargetBlendDescPassStruct
{
        Uint32 BlendEnable;
        Uint32 LogicOperationEnable;
        BLEND_FACTOR SrcBlend;
        BLEND_FACTOR DestBlend;
        BLEND_OPERATION BlendOp;
        BLEND_FACTOR SrcBlendAlpha;
        BLEND_FACTOR DestBlendAlpha;
        BLEND_OPERATION BlendOpAlpha;
        LOGIC_OPERATION LogicOp;
        Uint8 RenderTargetWriteMask;
};
}