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
    public enum LOGIC_OPERATION :  Int8
    {
        LOGIC_OP_CLEAR = 0,
        LOGIC_OP_SET,
        LOGIC_OP_COPY,
        LOGIC_OP_COPY_INVERTED,
        LOGIC_OP_NOOP,
        LOGIC_OP_INVERT,
        LOGIC_OP_AND,
        LOGIC_OP_NAND,
        LOGIC_OP_OR,
        LOGIC_OP_NOR,
        LOGIC_OP_XOR,
        LOGIC_OP_EQUIV,
        LOGIC_OP_AND_REVERSE,
        LOGIC_OP_AND_INVERTED,
        LOGIC_OP_OR_REVERSE,
        LOGIC_OP_OR_INVERTED,
        LOGIC_OP_NUM_OPERATIONS,
    }
}
