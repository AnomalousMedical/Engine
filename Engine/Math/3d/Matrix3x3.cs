using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Explicit, Size = 36)]
    public unsafe struct Matrix3x3
    {
        [FieldOffset(0)]
        public float m00;
        [FieldOffset(4)]
        public float m01;
        [FieldOffset(8)]
        public float m02;
        [FieldOffset(12)]
        public float m10;
        [FieldOffset(16)]
        public float m11;
        [FieldOffset(20)]
        public float m12;
        [FieldOffset(24)]
        public float m20;
        [FieldOffset(28)]
        public float m21;
        [FieldOffset(32)]
        public float m22;

        public Matrix3x3(
            float m00, float m01, float m02,
            float m10, float m11, float m12,
            float m20, float m21, float m22
        )
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
        }

        public Matrix3x3(float* m)
        {
            this.m00 = m[0];
            this.m01 = m[1];
            this.m02 = m[2];
            this.m10 = m[3];
            this.m11 = m[4];
            this.m12 = m[5];
            this.m20 = m[6];
            this.m21 = m[7];
            this.m22 = m[8];
        }

        public float this[int row, int column]
        {
            get
            {
                fixed (float* first = &m00)
                {
                    return first[row * 3 + column];
                }
            }
        }

        public Matrix3x3 transpose()
        {
            return new Matrix3x3
                (
                    m00, m10, m20,
                    m01, m11, m21,
                    m02, m12, m22
                );

            //for (size_t iRow = 0; iRow < 3; iRow++)
            //{
            //    for (size_t iCol = 0; iCol < 3; iCol++)
            //        transpose[iRow][iCol] = m[iCol][iRow];
            //}
        }

        public Vector3 getColumn(int index)
        {
            fixed (float* first = &m00)
            {
                return new Vector3(first[index], first[index + 3], first[index + 6]);
            }
        }

        public Vector3 getRow(int index)
        {
            fixed (float* first = &m00)
            {
                index *= 3;
                return new Vector3(first[index], first[index + 1], first[index + 2]);
            }
        }

        public static Vector3 operator *(Matrix3x3 mat, Vector3 v)
        {
            return new Vector3
                (
                    mat.m00 * v.x + mat.m01 * v.y + mat.m02 * v.z,
                    mat.m10 * v.x + mat.m11 * v.y + mat.m12 * v.z,
                    mat.m20 * v.x + mat.m21 * v.y + mat.m22 * v.z
                );

            //for (size_t iRow = 0; iRow < 3; iRow++)
            //{
            //    kProd[iRow] =
            //        m[iRow][0]*rkPoint[0] +
            //        m[iRow][1]*rkPoint[1] +
            //        m[iRow][2]*rkPoint[2];
            //}
            //return kProd;
        }

        public static Matrix3x3 operator -(Matrix3x3 v)
        {
            return new Matrix3x3(-v.m00, -v.m01, -v.m02,
                                 -v.m10, -v.m11, -v.m12,
                                 -v.m20, -v.m21, -v.m22);
            //Matrix3 kNeg;
            //for (size_t iRow = 0; iRow < 3; iRow++)
            //{
            //    for (size_t iCol = 0; iCol < 3; iCol++)
            //        kNeg[iRow][iCol] = -m[iRow][iCol];
            //}
            //return kNeg;
        }

        /// <summary>
        /// D3D-style left-handed matrix that rotates a point around the y axis. Angle (in radians)
        /// is measured clockwise when looking along the rotation axis toward the origin:
        /// (x' y' z' 1) = (x y z 1) * RotationY
        /// </summary>
        /// <param name="angleInRadians">The angle on the z axis in radians.</param>
        public static Matrix3x3 RotationY(float angleInRadians)
        {
            float s = (float)Math.Sin(angleInRadians);
            float c = (float)Math.Cos(angleInRadians);

            return new Matrix3x3
            (
                c, 0, -s,
                0, 1, 0,
                s, 0, c
            );
        }

        public static Matrix3x3 Scale(float x, float y, float z)
        {
            return new Matrix3x3
            (
                x, 0, 0,
                0, y, 0,
                0, 0, z
            );
        }
    }
}
