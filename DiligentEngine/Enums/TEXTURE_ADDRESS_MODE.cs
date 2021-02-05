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

namespace DiligentEngine
{
    public enum TEXTURE_ADDRESS_MODE :  Uint8
    {
        TEXTURE_ADDRESS_UNKNOWN = 0,
        TEXTURE_ADDRESS_WRAP = 1,
        TEXTURE_ADDRESS_MIRROR = 2,
        TEXTURE_ADDRESS_CLAMP = 3,
        TEXTURE_ADDRESS_BORDER = 4,
        TEXTURE_ADDRESS_MIRROR_ONCE = 5,
        TEXTURE_ADDRESS_NUM_MODES,
    }
}
