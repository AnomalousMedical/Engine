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
    public partial class BuildTLASAttribs
    {

        public BuildTLASAttribs()
        {
            
        }
        public ITopLevelAS pTLAS { get; set; }
        public RESOURCE_STATE_TRANSITION_MODE TLASTransitionMode { get; set; } = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_NONE;
        public RESOURCE_STATE_TRANSITION_MODE BLASTransitionMode { get; set; } = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_NONE;
        public List<TLASBuildInstanceData> pInstances { get; set; }
        public IBuffer pInstanceBuffer { get; set; }
        public Uint32 InstanceBufferOffset { get; set; } = 0;
        public RESOURCE_STATE_TRANSITION_MODE InstanceBufferTransitionMode { get; set; } = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_NONE;
        public Uint32 HitGroupStride { get; set; } = 1;
        public Uint32 BaseContributionToHitGroupIndex { get; set; } = 0;
        public HIT_GROUP_BINDING_MODE BindingMode { get; set; } = HIT_GROUP_BINDING_MODE.HIT_GROUP_BINDING_MODE_PER_GEOMETRY;
        public IBuffer pScratchBuffer { get; set; }
        public Uint32 ScratchBufferOffset { get; set; } = 0;
        public RESOURCE_STATE_TRANSITION_MODE ScratchBufferTransitionMode { get; set; } = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_NONE;
        public Bool Update { get; set; } = false;


    }
}
