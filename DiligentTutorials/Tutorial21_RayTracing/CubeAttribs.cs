using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;
using Float32 = System.Single;
using Uint16 = System.UInt16;
using PVoid = System.IntPtr;
using float4 = Engine.Vector4;
using float3 = Engine.Vector3;
using float2 = Engine.Vector2;
using float4x4 = Engine.Matrix4x4;
using BOOL = System.Boolean;
using System.Runtime.InteropServices;
using Engine;

namespace DiligentEngineRayTracing
{
    [StructLayout(LayoutKind.Explicit, Size = UVSize + NormalSize + PrimitivesSize)]
    unsafe struct CubeAttribs
    {
        public const int UVSize = float4.Size * 24;
        public const int NormalSize = float4.Size * 24;
        public const int PrimitivesSize = sizeof(uint) * 4 * 12;

        [FieldOffset(0)]
        fixed float _UVs[UVSize]; //float4[24]

        [FieldOffset(UVSize)]
        fixed float _Normals[NormalSize];//float4[24]

        [FieldOffset(UVSize + NormalSize)]
        fixed uint _Primitives[PrimitivesSize];//uint4[12]

        public void SetUvs(Span<float4> uvs)
        {
            fixed(float* fdst = &_UVs[0])
            {
                var dest = new Span<float4>(fdst, UVSize / sizeof(float4));
                uvs.CopyTo(dest);
            }
        }

        public void SetNormals(Span<float4> uvs)
        {
            fixed (float* fdst = &_Normals[0])
            {
                var dest = new Span<float4>(fdst, NormalSize / sizeof(float4));
                uvs.CopyTo(dest);
            }
        }

        public void SetPrimitive(int i, uint x, uint y, uint z, uint w)
        {
            var insert = i * 4;
            _Primitives[insert] = x;
            _Primitives[insert + 1] = y;
            _Primitives[insert + 2] = z;
            _Primitives[insert + 3] = w;
        }
    };
}
