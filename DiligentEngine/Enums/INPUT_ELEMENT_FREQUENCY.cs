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
    public enum  INPUT_ELEMENT_FREQUENCY
    {
        INPUT_ELEMENT_FREQUENCY_UNDEFINED = 0,
        INPUT_ELEMENT_FREQUENCY_PER_VERTEX,
        INPUT_ELEMENT_FREQUENCY_PER_INSTANCE,
        INPUT_ELEMENT_FREQUENCY_NUM_FREQUENCIES,
    }
}
