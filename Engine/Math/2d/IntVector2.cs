using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct IntVector2
    {
        [FieldOffset(0)]
        public int x;

        [FieldOffset(4)]
        public int y;



        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

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

        /// <summary>
        /// Translates this <see cref='IntRect'/> by the specified amount.
        /// </summary>
        public void Offset(int dx, int dy)
        {
            unchecked
            {
                X += dx;
                Y += dy;
            }
        }

        /// <summary>
        /// Translates this <see cref='System.Drawing.Point'/> by the specified amount.
        /// </summary>
        public void Offset(in IntVector2 p) => Offset(p.X, p.Y);

        /// <summary>
        /// Equals function.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the objects are equal.</returns>
        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(IntVector2) && this == (IntVector2)obj;
        }

        /// <summary>
        /// Hash code function.
        /// </summary>
        /// <returns>A hash code for this Vector3.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
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

        public static explicit operator IntVector2(Vector2 vec2)
        {
            return new IntVector2((int)vec2.x, (int)vec2.y);
        }
    }
}
