using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace CEGUIPlugin
{
    /// <summary>
    /// This class is works around issues with marshaling 8 byte float structs. There does not seem to be any info on google of how to solve this, however, having three fields works for now.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 12)]
    struct FloatStructHack
    {
        [FieldOffset(0)]
        public float x;

        [FieldOffset(4)]
        public float y;

        [FieldOffset(8)]
        public float z;

        public Size toSize()
        {
            return new Size(x, y);
        }

        public Vector2 toVector2()
        {
            return new Vector2(x, y);
        }

        public UDim toUDim()
        {
            return new UDim(x, y);
        }
    }
}
