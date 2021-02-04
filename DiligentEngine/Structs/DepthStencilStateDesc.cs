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
    public partial class DepthStencilStateDesc
    {

        public DepthStencilStateDesc()
        {
            
        }
        public Bool DepthEnable { get; set; } = true;
        public Bool DepthWriteEnable { get; set; } = true;
        public COMPARISON_FUNCTION DepthFunc { get; set; } = COMPARISON_FUNCTION.COMPARISON_FUNC_LESS;
        public Bool StencilEnable { get; set; } = false;
        public Uint8 StencilReadMask { get; set; } = 0xFF;
        public Uint8 StencilWriteMask { get; set; } = 0xFF;
        public StencilOpDesc FrontFace { get; set; } = new StencilOpDesc();
        public StencilOpDesc BackFace { get; set; } = new StencilOpDesc();


    }
}
