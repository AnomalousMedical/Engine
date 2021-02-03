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
    public partial class PipelineStateDesc : DeviceObjectAttribs
    {
        public PipelineStateDesc()
        {

        }
        public PIPELINE_TYPE PipelineType { get; set; } = PIPELINE_TYPE.PIPELINE_TYPE_GRAPHICS;
        public Uint32 SRBAllocationGranularity { get; set; } = 1;
        public Uint64 CommandQueueMask { get; set; } = 1;
        public PipelineResourceLayoutDesc ResourceLayout { get; set; } = new PipelineResourceLayoutDesc();


    }
}
