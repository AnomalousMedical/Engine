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
        public static readonly Matrix4x4 Identity = new Matrix4x4(1, 0, 0, 0,
                                                                  0, 1, 0, 0,
                                                                  0, 0, 1, 0,
                                                                  0, 0, 0, 1);

        public static Matrix4x4 Translation(float x, float y, float z)
        {
            return new Matrix4x4(1, 0, 0, 0,
                                 0, 1, 0, 0,
                                 0, 0, 1, 0,
                                 x, y, z, 1);
        }

        public static Matrix4x4 Translation(Vector3 t)
        {
            return new Matrix4x4(1, 0, 0, 0,
                                 0, 1, 0, 0,
                                 0, 0, 1, 0,
                                 t.x, t.y, t.z, 1);
        }

        public static Matrix4x4 Scale(float x, float y, float z)
        {
            return new Matrix4x4(
                x, 0, 0, 0,
                0, y, 0, 0,
                0, 0, z, 0,
                0, 0, 0, 1
            );
        }

        public static Matrix4x4 Scale(Vector3 v)
        {
            return new Matrix4x4(
                v.x, 0,   0,   0,
                0,   v.y, 0,   0,
                0,   0,   v.z, 0,
                0,   0,   0,   1
            );
        }

        public static Matrix4x4 ViewFromBasis(ref Vector3 f3X, ref Vector3 f3Y, ref Vector3 f3Z)
        {
            return new Matrix4x4
            (
                f3X.x, f3Y.x, f3Z.x,   0,
                f3X.y, f3Y.y, f3Z.y,   0,
                f3X.z, f3Y.z, f3Z.z,   0,
                    0,     0,     0,   1
            );
        }

        public Matrix4x4 RemoveTranslation()
        {
            return new Matrix4x4
            (
                m00, m01, m02, m03,
                m10, m11, m12, m13,
                m20, m21, m22, m23,
                  0,   0,   0, m33
            );
        }

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
        public Matrix4x4 Transpose()
        {
            return new Matrix4x4{
                m00 = this.m00, m01 = this.m10, m02 = this.m20, m03 = this.m30,
                m10 = this.m01, m11 = this.m11, m12 = this.m21, m13 = this.m31,
                m20 = this.m02, m21 = this.m12, m22 = this.m22, m23 = this.m32,
                m30 = this.m03, m31 = this.m13, m32 = this.m23, m33 = this.m33
            };
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


        public void SetNearFarClipPlanes(float zNear, float zFar, bool bIsGL)
        {
            if (bIsGL)
            {
                // https://www.opengl.org/sdk/docs/man2/xhtml/gluPerspective.xml
                // http://www.terathon.com/gdc07_lengyel.pdf
                // Note that OpenGL uses right-handed coordinate system, where
                // camera is looking in negative z direction:
                //   OO
                //  |__|<--------------------
                //         -z             +z
                // Consequently, OpenGL projection matrix given by these two
                // references inverts z axis.

                // We do not need to do this, because we use DX coordinate
                // system for the camera space. Thus we need to invert the
                // sign of the values in the third column in the matrix
                // from the references:

                m22 = -(-(zFar + zNear) / (zFar - zNear));
                m32 = -2 * zNear * zFar / (zFar - zNear);
                m23 = -(-1);
            }
            else
            {
                m22 = zFar / (zFar - zNear);
                m32 = -zNear * zFar / (zFar - zNear);
                m23 = 1;
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
            Matrix3x3 rot = orientation.toRotationMatrix3x3();

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

        public Vector3 GetTranslation()
        {
            return new Vector3(m30, m31, m32);
        }

        public void SetTranslation(Vector3 vector3)
        {
            m30 = vector3.x;
            m31 = vector3.y;
            m32 = vector3.z;
        }

        public void SetTranslation(float x, float y, float z)
        {
            m30 = x;
            m31 = y;
            m32 = z;
        }

        /// <summary>
        /// D3D-style left-handed matrix that rotates a point around the x axis. Angle (in radians)
        /// is measured clockwise when looking along the rotation axis toward the origin:
        /// (x' y' z' 1) = (x y z 1) * RotationX
        /// </summary>
        /// <param name="angleInRadians">The angle on the z axis in radians.</param>
        public static Matrix4x4 RotationX(float angleInRadians)
        {
            float s = (float)Math.Sin(angleInRadians);
            float c = (float)Math.Cos(angleInRadians);

            return new Matrix4x4 // clang-format off
            (
                1,  0,  0,  0,
                0,  c,  s,  0,
                0, -s,  c,  0,
                0,  0,  0,  1 // clang-format on
            );
        }

        /// <summary>
        /// D3D-style left-handed matrix that rotates a point around the y axis. Angle (in radians)
        /// is measured clockwise when looking along the rotation axis toward the origin:
        /// (x' y' z' 1) = (x y z 1) * RotationY
        /// </summary>
        /// <param name="angleInRadians">The angle on the z axis in radians.</param>
        public static Matrix4x4 RotationY(float angleInRadians)
        {
            float s = (float)Math.Sin(angleInRadians);
            float c = (float)Math.Cos(angleInRadians);

            return new Matrix4x4
            (
                c,  0, -s,  0,
                0,  1,  0,  0,
                s,  0,  c,  0,
                0,  0,  0,  1
            );
        }

        /// <summary>
        /// D3D-style left-handed matrix that rotates a point around the z axis. Angle (in radians)
        /// is measured clockwise when looking along the rotation axis toward the origin:
        /// (x' y' z' 1) = (x y z 1) * RotationZ 
        /// </summary>
        /// <param name="angleInRadians">The angle on the z axis in radians.</param>
        public static Matrix4x4 RotationZ(float angleInRadians)
        {
            float s = (float)Math.Sin(angleInRadians);
            float c = (float)Math.Cos(angleInRadians);

            return new Matrix4x4 // clang-format off
            (
                c, s, 0, 0,
                -s, c, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1 // clang-format on
            );
        }

        // 3D Rotation matrix for an arbitrary axis specified by x, y and z
        public static Matrix4x4 RotationArbitrary(Vector3 axis, float angleInRadians)
        {
            axis = axis.normalized();

            float sinAngle = (float)Math.Sin(angleInRadians);
            float cosAngle = (float)Math.Cos(angleInRadians);
            float oneMinusCosAngle = 1 - cosAngle;

            Matrix4x4 mOut;

            mOut.m00 = 1 + oneMinusCosAngle * (axis.x * axis.x - 1);
            mOut.m01 = axis.z * sinAngle + oneMinusCosAngle * axis.x * axis.y;
            mOut.m02 = -axis.y * sinAngle + oneMinusCosAngle * axis.x * axis.z;
            mOut.m03 = 0;

            mOut.m10 = -axis.z * sinAngle + oneMinusCosAngle * axis.y * axis.x;
            mOut.m11 = 1 + oneMinusCosAngle * (axis.y * axis.y - 1);
            mOut.m12 = axis.x * sinAngle + oneMinusCosAngle * axis.y * axis.z;
            mOut.m13 = 0;

            mOut.m20 = axis.y * sinAngle + oneMinusCosAngle * axis.z * axis.x;
            mOut.m21 = -axis.x * sinAngle + oneMinusCosAngle * axis.z * axis.y;
            mOut.m22 = 1 + oneMinusCosAngle * (axis.z * axis.z - 1);
            mOut.m23 = 0;

            mOut.m30 = 0;
            mOut.m31 = 0;
            mOut.m32 = 0;
            mOut.m33 = 1;

            return mOut;
        }
    }
}