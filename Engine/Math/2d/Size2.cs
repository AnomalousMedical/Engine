using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Engine
{
    /// <summary>
    /// A 2 dimensional size class.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct Size2
    {
        [FieldOffset(0)]
        public float Width;

        [FieldOffset(4)]
        public float Height;

        public Size2(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        public Size2(String size)
        {
            parseString(size, out Width, out Height);
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

        public bool fromString(String value)
        {
            return parseString(value, out Width, out Height);
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}", Width, Height);
        }

        /// <summary>
        /// Equals function.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the objects are equal.</returns>
        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(Size2) && this == (Size2)obj;
        }

        /// <summary>
        /// Hash code function.
        /// </summary>
        /// <returns>A hash code for this Vector3.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Width, Height);
        }

        public static Size2 operator +(Size2 v1, Size2 v2)
        {
            return new Size2(v1.Width + v2.Width, v1.Height + v2.Height);
        }

        public static Size2 operator *(Size2 v1, Size2 v2)
        {
            return new Size2(v1.Width * v2.Width, v1.Height * v2.Height);
        }

        public static Size2 operator -(Size2 v1, Size2 v2)
        {
            return new Size2(v1.Width - v2.Width, v1.Height - v2.Height);
        }

        public static Size2 operator -(Size2 v)
        {
            return new Size2(-v.Width, -v.Height);
        }

        public static Size2 operator *(Size2 v, float s)
        {
            return new Size2(v.Width * s, v.Height * s);
        }

        public static Size2 operator *(float s, Size2 v)
        {
            return v * s;
        }

        public static Size2 operator /(Size2 v, float s)
        {
            return v * (1.0f / s);
        }

        public static Size2 operator /(Size2 v1, Size2 v2)
        {
            return new Size2(v1.Width / v2.Width, v1.Height / v2.Height);
        }

        public static bool operator ==(Size2 p1, Size2 p2)
        {
            return p1.Width == p2.Width && p1.Height == p2.Height;
        }

        public static bool operator !=(Size2 p1, Size2 p2)
        {
            return !(p1.Width == p2.Width && p1.Height == p2.Height);
        }

        public static implicit operator Size2(IntSize2 size)
        {
            return new Size2(size.Width, size.Height);
        }

        private static char[] SEPS = { ',' };
        static public bool parseString(String value, out float width, out float height)
        {
            String[] nums = value.Split(SEPS);
            bool success = false;
            if (nums.Length == 2)
            {
                success = NumberParser.TryParse(nums[0], out width);
                success &= NumberParser.TryParse(nums[1], out height);
            }
            else
            {
                width = 0f;
                height = 0f;
            }
            return success;
        }
    }
}
