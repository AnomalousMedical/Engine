using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Engine
{
    /// <summary>
    /// Vector3 math class.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size=12)]
    public struct Vector3
    {
        #region Predefined Vectors
        public static Vector3 Zero = new Vector3(0.0f, 0.0f, 0.0f);
        public static Vector3 Forward = new Vector3(0.0f, 0.0f, -1.0f);
        public static Vector3 Backward = new Vector3(0.0f, 0.0f, 1.0f);
        public static Vector3 Left = new Vector3(-1.0f, 0.0f, 0.0f);
        public static Vector3 Right = new Vector3(1.0f, 0.0f, 0.0f);
        public static Vector3 Up = new Vector3(0.0f, 1.0f, 0.0f);
        public static Vector3 Down = new Vector3(0.0f, -1.0f, 0.0f);

        public static Vector3 UnitX = new Vector3(1.0f, 0.0f, 0.0f);
        public static Vector3 UnitY = new Vector3(0.0f, 1.0f, 0.0f);
        public static Vector3 UnitZ = new Vector3(0.0f, 0.0f, 1.0f);

        public static readonly Vector3 Invalid = new Vector3(float.NaN, float.NaN, float.NaN);
        public static readonly Vector3 Min = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        public static readonly Vector3 Max = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        public static Vector3 ScaleIdentity = new Vector3(1.0f, 1.0f, 1.0f);
        #endregion Predefined Vectors

        #region Fields

        [FieldOffset(0)]
        public float x;
        
        [FieldOffset(4)]
        public float y;

        [FieldOffset(8)]
        public float z;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize constructor.
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        /// <param name="z">Z value.</param>
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// String constructor. See setValue for details.
        /// </summary>
        /// <param name="value">A string in the format "x, y, z".</param>
        public Vector3(String value)
        {
            parseString(value, out x, out y, out z);
        }

        #endregion Constructors

        #region Members

        /// <summary>
        /// Set the value of this Vector3. Must be a string in the format "x, y,
        /// z" whitespace does not matter.
        /// </summary>
        /// <param name="value">A string in the format "x, y, z".</param>
        /// <returns>True if the value was set sucessfully.</returns>
        public bool setValue(String value)
        {
            return parseString(value, out x, out y, out z);
        }

        /// <summary>
        /// Compute the dot product of this vector and another.
        /// </summary>
        /// <param name="v">The other vector.</param>
        /// <returns>The dot product.</returns>
        public float dot(ref Vector3 v)
        {
            return x * v.x + y * v.y + z * v.z;
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
        public float distance2(ref Vector3 v)
        {
            return (v - this).length2();
        }

        /// <summary>
        /// Compute the distance between two vectors.
        /// </summary>
        /// <param name="v">The other vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public float distance(ref Vector3 v)
        {
            return (v - this).length();
        }

        /// <summary>
        /// Normalize this vector and return it. Modifies this vector to be
        /// normalized.
        /// </summary>
        /// <returns>The same vector normalized.</returns>
        public Vector3 normalize()
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
        public Vector3 normalized()
        {
            return this / length();
        }

        /// <summary>
        /// Rotate this vector about the given axis angle.
        /// </summary>
        /// <param name="wAxis">The axis to rotate about. Must be unit length.</param>
        /// <param name="angle">The angle of rotation.</param>
        /// <returns></returns>
        public Vector3 rotate(Vector3 wAxis, float angle)
        {
            // wAxis must be a unit lenght vector
	        Vector3 o = wAxis * wAxis.dot(ref this);
	        Vector3 x = this - o;
	        Vector3 y;

	        y = wAxis.cross(ref this);

	        return ( o + x * (float)System.Math.Cos( angle ) + y * (float)System.Math.Sin( angle ) );
        }

        /// <summary>
	    /// Compute the angle between two vectors.
	    /// </summary>
	    /// <param name="v">The other vector.</param>
	    /// <returns></returns>
        public float angle(ref Vector3 v)  
	    {
		    float s = (float)System.Math.Sqrt(length2() * v.length2());
		    return (float)System.Math.Acos(dot(ref v) / s);
	    }

	    /// <summary>
	    /// Get a new vector with the absolute value of this one.
	    /// </summary>
	    /// <returns>A new vector that is abs.</returns>
        public Vector3 absolute()  
	    {
		    return new Vector3(
			    (float)System.Math.Abs(x), 
			    (float)System.Math.Abs(y), 
			    (float)System.Math.Abs(z));
	    }

	    /// <summary>
	    /// Compute the cross product of two vectors.
	    /// </summary>
	    /// <param name="v">The other vector.</param>
	    /// <returns>The cross product of the two vectors.</returns>
        public Vector3 cross(ref Vector3 v) 
	    {
		    return new Vector3(
			    y * v.z - z * v.y,
			    z * v.x - x * v.z,
			    x * v.y - y * v.x);
	    }

        /// <summary>
        /// Compute the cross product of two vectors. Passing by value.
        /// </summary>
        /// <param name="v">The other vector.</param>
        /// <returns>The cross product of the two vectors.</returns>
        public Vector3 crossV(Vector3 v)
        {
            return cross(ref v);
        }

	    /// <summary>
	    /// Compute the triple product with this vector as the dot product.
	    /// </summary>
	    /// <param name="v1">Cross product vector 1.</param>
	    /// <param name="v2">Cross product vector 2.</param>
	    /// <returns>The triple of the three vectors.</returns>
        public float triple(ref Vector3 v1, ref Vector3 v2) 
	    {
		    return x * (v1.y * v2.z - v1.z * v2.y) + 
			    y * (v1.z * v2.x - v1.x * v2.z) + 
			    z * (v1.x * v2.y - v1.y * v2.x);
	    }

	    /// <summary>
	    /// Find the smallest axis.  Negative numbers count as smaller than positive ones.  See furthestAxis().
	    /// </summary>
	    /// <returns>
	    /// 0 if x is the smallest.
	    /// 1 if y is the smallest.
	    /// 2 if z is the smallest.
	    /// </returns>
        public int minAxis() 
	    {
		    return x < y ? (x < z ? 0 : 2) : (y < z ? 1 : 2);
	    }

	    /// <summary>
	    /// Find the biggest axis.  Negative numbers count as smaller than positive ones.  See closestAxis().
	    /// </summary>
	    /// <returns>
	    /// 0 if x is the biggest.
	    /// 1 if y is the biggest.
	    /// 2 if z is the biggest.
	    /// </returns>
        public int maxAxis()  
	    {
		    return x < y ? (y < z ? 2 : 1) : (x < z ? 2 : 0);
	    }

	    /// <summary>
	    /// Find the furthest axis.  Accounts for negative values.
	    /// </summary>
	    /// <returns>
	    /// 0 if x is the biggest.
	    /// 1 if y is the biggest.
	    /// 2 if z is the biggest.
	    /// </returns>
        public int furthestAxis() 
	    {
		    return absolute().minAxis();
	    }

	    /// <summary>
	    /// Find the closest axis.  Accounts for negative values.
	    /// </summary>
	    /// <returns>
	    /// 0 if x is the biggest.
	    /// 1 if y is the biggest.
	    /// 2 if z is the biggest.
	    /// </returns>
        public int closestAxis()  
	    {
		    return absolute().maxAxis();
	    }

	    /// <summary>
	    /// Interpolate between two vectors.  Set the result to this vector.
	    /// </summary>
	    /// <param name="v0">The source vector.</param>
	    /// <param name="v1">The destination vector.</param>
	    /// <param name="rt">The amount to interpolate.</param>
        public void setInterpolate3(ref Vector3 v0, ref Vector3 v1, float rt)
	    {
		    float s = 1.0f - rt;
		    x = s * v0.x + rt * v1.x;
		    y = s * v0.y + rt * v1.y;
		    z = s * v0.z + rt * v1.z;
	    }

	    /// <summary>
	    /// Liner interpolation between this vector and v over t
	    /// </summary>
	    /// <param name="v">Destination vector</param>
	    /// <param name="t">Time factor 0-1</param>
	    /// <returns></returns>
        public Vector3 lerp(ref Vector3 v, ref float t)  
	    {
		    return new Vector3(x + (v.x - x) * t,
			    y + (v.y - y) * t,
			    z + (v.z - z) * t);
	    }

        /// <summary>
        /// Liner interpolation between this vector and v over t
        /// </summary>
        /// <param name="v">Destination vector</param>
        /// <param name="t">Time factor 0-1</param>
        /// <returns></returns>
        public Vector3 lerp(Vector3 v, float t)
        {
            return new Vector3(x + (v.x - x) * t,
                y + (v.y - y) * t,
                z + (v.z - z) * t);
        }

        /// <summary>
        /// Set this vector's components to be the highest value of x, y and z between the two.
        /// </summary>
        /// <param name="cmp">The vector to compare.</param>
        public void makeCeiling(Vector3 cmp)
        {
            if (cmp.x > x) x = cmp.x;
            if (cmp.y > y) y = cmp.y;
            if (cmp.z > z) z = cmp.z;
        }

        /// <summary>
        /// Set this vector's components to be the lowest value of x, y and z between the two.
        /// </summary>
        /// <param name="cmp">The vector to compare.</param>
        public void makeFloor(Vector3 cmp)
        {
            if (cmp.x < x) x = cmp.x;
            if (cmp.y < y) y = cmp.y;
            if (cmp.z < z) z = cmp.z;
        }

        /// <summary>
        /// Equals function.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the objects are equal.</returns>
        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(Vector3) && this == (Vector3)obj;
        }

        /// <summary>
        /// Hash code function.
        /// </summary>
        /// <returns>A hash code for this Vector3.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// ToString.
        /// </summary>
        /// <returns>The Vector as a string. This string is in the correct format to be used with setValue.</returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}", x, y, z);
        }

        /// <summary>
        /// Determine if all components are numbers i.e != float.NaN.
        /// </summary>
        /// <returns>True if all components are != float.NaN</returns>
        public bool isNumber()
	    {
		    return !float.IsNaN(x) && !float.IsNaN(y) && !float.IsNaN(z);
	    }

        #endregion Memebers

        #region Operators

        internal unsafe float this[int i]
        {
            get
            {
                fixed (float* p = &this.x)
                {
                    return p[i];
                }
            }
            set
            {
                fixed (float* p = &this.x)
                {
                    p[i] = value;
                }
            }
        }

        public static Vector3 operator + (Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3 operator * (Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public static Vector3 operator - (Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3 operator - (Vector3 v)
        {
            return new Vector3(-v.x, -v.y, -v.z);
        }

        public static Vector3 operator * (Vector3 v, float s)
        {
            return new Vector3(v.x * s, v.y * s, v.z * s);
        }

        public static Vector3 operator * (float s, Vector3 v)
        {
            return v * s;
        }

        public static Vector3 operator / (Vector3 v, float s)
	    {
		    return v * (1.0f / s);
	    }

        public static Vector3 operator / (Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        public static bool operator == (Vector3 p1, Vector3 p2)
        {
            return p1.x == p2.x && p1.y == p2.y && p1.z == p2.z;
        }

        public static bool operator != (Vector3 p1, Vector3 p2)
        {
            return !(p1.x == p2.x && p1.y == p2.y && p1.z == p2.z);
        }

        public static implicit operator Vector3(IntVector3 vec2)
        {
            return new Vector3((float)vec2.x, (float)vec2.y, (float)vec2.z);
        }

        #endregion Operators

        #region Static Helper Functions

        private static char[] SEPS = { ',' };
        static private bool parseString(String value, out float x, out float y, out float z)
        {
            String[] nums = value.Split(SEPS);
            bool success = false;
            if (nums.Length == 3)
            {
                success = NumberParser.TryParse(nums[0], out x);
                success &= NumberParser.TryParse(nums[1], out y);
                success &= NumberParser.TryParse(nums[2], out z);
            }
            else
            {
                x = 0f;
                y = 0f;
                z = 0f;
            }
            return success;
        }

        #endregion Static Helper Functions
    }
}
