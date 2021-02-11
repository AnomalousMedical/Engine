using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using float4 = Engine.Vector4;
using float3 = Engine.Vector3;
using float2 = Engine.Vector2;

namespace DiligentEngine.GltfPbr
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GLTFVertex
    {
        public float3 pos;
        public float3 normal;
        public float2 uv0;
        public float2 uv1;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct GLTFVertexSkinAttribs
    {
        public float4 joint0;
        public float4 weight0;
    };
}
