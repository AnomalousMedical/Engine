using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public unsafe struct Matrix4x4
    {
        [FieldOffset(0)]
        public float m00;
        [FieldOffset(4)]
        public float m01;
        [FieldOffset(8)]
        public float m02;
        [FieldOffset(12)]
        public float m03;
        [FieldOffset(16)]
        public float m10;
        [FieldOffset(20)]
        public float m11;
        [FieldOffset(24)]
        public float m12;
        [FieldOffset(28)]
        public float m13;
        [FieldOffset(32)]
        public float m20;
        [FieldOffset(36)]
        public float m21;
        [FieldOffset(40)]
        public float m22;
        [FieldOffset(44)]
        public float m23;
        [FieldOffset(48)]
        public float m30;
        [FieldOffset(52)]
        public float m31;
        [FieldOffset(56)]
        public float m32;
        [FieldOffset(60)]
        public float m33;

        public Matrix4x4(
            float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23,
            float m30, float m31, float m32, float m33
        )
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m03 = m03;
            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;
            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;
            this.m30 = m30;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
        }

        public static Vector3 operator * (Matrix4x4 mat, Vector3 v)
        {
            Vector4* m = (Vector4*)&mat.m00;

            float invW = 1.0f / (m[3][0] * v.x + m[3][1] * v.y + m[3][2] * v.z + m[3][3]);

            return new Vector3(
            (m[0][0] * v.x + m[0][1] * v.y + m[0][2] * v.z + m[0][3]) * invW,
            (m[1][0] * v.x + m[1][1] * v.y + m[1][2] * v.z + m[1][3]) * invW,
            (m[2][0] * v.x + m[2][1] * v.y + m[2][2] * v.z + m[2][3]) * invW);
        }
    }
}

/*
float m00;
float m01;
float m02;
float m03;
float m10;
float m11;
float m12;
float m13;
float m20;
float m21;
float m22;
float m23;
float m30;
float m31;
float m32;
float m33;
*/