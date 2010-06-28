using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    [StructLayout(LayoutKind.Explicit, Size=16)]
    public struct IntCoord
    {
        [FieldOffset(0)]
        public int left;

        [FieldOffset(4)]
        public int top;

        [FieldOffset(8)]
        public int width;

        [FieldOffset(12)]
        public int height;

        public IntCoord(int left, int top, int width, int height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
        }
    }
}
