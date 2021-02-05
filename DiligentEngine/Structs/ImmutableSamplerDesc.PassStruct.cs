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

namespace DiligentEngine
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ImmutableSamplerDescPassStruct
    {
        public SHADER_TYPE ShaderStages;
        public String SamplerOrTextureName;
        public FILTER_TYPE MinFilter;
        public FILTER_TYPE MagFilter;
        public FILTER_TYPE MipFilter;
        public TEXTURE_ADDRESS_MODE AddressU;
        public TEXTURE_ADDRESS_MODE AddressV;
        public TEXTURE_ADDRESS_MODE AddressW;
        public Float32 MipLODBias;
        public Uint32 MaxAnisotropy;
        public COMPARISON_FUNCTION ComparisonFunc;
        public Float32 BorderColor_0;
        public Float32 BorderColor_1;
        public Float32 BorderColor_2;
        public Float32 BorderColor_3;
        public float MinLOD;
        public float MaxLOD;
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
                MinFilter = i.Desc.MinFilter,
                MagFilter = i.Desc.MagFilter,
                MipFilter = i.Desc.MipFilter,
                AddressU = i.Desc.AddressU,
                AddressV = i.Desc.AddressV,
                AddressW = i.Desc.AddressW,
                MipLODBias = i.Desc.MipLODBias,
                MaxAnisotropy = i.Desc.MaxAnisotropy,
                ComparisonFunc = i.Desc.ComparisonFunc,
                BorderColor_0 = i.Desc.BorderColor_0,
                BorderColor_1 = i.Desc.BorderColor_1,
                BorderColor_2 = i.Desc.BorderColor_2,
                BorderColor_3 = i.Desc.BorderColor_3,
                MinLOD = i.Desc.MinLOD,
                MaxLOD = i.Desc.MaxLOD,
            }).ToArray();
        }
    }
}
