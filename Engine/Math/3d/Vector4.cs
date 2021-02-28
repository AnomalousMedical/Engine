using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public unsafe struct Vector4
    {
        public static readonly Vector4 Zero = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);

        [FieldOffset(0)]
        public float x;

        [FieldOffset(4)]
        public float y;

        [FieldOffset(8)]
        public float z;

        [FieldOffset(12)]
        public float w;

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        internal float this[int i]
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

        public static Vector4 operator * (Vector4 v, Matrix4x4 mat)
        {
            return new Vector4(
                v.x * mat.m00 + v.y * mat.m10 + v.z * mat.m20 + v.w * mat.m30,
                v.x * mat.m01 + v.y * mat.m11 + v.z * mat.m21 + v.w * mat.m31,
                v.x * mat.m02 + v.y * mat.m12 + v.z * mat.m22 + v.w * mat.m32,
                v.x * mat.m03 + v.y * mat.m13 + v.z * mat.m23 + v.w * mat.m33
                );
        }

        public static Vector4 operator *(Vector4 v, float s)
        {
            return new Vector4(v.x * s, v.y * s, v.z * s, v.w * s);
        }

        public static Vector4 operator *(float s, Vector4 v)
        {
            return v * s;
        }
    }
}
