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
    public partial class DeviceObjectAttribs
    {
        internal protected IntPtr objPtr;

        public IntPtr ObjPtr => objPtr;

        public DeviceObjectAttribs(IntPtr objPtr)
        {
            this.objPtr = objPtr;
        }
        public String Name {get; set;}
    }
}
