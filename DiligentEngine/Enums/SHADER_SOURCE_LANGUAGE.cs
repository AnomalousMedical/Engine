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
    public enum SHADER_SOURCE_LANGUAGE :  Uint32
    {
        SHADER_SOURCE_LANGUAGE_DEFAULT = 0,
        SHADER_SOURCE_LANGUAGE_HLSL,
        SHADER_SOURCE_LANGUAGE_GLSL,
        SHADER_SOURCE_LANGUAGE_MSL,
        SHADER_SOURCE_LANGUAGE_GLSL_VERBATIM,
    }
}
