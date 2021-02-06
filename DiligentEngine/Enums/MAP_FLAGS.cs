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
    public enum MAP_FLAGS :  Uint8
    {
        MAP_FLAG_NONE = 0x000,
        MAP_FLAG_DO_NOT_WAIT = 0x001,
        MAP_FLAG_DISCARD = 0x002,
        MAP_FLAG_NO_OVERWRITE = 0x004,
    }
}
