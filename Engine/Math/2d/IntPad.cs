using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Engine
{
    /// <summary>
    /// A struct to represent padding on 4 sides.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct IntPad
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public IntPad(int value)
        {
            this.Left = value;
            this.Top = value;
            this.Right = value;
            this.Bottom = value;
        }

        public IntPad(int left, int top, int right, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        public IntSize2 ToSize()
        {
            return new IntSize2(Left + Right, Top + Bottom);
        }

        public static IntPad operator + (IntPad v1, IntPad v2)
        {
            return new IntPad(v1.Left + v2.Left, v1.Top+ v2.Top, v1.Right + v2.Right, v1.Bottom + v2.Bottom);
        }
    }
}
