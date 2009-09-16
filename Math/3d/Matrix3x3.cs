using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    class Matrix3x3
    {
        private const float SIMD_2_PI = 6.283185307179586232f;

        protected float cofac(int r1, int c1, int r2, int c2)
        {
            return m_el[r1][c1] * m_el[r2][c2] - m_el[r1][c2] * m_el[r2][c1];
        }

        protected Vector3[] m_el = new Vector3[3];

        public Matrix3x3()
        {
            setIdentity();
        }

        public Matrix3x3(Quaternion q)
        {
            setRotation(q);
        }

        public Matrix3x3(float xx, float xy, float xz,
                  float yx, float yy, float yz,
                  float zx, float zy, float zz)
        {
            setValue(xx, xy, xz,
                     yx, yy, yz,
                     zx, zy, zz);
        }

        public Vector3 getColumn(int i)
        {
            return new Vector3(m_el[0][i], m_el[1][i], m_el[2][i]);
        }

        public Vector3 getRow(int i)
        {
            return m_el[i];
        }

        public void setValue(float xx, float xy, float xz,
                      float yx, float yy, float yz,
                      float zx, float zy, float zz)
        {
            m_el[0].x = xx;
            m_el[0].y = xy;
            m_el[0].z = xz;
            m_el[1].x = yx;
            m_el[1].y = yy;
            m_el[1].z = yz;
            m_el[2].x = zx;
            m_el[2].y = zy;
            m_el[2].z = zz;
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

        public void getRotation(Quaternion q)
        {
            float trace = m_el[0].x + m_el[1].y + m_el[2].z;

            if (trace > 0.0f)
            {
                float s = (float)Math.Sqrt(trace + 1.0f);
                q.w = s * 0.5f;
                s = 0.5f / s;

                q.x = (m_el[2].y - m_el[1].z) * s;
                q.y = (m_el[0].z - m_el[2].x) * s;
                q.z = (m_el[1].x - m_el[0].y) * s;
            }
            else
            {
                int i = m_el[0].x < m_el[1].y ?
                    (m_el[1].y < m_el[2].z ? 2 : 1) :
                    (m_el[0].x < m_el[2].z ? 2 : 0);
                int j = (i + 1) % 3;
                int k = (i + 2) % 3;

                float s = (float)Math.Sqrt(m_el[i][i] - m_el[j][j] - m_el[k][k] + 1.0f);
                q[i] = s * 0.5f;
                s = 0.5f / s;

                q.w = (m_el[k][j] - m_el[j][k]) * s;
                q[j] = (m_el[j][i] + m_el[i][j]) * s;
                q[k] = (m_el[k][i] + m_el[i][k]) * s;
            }
        }

        public void getEuler(float yaw, float pitch, float roll)
        {
            pitch = (float)Math.Asin(-m_el[2].x);
            if (pitch < SIMD_2_PI)
            {
                if (pitch > SIMD_2_PI)
                {
                    yaw = (float)Math.Atan2(m_el[1].x, m_el[0].x);
                    roll = (float)Math.Atan2(m_el[2].y, m_el[2].z);
                }
                else
                {
                    yaw = -(float)Math.Atan2(-m_el[0].y, m_el[0].z);
                    roll = 0.0f;
                }
            }
            else
            {
                yaw = (float)Math.Atan2(-m_el[0].y, m_el[0].z);
                roll = 0.0f;
            }
        }

        public Vector3 getScaling()
        {
            return new Vector3(m_el[0].x * m_el[0].x + m_el[1].x * m_el[1].x + m_el[2].x * m_el[2].x,
                                   m_el[0].y * m_el[0].y + m_el[1].y * m_el[1].y + m_el[2].y * m_el[2].y,
                                   m_el[0].z * m_el[0].z + m_el[1].z * m_el[1].z + m_el[2].z * m_el[2].z);
        }


        public Matrix3x3 scaled(Vector3 s)
        {
            return new Matrix3x3(m_el[0].x * s.x, m_el[0].y * s.y, m_el[0].z * s.z,
                                     m_el[1].x * s.x, m_el[1].y * s.y, m_el[1].z * s.z,
                                     m_el[2].x * s.x, m_el[2].y * s.y, m_el[2].z * s.z);
        }

        public float tdot(int c, Vector3 v)
        {
            return m_el[0][c] * v.x + m_el[1][c] * v.y + m_el[2][c] * v.z;
        }

        public float determinant()
        {
            return m_el[0].triple(ref m_el[1], ref m_el[2]);
        }


        public Matrix3x3 absolute()
        {
            return new Matrix3x3(
                (float)Math.Abs(m_el[0].x), (float)Math.Abs(m_el[0].y), (float)Math.Abs(m_el[0].z),
                (float)Math.Abs(m_el[1].x), (float)Math.Abs(m_el[1].y), (float)Math.Abs(m_el[1].z),
                (float)Math.Abs(m_el[2].x), (float)Math.Abs(m_el[2].y), (float)Math.Abs(m_el[2].z));
        }

        public Matrix3x3 transpose()
        {
            return new Matrix3x3(m_el[0].x, m_el[1].x, m_el[2].x,
                                     m_el[0].y, m_el[1].y, m_el[2].y,
                                     m_el[0].z, m_el[1].z, m_el[2].z);
        }

        public Matrix3x3 adjoint()
        {
            return new Matrix3x3(cofac(1, 1, 2, 2), cofac(0, 2, 2, 1), cofac(0, 1, 1, 2),
                                     cofac(1, 2, 2, 0), cofac(0, 0, 2, 2), cofac(0, 2, 1, 0),
                                     cofac(1, 0, 2, 1), cofac(0, 1, 2, 0), cofac(0, 0, 1, 1));
        }

        public Matrix3x3 inverse()
        {
            Vector3 co = new Vector3(cofac(1, 1, 2, 2), cofac(1, 2, 2, 0), cofac(1, 0, 2, 1));
            float det = m_el[0].dot(ref co);
            float s = 1.0f / det;
            return new Matrix3x3(co.x * s, cofac(0, 2, 2, 1) * s, cofac(0, 1, 1, 2) * s,
                                     co.y * s, cofac(0, 0, 2, 2) * s, cofac(0, 2, 1, 0) * s,
                                     co.z * s, cofac(0, 1, 2, 0) * s, cofac(0, 0, 1, 1) * s);
        }

        public Matrix3x3 transposeTimes(Matrix3x3 m)
        {
            return new Matrix3x3(
                m_el[0].x * m.m_el[0].x + m_el[1].x * m.m_el[1].x + m_el[2].x * m.m_el[2].x,
                m_el[0].x * m.m_el[0].y + m_el[1].x * m.m_el[1].y + m_el[2].x * m.m_el[2].y,
                m_el[0].x * m.m_el[0].z + m_el[1].x * m.m_el[1].z + m_el[2].x * m.m_el[2].z,
                m_el[0].y * m.m_el[0].x + m_el[1].y * m.m_el[1].x + m_el[2].y * m.m_el[2].x,
                m_el[0].y * m.m_el[0].y + m_el[1].y * m.m_el[1].y + m_el[2].y * m.m_el[2].y,
                m_el[0].y * m.m_el[0].z + m_el[1].y * m.m_el[1].z + m_el[2].y * m.m_el[2].z,
                m_el[0].z * m.m_el[0].x + m_el[1].z * m.m_el[1].x + m_el[2].z * m.m_el[2].x,
                m_el[0].z * m.m_el[0].y + m_el[1].z * m.m_el[1].y + m_el[2].z * m.m_el[2].y,
                m_el[0].z * m.m_el[0].z + m_el[1].z * m.m_el[1].z + m_el[2].z * m.m_el[2].z);
        }

        public Matrix3x3 timesTranspose(Matrix3x3 m)
        {
            return new Matrix3x3(
                m_el[0].dot(ref m.m_el[0]), m_el[0].dot(ref m.m_el[1]), m_el[0].dot(ref m.m_el[2]),
                m_el[1].dot(ref m.m_el[0]), m_el[1].dot(ref m.m_el[1]), m_el[1].dot(ref m.m_el[2]),
                m_el[2].dot(ref m.m_el[0]), m_el[2].dot(ref m.m_el[1]), m_el[2].dot(ref m.m_el[2]));

        }

        public static Vector3 operator *(Matrix3x3 m, Vector3 v)
        {
            return new Vector3(m.m_el[0].dot(ref v), m.m_el[1].dot(ref v), m.m_el[2].dot(ref v));
        }

        public static Vector3 operator *(Vector3 v, Matrix3x3 m)
        {
            return new Vector3(m.tdot(0, v), m.tdot(1, v), m.tdot(2, v));
        }

        public static Matrix3x3 operator *(Matrix3x3 m1, Matrix3x3 m2)
        {
            return new Matrix3x3(
                m2.tdot(0, m1.m_el[0]), m2.tdot(1, m1.m_el[0]), m2.tdot(2, m1.m_el[0]),
                m2.tdot(0, m1.m_el[1]), m2.tdot(1, m1.m_el[1]), m2.tdot(2, m1.m_el[1]),
                m2.tdot(0, m1.m_el[2]), m2.tdot(1, m1.m_el[2]), m2.tdot(2, m1.m_el[2]));
        }

        public Matrix3x3 btMultTransposeLeft(Matrix3x3 m1, Matrix3x3 m2)
        {
            return new Matrix3x3(
                m1.m_el[0].x * m2.m_el[0].x + m1.m_el[1].x * m2.m_el[1].x + m1.m_el[2].x * m2.m_el[2].x,
                m1.m_el[0].x * m2.m_el[0].y + m1.m_el[1].x * m2.m_el[1].y + m1.m_el[2].x * m2.m_el[2].y,
                m1.m_el[0].x * m2.m_el[0].z + m1.m_el[1].x * m2.m_el[1].z + m1.m_el[2].x * m2.m_el[2].z,
                m1.m_el[0].y * m2.m_el[0].x + m1.m_el[1].y * m2.m_el[1].x + m1.m_el[2].y * m2.m_el[2].x,
                m1.m_el[0].y * m2.m_el[0].y + m1.m_el[1].y * m2.m_el[1].y + m1.m_el[2].y * m2.m_el[2].y,
                m1.m_el[0].y * m2.m_el[0].z + m1.m_el[1].y * m2.m_el[1].z + m1.m_el[2].y * m2.m_el[2].z,
                m1.m_el[0].z * m2.m_el[0].x + m1.m_el[1].z * m2.m_el[1].x + m1.m_el[2].z * m2.m_el[2].x,
                m1.m_el[0].z * m2.m_el[0].y + m1.m_el[1].z * m2.m_el[1].y + m1.m_el[2].z * m2.m_el[2].y,
                m1.m_el[0].z * m2.m_el[0].z + m1.m_el[1].z * m2.m_el[1].z + m1.m_el[2].z * m2.m_el[2].z);
        }
    };
}
