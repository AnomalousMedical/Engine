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
    public partial class GraphicsPipelineDesc
    {

        public GraphicsPipelineDesc()
        {
            
        }
        public BlendStateDesc BlendDesc { get; set; } = new BlendStateDesc();
        public Uint32 SampleMask { get; set; } = 0xFFFFFFFF;
        public RasterizerStateDesc RasterizerDesc { get; set; } = new RasterizerStateDesc();
        public DepthStencilStateDesc DepthStencilDesc { get; set; } = new DepthStencilStateDesc();
        public PRIMITIVE_TOPOLOGY PrimitiveTopology { get; set; } = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
        public Uint8 NumViewports { get; set; } = 1;
        public Uint8 NumRenderTargets { get; set; } = 0;
        public Uint8 SubpassIndex { get; set; } = 0;


    }
}
