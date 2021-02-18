using DiligentEngine;
using Engine;
using Engine.Platform;
using Engine.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{
    public class SharpGuiRenderer : IDisposable
    {
        private AutoPtr<IPipelineState> quadPipelineState;
        private AutoPtr<IShaderResourceBinding> quadShaderResourceBinding;
        private AutoPtr<IBuffer> quadVertexBuffer;
        private AutoPtr<IBuffer> quadIndexBuffer;

        private AutoPtr<IPipelineState> textPipelineState;
        private AutoPtr<IShaderResourceBinding> textShaderResourceBinding;
        private AutoPtr<IBuffer> textVertexBuffer;
        private AutoPtr<IBuffer> textIndexBuffer;
        private Font font;

        private readonly OSWindow osWindow;
        private readonly IResourceProvider<SharpGuiRenderer> resourceProvider;
        private DrawIndexedAttribs DrawAttrs;
        private uint maxNumberOfQuads;
        private uint maxNumberOfTextQuads;

        public unsafe SharpGuiRenderer(GraphicsEngine graphicsEngine, OSWindow osWindow, SharpGuiOptions options, IResourceProvider<SharpGuiRenderer> resourceProvider)
        {
            this.maxNumberOfQuads = options.MaxNumberOfQuads;
            this.maxNumberOfTextQuads = options.MaxNumberOfTextQuads;

            DrawAttrs = new DrawIndexedAttribs()
            {
                IndexType = VALUE_TYPE.VT_UINT32,
                Flags = DRAW_FLAGS.DRAW_FLAG_VERIFY_ALL,
            };

            var m_pSwapChain = graphicsEngine.SwapChain;
            var m_pDevice = graphicsEngine.RenderDevice;

            this.osWindow = osWindow;
            this.resourceProvider = resourceProvider;
            CreateQuadPso(graphicsEngine, m_pSwapChain, m_pDevice);
            CreateTextPso(graphicsEngine, m_pSwapChain, m_pDevice);

            quadVertexBuffer = CreateVertexBuffer(graphicsEngine.RenderDevice, "SharpGui Quad Vertex Buffer", (uint)sizeof(SharpImGuiVertex), maxNumberOfQuads);
            quadIndexBuffer = CreateIndexBuffer(graphicsEngine.RenderDevice, "SharpGui Quad Index Buffer", maxNumberOfQuads);

            textVertexBuffer = CreateVertexBuffer(graphicsEngine.RenderDevice, "SharpGui Text Vertex Buffer", (uint)sizeof(SharpImGuiTextVertex), maxNumberOfQuads);
            textIndexBuffer = CreateIndexBuffer(graphicsEngine.RenderDevice, "SharpGui Text Index Buffer", maxNumberOfQuads);
        }

        private void CreateQuadPso(GraphicsEngine graphicsEngine, ISwapChain m_pSwapChain, IRenderDevice m_pDevice)
        {
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
            PSOCreateInfo.PSODesc.Name = "SharpGui Quad PSO";
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

            this.quadPipelineState = m_pDevice.CreateGraphicsPipelineState(PSOCreateInfo);

            // Create a shader resource binding object and bind all static resources in it
            this.quadShaderResourceBinding = quadPipelineState.Obj.CreateShaderResourceBinding(true);
        }

        private void CreateTextPso(GraphicsEngine graphicsEngine, ISwapChain m_pSwapChain, IRenderDevice m_pDevice)
        {
            var ShaderCI = new ShaderCreateInfo();
            ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;
            // OpenGL backend requires emulated combined HLSL texture samplers (g_Texture + g_Texture_sampler combination)
            ShaderCI.UseCombinedTextureSamplers = true;

            // Create a vertex shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_VERTEX;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "SharpGui Text VS";
            ShaderCI.Source = TextVSSource;
            using var pVS = graphicsEngine.RenderDevice.CreateShader(ShaderCI);

            //Create pixel shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_PIXEL;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "SharpGui Text PS";
            ShaderCI.Source = TextPSSource;
            using var pPS = graphicsEngine.RenderDevice.CreateShader(ShaderCI);

            var PSOCreateInfo = new GraphicsPipelineStateCreateInfo();
            PSOCreateInfo.PSODesc.Name = "SharpGui Text PSO";
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
                // Attribute 2 - uv
                new LayoutElement
                {
                    InputIndex = 2,
                    BufferSlot = 0,
                    NumComponents = 2,
                    ValueType = VALUE_TYPE.VT_FLOAT32,
                    IsNormalized = false
                },
            };

            PSOCreateInfo.GraphicsPipeline.InputLayout.LayoutElements = LayoutElems;

            PSOCreateInfo.pVS = pVS.Obj;
            PSOCreateInfo.pPS = pPS.Obj;

            // Define variable type that will be used by default
            PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC;

            // Shader variables should typically be mutable, which means they are expected
            // to change on a per-instance basis
            var Vars = new List<ShaderResourceVariableDesc>()
            {
                new ShaderResourceVariableDesc
                {ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, Name = "g_Texture", Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_MUTABLE}
            };
            PSOCreateInfo.PSODesc.ResourceLayout.Variables = Vars;

            // Define immutable sampler for g_Texture. Immutable samplers should be used whenever possible
            SamplerDesc SamLinearClampDesc = new SamplerDesc();
            var ImtblSamplers = new List<ImmutableSamplerDesc>()
            {
                new ImmutableSamplerDesc
                {ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, SamplerOrTextureName = "g_Texture", Desc = SamLinearClampDesc}
            };
            PSOCreateInfo.PSODesc.ResourceLayout.ImmutableSamplers = ImtblSamplers;

            this.textPipelineState = m_pDevice.CreateGraphicsPipelineState(PSOCreateInfo);

            // Create a shader resource binding object and bind all static resources in it
            this.textShaderResourceBinding = textPipelineState.Obj.CreateShaderResourceBinding(true);

            LoadFontTexture(graphicsEngine);
        }

        private void LoadFontTexture(GraphicsEngine graphicsEngine)
        {
            //Load Font Texture
            using var fontStream = resourceProvider.openFile("fonts/Roboto-Regular.ttf");
            var bytes = new byte[fontStream.Length];
            var span = new Span<byte>(bytes);
            while (fontStream.Read(span) != 0) { }
            using var font = new MyGUITrueTypeFont(bytes);

            //Debug
            //unsafe {
            //    using var fib = new FreeImageAPI.FreeImageBitmap(font.TextureBufferWidth, font.TextureBufferHeight, FreeImageAPI.PixelFormat.Format32bppRgb);
            //    var fibStart = (byte*)fib.Scan0.ToPointer() - (-fib.Stride * (fib.Height - 1));
            //    var fibSpan = new Span<byte>(fibStart, fib.DataSize);
            //    var ftSpan = new Span<byte>(font.TextureBuffer.ToPointer(), (int)font.TextureBufferSize.ToUInt64());
            //    ftSpan.CopyTo(fibSpan);
            //    using var saveStream = System.IO.File.Open("test.png", System.IO.FileMode.Create);
            //    fib.Save(saveStream, FreeImageAPI.FREE_IMAGE_FORMAT.FIF_PNG);
            //}
            ////End

            uint width = (uint)font.TextureBufferWidth;
            uint height = (uint)font.TextureBufferHeight;

            //const auto& ImgDesc = pSrcImage->GetDesc();
            TextureDesc TexDesc = new TextureDesc();
            TexDesc.Name = "Font texture";
            TexDesc.Type = RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D;
            TexDesc.Width = width;
            TexDesc.Height = height;
            TexDesc.MipLevels = 1;// (uint)Math.Min(TexDesc.MipLevels, MipLevels);
            TexDesc.Usage = USAGE.USAGE_IMMUTABLE;
            TexDesc.BindFlags = BIND_FLAGS.BIND_SHADER_RESOURCE;
            TexDesc.Format = TEXTURE_FORMAT.TEX_FORMAT_RGBA8_UNORM;
            TexDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_NONE;

            var pSubResources = new List<TextureSubResData>(1);
            pSubResources.Add(new TextureSubResData()
            {
                pData = font.TextureBuffer,
                Stride = sizeof(UInt32) * width,
            });

            TextureData TexData = new TextureData();
            TexData.pSubResources = pSubResources;

            using var tex = graphicsEngine.RenderDevice.CreateTexture(TexDesc, TexData); //This does not do anything with this pointer, just pass it along and let the caller handle it
            var m_TextureSRV = tex.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);

            // Set texture SRV in the SRB
            textShaderResourceBinding.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "g_Texture").Set(m_TextureSRV);

            this.font = new Font()
            {
                CharMap = font.CharMap,
                GlyphInfo = font.GlyphInfo
            };
        }

        public Font Font => this.font;

        public void Dispose()
        {
            textIndexBuffer.Dispose();
            textVertexBuffer.Dispose();
            textShaderResourceBinding.Dispose();
            textPipelineState.Dispose();

            quadIndexBuffer.Dispose();
            quadVertexBuffer.Dispose();
            quadShaderResourceBinding.Dispose();
            quadPipelineState.Dispose();
        }

        public unsafe void Render(SharpGuiBuffer buffer, IDeviceContext immediateContext)
        {
            if (buffer.NumQuadIndices != 0)
            {
                immediateContext.SetPipelineState(quadPipelineState.Obj);

                //Copy quad vertices
                {
                    IntPtr data = immediateContext.MapBuffer(quadVertexBuffer.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);

                    var dest = new Span<SharpImGuiVertex>(data.ToPointer(), (int)maxNumberOfQuads * 4); //Yea this typecast is bad
                    var src = new Span<SharpImGuiVertex>(buffer.QuadVerts);
                    src.CopyTo(dest);

                    immediateContext.UnmapBuffer(quadVertexBuffer.Obj, MAP_TYPE.MAP_WRITE);
                }

                //Render quad vertices
                {
                    IBuffer[] pBuffs = new IBuffer[] { quadVertexBuffer.Obj };
                    immediateContext.SetVertexBuffers(0, 1, pBuffs, null, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION, SET_VERTEX_BUFFERS_FLAGS.SET_VERTEX_BUFFERS_FLAG_RESET);
                    immediateContext.SetIndexBuffer(quadIndexBuffer.Obj, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
                    immediateContext.CommitShaderResources(quadShaderResourceBinding.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

                    DrawAttrs.NumIndices = buffer.NumQuadIndices;
                    immediateContext.DrawIndexed(DrawAttrs);
                }
            }

            if (buffer.NumTextIndices != 0)
            {
                immediateContext.SetPipelineState(textPipelineState.Obj);

                //Copy text vertices
                {
                    IntPtr data = immediateContext.MapBuffer(textVertexBuffer.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);

                    var dest = new Span<SharpImGuiTextVertex>(data.ToPointer(), (int)maxNumberOfTextQuads * 4); //Yea this typecast is bad
                    var src = new Span<SharpImGuiTextVertex>(buffer.TextVerts);
                    src.CopyTo(dest);

                    immediateContext.UnmapBuffer(textVertexBuffer.Obj, MAP_TYPE.MAP_WRITE);
                }

                //Render text vertices
                {
                    IBuffer[] pBuffs = new IBuffer[] { textVertexBuffer.Obj };
                    immediateContext.SetVertexBuffers(0, 1, pBuffs, null, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION, SET_VERTEX_BUFFERS_FLAGS.SET_VERTEX_BUFFERS_FLAG_RESET);
                    immediateContext.SetIndexBuffer(textIndexBuffer.Obj, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
                    immediateContext.CommitShaderResources(textShaderResourceBinding.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

                    DrawAttrs.NumIndices = buffer.NumTextIndices;
                    immediateContext.DrawIndexed(DrawAttrs);
                }
            }
        }

        public uint NumIndices { get; private set; }

        unsafe static AutoPtr<IBuffer> CreateVertexBuffer(IRenderDevice device, string name, uint vertexSize, uint maxNumberOfQuads)
        {
            BufferDesc VertBuffDesc = new BufferDesc();
            VertBuffDesc.Name = name;
            VertBuffDesc.Usage = USAGE.USAGE_DYNAMIC;
            VertBuffDesc.BindFlags = BIND_FLAGS.BIND_VERTEX_BUFFER;
            VertBuffDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;
            VertBuffDesc.uiSizeInBytes = vertexSize * maxNumberOfQuads * 4;
            
            return device.CreateBuffer(VertBuffDesc);
            
        }

        unsafe static AutoPtr<IBuffer> CreateIndexBuffer(IRenderDevice device, string name, uint maxNumberOfQuads)
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
            IndBuffDesc.Name = name;
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
    float2 UV    : TEX_COORD; 
};

struct PSOutput
{ 
    float4 Color : SV_TARGET; 
};

void main(in  PSInput  PSIn,
          out PSOutput PSOut)
{
    float4 texColor = g_Texture.Sample(g_Texture_sampler, PSIn.UV);
    PSOut.Color.rgb = PSIn.Color.rgb * texColor.rgb;
    PSOut.Color.a = texColor.a;
}";
    }
}
