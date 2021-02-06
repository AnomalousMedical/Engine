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
    public partial class RenderTargetBlendDesc
    {

        public RenderTargetBlendDesc()
        {
            
        }
        public Bool BlendEnable { get; set; } = false;
        public Bool LogicOperationEnable { get; set; } = false;
        public BLEND_FACTOR SrcBlend { get; set; } = BLEND_FACTOR.BLEND_FACTOR_ONE;
        public BLEND_FACTOR DestBlend { get; set; } = BLEND_FACTOR.BLEND_FACTOR_ZERO;
        public BLEND_OPERATION BlendOp { get; set; } = BLEND_OPERATION.BLEND_OPERATION_ADD;
        public BLEND_FACTOR SrcBlendAlpha { get; set; } = BLEND_FACTOR.BLEND_FACTOR_ONE;
        public BLEND_FACTOR DestBlendAlpha { get; set; } = BLEND_FACTOR.BLEND_FACTOR_ZERO;
        public BLEND_OPERATION BlendOpAlpha { get; set; } = BLEND_OPERATION.BLEND_OPERATION_ADD;
        public LOGIC_OPERATION LogicOp { get; set; } = LOGIC_OPERATION.LOGIC_OP_NOOP;
        public Uint8 RenderTargetWriteMask { get; set; } = (Uint8)COLOR_MASK.COLOR_MASK_ALL;


    }
}
