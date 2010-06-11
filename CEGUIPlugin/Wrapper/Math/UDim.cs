using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CEGUIPlugin
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct UDim
    {
        [FieldOffset(0)]
        public float scale;

        [FieldOffset(4)]
        public float offset;

        public UDim(float scale, float offset)
        {
            this.scale = scale;
            this.offset = offset;
        }
    }
}
