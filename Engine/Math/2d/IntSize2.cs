using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Saving;

namespace Engine
{
    /// <summary>
    /// A 2 dimensional size class.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct IntSize2 : Saveable
    {
        public static readonly IntSize2 MinValue = new IntSize2(int.MinValue, int.MinValue);
        public static readonly IntSize2 MaxValue = new IntSize2(int.MaxValue, int.MaxValue);

        [FieldOffset(0)]
        public int Width;

        [FieldOffset(4)]
        public int Height;

        public IntSize2(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public IntSize2(String value)
        {
            parseString(value, out Width, out Height);
        }

        //public int Width
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

        //public int Height
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

        public static IntSize2 operator +(IntSize2 v1, IntSize2 v2)
        {
            return new IntSize2(v1.Width + v2.Width, v1.Height + v2.Height);
        }

        public static IntSize2 operator *(IntSize2 v1, IntSize2 v2)
        {
            return new IntSize2(v1.Width * v2.Width, v1.Height * v2.Height);
        }

        public static IntSize2 operator -(IntSize2 v1, IntSize2 v2)
        {
            return new IntSize2(v1.Width - v2.Width, v1.Height - v2.Height);
        }

        public static IntSize2 operator -(IntSize2 v)
        {
            return new IntSize2(-v.Width, -v.Height);
        }

        public static IntSize2 operator /(IntSize2 v1, IntSize2 v2)
        {
            return new IntSize2(v1.Width / v2.Width, v1.Height / v2.Height);
        }

        public static bool operator ==(IntSize2 p1, IntSize2 p2)
        {
            return p1.Width == p2.Width && p1.Height == p2.Height;
        }

        public static bool operator !=(IntSize2 p1, IntSize2 p2)
        {
            return !(p1.Width == p2.Width && p1.Height == p2.Height);
        }

        public static Size2 operator *(IntSize2 v, float s)
        {
            return new Size2(v.Width * s, v.Height * s);
        }

        public static Size2 operator /(IntSize2 v, float s)
        {
            return new Size2(v.Width / s, v.Height / s);
        }

        public static Size2 operator *(float s, IntSize2 v)
        {
            return v * s;
        }

        public static explicit operator IntSize2(Size2 size)
        {
            return new IntSize2((int)size.Width, (int)size.Height);
        }

        private static char[] SEPS = { ',' };
        static public bool parseString(String value, out int width, out int height)
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
                width = 0;
                height = 0;
            }
            return success;
        }

        #region Saving

        private IntSize2(LoadInfo info)
        {
            Width = info.GetInt32("Width", 0);
            Height = info.GetInt32("Height", 0);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue("Width", Width);
            info.AddValue("Height", Height);
        }

        #endregion
    }
}
