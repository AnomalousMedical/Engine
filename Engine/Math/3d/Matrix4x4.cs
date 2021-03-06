﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public unsafe struct Matrix4x4
    {
        public static Matrix4x4 Identity = new Matrix4x4(1, 0, 0, 0,
                                                         0, 1, 0, 0,
                                                         0, 0, 1, 0,
                                                         0, 0, 0, 1);

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

        public Matrix4x4(float* m)
        {
            this.m00 = m[0];
            this.m01 = m[1];
            this.m02 = m[2];
            this.m03 = m[3];
            this.m10 = m[4];
            this.m11 = m[5];
            this.m12 = m[6];
            this.m13 = m[7];
            this.m20 = m[8];
            this.m21 = m[9];
            this.m22 = m[10];
            this.m23 = m[11];
            this.m30 = m[12];
            this.m31 = m[13];
            this.m32 = m[14];
            this.m33 = m[15];
        }

        public Matrix4x4 inverse()
        {
            float v0 = m20 * m31 - m21 * m30;
            float v1 = m20 * m32 - m22 * m30;
            float v2 = m20 * m33 - m23 * m30;
            float v3 = m21 * m32 - m22 * m31;
            float v4 = m21 * m33 - m23 * m31;
            float v5 = m22 * m33 - m23 * m32;

            float t00 = +(v5 * m11 - v4 * m12 + v3 * m13);
            float t10 = -(v5 * m10 - v2 * m12 + v1 * m13);
            float t20 = +(v4 * m10 - v2 * m11 + v0 * m13);
            float t30 = -(v3 * m10 - v1 * m11 + v0 * m12);

            float invDet = 1 / (t00 * m00 + t10 * m01 + t20 * m02 + t30 * m03);

            float d00 = t00 * invDet;
            float d10 = t10 * invDet;
            float d20 = t20 * invDet;
            float d30 = t30 * invDet;

            float d01 = -(v5 * m01 - v4 * m02 + v3 * m03) * invDet;
            float d11 = +(v5 * m00 - v2 * m02 + v1 * m03) * invDet;
            float d21 = -(v4 * m00 - v2 * m01 + v0 * m03) * invDet;
            float d31 = +(v3 * m00 - v1 * m01 + v0 * m02) * invDet;

            v0 = m10 * m31 - m11 * m30;
            v1 = m10 * m32 - m12 * m30;
            v2 = m10 * m33 - m13 * m30;
            v3 = m11 * m32 - m12 * m31;
            v4 = m11 * m33 - m13 * m31;
            v5 = m12 * m33 - m13 * m32;

            float d02 = +(v5 * m01 - v4 * m02 + v3 * m03) * invDet;
            float d12 = -(v5 * m00 - v2 * m02 + v1 * m03) * invDet;
            float d22 = +(v4 * m00 - v2 * m01 + v0 * m03) * invDet;
            float d32 = -(v3 * m00 - v1 * m01 + v0 * m02) * invDet;

            v0 = m21 * m10 - m20 * m11;
            v1 = m22 * m10 - m20 * m12;
            v2 = m23 * m10 - m20 * m13;
            v3 = m22 * m11 - m21 * m12;
            v4 = m23 * m11 - m21 * m13;
            v5 = m23 * m12 - m22 * m13;

            float d03 = -(v5 * m01 - v4 * m02 + v3 * m03) * invDet;
            float d13 = +(v5 * m00 - v2 * m02 + v1 * m03) * invDet;
            float d23 = -(v4 * m00 - v2 * m01 + v0 * m03) * invDet;
            float d33 = +(v3 * m00 - v1 * m01 + v0 * m02) * invDet;

            return new Matrix4x4(
                d00, d01, d02, d03,
                d10, d11, d12, d13,
                d20, d21, d22, d23,
                d30, d31, d32, d33);
        }

        public void setRotation(Matrix3x3 rotMat)
        {
            m00 = rotMat.m00;
            m01 = rotMat.m01;
            m02 = rotMat.m02;

            m10 = rotMat.m10;
            m11 = rotMat.m11;
            m12 = rotMat.m12;

            m20 = rotMat.m20;
            m21 = rotMat.m21;
            m22 = rotMat.m22;
        }

        public static Vector3 operator *(Matrix4x4 mat, Vector3 v)
        {
            float invW = 1.0f / (mat.m30 * v.x + mat.m31 * v.y + mat.m32 * v.z + mat.m33);

            return new Vector3(
            (mat.m00 * v.x + mat.m01 * v.y + mat.m02 * v.z + mat.m03) * invW,
            (mat.m10 * v.x + mat.m11 * v.y + mat.m12 * v.z + mat.m13) * invW,
            (mat.m20 * v.x + mat.m21 * v.y + mat.m22 * v.z + mat.m23) * invW);
        }

        public static Vector4 operator * (Matrix4x4 mat, Vector4 v)
        {
            return new Vector4(
                mat.m00 * v.x + mat.m01 * v.y + mat.m02 * v.z + mat.m03 * v.w, 
                mat.m10 * v.x + mat.m11 * v.y + mat.m12 * v.z + mat.m13 * v.w,
                mat.m20 * v.x + mat.m21 * v.y + mat.m22 * v.z + mat.m23 * v.w,
                mat.m30 * v.x + mat.m31 * v.y + mat.m32 * v.z + mat.m33 * v.w
                );
        }

        public static Matrix4x4 operator *(Matrix4x4 m1, Matrix4x4 m2)
        {
            return new Matrix4x4
            (
                m1.m00 * m2.m00 + m1.m01 * m2.m10 + m1.m02 * m2.m20 + m1.m03 * m2.m30,
                m1.m00 * m2.m01 + m1.m01 * m2.m11 + m1.m02 * m2.m21 + m1.m03 * m2.m31,
                m1.m00 * m2.m02 + m1.m01 * m2.m12 + m1.m02 * m2.m22 + m1.m03 * m2.m32,
                m1.m00 * m2.m03 + m1.m01 * m2.m13 + m1.m02 * m2.m23 + m1.m03 * m2.m33,

                m1.m10 * m2.m00 + m1.m11 * m2.m10 + m1.m12 * m2.m20 + m1.m13 * m2.m30,
                m1.m10 * m2.m01 + m1.m11 * m2.m11 + m1.m12 * m2.m21 + m1.m13 * m2.m31,
                m1.m10 * m2.m02 + m1.m11 * m2.m12 + m1.m12 * m2.m22 + m1.m13 * m2.m32,
                m1.m10 * m2.m03 + m1.m11 * m2.m13 + m1.m12 * m2.m23 + m1.m13 * m2.m33,

                m1.m20 * m2.m00 + m1.m21 * m2.m10 + m1.m22 * m2.m20 + m1.m23 * m2.m30,
                m1.m20 * m2.m01 + m1.m21 * m2.m11 + m1.m22 * m2.m21 + m1.m23 * m2.m31,
                m1.m20 * m2.m02 + m1.m21 * m2.m12 + m1.m22 * m2.m22 + m1.m23 * m2.m32,
                m1.m20 * m2.m03 + m1.m21 * m2.m13 + m1.m22 * m2.m23 + m1.m23 * m2.m33,

                m1.m30 * m2.m00 + m1.m31 * m2.m10 + m1.m32 * m2.m20 + m1.m33 * m2.m30,
                m1.m30 * m2.m01 + m1.m31 * m2.m11 + m1.m32 * m2.m21 + m1.m33 * m2.m31,
                m1.m30 * m2.m02 + m1.m31 * m2.m12 + m1.m32 * m2.m22 + m1.m33 * m2.m32,
                m1.m30 * m2.m03 + m1.m31 * m2.m13 + m1.m32 * m2.m23 + m1.m33 * m2.m33
            );
        }

        public String DisplayString
        {
            get
            {
                return String.Format("{0,10}, {1,10}, {2,10}, {3,10}\n{4,10}, {5,10}, {6,10}, {7,10}\n{8,10}, {9,10}, {10,10}, {11,10},\n{12,10}, {13,10}, {14,10}, {15,10}", m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
            }
        }

        /// <summary>
        /// Calculate a view matrix. It will match what ogre creates. This method is from their codebase.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="orientation">The orientation.</param>
        /// <returns>A Matrix4x4 that is the view matrix for a camera with the given position and orientation.</returns>
        public static Matrix4x4 makeViewMatrix(Vector3 position, Quaternion orientation)
        //,const Matrix4x4 reflectMatrix)
        {
            Matrix4x4 viewMatrix;

            // View matrix is:
            //
            //  [ Lx  Uy  Dz  Tx  ]
            //  [ Lx  Uy  Dz  Ty  ]
            //  [ Lx  Uy  Dz  Tz  ]
            //  [ 0   0   0   1   ]
            //
            // Where T = -(Transposed(Rot) * Pos)

            // This is most efficiently done using 3x3 Matrices
            Matrix3x3 rot = orientation.toRotationMatrix();

            // Make the translation relative to new axes
            Matrix3x3 rotT = rot.transpose();
            Vector3 trans = -rotT * position;

            // Make final matrix
            viewMatrix = Matrix4x4.Identity;
            viewMatrix.setRotation(rotT); // fills upper 3x3
            viewMatrix.m03 = trans.x;
            viewMatrix.m13 = trans.y;
            viewMatrix.m23 = trans.z;

            // Deal with reflections
            //if (reflectMatrix)
            //{
            //    viewMatrix = viewMatrix * (reflectMatrix);
            //}

            return viewMatrix;
        }
    }
}