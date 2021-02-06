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
    public partial class TextureDesc : DeviceObjectAttribs
    {
        public TextureDesc()
        {

        }
        public RESOURCE_DIMENSION Type { get; set; } = RESOURCE_DIMENSION.RESOURCE_DIM_UNDEFINED;
        public Uint32 Width { get; set; } = 0;
        public Uint32 Height { get; set; } = 0;
        public Uint32 ArraySize { get; set; } = 1;
        public TEXTURE_FORMAT Format { get; set; } = TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN;
        public Uint32 MipLevels { get; set; } = 1;
        public Uint32 SampleCount { get; set; } = 1;
        public USAGE Usage { get; set; } = USAGE.USAGE_DEFAULT;
        public BIND_FLAGS BindFlags { get; set; } = BIND_FLAGS.BIND_NONE;
        public CPU_ACCESS_FLAGS CPUAccessFlags { get; set; } = CPU_ACCESS_FLAGS.CPU_ACCESS_NONE;
        public MISC_TEXTURE_FLAGS MiscFlags { get; set; } = MISC_TEXTURE_FLAGS.MISC_TEXTURE_FLAG_NONE;
        public OptimizedClearValue ClearValue { get; set; } = new OptimizedClearValue();
        public Uint64 CommandQueueMask { get; set; } = 1;


    }
}
