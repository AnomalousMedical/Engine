using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Platform
{
    [StructLayout(LayoutKind.Explicit, Size = 20)]
    public struct TouchInfo
    {
        [FieldOffset(0)]
        public float normalizedX;
        [FieldOffset(4)]
        public float normalizedY;
        [FieldOffset(8)]
        public int pixelX;
        [FieldOffset(12)]
        public int pixelY;
        [FieldOffset(16)]
        public int id;
    };
}
