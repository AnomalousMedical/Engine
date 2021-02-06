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
using float4 = Engine.Vector4;
using float3 = Engine.Vector3;
using float4x4 = Engine.Matrix4x4;
using BOOL = System.Boolean;

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
        public InputLayoutDesc InputLayout { get; set; } = new InputLayoutDesc();
        public PRIMITIVE_TOPOLOGY PrimitiveTopology { get; set; } = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
        public Uint8 NumViewports { get; set; } = 1;
        public Uint8 NumRenderTargets { get; set; } = 0;
        public Uint8 SubpassIndex { get; set; } = 0;
        public TEXTURE_FORMAT RTVFormats_0 { get; set; }
        public TEXTURE_FORMAT RTVFormats_1 { get; set; }
        public TEXTURE_FORMAT RTVFormats_2 { get; set; }
        public TEXTURE_FORMAT RTVFormats_3 { get; set; }
        public TEXTURE_FORMAT RTVFormats_4 { get; set; }
        public TEXTURE_FORMAT RTVFormats_5 { get; set; }
        public TEXTURE_FORMAT RTVFormats_6 { get; set; }
        public TEXTURE_FORMAT RTVFormats_7 { get; set; }
        public TEXTURE_FORMAT DSVFormat { get; set; } = TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN;
        public SampleDesc SmplDesc { get; set; } = new SampleDesc();
        public Uint32 NodeMask { get; set; } = 0;


    }
}
