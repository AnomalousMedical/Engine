using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;

namespace DiligentEngine
{
    public enum USAGE :  Uint8
    {
        USAGE_IMMUTABLE = 0,
        USAGE_DEFAULT,
        USAGE_DYNAMIC,
        USAGE_STAGING,
        USAGE_UNIFIED,
        USAGE_NUM_USAGES,
    }
}
