using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Engine
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

        public bool contains(IntCoord inner)
        {
            return inner.top > this.top && 
                   inner.Bottom < this.Bottom && 
                   inner.left > this.left && 
                   inner.Right < this.Right;
        }

        public bool overlaps(IntCoord other)
        {
            return this.left <= other.Right &&
                    this.Right >= other.left &&
                    this.top <= other.Bottom &&
                    this.Bottom >= other.top;
        }

        public int Bottom
        {
            get
            {
                return top + height;
            }
        }

        public int Right
        {
            get
            {
                return left + width;
            }
        }

        public override string ToString()
        {
            return String.Format("{{{0}, {1}, {2}, {3}}}", left, top, width, height);
        }
    }
}
