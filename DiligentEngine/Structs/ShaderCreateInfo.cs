using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;

namespace DiligentEngine
{
    public partial class ShaderCreateInfo
    {

        public ShaderCreateInfo()
        {
            
        }
        public String FilePath { get; set; }
        public String Source { get; set; }
        public String EntryPoint { get; set; }


    }
}
