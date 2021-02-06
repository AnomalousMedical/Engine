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
    public enum PSO_CREATE_FLAGS :  Uint32
    {
        PSO_CREATE_FLAG_NONE = 0x00,
        PSO_CREATE_FLAG_IGNORE_MISSING_VARIABLES = 0x01,
        PSO_CREATE_FLAG_IGNORE_MISSING_IMMUTABLE_SAMPLERS = 0x02,
    }
}
