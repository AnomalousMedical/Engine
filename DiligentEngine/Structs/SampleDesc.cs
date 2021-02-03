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
    public partial class SampleDesc
    {

        public SampleDesc()
        {
            
        }
        public Uint8 Count { get; set; } = 1;
        public Uint8 Quality { get; set; } = 0;


    }
}
