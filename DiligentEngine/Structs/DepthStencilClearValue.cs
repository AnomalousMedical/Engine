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
    public partial class DepthStencilClearValue
    {

        public DepthStencilClearValue()
        {
            
        }
        public Float32 Depth { get; set; } = 1f;
        public Uint8 Stencil { get; set; } = 0;


    }
}
