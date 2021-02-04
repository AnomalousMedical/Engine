using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

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
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct LayoutElementPassStruct
    {
        public String HLSLSemantic;
        public Uint32 InputIndex;
        public Uint32 BufferSlot;
        public Uint32 NumComponents;
        public VALUE_TYPE ValueType;
        public Uint32 IsNormalized;
        public Uint32 RelativeOffset;
        public Uint32 Stride;
        public INPUT_ELEMENT_FREQUENCY Frequency;
        public Uint32 InstanceDataStepRate;
        public static LayoutElementPassStruct[] ToStruct(IEnumerable<LayoutElement> vals)
        {
            if(vals == null)
            {
                return null;
            }

            return vals.Select(i => new LayoutElementPassStruct
            {
                HLSLSemantic = i.HLSLSemantic,
                InputIndex = i.InputIndex,
                BufferSlot = i.BufferSlot,
                NumComponents = i.NumComponents,
                ValueType = i.ValueType,
                IsNormalized = Convert.ToUInt32(i.IsNormalized),
                RelativeOffset = i.RelativeOffset,
                Stride = i.Stride,
                Frequency = i.Frequency,
                InstanceDataStepRate = i.InstanceDataStepRate,
            }).ToArray();
        }
    }
}
