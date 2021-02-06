using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;
using Float32 = System.Single;
using Uint16 = System.UInt16;
using PVoid = System.IntPtr;
using float4 = Engine.Vector4;
using float3 = Engine.Vector3;

namespace DiligentEngine
{
    public enum SWAP_CHAIN_USAGE_FLAGS :  Uint32
    {
        SWAP_CHAIN_USAGE_NONE = 0x00,
        SWAP_CHAIN_USAGE_RENDER_TARGET = 0x01,
        SWAP_CHAIN_USAGE_SHADER_INPUT = 0x02,
        SWAP_CHAIN_USAGE_COPY_SOURCE = 0x04,
        SWAP_CHAIN_USAGE_LAST = SWAP_CHAIN_USAGE_COPY_SOURCE,
    }
}
