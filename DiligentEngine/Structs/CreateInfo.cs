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
    public partial class CreateInfo
    {

        public CreateInfo()
        {
            
        }
        public TEXTURE_FORMAT RTVFmt { get; set; } = TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN;
        public TEXTURE_FORMAT DSVFmt { get; set; } = TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN;
        public bool FrontCCW { get; set; } = false;
        public bool AllowDebugView { get; set; } = false;
        public bool UseIBL { get; set; } = false;
        public bool UseAO { get; set; } = true;
        public bool UseEmissive { get; set; } = true;
        public bool UseImmutableSamplers { get; set; } = true;
        public bool UseTextureAtals { get; set; } = false;
        public SamplerDesc ColorMapImmutableSampler { get; set; } = new SamplerDesc();
        public SamplerDesc PhysDescMapImmutableSampler { get; set; } = new SamplerDesc();
        public SamplerDesc NormalMapImmutableSampler { get; set; } = new SamplerDesc();
        public SamplerDesc AOMapImmutableSampler { get; set; } = new SamplerDesc();
        public SamplerDesc EmissiveMapImmutableSampler { get; set; } = new SamplerDesc();
        public Uint32 MaxJointCount { get; set; } = 64;


    }
}
