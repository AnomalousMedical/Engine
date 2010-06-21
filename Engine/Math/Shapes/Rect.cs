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
        private float top;
        [FieldOffset(4)]
        private float bottom;
        [FieldOffset(8)]
        private float left;
        [FieldOffset(12)]
        private float right;

        public Rect(float top, float bottom, float left, float right)
        {
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;
        }

        public void setValues(float top, float bottom, float left, float right)
        {
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;
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

        public float Bottom
        {
            get
            {
                return bottom;
            }
            set
            {
                bottom = value;
            }
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

        public float Right
        {
            get
            {
                return right;
            }
            set
            {
                right = value;
            }
        }
    }
}
