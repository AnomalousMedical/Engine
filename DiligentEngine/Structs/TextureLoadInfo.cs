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
    public partial class TextureLoadInfo
    {

        public TextureLoadInfo()
        {
            
        }
        public String Name { get; set; }
        public USAGE Usage { get; set; } = USAGE.USAGE_IMMUTABLE;
        public BIND_FLAGS BindFlags { get; set; } = BIND_FLAGS.BIND_SHADER_RESOURCE;
        public Uint32 MipLevels { get; set; } = 0;
        public CPU_ACCESS_FLAGS CPUAccessFlags { get; set; } = CPU_ACCESS_FLAGS.CPU_ACCESS_NONE;
        public Bool IsSRGB { get; set; } = false;
        public Bool GenerateMips { get; set; } = true;
        public TEXTURE_FORMAT Format { get; set; } = TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN;


    }
}
