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
using float2 = Engine.Vector2;
using float4x4 = Engine.Matrix4x4;
using BOOL = System.Boolean;

namespace DiligentEngine
{
    public enum VALUE_TYPE :  Uint8
    {
        VT_UNDEFINED = 0,
        VT_INT8,
        VT_INT16,
        VT_INT32,
        VT_UINT8,
        VT_UINT16,
        VT_UINT32,
        VT_FLOAT16,
        VT_FLOAT32,
        VT_NUM_TYPES,
    }
}
