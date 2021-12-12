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
    struct TLASBuildInstanceDataPassStruct
    {
        public String InstanceName;
        public IntPtr pBLAS;
        public InstanceMatrix Transform;
        public Uint32 CustomId;
        public RAYTRACING_INSTANCE_FLAGS Flags;
        public Uint8 Mask;
        public Uint32 ContributionToHitGroupIndex;
        public static TLASBuildInstanceDataPassStruct[] ToStruct(IEnumerable<TLASBuildInstanceData> vals)
        {
            if(vals == null)
            {
                return null;
            }

            return vals.Select(i => new TLASBuildInstanceDataPassStruct
            {
                InstanceName = i.InstanceName,
                pBLAS = i.pBLAS == null ? IntPtr.Zero : i.pBLAS.objPtr,
                Transform = i.Transform,
                CustomId = i.CustomId,
                Flags = i.Flags,
                Mask = i.Mask,
                ContributionToHitGroupIndex = i.ContributionToHitGroupIndex,
            }).ToArray();
        }
    }
}
