using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Engine
{
    /// <summary>
    /// IntVector3 math class.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size=12)]
    public struct IntVector3
    {
        public static readonly IntVector3 Zero = new IntVector3(0, 0, 0);

        [FieldOffset(0)]
        public int x;
        
        [FieldOffset(4)]
        public int y;

        [FieldOffset(8)]
        public int z;

        /// <summary>
        /// Initialize constructor.
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        /// <param name="z">Z value.</param>
        public IntVector3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Equals function.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the objects are equal.</returns>
        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(IntVector3) && this == (IntVector3)obj;
        }

        /// <summary>
        /// Hash code function.
        /// </summary>
        /// <returns>A hash code for this Vector3.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public static IntVector3 operator +(IntVector3 v1, IntVector3 v2)
        {
            return new IntVector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static IntVector3 operator *(IntVector3 v1, IntVector3 v2)
        {
            return new IntVector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public static IntVector3 operator -(IntVector3 v1, IntVector3 v2)
        {
            return new IntVector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static IntVector3 operator -(IntVector3 v)
        {
            return new IntVector3(-v.x, -v.y, -v.z);
        }

        public static IntVector3 operator *(IntVector3 v, int s)
        {
            return new IntVector3(v.x * s, v.y * s, v.z * s);
        }

        public static IntVector3 operator *(int s, IntVector3 v)
        {
            return v * s;
        }

        public static IntVector3 operator /(IntVector3 v, int s)
        {
            return new IntVector3(v.x / s, v.y / s, v.z / s);
        }

        public static IntVector3 operator /(IntVector3 v1, IntVector3 v2)
        {
            return new IntVector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        public static bool operator ==(IntVector3 p1, IntVector3 p2)
        {
            return p1.x == p2.x && p1.y == p2.y && p1.z == p2.z;
        }

        public static bool operator !=(IntVector3 p1, IntVector3 p2)
        {
            return !(p1.x == p2.x && p1.y == p2.y && p1.z == p2.z);
        }

        public static explicit operator IntVector3(Vector3 vec2)
        {
            return new IntVector3((int)vec2.x, (int)vec2.y, (int)vec2.z);
        }
    }
}
