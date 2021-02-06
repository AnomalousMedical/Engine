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
    public partial class LightAttribs
    {

        public LightAttribs()
        {
            
        }
        public float4 f4Direction { get; set; }
        public float4 f4AmbientLight { get; set; }
        public float4 f4Intensity { get; set; }
        public ShadowMapAttribs ShadowAttribs { get; set; }


    }
}
