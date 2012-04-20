using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace libRocketPlugin
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

        public Vector2i toVector2i()
        {
            return new Vector2i(x, y);
        }
    }
}
