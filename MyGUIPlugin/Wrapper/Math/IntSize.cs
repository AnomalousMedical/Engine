using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct IntSize
    {
        [FieldOffset(0)]
        int width;

        [FieldOffset(4)]
        int height;

        public IntSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}
