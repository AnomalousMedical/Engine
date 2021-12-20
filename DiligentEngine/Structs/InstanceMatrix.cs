using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine
{
    [StructLayout(LayoutKind.Explicit, Size = 48)]
    public struct InstanceMatrix
    {
        ///        rotation        translation
        /// ([0,0]  [0,1]  [0,2])   ([0,3])
        /// ([1,0]  [1,1]  [1,2])   ([1,3])
        /// ([2,0]  [2,1]  [2,2])   ([2,3])

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

        /// <summary>
        ///        rotation        translation
        /// ([0,0]  [0,1]  [0,2])   ([0,3])
        /// ([1,0]  [1,1]  [1,2])   ([1,3])
        /// ([2,0]  [2,1]  [2,2])   ([2,3])
        /// </summary>
        public InstanceMatrix(
            float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23
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
        }

        public InstanceMatrix(in Matrix4x4 input)
        {
            this.m00 = input.m00;
            this.m01 = input.m01;
            this.m02 = input.m02;
            this.m10 = input.m10;
            this.m11 = input.m11;
            this.m12 = input.m12;
            this.m20 = input.m20;
            this.m21 = input.m21;
            this.m22 = input.m22;

            this.m03 = input.m30;
            this.m13 = input.m31;
            this.m23 = input.m32;
        }

        public InstanceMatrix(in Vector3 trans, in Quaternion rot)
            : this(trans, rot.toRotationMatrix3x3())
        {
            
        }

        public InstanceMatrix(in Vector3 trans, in Matrix3x3 rot)
        {
            this.m00 = rot.m00;
            this.m01 = rot.m01;
            this.m02 = rot.m02;
            this.m10 = rot.m10;
            this.m11 = rot.m11;
            this.m12 = rot.m12;
            this.m20 = rot.m20;
            this.m21 = rot.m21;
            this.m22 = rot.m22;

            this.m03 = trans.x;
            this.m13 = trans.y;
            this.m23 = trans.z;
        }

        public InstanceMatrix(float x, float y, float z, in Matrix3x3 rot)
        {
            this.m00 = rot.m00;
            this.m01 = rot.m01;
            this.m02 = rot.m02;
            this.m10 = rot.m10;
            this.m11 = rot.m11;
            this.m12 = rot.m12;
            this.m20 = rot.m20;
            this.m21 = rot.m21;
            this.m22 = rot.m22;

            this.m03 = x;
            this.m13 = y;
            this.m23 = z;
        }

        public InstanceMatrix(in Vector3 trans)
            :this(1, 0, 0, trans.x,
                  0, 1, 0, trans.y,
                  0, 0, 1, trans.z)
        {

        }

        public InstanceMatrix(float x, float y, float z)
            : this(1, 0, 0, x,
                  0, 1, 0, y,
                  0, 0, 1, z)
        {

        }

        public void SetRotation(in Matrix3x3 rot)
        {
            this.m00 = rot.m00;
            this.m01 = rot.m01;
            this.m02 = rot.m02;
            this.m10 = rot.m10;
            this.m11 = rot.m11;
            this.m12 = rot.m12;
            this.m20 = rot.m20;
            this.m21 = rot.m21;
            this.m22 = rot.m22;
        }

        public void SetTranslation(in Vector3 trans)
        {
            this.m03 = trans.x;
            this.m13 = trans.y;
            this.m23 = trans.z;
        }

        public void SetTranslation(float x, float y, float z)
        {
            this.m03 = x;
            this.m13 = y;
            this.m23 = z;
        }

        public static readonly InstanceMatrix Identity = new InstanceMatrix(0, 0, 0);
    }
}
