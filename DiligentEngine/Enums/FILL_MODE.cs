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

namespace DiligentEngine
{
    public enum FILL_MODE :  Int8
    {
        FILL_MODE_UNDEFINED = 0,
        FILL_MODE_WIREFRAME,
        FILL_MODE_SOLID,
        FILL_MODE_NUM_MODES,
    }
}
