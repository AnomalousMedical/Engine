#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

namespace Diligent 
{
struct DeviceCapsPassStruct
{
        RENDER_DEVICE_TYPE DevType;
        Int32 MajorVersion;
        Int32 MinorVersion;
        GraphicsAdapterInfo AdapterInfo;
        SamplerCaps SamCaps;
        TextureCaps TexCaps;
        DeviceFeatures Features;
};
}