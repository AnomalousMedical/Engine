using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct Size
    {
        [FieldOffset(0)]
        public float width;

        [FieldOffset(4)]
        public float height;
    }
}
