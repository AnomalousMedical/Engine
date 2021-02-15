#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

namespace Diligent 
{
struct GraphicsAdapterInfoPassStruct
{
        char Description[128];
        ADAPTER_TYPE Type;
        ADAPTER_VENDOR Vendor;
        Uint32 VendorId;
        Uint32 DeviceId;
        Uint32 NumOutputs;
        Uint64 DeviceLocalMemory;
        Uint64 HostVisibileMemory;
        Uint64 UnifiedMemory;
        CPU_ACCESS_FLAGS UnifiedMemoryCPUAccess;
};
}