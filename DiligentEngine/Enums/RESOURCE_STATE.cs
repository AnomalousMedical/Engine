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
    public enum RESOURCE_STATE :  Uint32
    {
        RESOURCE_STATE_UNKNOWN = 0x00000,
        RESOURCE_STATE_UNDEFINED = 0x00001,
        RESOURCE_STATE_VERTEX_BUFFER = 0x00002,
        RESOURCE_STATE_CONSTANT_BUFFER = 0x00004,
        RESOURCE_STATE_INDEX_BUFFER = 0x00008,
        RESOURCE_STATE_RENDER_TARGET = 0x00010,
        RESOURCE_STATE_UNORDERED_ACCESS = 0x00020,
        RESOURCE_STATE_DEPTH_WRITE = 0x00040,
        RESOURCE_STATE_DEPTH_READ = 0x00080,
        RESOURCE_STATE_SHADER_RESOURCE = 0x00100,
        RESOURCE_STATE_STREAM_OUT = 0x00200,
        RESOURCE_STATE_INDIRECT_ARGUMENT = 0x00400,
        RESOURCE_STATE_COPY_DEST = 0x00800,
        RESOURCE_STATE_COPY_SOURCE = 0x01000,
        RESOURCE_STATE_RESOLVE_DEST = 0x02000,
        RESOURCE_STATE_RESOLVE_SOURCE = 0x04000,
        RESOURCE_STATE_INPUT_ATTACHMENT = 0x08000,
        RESOURCE_STATE_PRESENT = 0x10000,
        RESOURCE_STATE_BUILD_AS_READ = 0x20000,
        RESOURCE_STATE_BUILD_AS_WRITE = 0x40000,
        RESOURCE_STATE_RAY_TRACING = 0x80000,
        RESOURCE_STATE_MAX_BIT = RESOURCE_STATE_RAY_TRACING,
        RESOURCE_STATE_GENERIC_READ = RESOURCE_STATE_VERTEX_BUFFER     |,
        RESOURCE_STATE_CONSTANT_BUFFER   |,
        RESOURCE_STATE_INDEX_BUFFER      |,
        RESOURCE_STATE_SHADER_RESOURCE   |,
        RESOURCE_STATE_INDIRECT_ARGUMENT |,
        RESOURCE_STATE_COPY_SOURCE,
    }
}
