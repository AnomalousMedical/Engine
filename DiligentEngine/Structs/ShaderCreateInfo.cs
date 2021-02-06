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

namespace DiligentEngine
{
    public partial class ShaderCreateInfo
    {

        public ShaderCreateInfo()
        {
            
        }
        public String FilePath { get; set; }
        public String Source { get; set; }
        public String EntryPoint { get; set; } = "main";
        public bool UseCombinedTextureSamplers { get; set; } = false;
        public String CombinedSamplerSuffix { get; set; } = "_sampler";
        public ShaderDesc Desc { get; set; } = new ShaderDesc();
        public SHADER_SOURCE_LANGUAGE SourceLanguage { get; set; } = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_DEFAULT;


    }
}
