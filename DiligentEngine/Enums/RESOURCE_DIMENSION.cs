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
    public enum RESOURCE_DIMENSION :  Uint8
    {
        RESOURCE_DIM_UNDEFINED = 0,
        RESOURCE_DIM_BUFFER,
        RESOURCE_DIM_TEX_1D,
        RESOURCE_DIM_TEX_1D_ARRAY,
        RESOURCE_DIM_TEX_2D,
        RESOURCE_DIM_TEX_2D_ARRAY,
        RESOURCE_DIM_TEX_3D,
        RESOURCE_DIM_TEX_CUBE,
        RESOURCE_DIM_TEX_CUBE_ARRAY,
        RESOURCE_DIM_NUM_DIMENSIONS,
    }
}
