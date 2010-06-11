using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CEGUIPlugin
{
    [StructLayout(LayoutKind.Explicit, Size = 32)]
    public class URect
    {
        [FieldOffset(0)]
        public UVector2 min;

        [FieldOffset(8)]
        public UVector2 max;
    }
}
