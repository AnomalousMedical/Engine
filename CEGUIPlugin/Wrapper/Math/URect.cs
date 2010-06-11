using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CEGUIPlugin
{
    [StructLayout(LayoutKind.Explicit, Size = 32)]
    public struct URect
    {
        [FieldOffset(0)]
        public UVector2 min;

        [FieldOffset(16)]
        public UVector2 max;

        public URect(UDim left, UDim top, UDim right, UDim bottom)
        {
            min = new UVector2(left, top);
            max = new UVector2(right, bottom);
        }

        public URect(UVector2 min, UVector2 max)
        {
            this.min = min;
            this.max = max;
        }
    }
}
