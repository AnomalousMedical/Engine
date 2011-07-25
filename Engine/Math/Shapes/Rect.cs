using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct Rect
    {
        [FieldOffset(0)]
        private float left;
        [FieldOffset(4)]
        private float top;
        [FieldOffset(8)]
        private float width;
        [FieldOffset(12)]
        private float height;

        public Rect(float left, float top, float width, float height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
        }

        public void setValues(float left, float top, float width, float height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
        }

        public float Left
        {
            get
            {
                return left;
            }
            set
            {
                left = value;
            }
        }

        public float Top
        {
            get
            {
                return top;
            }
            set
            {
                top = value;
            }
        }

        public float Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public float Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        private static readonly char[] SEPS = { ',' };

        public void fromString(String str)
        {
            String[] subStrs = str.Split(SEPS);
            if (subStrs.Length == 4)
            {
                NumberParser.TryParse(subStrs[0], out left);
                NumberParser.TryParse(subStrs[1], out top);
                NumberParser.TryParse(subStrs[2], out width);
                NumberParser.TryParse(subStrs[3], out height);
            }
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}, {3}", left, top, width, height);
        }
    }
}
