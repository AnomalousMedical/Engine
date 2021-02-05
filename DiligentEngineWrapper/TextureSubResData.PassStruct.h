#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

namespace Diligent 
{
struct TextureSubResDataPassStruct
{
        void* pData;
        IBuffer* pSrcBuffer;
        Uint32 SrcOffset;
        Uint32 Stride;
        Uint32 DepthStride;
};
}