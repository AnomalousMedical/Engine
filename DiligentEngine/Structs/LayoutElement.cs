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
    public partial class LayoutElement
    {

        public LayoutElement()
        {
            
        }
        public String HLSLSemantic { get; set; } = "ATTRIB";
        public Uint32 InputIndex { get; set; } = 0;
        public Uint32 BufferSlot { get; set; } = 0;
        public Uint32 NumComponents { get; set; } = 0;
        public VALUE_TYPE ValueType { get; set; } = VALUE_TYPE.VT_FLOAT32;
        public Bool IsNormalized { get; set; } = true;
        public Uint32 RelativeOffset { get; set; } = LAYOUT_ELEMENT_AUTO_OFFSET;
        public Uint32 Stride { get; set; } = LAYOUT_ELEMENT_AUTO_STRIDE;
        public INPUT_ELEMENT_FREQUENCY Frequency { get; set; } = INPUT_ELEMENT_FREQUENCY.INPUT_ELEMENT_FREQUENCY_PER_VERTEX;
        public Uint32 InstanceDataStepRate { get; set; } = 1;


    }
}
