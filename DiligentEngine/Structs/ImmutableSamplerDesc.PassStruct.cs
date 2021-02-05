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
        public SamplerDesc Desc;
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
                Desc = i.Desc,
            }).ToArray();
        }
    }
}
