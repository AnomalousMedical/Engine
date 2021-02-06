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
    public partial class SamplerDesc : DeviceObjectAttribs
    {
        public SamplerDesc()
        {

        }
        public FILTER_TYPE MinFilter { get; set; } = FILTER_TYPE.FILTER_TYPE_LINEAR;
        public FILTER_TYPE MagFilter { get; set; } = FILTER_TYPE.FILTER_TYPE_LINEAR;
        public FILTER_TYPE MipFilter { get; set; } = FILTER_TYPE.FILTER_TYPE_LINEAR;
        public TEXTURE_ADDRESS_MODE AddressU { get; set; } = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_CLAMP;
        public TEXTURE_ADDRESS_MODE AddressV { get; set; } = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_CLAMP;
        public TEXTURE_ADDRESS_MODE AddressW { get; set; } = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_CLAMP;
        public Float32 MipLODBias { get; set; } = 0;
        public Uint32 MaxAnisotropy { get; set; } = 0;
        public COMPARISON_FUNCTION ComparisonFunc { get; set; } = COMPARISON_FUNCTION.COMPARISON_FUNC_NEVER;
        public Float32 BorderColor_0 { get; set; }
        public Float32 BorderColor_1 { get; set; }
        public Float32 BorderColor_2 { get; set; }
        public Float32 BorderColor_3 { get; set; }
        public float MinLOD { get; set; } = 0;
        public float MaxLOD { get; set; } = +3.402823466e+38F;


    }
}
