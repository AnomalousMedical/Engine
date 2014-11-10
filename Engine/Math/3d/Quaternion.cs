using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Engine
{
    /// <summary>
    /// Quaternion math class.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct Quaternion
    {
        #region Static

        private const string TO_STRING_FORMAT = "{0}, {1}, {2}, {3}";

        const float epsilon = 1e-03f;

        #region Predefined Quaternions

        /// <summary>
        /// The identity (no rotation) quaternion.
        /// </summary>
        public static Quaternion Identity = new Quaternion(0, 0, 0, 1);

        #endregion Predefined Quaternions

        #endregion Static

        #region Fields

        [FieldOffset(0)]
        public float x;
        [FieldOffset(4)]
        public float y;
        [FieldOffset(8)]
        public float z;
        [FieldOffset(12)]
        public float w;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.  Set components directly.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        /// <param name="w">W</param>
        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Constructor.  Set the value using a string in the format "X, Y, Z, W".
        /// </summary>
        /// <param name="value">The value string.</param>
        public Quaternion(String value)
        {
            setValue(value, out x, out y, out z, out w);
        }

        /// <summary>
        /// Constructor.  Set the value using axis angle.
        /// </summary>
        /// <param name="axis">Rotation axis.</param>
        /// <param name="angle">Angle of rotation around that axis.</param>
        public Quaternion(ref Vector3 axis, float angle)
        {
            setRotation(ref axis, ref angle, out x, out y, out z, out w);
        }

        /// <summary>
        /// Constructor.  Set the value using axis angle.
        /// </summary>
        /// <param name="axis">Rotation axis.</param>
        /// <param name="angle">Angle of rotation around that axis.</param>
        public Quaternion(Vector3 axis, float angle)
        {
            setRotation(ref axis, ref angle, out x, out y, out z, out w);
        }

        /// <summary>
        /// Constructor.  Set the value using euler angles.
        /// </summary>
        /// <param name="yaw">The yaw (Y-Axis)</param>
        /// <param name="pitch">The pitch (Z-Axis)</param>
        /// <param name="roll">The roll (X-Axis)</param>
        public Quaternion(float yaw, float pitch, float roll)
        {
            setEuler(ref yaw, ref pitch, ref roll, out x, out y, out z, out w);
        }

        public Quaternion(Vector3 eulerVec)
        {
            setEuler(ref eulerVec.x, ref eulerVec.y, ref eulerVec.z, out x, out y, out z, out w);
        }

        #endregion Constructors

        #region Members

        /// <summary>
        /// Set the value using axis angle.
        /// </summary>
        /// <param name="axis">Rotation axis.</param>
        /// <param name="angle">Angle of rotation around that axis. In Radians.</param>
        public void setRotation(ref Vector3 axis, float angle)
        {
            setRotation(ref axis, ref angle, out x, out y, out z, out w);
        }

        /// <summary>
        /// Set the value using euler angles.
        /// </summary>
        /// <param name="yaw">The yaw (Y-Axis)</param>
        /// <param name="pitch">The pitch (Z-Axis)</param>
        /// <param name="roll">The roll (X-Axis)</param>
        public void setEuler(float yaw, float pitch, float roll)
        {
            setEuler(ref yaw, ref pitch, ref roll, out x, out y, out z, out w);
        }

        public void setEuler(Vector3 eulerVec)
        {
            setEuler(ref eulerVec.x, ref eulerVec.y, ref eulerVec.z, out x, out y, out z, out w);
        }

        /// <summary>
        /// Get a set of euler angles that represent this quaternion. X - yaw Y - pitch Z - roll.
        /// </summary>
        /// <returns>A Vector3 with the angles.</returns>
        public Vector3 getEuler()
        {
            double sqw = w * w;
            double sqx = x * x;
            double sqy = y * y;
            double sqz = z * z;
            double unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            double test = x * y + z * w;
            if (test > 0.499 * unit)
            { // singularity at north pole
                return new Vector3(
                    (float)(2 * System.Math.Atan2(x, w)),
                    (float)(System.Math.PI / 2),
                    0);
            }
            if (test < -0.499 * unit)
            { // singularity at south pole
                return new Vector3(
                    (float)(-2 * System.Math.Atan2(x, w)),
                    (float)(-System.Math.PI / 2),
                    0);
            }
            return new Vector3(
                (float)(System.Math.Atan2(2 * y * w - 2 * x * z, sqx - sqy - sqz + sqw)),
                (float)(System.Math.Asin(2 * test / unit)),
                (float)(System.Math.Atan2(2 * x * w - 2 * y * z, -sqx + sqy - sqz + sqw)));

        }

        /// <summary>
        /// Compute the dot product with another quaternion.
        /// </summary>
        /// <param name="q">The Quaternion to compute the dot product of.</param>
        /// <returns>The dot product.</returns>
        public float dot(ref Quaternion q)
        {
            return x * q.x + y * q.y + z * q.z + w * q.w;
        }

        /// <summary>
        /// Compute the length squared of this Quaternion.  Avoids SQRT call.
        /// </summary>
        /// <returns>The squared length of this Quaternion.</returns>
        public float length2()
        {
            return dot(ref this);
        }

        /// <summary>
        /// Compute the length of this quaterion.
        /// </summary>
        /// <returns>The length of this quaternion.</returns>
        public float length()
        {
            return (float)System.Math.Sqrt(length2());
        }

        /// <summary>
        /// Normalize this quaternion and return it.  This will modify this quaternion.
        /// </summary>
        /// <returns>This quaternion normalized.</returns>
        public Quaternion normalize()
        {
            return this /= length();
        }

        /// <summary>
        /// Get another quaternion that is the normalized version of this one.
        /// </summary>
        /// <returns>A new quaternion with this one's normalized value.</returns>
        public Quaternion normalized()
        {
            return this / length();
        }

        /// <summary>
        /// Compute the angle between this quaternion and q.
        /// </summary>
        /// <param name="q">The other quaterinon.</param>
        /// <returns>The angle between these two quaternions.</returns>
        public float angle(ref Quaternion q)
        {
            float s = (float)System.Math.Sqrt(length2() * q.length2());
            return (float)System.Math.Acos(dot(ref q) / s);
        }

        /// <summary>
        /// Compute the axis component of this quaternion for axis-angle.
        /// </summary>
        /// <returns>The axis component of this quaternion.</returns>
        public Vector3 getAxis()
        {
            float fSqrLength = w * w;
            if (fSqrLength > 0.0)
            {
                float fInvLength = (float)(1.0f / System.Math.Sqrt(1.0f - fSqrLength));
                return new Vector3(x * fInvLength, y * fInvLength, z * fInvLength);
            }
            else
            {
                return new Vector3(1.0f, 0.0f, 0.0f);
            }
        }

        /// <summary>
        /// Compute the angle component of this quaternion for axis-angle.
        /// </summary>
        /// <returns>The angle component of this quaternion.</returns>
        public float getAngle()
        {
            float fSqrLength = x * x + y * y + z * z;
            float s = 0.0f;
            if (fSqrLength > 0.0)
            {
                s = 2f * (float)System.Math.Acos(w);
            }
            return s;
        }

        /// <summary>
        /// Compute the inverse of this quaternion (invert x, y, z, w).
        /// </summary>
        /// <returns>A new quaternion the inverse of this one.</returns>
        public Quaternion inverse()
        {
            return new Quaternion(-x, -y, -z, w);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qd"></param>
        /// <returns></returns>
        public Quaternion farthest(ref Quaternion qd)
        {
            Quaternion diff, sum;
            diff = this - qd;
            sum = this + qd;
            if (diff.dot(ref diff) > sum.dot(ref sum))
                return qd;
            return (-qd);
        }

        /// <summary>
        /// Liner interpolation between this quaternion and q over t.
        /// </summary>
        /// <param name="q">Destination quaternion</param>
        /// <param name="t">Time factor 0-1</param>
        /// <returns></returns>
        public Quaternion lerp(ref Quaternion q, ref float t)
        {
            return new Quaternion(x + (q.x - x) * t,
                y + (q.y - y) * t,
                z + (q.z - z) * t,
                w + (q.w - w) * t);
        }

        /// <summary>
        /// Normalized Liner interpolation between this quaternion and q over t.
        /// </summary>
        /// <remarks>
        /// Can use this instead of Slerp. It will not have constant velocity (although it doesn't
        /// seem noticable in practice), but is commutative and torque-mimimal. nlerp is faster for 
        /// pretty much the same results.
        /// </remarks>
        /// <param name="q">Destination quaternion</param>
        /// <param name="t">Time factor 0-1</param>
        /// <returns></returns>
        public Quaternion nlerp(ref Quaternion q, ref float t)
        {
            return lerp(ref q, ref t).normalized();
        }

        /// <summary>
        /// Compute a spherical linear interpolation for this quaternion to rotate
        /// to q over t.
        /// </summary>
        /// <remarks>
        /// This will do a slerp, which is constant velocity and torque-minimal, but 
        /// is not commutative, be careful what order you put in the arguments.
        /// </remarks>
        /// <param name="q">The goal quaternion.</param>
        /// <param name="t">The amount of time passed between 0 and 1.</param>
        /// <returns></returns>
        public Quaternion slerp(ref Quaternion q, float t)
        {
            float theta = angle(ref q);
            if (theta != 0.0f)
            {
                float d = 1.0f / (float)System.Math.Sin(theta);
                float s0 = (float)System.Math.Sin((1.0f - t) * theta);
                float sinYaw = (float)System.Math.Sin(t * theta);
                return new Quaternion((x * s0 + q.x * sinYaw) * d,
                    (y * s0 + q.y * sinYaw) * d,
                    (z * s0 + q.z * sinYaw) * d,
                    (w * s0 + q.w * sinYaw) * d);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Taken from ogre since it also adds the shortest path.
        /// </summary>
        /// <param name="q"></param>
        /// <param name="t"></param>
        /// <param name="shortestPath"></param>
        /// <returns></returns>
        public Quaternion slerp(ref Quaternion q, float fT, bool shortestPath)
        {
            Quaternion rkP = this;
            Quaternion rkQ = q;

            float fCos = rkP.dot(ref rkQ);
            Quaternion rkT;

            // Do we need to invert rotation?
            if (fCos < 0.0f && shortestPath)
            {
                fCos = -fCos;
                rkT = -rkQ;
            }
            else
            {
                rkT = rkQ;
            }

            if (Math.Abs(fCos) < 1 - epsilon)
            {
                // Standard case (slerp)
                float fSin = (float)Math.Sqrt(1 - fCos * fCos);
                Radian fAngle = (float)Math.Atan2(fSin, fCos);
                float fInvSin = 1.0f / fSin;
                float fCoeff0 = (float)Math.Sin((1.0f - fT) * fAngle) * fInvSin;
                float fCoeff1 = (float)Math.Sin(fT * fAngle) * fInvSin;
                return fCoeff0 * rkP + fCoeff1 * rkT;
            }
            else
            {
                // There are two situations:
                // 1. "rkP" and "rkQ" are very close (fCos ~= +1), so we can do a linear
                //    interpolation safely.
                // 2. "rkP" and "rkQ" are almost inverse of each other (fCos ~= -1), there
                //    are an infinite number of possibilities interpolation. but we haven't
                //    have method to fix this case, so just use linear interpolation here.
                Quaternion t = (1.0f - fT) * rkP + fT * rkT;
                // taking the complement requires renormalisation
                t.normalize();
                return t;
            }
        }

        public Matrix3x3 toRotationMatrix()
        {
            float fTx = x + x;
            float fTy = y + y;
            float fTz = z + z;
            float fTwx = fTx * w;
            float fTwy = fTy * w;
            float fTwz = fTz * w;
            float fTxx = fTx * x;
            float fTxy = fTy * x;
            float fTxz = fTz * x;
            float fTyy = fTy * y;
            float fTyz = fTz * y;
            float fTzz = fTz * z;

            Matrix3x3 kRot;
            kRot.m00 = 1.0f - (fTyy + fTzz);
            kRot.m01 = fTxy - fTwz;
            kRot.m02 = fTxz + fTwy;
            kRot.m10 = fTxy + fTwz;
            kRot.m11 = 1.0f - (fTxx + fTzz);
            kRot.m12 = fTyz - fTwx;
            kRot.m20 = fTxz - fTwy;
            kRot.m21 = fTyz + fTwx;
            kRot.m22 = 1.0f - (fTxx + fTyy);

            return kRot;
        }

        static int[] fromRotNext = { 1, 2, 0 };
        public void fromRotationMatrix(Matrix3x3 kRot)
        {
            // Algorithm in Ken Shoemake's article in 1987 SIGGRAPH course notes
            // article "Quaternion Calculus and Fast Animation".

            float fTrace = kRot.m00 + kRot.m11 + kRot.m22;
            float fRoot;

            if (fTrace > 0.0)
            {
                // |w| > 1/2, may as well choose w > 1/2
                fRoot = (float)Math.Sqrt(fTrace + 1.0f);  // 2w
                w = 0.5f * fRoot;
                fRoot = 0.5f / fRoot;  // 1/(4w)
                x = (kRot.m21 - kRot.m12) * fRoot;
                y = (kRot.m02 - kRot.m20) * fRoot;
                z = (kRot.m10 - kRot.m01) * fRoot;
            }
            else
            {
                // |w| <= 1/2
                int i = 0;
                if (kRot.m11 > kRot.m00)
                {
                    i = 1;
                }
                if (kRot.m22 > kRot[i, i])
                {
                    i = 2;
                }
                int j = fromRotNext[i];
                int k = fromRotNext[j];

                fRoot = (float)Math.Sqrt(kRot[i, i] - kRot[j, j] - kRot[k, k] + 1.0f);
                Vector3 apkQuat = new Vector3();
                apkQuat[i] = 0.5f * fRoot;
                fRoot = 0.5f / fRoot;
                w = (kRot[k, j] - kRot[j, k]) * fRoot;
                apkQuat[j] = (kRot[j, i] + kRot[i, j]) * fRoot;
                apkQuat[k] = (kRot[k, i] + kRot[i, k]) * fRoot;

                x = apkQuat.x;
                y = apkQuat.y;
                z = apkQuat.z;
            }
        }

        public void toAxes(Vector3[] axis)
        {
            Matrix3x3 kRot = toRotationMatrix();

            axis[0].x = kRot.m00;
            axis[0].y = kRot.m10;
            axis[0].z = kRot.m20;

            axis[1].x = kRot.m01;
            axis[1].y = kRot.m11;
            axis[1].z = kRot.m21;

            axis[2].x = kRot.m02;
            axis[2].y = kRot.m12;
            axis[2].z = kRot.m22;
        }

        public void fromAxes(Vector3 xaxis, Vector3 yaxis, Vector3 zaxis)
        {
            Matrix3x3 kRot = new Matrix3x3(xaxis.x, yaxis.x, zaxis.x,
                                           xaxis.y, yaxis.y, zaxis.y,
                                           xaxis.z, yaxis.z, zaxis.z);
            fromRotationMatrix(kRot);
        }

        /// <summary>
        /// Set the value directly all at once.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        /// <param name="w">W</param>
        public void setValue(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Set the value using a string in the format "X, Y, Z, W".
        /// </summary>
        /// <param name="value">The value string.</param>
        /// <returns>True if the value was accepted.</returns>
        public bool setValue(String value)
        {
            return setValue(value, out x, out y, out z, out w);
        }

        /// <summary>
        /// Determine if all components are numbers i.e != float.NaN.
        /// </summary>
        /// <returns>True if all components are != float.NaN</returns>
        public bool isNumber()
        {
            return !float.IsNaN(x) && !float.IsNaN(y) && !float.IsNaN(z) && !float.IsNaN(w);
        }

        /// <summary>
        /// Rotate the v by q.
        /// </summary>
        /// <param name="v">The vector to rotate.</param>
        /// <param name="rotation">The quaternion containing how far to rotate.</param>
        /// <returns>The rotated vector.</returns>
        public static Vector3 quatRotate(ref Quaternion rotation, ref Vector3 v)
        {
            Quaternion q = rotation * v;
            q *= rotation.inverse();
            return new Vector3(q.x, q.y, q.z);
        }

        /// <summary>
        /// Rotate the v by q.
        /// </summary>
        /// <param name="v">The vector to rotate.</param>
        /// <param name="rotation">The quaternion containing how far to rotate.</param>
        /// <returns>The rotated vector.</returns>
        public static Vector3 quatRotate(Quaternion rotation, Vector3 v)
        {
            return Quaternion.quatRotate(ref rotation, ref v);
        }

        /// <summary>
        /// Find the shortest arc between two vectors.
        /// </summary>
        /// <param name="v0">Source vector.</param>
        /// <param name="v1">Destination vector.</param>
        /// <returns>The quaternion with the shortest arc.</returns>
        public static Quaternion shortestArcQuat(ref Vector3 v0, ref Vector3 v1)
        {
            Vector3 c = v0.cross(ref v1);
            float d = v0.dot(ref v1);

            if (d < -1.0 + FLT_EPSILON)
                return new Quaternion(0.0f, 1.0f, 0.0f, 0.0f); // just pick any vector

            float s = (float)System.Math.Sqrt((1.0f + d) * 2.0f);
            float rs = 1.0f / s;

            return new Quaternion(c.x * rs, c.y * rs, c.z * rs, s * 0.5f).normalized();
        }

        /// <summary>
        /// Find the shortest arc between two vectors.
        /// </summary>
        /// <param name="v0">Source vector.</param>
        /// <param name="v1">Destination vector.</param>
        /// <returns>The quaternion with the shortest arc.</returns>
        public static Quaternion shortestArcQuat(Vector3 v0, Vector3 v1)
        {
            return shortestArcQuat(ref v0, ref v1);
        }

        /// <summary>
        /// Find the shortest arc, but first normalize the two vectors.
        /// </summary>
        /// <param name="v0">Source vector.</param>
        /// <param name="v1">Destination vector.</param>
        /// <returns>The quaternion with the shortest arc.</returns>
        public static Quaternion shortestArcQuatNormalize2(ref Vector3 v0, ref Vector3 v1)
        {
            v0.normalize();
            v1.normalize();
            return shortestArcQuat(ref v0, ref v1);
        }

        /// <summary>
        /// Compute the shortest arc quaternion with a fixed yaw axis.
        /// </summary>
        /// <param name="direction">The direction to face. Must be normalized.</param>
        /// <returns>The quaternion that will get to this orientation.</returns>
        public static Quaternion shortestArcQuatFixedYaw(ref Vector3 direction)
        {
            return shortestArcQuatFixedYaw(ref direction, ref Vector3.Up);
        }

        /// <summary>
        /// Compute the shortest arc quaternion with a fixed yaw axis.
        /// </summary>
        /// <param name="direction">The direction to face. Must be normalized.</param>
        /// <param name="yawFixedAxis">The axis to fix as yaw.</param>
        /// <returns>The quaternion that will get to this orientation.</returns>
        public static Quaternion shortestArcQuatFixedYaw(ref Vector3 direction, ref Vector3 yawFixedAxis)
        {
            Vector3 xVec = yawFixedAxis.cross(ref direction);
            xVec.normalize();

            Vector3 yVec = direction.cross(ref xVec);
            yVec.normalize();

            Quaternion targetWorldOrientation = new Quaternion();
            targetWorldOrientation.fromAxes(xVec, yVec, direction);

            return targetWorldOrientation;
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, TO_STRING_FORMAT, x, y, z, w);
        }

        #endregion Members

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

        public static Quaternion operator *(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y,
                q1.w * q2.y + q1.y * q2.w + q1.z * q2.x - q1.x * q2.z,
                q1.w * q2.z + q1.z * q2.w + q1.x * q2.y - q1.y * q2.x,
                q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z);
        }

        public static Quaternion operator *(Quaternion q, Vector3 w)
        {
            return new Quaternion(q.w * w.x + q.y * w.z - q.z * w.y,
                q.w * w.y + q.z * w.x - q.x * w.z,
                q.w * w.z + q.x * w.y - q.y * w.x,
                -q.x * w.x - q.y * w.y - q.z * w.z);
        }

        public static Quaternion operator *(Vector3 w, Quaternion q)
        {
            return new Quaternion(w.x * q.w + w.y * q.z - w.z * q.y,
                w.y * q.w + w.z * q.x - w.x * q.z,
                w.z * q.w + w.x * q.y - w.y * q.x,
                -w.x * q.x - w.y * q.y - w.z * q.z);
        }

        public static Quaternion operator *(Quaternion q, float s)
        {
            return new Quaternion(q.x * s, q.y * s, q.z * s, q.w * s);
        }

        public static Quaternion operator *(float s, Quaternion q)
        {
            return new Quaternion(q.x * s, q.y * s, q.z * s, q.w * s);
        }

        public static Quaternion operator /(Quaternion q, float s)
        {
            return q * (1.0f / s);
        }

        public static Quaternion operator +(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(q1.x + q2.x, q1.y + q2.y, q1.z + q2.z, q1.w + q2.w);
        }

        public static Quaternion operator -(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(q1.x - q2.x, q1.y - q2.y, q1.z - q2.z, q1.w - q2.w);
        }

        public static Quaternion operator -(Quaternion q2)
        {
            return new Quaternion(-q2.x, -q2.y, -q2.z, -q2.w);
        }

        public static bool operator ==(Quaternion p1, Quaternion p2)
        {
            return p1.x == p2.x && p1.y == p2.y && p1.z == p2.z && p1.w != p2.w;
        }

        public static bool operator !=(Quaternion p1, Quaternion p2)
        {
            return !(p1.x == p2.x && p1.y == p2.y && p1.z == p2.z && p1.w != p2.w);
        }

        #endregion Operators

        #region Static Helpers

        private static float FLT_EPSILON = 1.192092896e-07f; /* smallest such that 1.0+FLT_EPSILON != 1.0 */
        private static char[] SEPS = { ',' };

        private static bool setValue(String value, out float x, out float y, out float z, out float w)
        {
            String[] nums = value.Split(SEPS);
            bool success = false;
            if (nums.Length == 4)
            {
                success = NumberParser.TryParse(nums[0], out x);
                success &= NumberParser.TryParse(nums[1], out y);
                success &= NumberParser.TryParse(nums[2], out z);
                success &= NumberParser.TryParse(nums[3], out w);
            }
            else
            {
                x = 0f;
                y = 0f;
                z = 0f;
                w = 0f;
            }
            return success;
        }

        private static void setEuler(ref float yaw, ref float pitch, ref float roll, out float x, out float y, out float z, out float w)
        {
            double cosYaw = System.Math.Cos(yaw / 2);
            double sinYaw = System.Math.Sin(yaw / 2);
            double cosPitch = System.Math.Cos(pitch / 2);
            double sinPitch = System.Math.Sin(pitch / 2);
            double cosRoll = System.Math.Cos(roll / 2);
            double sinRoll = System.Math.Sin(roll / 2);
            double cosYawPitch = cosYaw * cosPitch;
            double sinYawPitch = sinYaw * sinPitch;
            w = (float)(cosYawPitch * cosRoll - sinYawPitch * sinRoll);
            x = (float)(cosYawPitch * sinRoll + sinYawPitch * cosRoll);
            y = (float)(sinYaw * cosPitch * cosRoll + cosYaw * sinPitch * sinRoll);
            z = (float)(cosYaw * sinPitch * cosRoll - sinYaw * cosPitch * sinRoll);
        }

        /// <summary>
        /// Set the rotation from angle axis, angle is in radians
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        private static void setRotation(ref Vector3 axis, ref float angle, out float x, out float y, out float z, out float w)
        {
            float d = axis.length();
            float s = (float)System.Math.Sin(angle * 0.5f) / d;
            x = axis.x * s;
            y = axis.y * s;
            z = axis.z * s;
            w = (float)System.Math.Cos(angle * 0.5f);
        }

        #endregion Static Helpers
    }
}
