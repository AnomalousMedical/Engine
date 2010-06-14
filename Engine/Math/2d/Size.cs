using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct Size
    {
        [FieldOffset(0)]
        public float Width;

        [FieldOffset(4)]
        public float Height;

        public Size(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        //public float Width
        //{
        //    get
        //    {
        //        return width;
        //    }
        //    set
        //    {
        //        width = value;
        //    }
        //}

        //public float Height
        //{
        //    get
        //    {
        //        return height;
        //    }
        //    set
        //    {
        //        height = value;
        //    }
        //}

        public static Size operator +(Size v1, Size v2)
        {
            return new Size(v1.Width + v2.Width, v1.Height + v2.Height);
        }

        public static Size operator *(Size v1, Size v2)
        {
            return new Size(v1.Width * v2.Width, v1.Height * v2.Height);
        }

        public static Size operator -(Size v1, Size v2)
        {
            return new Size(v1.Width - v2.Width, v1.Height - v2.Height);
        }

        public static Size operator -(Size v)
        {
            return new Size(-v.Width, -v.Height);
        }

        public static Size operator *(Size v, float s)
        {
            return new Size(v.Width + s, v.Height + s);
        }

        public static Size operator *(float s, Size v)
        {
            return v * s;
        }

        public static Size operator /(Size v, float s)
        {
            return v * (1.0f / s);
        }

        public static Size operator /(Size v1, Size v2)
        {
            return new Size(v1.Width / v2.Width, v1.Height / v2.Height);
        }
    }
}
