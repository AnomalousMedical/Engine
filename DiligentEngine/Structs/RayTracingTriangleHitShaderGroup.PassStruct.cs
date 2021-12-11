using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

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

namespace DiligentEngine
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct RayTracingTriangleHitShaderGroupPassStruct
    {
        public String Name;
        public IntPtr pClosestHitShader;
        public IntPtr pAnyHitShader;
        public static RayTracingTriangleHitShaderGroupPassStruct[] ToStruct(IEnumerable<RayTracingTriangleHitShaderGroup> vals)
        {
            if(vals == null)
            {
                return null;
            }

            return vals.Select(i => new RayTracingTriangleHitShaderGroupPassStruct
            {
                Name = i.Name,
                pClosestHitShader = i.pClosestHitShader == null ? IntPtr.Zero : i.pClosestHitShader.objPtr,
                pAnyHitShader = i.pAnyHitShader == null ? IntPtr.Zero : i.pAnyHitShader.objPtr,
            }).ToArray();
        }
    }
}
