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
    public partial class TextureViewDesc : DeviceObjectAttribs
    {
        public TextureViewDesc()
        {

        }
        public TEXTURE_VIEW_TYPE ViewType { get; set; } = TEXTURE_VIEW_TYPE.TEXTURE_VIEW_UNDEFINED;
        public RESOURCE_DIMENSION TextureDim { get; set; } = RESOURCE_DIMENSION.RESOURCE_DIM_UNDEFINED;
        public TEXTURE_FORMAT Format { get; set; } = TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN;
        public Uint32 MostDetailedMip { get; set; } = 0;
        public Uint32 NumMipLevels { get; set; } = 0;
        public Uint32 FirstArraySlice { get; set; } = 0;
        public Uint32 NumArraySlices { get; set; } = 0;
        public UAV_ACCESS_FLAG AccessFlags { get; set; } = UAV_ACCESS_FLAG.UAV_ACCESS_UNSPECIFIED;
        public TEXTURE_VIEW_FLAGS Flags { get; set; } = TEXTURE_VIEW_FLAGS.TEXTURE_VIEW_FLAG_NONE;


    }
}
