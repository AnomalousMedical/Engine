using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Explicit, Size = 36)]
    public struct Matrix3x3
    {
        private const float SIMD_2_PI = 6.283185307179586232f;

        public unsafe float cofac(int r1, int c1, int r2, int c2)
        {
            fixed (float* m_el_f = &this.xx)
            {
                Vector3* m_el = (Vector3*)m_el_f;
                return m_el[r1][c1] * m_el[r2][c2] - m_el[r1][c2] * m_el[r2][c1];
            }
        }

        //protected Vector3[] m_el = new Vector3[3];

        [FieldOffset(0)]
        public float xx;
        [FieldOffset(4)]
        public float xy;
        [FieldOffset(8)]
        public float xz;
        [FieldOffset(12)]
        public float yx;
        [FieldOffset(16)]
        public float yy; 
        [FieldOffset(20)]
        public float yz;
        [FieldOffset(24)]
        public float zx;
        [FieldOffset(28)]
        public float zy; 
        [FieldOffset(32)]
        public float zz;

        public Matrix3x3(Quaternion q)
        {
            this.xx = 0.0f;
            this.xy = 0.0f;
            this.xz = 0.0f;
            this.yx = 0.0f;
            this.yy = 0.0f;
            this.yz = 0.0f;
            this.zx = 0.0f;
            this.zy = 0.0f;
            this.zz = 0.0f;
            setRotation(q);
        }

        public Matrix3x3(float xx, float xy, float xz,
                  float yx, float yy, float yz,
                  float zx, float zy, float zz)
        {
            this.xx = xx;
            this.xy = xy;
            this.xz = xz;
            this.yx = yx;
            this.yy = yy;
            this.yz = yz;
            this.zx = zx;
            this.zy = zy;
            this.zz = zz;
        }

        public unsafe Vector3 getColumn(int i)
        {
            fixed (float* m_el_f = &this.xx)
            {
                Vector3* m_el = (Vector3*)m_el_f;
                return new Vector3(m_el[0][i], m_el[1][i], m_el[2][i]);
            }
        }

        public unsafe Vector3 getRow(int i)
        {
            fixed (float* m_el_f = &this.xx)
            {
                Vector3* m_el = (Vector3*)m_el_f;
                return m_el[i];
            }
        }

        public void setValue(float xx, float xy, float xz,
                      float yx, float yy, float yz,
                      float zx, float zy, float zz)
        {
            this.xx = xx;
            this.xy = xy;
            this.xz = xz;
            this.yx = yx;
            this.yy = yy;
            this.yz = yz;
            this.zx = zx;
            this.zy = zy;
            this.zz = zz;
        }

        public void setRotation(Quaternion q)
        {
            float d = q.length2();
            float s = 2.0f / d;
            float xs = q.x * s, ys = q.y * s, zs = q.z * s;
            float wx = q.w * xs, wy = q.w * ys, wz = q.w * zs;
            float xx = q.x * xs, xy = q.x * ys, xz = q.x * zs;
            float yy = q.y * ys, yz = q.y * zs, zz = q.z * zs;
            setValue(1.0f - (yy + zz), xy - wz, xz + wy,
                     xy + wz, 1.0f - (xx + zz), yz - wx,
                     xz - wy, yz + wx, 1.0f - (xx + yy));
        }



        public void setEulerYPR(float yaw, float pitch, float roll)
        {
            float cy = ((float)Math.Cos(yaw));
            float sy = ((float)Math.Sin(yaw));
            float cp = ((float)Math.Cos(pitch));
            float sp = ((float)Math.Sin(pitch));
            float cr = ((float)Math.Cos(roll));
            float sr = ((float)Math.Sin(roll));
            float cc = cy * cr;
            float cs = cy * sr;
            float sc = sy * cr;
            float ss = sy * sr;
            setValue(cc - sp * ss, -cs - sp * sc, -sy * cp,
                     cp * sr, cp * cr, -sp,
                     sc + sp * cs, -ss + sp * cc, cy * cp);

        }

        /**
         * setEulerZYX
         * @param euler a  reference to a Vector3 of euler angles
         * These angles are used to produce a rotation matrix. The euler
         * angles are applied in ZYX order. I.e a vector is first rotated 
         * about X then Y and then Z
         **/

        public void setEulerZYX(float eulerX, float eulerY, float eulerZ)
        {
            float ci = ((float)Math.Cos(eulerX));
            float cj = ((float)Math.Cos(eulerY));
            float ch = ((float)Math.Cos(eulerZ));
            float si = ((float)Math.Sin(eulerX));
            float sj = ((float)Math.Sin(eulerY));
            float sh = ((float)Math.Sin(eulerZ));
            float cc = ci * ch;
            float cs = ci * sh;
            float sc = si * ch;
            float ss = si * sh;

            setValue(cj * ch, sj * sc - cs, sj * cc + ss,
                     cj * sh, sj * ss + cc, sj * cs - sc,
                         -sj, cj * si, cj * ci);
        }

        public void setIdentity()
        {
            setValue(1.0f, 0.0f, 0.0f,
                     0.0f, 1.0f, 0.0f,
                     0.0f, 0.0f, 1.0f);
        }

        public unsafe void getRotation(Quaternion q)
        {
            float trace = xx + yy + zz;

            if (trace > 0.0f)
            {
                float s = (float)Math.Sqrt(trace + 1.0f);
                q.w = s * 0.5f;
                s = 0.5f / s;

                q.x = (zy - yz) * s;
                q.y = (xz - zx) * s;
                q.z = (yx - xy) * s;
            }
            else
            {
                int i = xx < yy ?
                    (yy < zz ? 2 : 1) :
                    (xx < zz ? 2 : 0);
                int j = (i + 1) % 3;
                int k = (i + 2) % 3;

                fixed (float* m_el_f = &this.xx)
                {
                    Vector3* m_el = (Vector3*)m_el_f;
                    float s = (float)Math.Sqrt(m_el[i][i] - m_el[j][j] - m_el[k][k] + 1.0f);
                    q[i] = s * 0.5f;
                    s = 0.5f / s;

                    q.w = (m_el[k][j] - m_el[j][k]) * s;
                    q[j] = (m_el[j][i] + m_el[i][j]) * s;
                    q[k] = (m_el[k][i] + m_el[i][k]) * s;
                }
            }
        }

        public void getEuler(float yaw, float pitch, float roll)
        {
            pitch = (float)Math.Asin(-zx);
            if (pitch < SIMD_2_PI)
            {
                if (pitch > SIMD_2_PI)
                {
                    yaw = (float)Math.Atan2(yx, xx);
                    roll = (float)Math.Atan2(zy, zz);
                }
                else
                {
                    yaw = -(float)Math.Atan2(-xy, xz);
                    roll = 0.0f;
                }
            }
            else
            {
                yaw = (float)Math.Atan2(-xy, xz);
                roll = 0.0f;
            }
        }

        public Vector3 getScaling()
        {
            return new Vector3(xx * xx + yx * yx + zx * zx,
                                   xy * xy + yy * yy + zy * zy,
                                   xz * xz + yz * yz + zz * zz);
        }


        public Matrix3x3 scaled(Vector3 s)
        {
            return new Matrix3x3(xx * s.x, xy * s.y, xz * s.z,
                                     yx * s.x, yy * s.y, yz * s.z,
                                     zx * s.x, zy * s.y, zz * s.z);
        }

        public unsafe float tdot(int c, Vector3 v)
        {
            fixed (float* m_el_f = &this.xx)
            {
                Vector3* m_el = (Vector3*)m_el_f;
                return m_el[0][c] * v.x + m_el[1][c] * v.y + m_el[2][c] * v.z;
            }
        }

        public unsafe float determinant()
        {
            fixed (float* m_el_f = &this.xx)
            {
                Vector3* m_el = (Vector3*)m_el_f;
                return m_el[0].triple(ref m_el[1], ref m_el[2]);
            }
        }


        public Matrix3x3 absolute()
        {
            return new Matrix3x3(
                (float)Math.Abs(xx), (float)Math.Abs(xy), (float)Math.Abs(xz),
                (float)Math.Abs(yx), (float)Math.Abs(yy), (float)Math.Abs(yz),
                (float)Math.Abs(zx), (float)Math.Abs(zy), (float)Math.Abs(zz));
        }

        public Matrix3x3 transpose()
        {
            return new Matrix3x3(xx, yx, zx,
                                     xy, yy, zy,
                                     xz, yz, zz);
        }

        public Matrix3x3 adjoint()
        {
            return new Matrix3x3(cofac(1, 1, 2, 2), cofac(0, 2, 2, 1), cofac(0, 1, 1, 2),
                                     cofac(1, 2, 2, 0), cofac(0, 0, 2, 2), cofac(0, 2, 1, 0),
                                     cofac(1, 0, 2, 1), cofac(0, 1, 2, 0), cofac(0, 0, 1, 1));
        }

        public unsafe Matrix3x3 inverse()
        {
            Vector3 co = new Vector3(cofac(1, 1, 2, 2), cofac(1, 2, 2, 0), cofac(1, 0, 2, 1));
            float det;
            fixed (float* m_el_f = &this.xx)
            {
                Vector3* m_el = (Vector3*)m_el_f;
                det = m_el[0].dot(ref co);
            }
            float s = 1.0f / det;
            return new Matrix3x3(co.x * s, cofac(0, 2, 2, 1) * s, cofac(0, 1, 1, 2) * s,
                                     co.y * s, cofac(0, 0, 2, 2) * s, cofac(0, 2, 1, 0) * s,
                                     co.z * s, cofac(0, 1, 2, 0) * s, cofac(0, 0, 1, 1) * s);
        }

        public Matrix3x3 transposeTimes(Matrix3x3 m)
        {
            return new Matrix3x3(
                xx * m.xx + yx * m.yx + zx * m.zx,
                xx * m.xy + yx * m.yy + zx * m.zy,
                xx * m.xz + yx * m.yz + zx * m.zz,
                xy * m.xx + yy * m.yx + zy * m.zx,
                xy * m.xy + yy * m.yy + zy * m.zy,
                xy * m.xz + yy * m.yz + zy * m.zz,
                xz * m.xx + yz * m.yx + zz * m.zx,
                xz * m.xy + yz * m.yy + zz * m.zy,
                xz * m.xz + yz * m.yz + zz * m.zz);
        }

        public unsafe Matrix3x3 timesTranspose(Matrix3x3 m)
        {
            fixed (float* m_el_f = &this.xx)
            {
                Vector3* m_el = (Vector3*)m_el_f;
                Vector3* other_el = (Vector3*)&m.xx;
                return new Matrix3x3(
                    m_el[0].dot(ref other_el[0]), m_el[0].dot(ref other_el[1]), m_el[0].dot(ref other_el[2]),
                    m_el[1].dot(ref other_el[0]), m_el[1].dot(ref other_el[1]), m_el[1].dot(ref other_el[2]),
                    m_el[2].dot(ref other_el[0]), m_el[2].dot(ref other_el[1]), m_el[2].dot(ref other_el[2]));
            }

        }

        public unsafe static Vector3 operator *(Matrix3x3 m, Vector3 v)
        {
            Vector3* m_el = (Vector3*)&m.xx;
            return new Vector3(m_el[0].dot(ref v), m_el[1].dot(ref v), m_el[2].dot(ref v));
        }

        public static Vector3 operator *(Vector3 v, Matrix3x3 m)
        {
            return new Vector3(m.tdot(0, v), m.tdot(1, v), m.tdot(2, v));
        }

        public unsafe static Matrix3x3 operator *(Matrix3x3 m1, Matrix3x3 m2)
        {
            Vector3* m_el_1 = (Vector3*)&m1.xx;
            return new Matrix3x3(
                m2.tdot(0, m_el_1[0]), m2.tdot(1, m_el_1[0]), m2.tdot(2, m_el_1[0]),
                m2.tdot(0, m_el_1[1]), m2.tdot(1, m_el_1[1]), m2.tdot(2, m_el_1[1]),
                m2.tdot(0, m_el_1[2]), m2.tdot(1, m_el_1[2]), m2.tdot(2, m_el_1[2]));
        }

        public Matrix3x3 btMultTransposeLeft(Matrix3x3 m1, Matrix3x3 m2)
        {
            return new Matrix3x3(
                m1.xx * m2.xx + m1.yx * m2.yx + m1.zx * m2.zx,
                m1.xx * m2.xy + m1.yx * m2.yy + m1.zx * m2.zy,
                m1.xx * m2.xz + m1.yx * m2.yz + m1.zx * m2.zz,
                m1.xy * m2.xx + m1.yy * m2.yx + m1.zy * m2.zx,
                m1.xy * m2.xy + m1.yy * m2.yy + m1.zy * m2.zy,
                m1.xy * m2.xz + m1.yy * m2.yz + m1.zy * m2.zz,
                m1.xz * m2.xx + m1.yz * m2.yx + m1.zz * m2.zx,
                m1.xz * m2.xy + m1.yz * m2.yy + m1.zz * m2.zy,
                m1.xz * m2.xz + m1.yz * m2.yz + m1.zz * m2.zz);
        }
    };
}
