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
    public enum FILTER_TYPE :  Uint8
    {
        FILTER_TYPE_UNKNOWN = 0,
        FILTER_TYPE_POINT,
        FILTER_TYPE_LINEAR,
        FILTER_TYPE_ANISOTROPIC,
        FILTER_TYPE_COMPARISON_POINT,
        FILTER_TYPE_COMPARISON_LINEAR,
        FILTER_TYPE_COMPARISON_ANISOTROPIC,
        FILTER_TYPE_MINIMUM_POINT,
        FILTER_TYPE_MINIMUM_LINEAR,
        FILTER_TYPE_MINIMUM_ANISOTROPIC,
        FILTER_TYPE_MAXIMUM_POINT,
        FILTER_TYPE_MAXIMUM_LINEAR,
        FILTER_TYPE_MAXIMUM_ANISOTROPIC,
        FILTER_TYPE_NUM_FILTERS,
    }
}
