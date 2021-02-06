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
    public enum BLEND_FACTOR :  Int8
    {
        BLEND_FACTOR_UNDEFINED = 0,
        BLEND_FACTOR_ZERO,
        BLEND_FACTOR_ONE,
        BLEND_FACTOR_SRC_COLOR,
        BLEND_FACTOR_INV_SRC_COLOR,
        BLEND_FACTOR_SRC_ALPHA,
        BLEND_FACTOR_INV_SRC_ALPHA,
        BLEND_FACTOR_DEST_ALPHA,
        BLEND_FACTOR_INV_DEST_ALPHA,
        BLEND_FACTOR_DEST_COLOR,
        BLEND_FACTOR_INV_DEST_COLOR,
        BLEND_FACTOR_SRC_ALPHA_SAT,
        BLEND_FACTOR_BLEND_FACTOR,
        BLEND_FACTOR_INV_BLEND_FACTOR,
        BLEND_FACTOR_SRC1_COLOR,
        BLEND_FACTOR_INV_SRC1_COLOR,
        BLEND_FACTOR_SRC1_ALPHA,
        BLEND_FACTOR_INV_SRC1_ALPHA,
        BLEND_FACTOR_NUM_FACTORS,
    }
}
