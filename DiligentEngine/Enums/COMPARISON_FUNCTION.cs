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
using float4x4 = Engine.Matrix4x4;
using BOOL = System.Boolean;

namespace DiligentEngine
{
    public enum COMPARISON_FUNCTION :  Uint8
    {
        COMPARISON_FUNC_UNKNOWN = 0,
        COMPARISON_FUNC_NEVER,
        COMPARISON_FUNC_LESS,
        COMPARISON_FUNC_EQUAL,
        COMPARISON_FUNC_LESS_EQUAL,
        COMPARISON_FUNC_GREATER,
        COMPARISON_FUNC_NOT_EQUAL,
        COMPARISON_FUNC_GREATER_EQUAL,
        COMPARISON_FUNC_ALWAYS,
        COMPARISON_FUNC_NUM_FUNCTIONS,
    }
}
