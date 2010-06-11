using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CEGUIPlugin
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public class UVector2
    {
        [FieldOffset(0)]
        public UDim x;

        [FieldOffset(8)]
        public UDim y;
    }
}
