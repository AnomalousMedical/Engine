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
    public partial class BufferDesc : DeviceObjectAttribs
    {
            public BufferDesc(IntPtr objPtr)
                : base(objPtr)
            {

            }
        public Uint32 uiSizeInBytes {get; set;}
        public BIND_FLAGS BindFlags {get; set;}
        public USAGE Usage {get; set;}
        public CPU_ACCESS_FLAGS CPUAccessFlags {get; set;}
        public BUFFER_MODE Mode {get; set;}
        public Uint32 ElementByteStride {get; set;}
        public Uint64 CommandQueueMask {get; set;}
    }
}
