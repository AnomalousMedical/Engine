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
using float4x4 = Engine.Matrix4x4;
using BOOL = System.Boolean;

namespace DiligentEngine
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ImmutableSamplerDescPassStruct
    {
        public SHADER_TYPE ShaderStages;
        public String SamplerOrTextureName;
        public FILTER_TYPE Desc_MinFilter;
        public FILTER_TYPE Desc_MagFilter;
        public FILTER_TYPE Desc_MipFilter;
        public TEXTURE_ADDRESS_MODE Desc_AddressU;
        public TEXTURE_ADDRESS_MODE Desc_AddressV;
        public TEXTURE_ADDRESS_MODE Desc_AddressW;
        public Float32 Desc_MipLODBias;
        public Uint32 Desc_MaxAnisotropy;
        public COMPARISON_FUNCTION Desc_ComparisonFunc;
        public Float32 Desc_BorderColor_0;
        public Float32 Desc_BorderColor_1;
        public Float32 Desc_BorderColor_2;
        public Float32 Desc_BorderColor_3;
        public float Desc_MinLOD;
        public float Desc_MaxLOD;
        public static ImmutableSamplerDescPassStruct[] ToStruct(IEnumerable<ImmutableSamplerDesc> vals)
        {
            if(vals == null)
            {
                return null;
            }

            return vals.Select(i => new ImmutableSamplerDescPassStruct
            {
                ShaderStages = i.ShaderStages,
                SamplerOrTextureName = i.SamplerOrTextureName,
                Desc_MinFilter = i.Desc.MinFilter,
                Desc_MagFilter = i.Desc.MagFilter,
                Desc_MipFilter = i.Desc.MipFilter,
                Desc_AddressU = i.Desc.AddressU,
                Desc_AddressV = i.Desc.AddressV,
                Desc_AddressW = i.Desc.AddressW,
                Desc_MipLODBias = i.Desc.MipLODBias,
                Desc_MaxAnisotropy = i.Desc.MaxAnisotropy,
                Desc_ComparisonFunc = i.Desc.ComparisonFunc,
                Desc_BorderColor_0 = i.Desc.BorderColor_0,
                Desc_BorderColor_1 = i.Desc.BorderColor_1,
                Desc_BorderColor_2 = i.Desc.BorderColor_2,
                Desc_BorderColor_3 = i.Desc.BorderColor_3,
                Desc_MinLOD = i.Desc.MinLOD,
                Desc_MaxLOD = i.Desc.MaxLOD,
            }).ToArray();
        }
    }
}
