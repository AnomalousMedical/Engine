using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Saving;

namespace Engine
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct Vector2 : Saveable
    {
        [FieldOffset(0)]
        public float x;

        [FieldOffset(4)]
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2 operator *(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x * v2.x, v1.y * v2.y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.x, -v.y);
        }

        public static Vector2 operator *(Vector2 v, float s)
        {
            return new Vector2(v.x + s, v.y + s);
        }

        public static Vector2 operator *(float s, Vector2 v)
        {
            return v * s;
        }

        public static Vector2 operator /(Vector2 v, float s)
        {
            return v * (1.0f / s);
        }

        public static Vector2 operator /(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x / v2.x, v1.y / v2.y);
        }

        #region Saving

        private Vector2(LoadInfo info)
        {
            x = info.GetFloat("x", 0.0f);
            y = info.GetFloat("y", 0.0f);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue("x", x);
            info.AddValue("y", y);
        }

        #endregion
    }
}
