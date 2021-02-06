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
    public partial class GraphicsPipelineStateCreateInfo : PipelineStateCreateInfo
    {
        public GraphicsPipelineStateCreateInfo()
        {

        }
        public GraphicsPipelineDesc GraphicsPipeline { get; set; } = new GraphicsPipelineDesc();
        public IShader pVS { get; set; }
        public IShader pPS { get; set; }
        public IShader pDS { get; set; }
        public IShader pHS { get; set; }
        public IShader pGS { get; set; }
        public IShader pAS { get; set; }
        public IShader pMS { get; set; }


    }
}
