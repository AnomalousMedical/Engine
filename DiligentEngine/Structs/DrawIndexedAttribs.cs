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
    public partial class DrawIndexedAttribs
    {

        public DrawIndexedAttribs()
        {
            
        }
        public Uint32 NumIndices { get; set; } = 0;
        public VALUE_TYPE IndexType { get; set; } = VALUE_TYPE.VT_UNDEFINED;
        public DRAW_FLAGS Flags { get; set; } = DRAW_FLAGS.DRAW_FLAG_NONE;
        public Uint32 NumInstances { get; set; } = 1;
        public Uint32 FirstIndexLocation { get; set; } = 0;
        public Uint32 BaseVertex { get; set; } = 0;
        public Uint32 FirstInstanceLocation { get; set; } = 0;


    }
}
