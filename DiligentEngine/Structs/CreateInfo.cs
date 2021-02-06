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

namespace DiligentEngine
{
    public partial class CreateInfo
    {

        public CreateInfo()
        {
            
        }
        public TEXTURE_FORMAT RTVFmt { get; set; }
        public TEXTURE_FORMAT DSVFmt { get; set; }
        public bool FrontCCW { get; set; }
        public bool AllowDebugView { get; set; }
        public bool UseIBL { get; set; }
        public bool UseAO { get; set; }
        public bool UseEmissive { get; set; }
        public bool UseImmutableSamplers { get; set; }
        public bool UseTextureAtals { get; set; }
        public SamplerDesc ColorMapImmutableSampler { get; set; } = new SamplerDesc();
        public SamplerDesc PhysDescMapImmutableSampler { get; set; } = new SamplerDesc();
        public SamplerDesc NormalMapImmutableSampler { get; set; } = new SamplerDesc();
        public SamplerDesc AOMapImmutableSampler { get; set; } = new SamplerDesc();
        public SamplerDesc EmissiveMapImmutableSampler { get; set; } = new SamplerDesc();
        public Uint32 MaxJointCount { get; set; }


    }
}
