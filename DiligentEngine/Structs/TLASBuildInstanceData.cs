using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

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
    public partial class TLASBuildInstanceData
    {

        public TLASBuildInstanceData()
        {
            
        }
        public String InstanceName { get; set; }
        public IBottomLevelAS pBLAS { get; set; }
        public InstanceMatrix Transform { get; set; }
        public Uint32 CustomId { get; set; } = 0;
        public RAYTRACING_INSTANCE_FLAGS Flags { get; set; } = RAYTRACING_INSTANCE_FLAGS.RAYTRACING_INSTANCE_NONE;
        public Uint8 Mask { get; set; } = 0xFF;
        public Uint32 ContributionToHitGroupIndex { get; set; } = ITopLevelAS.TLAS_INSTANCE_OFFSET_AUTO;


    }
}
