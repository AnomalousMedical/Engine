using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct Vector2i
    {
        [FieldOffset(0)]
        private int x;

        [FieldOffset(4)]
        private int y;

        public Vector2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
    }
}
