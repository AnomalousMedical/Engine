using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{
    public class SharpGuiRenderer : IDisposable
    {
        private AutoPtr<IPipelineState> pipelineState;
        private AutoPtr<IShaderResourceBinding> shaderResourceBinding;
        private AutoPtr<IBuffer> vertexBuffer;
        private AutoPtr<IBuffer> indexBuffer;
        private readonly OSWindow osWindow;
        private DrawIndexedAttribs DrawAttrs;
        private int maxNumberOfQuads;

        public SharpGuiRenderer(GraphicsEngine graphicsEngine, OSWindow osWindow, SharpGuiOptions options)
        {
            this.maxNumberOfQuads = options.MaxNumberOfQuads;

            DrawAttrs = new DrawIndexedAttribs()
            {
                IndexType = VALUE_TYPE.VT_UINT32,
                Flags = DRAW_FLAGS.DRAW_FLAG_VERIFY_ALL,
            };

            var m_pSwapChain = graphicsEngine.SwapChain;
            var m_pDevice = graphicsEngine.RenderDevice;

            this.osWindow = osWindow;

            var ShaderCI = new ShaderCreateInfo();
            ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;
            // OpenGL backend requires emulated combined HLSL texture samplers (g_Texture + g_Texture_sampler combination)
            ShaderCI.UseCombinedTextureSamplers = true;

            // Create a vertex shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_VERTEX;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "SharpGui Quad VS";
            ShaderCI.Source = VSSource;
            using var pVS = graphicsEngine.RenderDevice.CreateShader(ShaderCI);

            //Create pixel shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_PIXEL;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "SharpGui Quad PS";
            ShaderCI.Source = PSSource;
            using var pPS = graphicsEngine.RenderDevice.CreateShader(ShaderCI);

            var PSOCreateInfo = new GraphicsPipelineStateCreateInfo();
            PSOCreateInfo.PSODesc.Name = "SharpGui PSO";
            PSOCreateInfo.PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_GRAPHICS;
            PSOCreateInfo.GraphicsPipeline.NumRenderTargets = 1;
            PSOCreateInfo.GraphicsPipeline.RTVFormats_0 = m_pSwapChain.GetDesc_ColorBufferFormat;
            PSOCreateInfo.GraphicsPipeline.DSVFormat = m_pSwapChain.GetDesc_DepthBufferFormat;
            PSOCreateInfo.GraphicsPipeline.PrimitiveTopology = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
            PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE.CULL_MODE_BACK;
            PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthEnable = true;
            var RT0 = PSOCreateInfo.GraphicsPipeline.BlendDesc.RenderTargets_0;
            RT0.BlendEnable = true;
            RT0.SrcBlend = BLEND_FACTOR.BLEND_FACTOR_SRC_ALPHA;
            RT0.DestBlend = BLEND_FACTOR.BLEND_FACTOR_INV_SRC_ALPHA;
            RT0.BlendOp = BLEND_OPERATION.BLEND_OPERATION_ADD;
            RT0.SrcBlendAlpha = BLEND_FACTOR.BLEND_FACTOR_INV_SRC_ALPHA;
            RT0.DestBlendAlpha = BLEND_FACTOR.BLEND_FACTOR_ZERO;
            RT0.BlendOpAlpha = BLEND_OPERATION.BLEND_OPERATION_ADD;

            // Define vertex shader input layout
            var LayoutElems = new List<LayoutElement>
            {
                // Attribute 0 - vertex position
                new LayoutElement()
                {
                    InputIndex = 0,
                    BufferSlot = 0,
                    NumComponents = 3,
                    ValueType = VALUE_TYPE.VT_FLOAT32,
                    IsNormalized = false
                },
                // Attribute 1 - vertex color
                new LayoutElement
                {
                    InputIndex = 1,
                    BufferSlot = 0,
                    NumComponents = 4,
                    ValueType = VALUE_TYPE.VT_FLOAT32,
                    IsNormalized = false
                },
            };

            PSOCreateInfo.GraphicsPipeline.InputLayout.LayoutElements = LayoutElems;

            PSOCreateInfo.pVS = pVS.Obj;
            PSOCreateInfo.pPS = pPS.Obj;

            // Define variable type that will be used by default
            PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC;

            this.pipelineState = m_pDevice.CreateGraphicsPipelineState(PSOCreateInfo);

            // Create a shader resource binding object and bind all static resources in it
            shaderResourceBinding = pipelineState.Obj.CreateShaderResourceBinding(true);

            vertexBuffer = CreateVertexBuffer(graphicsEngine.RenderDevice, maxNumberOfQuads);
            indexBuffer = CreateIndexBuffer(graphicsEngine.RenderDevice, maxNumberOfQuads);
        }

        public void Dispose()
        {
            indexBuffer.Dispose();
            vertexBuffer.Dispose();
            shaderResourceBinding.Dispose();
            pipelineState.Dispose();
        }

        public unsafe void Render(SharpGuiBuffer buffer, IDeviceContext immediateContext)
        {
            immediateContext.SetPipelineState(pipelineState.Obj);

            IntPtr data = immediateContext.MapBuffer(vertexBuffer.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);

            var dest = new Span<SharpImGuiVertex>(data.ToPointer(), maxNumberOfQuads * 4);
            var src = new Span<SharpImGuiVertex>(buffer.QuadVerts);
            src.CopyTo(dest);

            immediateContext.UnmapBuffer(vertexBuffer.Obj, MAP_TYPE.MAP_WRITE);

            UInt32[] offset = new UInt32[] { 0 };
            IBuffer[] pBuffs = new IBuffer[] { vertexBuffer.Obj };
            immediateContext.SetVertexBuffers(0, 1, pBuffs, offset, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION, SET_VERTEX_BUFFERS_FLAGS.SET_VERTEX_BUFFERS_FLAG_RESET);
            immediateContext.SetIndexBuffer(indexBuffer.Obj, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.CommitShaderResources(shaderResourceBinding.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            DrawAttrs.NumIndices = buffer.NumQuadIndices;
            immediateContext.DrawIndexed(DrawAttrs);
        }

        public uint NumIndices { get; private set; }

        unsafe static AutoPtr<IBuffer> CreateVertexBuffer(IRenderDevice device, int maxNumberOfQuads)
        {
            BufferDesc VertBuffDesc = new BufferDesc();
            VertBuffDesc.Name = "Cube vertex buffer";
            VertBuffDesc.Usage = USAGE.USAGE_DYNAMIC;
            VertBuffDesc.BindFlags = BIND_FLAGS.BIND_VERTEX_BUFFER;
            VertBuffDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;
            VertBuffDesc.uiSizeInBytes = (uint)(sizeof(SharpImGuiVertex) * maxNumberOfQuads * 4);
            
            return device.CreateBuffer(VertBuffDesc);
            
        }

        unsafe static AutoPtr<IBuffer> CreateIndexBuffer(IRenderDevice device, int maxNumberOfQuads)
        {
            var Indices = new UInt32[maxNumberOfQuads * 6];

            uint indexBlock = 0;
            for(int i = 0; i < Indices.Length; i += 6)
            {
                Indices[i] = indexBlock;
                Indices[i + 1] = indexBlock + 1;
                Indices[i + 2] = indexBlock + 2;

                Indices[i + 3] = indexBlock + 2;
                Indices[i + 4] = indexBlock + 3;
                Indices[i + 5] = indexBlock;

                indexBlock += 4;
            }

            BufferDesc IndBuffDesc = new BufferDesc();
            IndBuffDesc.Name = "Cube index buffer";
            IndBuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
            IndBuffDesc.BindFlags = BIND_FLAGS.BIND_INDEX_BUFFER;
            IndBuffDesc.uiSizeInBytes = (uint)(sizeof(UInt32) * Indices.Length);
            BufferData IBData = new BufferData();
            fixed (UInt32* pIndices = Indices)
            {
                IBData.pData = new IntPtr(pIndices);
                IBData.DataSize = (uint)(sizeof(UInt32) * Indices.Length);
                return device.CreateBuffer(IndBuffDesc, IBData);
            }
        }

        const String VSSource =
@"
// Vertex shader takes two inputs: vertex position and color.
// By convention, Diligent Engine expects vertex shader inputs to be 
// labeled 'ATTRIBn', where n is the attribute number.
struct VSInput
{
    float3 Pos   : ATTRIB0;
    float4 Color : ATTRIB1;
};

struct PSInput 
{ 
    float4 Pos   : SV_POSITION; 
    float4 Color : COLOR0; 
};

void main(in  VSInput VSIn,
          out PSInput PSIn) 
{
    PSIn.Pos   = float4(VSIn.Pos, 1);
    PSIn.Color = VSIn.Color;
}";

        const String PSSource =
@"struct PSInput 
{ 
    float4 Pos   : SV_POSITION; 
    float4 Color : COLOR0; 
};

struct PSOutput
{ 
    float4 Color : SV_TARGET; 
};

void main(in  PSInput  PSIn,
          out PSOutput PSOut)
{
    PSOut.Color = PSIn.Color; 
}";

        const String TextVSSource =
@"
struct VSInput
{
    float3 Pos   : ATTRIB0;
    float4 Color : ATTRIB1;
    float2 UV    : ATTRIB2;
};

struct PSInput 
{ 
    float4 Pos   : SV_POSITION; 
    float4 Color : COLOR0; 
    float2 UV    : TEX_COORD; 
};

void main(in  VSInput VSIn,
          out PSInput PSIn) 
{
    PSIn.Pos   = float4(VSIn.Pos, 1);
    PSIn.Color = VSIn.Color;
    PSIn.UV    = VSIn.UV;
}";

        const String TextPSSource =
@"
Texture2D    g_Texture;
SamplerState g_Texture_sampler;

struct PSInput 
{ 
    float4 Pos   : SV_POSITION; 
    float4 Color : COLOR0; 
};

struct PSOutput
{ 
    float4 Color : SV_TARGET; 
};

void main(in  PSInput  PSIn,
          out PSOutput PSOut)
{
    PSOut.Color = PSIn.Color * g_Texture.Sample(g_Texture_sampler, PSIn.UV); 
}";
    }
}
