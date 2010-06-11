using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CEGUIPlugin
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct UVector2
    {
        [FieldOffset(0)]
        public UDim x;

        [FieldOffset(8)]
        public UDim y;

        public UVector2(float xScale, float xOffset, float yScale, float yOffset)
        {
            x = new UDim(xScale, xOffset);
            y = new UDim(yScale, yOffset);
        }

        public UVector2(UDim x, UDim y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
