using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct Vector2
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

        /// <summary>
        /// String constructor. See setValue for details.
        /// </summary>
        /// <param name="value">A string in the format "x, y, z".</param>
        public Vector2(String value)
        {
            parseString(value, out x, out y);
        }

        /// <summary>
        /// Compute the dot product of this vector and another.
        /// </summary>
        /// <param name="v">The other vector.</param>
        /// <returns>The dot product.</returns>
        public float dot(ref Vector2 v)
        {
            return x * v.x + y * v.y;
        }

        /// <summary>
        /// Compute the length squared of this vector.  Avoids sqrt call.
        /// </summary>
        /// <returns>The sqared length.</returns>
        public float length2()
        {
            return dot(ref this);
        }

        /// <summary>
        /// Compute the length of this vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        public float length()
        {
            return (float)System.Math.Sqrt(length2());
        }

        /// <summary>
        /// Compute the squared distance between two vectors.  Avoids sqrt call.
        /// </summary>
        /// <param name="v">The other vector.</param>
        /// <returns>The squared distance between the two vectors,</returns>
        public float distance2(ref Vector2 v)
        {
            return (v - this).length2();
        }

        /// <summary>
        /// Compute the distance between two vectors.
        /// </summary>
        /// <param name="v">The other vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public float distance(ref Vector2 v)
        {
            return (v - this).length();
        }

        /// <summary>
        /// Normalize this vector and return it. Modifies this vector to be
        /// normalized.
        /// </summary>
        /// <returns>The same vector normalized.</returns>
        public Vector2 normalize()
        {
            float len = length();
            if (len != 0)
            {
                return this /= len;
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Get a normalized copy of this vector. Does not modify this vector.
        /// </summary>
        /// <returns>A normalized copy of this vector.</returns>
        public Vector2 normalized()
        {
            return this / length();
        }

        /// <summary>
        /// ToString.
        /// </summary>
        /// <returns>The Vector as a string. This string is in the correct format to be used with setValue.</returns>
        public override string ToString()
        {
            return String.Format("{0}, {1}", x, y);
        }

        /// <summary>
        /// Equals function.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the objects are equal.</returns>
        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(Vector2) && this == (Vector2)obj;
        }

        /// <summary>
        /// Hash code function.
        /// </summary>
        /// <returns>A hash code for this Vector3.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public System.Numerics.Vector2 ToSystemNumerics()
        {
            return new System.Numerics.Vector2(x, y);
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
            return new Vector2(v.x * s, v.y * s);
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

        public static bool operator ==(Vector2 p1, Vector2 p2)
        {
            return p1.x == p2.x && p1.y == p2.y;
        }

        public static bool operator !=(Vector2 p1, Vector2 p2)
        {
            return !(p1.x == p2.x && p1.y == p2.y);
        }

        private static char[] SEPS = { ',' };
        static public bool parseString(String value, out float x, out float y)
        {
            String[] nums = value.Split(SEPS);
            bool success = false;
            if (nums.Length == 2)
            {
                success = NumberParser.TryParse(nums[0], out x);
                success &= NumberParser.TryParse(nums[1], out y);
            }
            else
            {
                x = 0f;
                y = 0f;
            }
            return success;
        }

        public static implicit operator Vector2(IntVector2 i)
        {
            return new Vector2(i.x, i.y);
        }
    }
}
