using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct Vector2
    {
        [FieldOffset(0)]
        public float x;

        [FieldOffset(4)]
        public float y;
    }
}
