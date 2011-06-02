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

        /// <summary>
        /// Compute the dot product of this vector and another.
        /// </summary>
        /// <param name="v">The other vector.</param>
        /// <returns>The dot product.</returns>
        public int dot(ref IntVector2 v)
        {
            return x * v.x + y * v.y;
        }

        /// <summary>
        /// Compute the length squared of this vector.  Avoids sqrt call.
        /// </summary>
        /// <returns>The sqared length.</returns>
        public int length2()
        {
            return dot(ref this);
        }

        /// <summary>
        /// Compute the length of this vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        public int length()
        {
            return (int)System.Math.Sqrt(length2());
        }

        /// <summary>
        /// Compute the squared distance between two vectors.  Avoids sqrt call.
        /// </summary>
        /// <param name="v">The other vector.</param>
        /// <returns>The squared distance between the two vectors,</returns>
        public float distance2(ref IntVector2 v)
        {
            return (v - this).length2();
        }

        /// <summary>
        /// Compute the distance between two vectors.
        /// </summary>
        /// <param name="v">The other vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public float distance(ref IntVector2 v)
        {
            return (v - this).length();
        }

        /// <summary>
        /// Get a normalized copy of this vector. Does not modify this vector.
        /// </summary>
        /// <returns>A normalized copy of this vector.</returns>
        public Vector2 normalized()
        {
            Vector2 vec2 = new Vector2(x, y);
            return vec2.normalized();
        }

        /// <summary>
        /// ToString.
        /// </summary>
        /// <returns>The Vector as a string. This string is in the correct format to be used with setValue.</returns>
        public override string ToString()
        {
            return String.Format("{0}, {1}", x, y);
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
            return new IntVector2(v.x * s, v.y * s);
        }

        public static IntVector2 operator *(IntVector2 v, float s)
        {
            return new IntVector2((int)(v.x * s), (int)(v.y * s));
        }

        public static IntVector2 operator *(int s, IntVector2 v)
        {
            return v * s;
        }

        public static IntVector2 operator /(IntVector2 v1, IntVector2 v2)
        {
            return new IntVector2(v1.x / v2.x, v1.y / v2.y);
        }

        public static IntVector2 operator /(IntVector2 v, float s)
        {
            return v * (1.0f / s);
        }

        //Vector2 operators
        public static Vector2 operator +(IntVector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2 operator *(IntVector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x * v2.x, v1.y * v2.y);
        }

        public static Vector2 operator -(IntVector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vector2 operator /(IntVector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x / v2.x, v1.y / v2.y);
        }

        //Vector 2 other way
        public static Vector2 operator +(Vector2 v1, IntVector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2 operator *(Vector2 v1, IntVector2 v2)
        {
            return new Vector2(v1.x * v2.x, v1.y * v2.y);
        }

        public static Vector2 operator -(Vector2 v1, IntVector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vector2 operator /(Vector2 v1, IntVector2 v2)
        {
            return new Vector2(v1.x / v2.x, v1.y / v2.y);
        }

        public static bool operator ==(IntVector2 p1, IntVector2 p2)
        {
            return p1.x == p2.x && p1.y == p2.y;
        }

        public static bool operator !=(IntVector2 p1, IntVector2 p2)
        {
            return !(p1.x == p2.x && p1.y == p2.y);
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
