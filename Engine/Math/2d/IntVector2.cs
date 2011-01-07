using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Saving;

namespace Engine
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct IntVector2 : Saveable
    {
        [FieldOffset(0)]
        public int x;

        [FieldOffset(4)]
        public int y;

        public IntVector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static IntVector2 operator +(IntVector2 v1, IntVector2 v2)
        {
            return new IntVector2(v1.x + v2.x, v1.y + v2.y);
        }

        public static IntVector2 operator *(IntVector2 v1, IntVector2 v2)
        {
            return new IntVector2(v1.x * v2.x, v1.y * v2.y);
        }

        public static IntVector2 operator -(IntVector2 v1, IntVector2 v2)
        {
            return new IntVector2(v1.x - v2.x, v1.y - v2.y);
        }

        public static IntVector2 operator -(IntVector2 v)
        {
            return new IntVector2(-v.x, -v.y);
        }

        public static IntVector2 operator *(IntVector2 v, int s)
        {
            return new IntVector2(v.x + s, v.y + s);
        }

        public static IntVector2 operator *(int s, IntVector2 v)
        {
            return v * s;
        }

        public static IntVector2 operator /(IntVector2 v1, IntVector2 v2)
        {
            return new IntVector2(v1.x / v2.x, v1.y / v2.y);
        }

        #region Saving

        private IntVector2(LoadInfo info)
        {
            x = info.GetInt32("x", 0);
            y = info.GetInt32("y", 0);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue("x", x);
            info.AddValue("y", y);
        }

        #endregion
    }
}
