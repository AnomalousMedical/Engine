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
    public partial class OptimizedClearValue
    {

        public OptimizedClearValue()
        {
            
        }
        public TEXTURE_FORMAT Format { get; set; } = TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN;
        public Float32 Color_0 { get; set; }
        public Float32 Color_1 { get; set; }
        public Float32 Color_2 { get; set; }
        public Float32 Color_3 { get; set; }
        public DepthStencilClearValue DepthStencil { get; set; } = new DepthStencilClearValue();


    }
}
