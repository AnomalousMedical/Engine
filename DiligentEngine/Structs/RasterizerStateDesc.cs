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
using float4x4 = Engine.Matrix4x4;
using BOOL = System.Boolean;

namespace DiligentEngine
{
    public partial class RasterizerStateDesc
    {

        public RasterizerStateDesc()
        {
            
        }
        public FILL_MODE FillMode { get; set; } = FILL_MODE.FILL_MODE_SOLID;
        public CULL_MODE CullMode { get; set; } = CULL_MODE.CULL_MODE_BACK;
        public Bool FrontCounterClockwise { get; set; } = false;
        public Bool DepthClipEnable { get; set; } = true;
        public Bool ScissorEnable { get; set; } = false;
        public Bool AntialiasedLineEnable { get; set; } = false;
        public Int32 DepthBias { get; set; } = 0;
        public Float32 DepthBiasClamp { get; set; } = 0f;
        public Float32 SlopeScaledDepthBias { get; set; } = 0f;


    }
}
