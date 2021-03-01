using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct IntRect
    {
        [FieldOffset(0)]
        private int left;
        [FieldOffset(4)]
        private int top;
        [FieldOffset(8)]
        private int width;
        [FieldOffset(12)]
        private int height;

        public IntRect(int left, int top, int width, int height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
        }

        public void setValues(int left, int top, int width, int height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
        }

        public int X
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

        public int Left
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

        public int Y
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

        public int Top
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

        public int Width
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

        public int Height
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

        public int Right
        {
            get
            {
                return left + width;
            }
        }

        public int Bottom
        {
            get
            {
                return top + height;
            }
        }

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined by this
        /// <see cref='IntRect'/> .
        /// </summary>
        public readonly bool Contains(int x, int y) => left <= x && x < left + width && top <= y && y < top + height;

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined by this
        /// <see cref='IntRect'/> .
        /// </summary>
        public readonly bool Contains(in IntVector2 pt) => Contains(pt.x, pt.y);

        /// <summary>
        /// Determines if the rectangular region represented by <paramref name="rect"/> is entirely contained within the
        /// rectangular region represented by this <see cref='IntRect'/> .
        /// </summary>
        public readonly bool Contains(in IntRect rect) =>
            (left <= rect.left) && (rect.left + rect.width <= left + width) &&
            (top <= rect.top) && (rect.top + rect.height <= top + height);

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(in IntVector2 pos) => Offset(pos.X, pos.Y);

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(int x, int y)
        {
            unchecked
            {
                X += x;
                Y += y;
            }
        }

        /// <summary>
        /// Inflates this <see cref='IntRect'/> by the specified amount.
        /// </summary>
        public void Inflate(int width, int height)
        {
            unchecked
            {
                X -= width;
                Y -= height;

                Width += 2 * width;
                Height += 2 * height;
            }
        }

        /// <summary>
        /// Inflates this <see cref='IntRect'/> by the specified amount.
        /// </summary>
        public void Inflate(in IntSize2 size) => Inflate(size.Width, size.Height);

        /// <summary>
        /// Determines if this rectangle intersects with rect.
        /// </summary>
        public readonly bool IntersectsWith(in IntRect rect) =>
            (rect.left < left + width) && (left < rect.left + rect.width) &&
            (rect.top < top + height) && (top < rect.top + rect.height);

        public static IntRect operator *(IntRect v, int s)
        {
            return new IntRect(v.Left * s, v.Top * s, v.Width * s, v.Height * s);
        }

        public static IntRect operator *(int s, IntRect v)
        {
            return v * s;
        }

        public static Rect operator *(IntRect v, float s)
        {
            return new Rect(v.Left * s, v.Top * s, v.Width * s, v.Height * s);
        }

        public static Rect operator *(float s, IntRect v)
        {
            return v * s;
        }

        public static IntRect operator /(IntRect v, int s)
        {
            return new IntRect(v.Left / s, v.Top / s, v.Width / s, v.Height / s);
        }

        public static Rect operator /(IntRect v, float s)
        {
            return new Rect(v.Left / s, v.Top / s, v.Width / s, v.Height / s);
        }

        public static explicit operator IntRect(Rect s)
        {
            return new IntRect((int)s.Left, (int)s.Top, (int)s.Width, (int)s.Height);
        }

        /// <summary>
        /// Equals function.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the objects are equal.</returns>
        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(IntRect) && this == (IntRect)obj;
        }

        /// <summary>
        /// Hash code function.
        /// </summary>
        /// <returns>A hash code for this Quaternion.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(left, top, width, height);
        }

        public static bool operator ==(IntRect p1, IntRect p2)
        {
            return p1.left == p2.left && p1.top == p2.top && p1.width == p2.width && p1.height == p2.height;
        }

        public static bool operator !=(IntRect p1, IntRect p2)
        {
            return !(p1.left == p2.left && p1.top == p2.top && p1.width == p2.width && p1.height == p2.height);
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
