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
    public partial class LayoutElement
    {
        public const Uint32 MAX_LAYOUT_ELEMENTS = 16;
        public const Uint32 LAYOUT_ELEMENT_AUTO_OFFSET = 0xFFFFFFFF;
        public const Uint32 LAYOUT_ELEMENT_AUTO_STRIDE = 0xFFFFFFFF;
    }
}
