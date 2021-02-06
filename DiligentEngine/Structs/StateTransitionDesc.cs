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
    public partial class StateTransitionDesc
    {

        public StateTransitionDesc()
        {
            
        }
        public IDeviceObject pResource { get; set; }
        public Uint32 FirstMipLevel { get; set; } = 0;
        public Uint32 MipLevelsCount { get; set; } = REMAINING_MIP_LEVELS;
        public Uint32 FirstArraySlice { get; set; } = 0;
        public Uint32 ArraySliceCount { get; set; } = REMAINING_ARRAY_SLICES;
        public RESOURCE_STATE OldState { get; set; } = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN;
        public RESOURCE_STATE NewState { get; set; } = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN;
        public STATE_TRANSITION_TYPE TransitionType { get; set; } = STATE_TRANSITION_TYPE.STATE_TRANSITION_TYPE_IMMEDIATE;
        public bool UpdateResourceState { get; set; } = false;


    }
}
