using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace MyGUIPlugin
{
    /// <summary>
    /// This struct exists to get around an issue when marshaling classes that only have Size=8.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size=12)]
    struct ThreeIntHack
    {
        [FieldOffset(0)]
        public int x;

        [FieldOffset(4)]
        public int y;

        [FieldOffset(8)]
        public int z;

        public ThreeIntHack(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        public IntSize2 toIntSize2()
        {
            return new IntSize2(x, y);
        }

        public Vector2 toVector2()
        {
            return new Vector2(x, y);
        }

        public IntVector2 toIntVector2()
        {
            return new IntVector2(x, y);
        }
    }
}
