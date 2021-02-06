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

namespace DiligentEngine
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ShaderResourceVariableDescPassStruct
    {
        public SHADER_TYPE ShaderStages;
        public String Name;
        public SHADER_RESOURCE_VARIABLE_TYPE Type;
        public static ShaderResourceVariableDescPassStruct[] ToStruct(IEnumerable<ShaderResourceVariableDesc> vals)
        {
            if(vals == null)
            {
                return null;
            }

            return vals.Select(i => new ShaderResourceVariableDescPassStruct
            {
                ShaderStages = i.ShaderStages,
                Name = i.Name,
                Type = i.Type,
            }).ToArray();
        }
    }
}
