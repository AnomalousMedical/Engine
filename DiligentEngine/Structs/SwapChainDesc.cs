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

namespace DiligentEngine
{
    public partial class SwapChainDesc
    {

        public SwapChainDesc()
        {
            
        }
        public Uint32 Width { get; set; } = 0;
        public Uint32 Height { get; set; } = 0;
        public TEXTURE_FORMAT ColorBufferFormat { get; set; } = TEXTURE_FORMAT.TEX_FORMAT_RGBA8_UNORM_SRGB;
        public TEXTURE_FORMAT DepthBufferFormat { get; set; } = TEXTURE_FORMAT.TEX_FORMAT_D32_FLOAT;
        public SWAP_CHAIN_USAGE_FLAGS Usage { get; set; } = SWAP_CHAIN_USAGE_FLAGS.SWAP_CHAIN_USAGE_RENDER_TARGET;
        public SURFACE_TRANSFORM PreTransform { get; set; } = SURFACE_TRANSFORM.SURFACE_TRANSFORM_OPTIMAL;
        public Uint32 BufferCount { get; set; } = 2;
        public Float32 DefaultDepthValue { get; set; } = 1f;
        public Uint8 DefaultStencilValue { get; set; } = 0;
        public bool IsPrimary { get; set; } = true;


    }
}
