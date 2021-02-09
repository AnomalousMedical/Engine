using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace DiligentEngine.GltfPbr
{
    [StructLayout(LayoutKind.Sequential)]
    struct GLTFNodeShaderTransforms
    {
        public float4x4 NodeMatrix;

        public int JointCount;
        public float Dummy0;
        public float Dummy1;
        public float Dummy2;
    }
}
