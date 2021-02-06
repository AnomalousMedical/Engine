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
    public enum BIND_FLAGS :  Uint32
    {
        BIND_NONE = 0x0,
        BIND_VERTEX_BUFFER = 0x1,
        BIND_INDEX_BUFFER = 0x2,
        BIND_UNIFORM_BUFFER = 0x4,
        BIND_SHADER_RESOURCE = 0x8,
        BIND_STREAM_OUTPUT = 0x10,
        BIND_RENDER_TARGET = 0x20,
        BIND_DEPTH_STENCIL = 0x40,
        BIND_UNORDERED_ACCESS = 0x80,
        BIND_INDIRECT_DRAW_ARGS = 0x100,
        BIND_INPUT_ATTACHMENT = 0x200,
        BIND_RAY_TRACING = 0x400,
        BIND_FLAGS_LAST = 0x400,
    }
}
