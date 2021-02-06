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
    public enum BLEND_OPERATION :  Int8
    {
        BLEND_OPERATION_UNDEFINED = 0,
        BLEND_OPERATION_ADD,
        BLEND_OPERATION_SUBTRACT,
        BLEND_OPERATION_REV_SUBTRACT,
        BLEND_OPERATION_MIN,
        BLEND_OPERATION_MAX,
        BLEND_OPERATION_NUM_OPERATIONS,
    }
}
