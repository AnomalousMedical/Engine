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

namespace DiligentEngine.RT
{
    [StructLayout(LayoutKind.Explicit, Size = UVSize + TangentSize + BinormalSize + NormalSize + PrimitivesSize)]
    unsafe struct CubeAttribs
    {
        public const int NumQuads = 6;
        public const int NumVertices = NumQuads * 4;
        public const int NumIndices = NumQuads * 6;
        
        public const int UVSize = float4.Size * NumVertices;
        public const int TangentSize = float4.Size * NumVertices;
        public const int BinormalSize = float4.Size * NumVertices;
        public const int NormalSize = float4.Size * NumVertices;
        public const int PrimitivesSize = sizeof(uint) * 4 * NumIndices;

        [FieldOffset(0)]
        fixed float _UVs[UVSize]; //float4[24]

        [FieldOffset(UVSize)]
        fixed float _Tangents[TangentSize];//float4[24]

        [FieldOffset(UVSize + TangentSize)]
        fixed float _Binormals[BinormalSize];//float4[24]

        [FieldOffset(UVSize + TangentSize + BinormalSize)]
        fixed float _Normals[NormalSize];//float4[24]

        [FieldOffset(UVSize + TangentSize + BinormalSize + NormalSize)]
        fixed uint _Primitives[PrimitivesSize];//uint4[12]

        public void SetUvs(Span<float4> uvs)
        {
            fixed(float* fdst = &_UVs[0])
            {
                var dest = new Span<float4>(fdst, UVSize / sizeof(float4));
                uvs.CopyTo(dest);
            }
        }

        public void SetTangents(Span<float4> uvs)
        {
            fixed (float* fdst = &_Tangents[0])
            {
                var dest = new Span<float4>(fdst, TangentSize / sizeof(float4));
                uvs.CopyTo(dest);
            }
        }

        public void SetBinormals(Span<float4> uvs)
        {
            fixed (float* fdst = &_Binormals[0])
            {
                var dest = new Span<float4>(fdst, BinormalSize / sizeof(float4));
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
