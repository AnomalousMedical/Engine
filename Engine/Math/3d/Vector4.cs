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
        [FieldOffset(0)]
        public float x;

        [FieldOffset(4)]
        public float y;

        [FieldOffset(8)]
        public float z;

        [FieldOffset(12)]
        public float w;

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
    }
}
