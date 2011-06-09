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
        [FieldOffset(0)]
        public int Width;

        [FieldOffset(4)]
        public int Height;

        public IntSize2(int width, int height)
        {
            this.Width = width;
            this.Height = height;
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

        public static explicit operator IntSize2(Size2 size)
        {
            return new IntSize2((int)size.Width, (int)size.Height);
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
