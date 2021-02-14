using Anomalous.OSPlatform;
using DiligentEngine;
using Engine;
using Engine.Platform;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


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
using float2 = Engine.Vector2;
using float4x4 = Engine.Matrix4x4;
using BOOL = System.Boolean;
using System.IO;

namespace Tutorial13_ShadowMap
{
    class ShadowMapUpdateListener : UpdateListener, IDisposable
    {
        private readonly GraphicsEngine graphicsEngine;
        private readonly NativeOSWindow window;
        private readonly TextureLoader textureLoader;
        private readonly ShaderLoader<ShadowMapUpdateListener> shaderLoader;
        private readonly ISwapChain m_pSwapChain;
        private readonly IDeviceContext m_pImmediateContext;
        private readonly IRenderDevice m_pDevice;

        private AutoPtr<IBuffer> m_VSConstants;
        private AutoPtr<IPipelineState> m_pCubePSO;

        public unsafe ShadowMapUpdateListener(GraphicsEngine graphicsEngine, NativeOSWindow window, TextureLoader textureLoader, ShaderLoader<ShadowMapUpdateListener> shaderLoader)
        {
            this.graphicsEngine = graphicsEngine;
            this.window = window;
            this.textureLoader = textureLoader;
            this.shaderLoader = shaderLoader;
            this.m_pSwapChain = graphicsEngine.SwapChain;
            this.m_pImmediateContext = graphicsEngine.ImmediateContext;
            this.m_pDevice = graphicsEngine.RenderDevice;

            var Barriers = new List<StateTransitionDesc>();
            // Create dynamic uniform buffer that will store our transformation matrices
            // Dynamic buffers can be frequently updated by the CPU

            {
                BufferDesc CBDesc = new BufferDesc();
                CBDesc.Name = "VS constants CB";
                CBDesc.uiSizeInBytes = (uint)(sizeof(float4x4) * 2 + sizeof(float4));
                CBDesc.Usage = USAGE.USAGE_DYNAMIC;
                CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;

                m_VSConstants = m_pDevice.CreateBuffer(CBDesc);
                Barriers.Add(new StateTransitionDesc()
                {
                    pResource = m_VSConstants.Obj,
                    OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN,
                    NewState = RESOURCE_STATE.RESOURCE_STATE_CONSTANT_BUFFER,
                    UpdateResourceState = true
                });
            }

            CreateCubePSO();
            //CreatePlanePSO();
            //CreateShadowMapVisPSO();

            //// Load cube

            //// In this tutorial we need vertices with normals
            //CreateVertexBuffer();
            //// Load index buffer
            //m_CubeIndexBuffer = TexturedCube::CreateIndexBuffer(m_pDevice);
            //// Explicitly transition vertex and index buffers to required states
            //Barriers.emplace_back(m_CubeVertexBuffer, RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_VERTEX_BUFFER, true);
            //Barriers.emplace_back(m_CubeIndexBuffer, RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_INDEX_BUFFER, true);
            //// Load texture
            //auto CubeTexture = TexturedCube::LoadTexture(m_pDevice, "DGLogo.png");
            //m_CubeSRB->GetVariableByName(SHADER_TYPE_PIXEL, "g_Texture")->Set(CubeTexture->GetDefaultView(TEXTURE_VIEW_SHADER_RESOURCE));
            //// Transition the texture to shader resource state
            //Barriers.emplace_back(CubeTexture, RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_SHADER_RESOURCE, true);

            //CreateShadowMap();

            m_pImmediateContext.TransitionResourceStates(Barriers);
        }

        static readonly List<LayoutElement> DefaultLayoutElems = new List<LayoutElement>
        {
            // Per-vertex data - first buffer slot
            // Attribute 0 - vertex position
            new LayoutElement{InputIndex = 0, BufferSlot = 0, NumComponents = 3, ValueType = VALUE_TYPE.VT_FLOAT32, IsNormalized = false},
            // Attribute 1 - texture coordinates
            new LayoutElement{InputIndex = 1, BufferSlot = 0, NumComponents = 2, ValueType = VALUE_TYPE.VT_FLOAT32, IsNormalized = false}
        };

        private static AutoPtr<IPipelineState> CreateTexCubePipelineState(IRenderDevice                   pDevice,
                                                  TEXTURE_FORMAT                   RTVFormat,
                                                  TEXTURE_FORMAT                   DSVFormat,
                                                  String                      VSSource,
                                                  String                      PSSource,
                                                  List<LayoutElement>                   LayoutElements = null,
                                                  Uint8                            SampleCount = 1)
        {
            var PSOCreateInfo    = new GraphicsPipelineStateCreateInfo();
            var PSODesc          = PSOCreateInfo.PSODesc;
            var ResourceLayout   = PSODesc.ResourceLayout;
            var GraphicsPipeline = PSOCreateInfo.GraphicsPipeline;

            // This is a graphics pipeline
            PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_GRAPHICS;

            // Pipeline state name is used by the engine to report issues.
            // It is always a good idea to give objects descriptive names.
            PSODesc.Name = "Cube PSO";

            // clang-format off
            // This tutorial will render to a single render target
            GraphicsPipeline.NumRenderTargets             = 1;
            // Set render target format which is the format of the swap chain's color buffer
            GraphicsPipeline.RTVFormats_0                 = RTVFormat;
            // Set depth buffer format which is the format of the swap chain's back buffer
            GraphicsPipeline.DSVFormat                    = DSVFormat;
            // Set the desired number of samples
            GraphicsPipeline.SmplDesc.Count               = SampleCount;
            // Primitive topology defines what kind of primitives will be rendered by this pipeline state
            GraphicsPipeline.PrimitiveTopology            = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
            // Cull back faces
            GraphicsPipeline.RasterizerDesc.CullMode      = CULL_MODE.CULL_MODE_BACK;
            // Enable depth testing
            GraphicsPipeline.DepthStencilDesc.DepthEnable = true;
            // clang-format on
            var ShaderCI = new ShaderCreateInfo();
            // Tell the system that the shader source code is in HLSL.
            // For OpenGL, the engine will convert this into GLSL under the hood.
            ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;

            // OpenGL backend requires emulated combined HLSL texture samplers (g_Texture + g_Texture_sampler combination)
            ShaderCI.UseCombinedTextureSamplers = true;

            // Create a vertex shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_VERTEX;
            ShaderCI.EntryPoint      = "main";
            ShaderCI.Desc.Name       = "Cube VS";
            ShaderCI.Source          = VSSource;
            using var pVS = pDevice.CreateShader(ShaderCI);

            // Create a pixel shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_PIXEL;
            ShaderCI.EntryPoint      = "main";
            ShaderCI.Desc.Name       = "Cube PS";
            ShaderCI.FilePath        = PSSource;
            using var pPS = pDevice.CreateShader(ShaderCI);

            // Define vertex shader input layout
            // This tutorial uses two types of input: per-vertex data and per-instance data.
            // clang-format off

            // clang-format on

            PSOCreateInfo.pVS = pVS.Obj;
            PSOCreateInfo.pPS = pPS.Obj;

            GraphicsPipeline.InputLayout.LayoutElements = LayoutElements != null ? LayoutElements : DefaultLayoutElems;

            // Define variable type that will be used by default
            ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC;

            // Shader variables should typically be mutable, which means they are expected
            // to change on a per-instance basis
            var Vars = new List<ShaderResourceVariableDesc>
            {
                new ShaderResourceVariableDesc { ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, Name = "g_Texture", Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_MUTABLE}
            };
            ResourceLayout.Variables    = Vars;

            // Define immutable sampler for g_Texture. Immutable samplers should be used whenever possible
            var SamLinearClampDesc = new SamplerDesc();
            var ImtblSamplers = new List<ImmutableSamplerDesc>
            {
                new ImmutableSamplerDesc{ ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, SamplerOrTextureName = "g_Texture", Desc = SamLinearClampDesc}
            };
            ResourceLayout.ImmutableSamplers    = ImtblSamplers;

            return pDevice.CreateGraphicsPipelineState(PSOCreateInfo);
        }

        void CreateCubePSO()
        {
            // Define vertex shader input layout
            var LayoutElems = new List<LayoutElement>
            {
                // Attribute 0 - vertex position
                new LayoutElement{InputIndex = 0, BufferSlot = 0, NumComponents = 3, ValueType = VALUE_TYPE.VT_FLOAT32, IsNormalized = false},
                // Attribute 1 - texture coordinates
                new LayoutElement{InputIndex = 1, BufferSlot = 0, NumComponents = 2, ValueType = VALUE_TYPE.VT_FLOAT32, IsNormalized = false},
                // Attribute 2 - normal
                new LayoutElement{InputIndex = 2, BufferSlot = 0, NumComponents = 3, ValueType = VALUE_TYPE.VT_FLOAT32, IsNormalized = false},
            };

            m_pCubePSO = CreateTexCubePipelineState(m_pDevice,
                                                    m_pSwapChain.GetDesc_ColorBufferFormat,
                                                    m_pSwapChain.GetDesc_DepthBufferFormat,
                                                    shaderLoader.LoadShader("cube.vsh"),
                                                    shaderLoader.LoadShader("cube.psh"),
                                                    LayoutElems);

            // Since we did not explcitly specify the type for 'Constants' variable, default
            // type (SHADER_RESOURCE_VARIABLE_TYPE_STATIC) will be used. Static variables never
            // change and are bound directly through the pipeline state object.
            m_pCubePSO.Obj.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_VERTEX, "Constants").Set(m_VSConstants.Obj);

            //// Since we are using mutable variable, we must create a shader resource binding object
            //// http://diligentgraphics.com/2016/03/23/resource-binding-model-in-diligent-engine-2-0/
            //m_pCubePSO->CreateShaderResourceBinding(&m_CubeSRB, true);


            //// Create shadow pass PSO
            //GraphicsPipelineStateCreateInfo PSOCreateInfo;

            //PSOCreateInfo.PSODesc.Name = "Cube shadow PSO";

            //// This is a graphics pipeline
            //PSOCreateInfo.PSODesc.PipelineType = PIPELINE_TYPE_GRAPHICS;

            //// clang-format off
            //// Shadow pass doesn't use any render target outputs
            //PSOCreateInfo.GraphicsPipeline.NumRenderTargets             = 0;
            //PSOCreateInfo.GraphicsPipeline.RTVFormats[0]                = TEX_FORMAT_UNKNOWN;
            //// The DSV format is the shadow map format
            //PSOCreateInfo.GraphicsPipeline.DSVFormat                    = m_ShadowMapFormat;
            //PSOCreateInfo.GraphicsPipeline.PrimitiveTopology            = PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
            //// Cull back faces
            //PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode      = CULL_MODE_BACK;
            //// Enable depth testing
            //PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthEnable = True;
            //// clang-format on

            //ShaderCreateInfo ShaderCI;
            //ShaderCI.pShaderSourceStreamFactory = pShaderSourceFactory;
            //// Tell the system that the shader source code is in HLSL.
            //// For OpenGL, the engine will convert this into GLSL under the hood.
            //ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE_HLSL;
            //// OpenGL backend requires emulated combined HLSL texture samplers (g_Texture + g_Texture_sampler combination)
            //ShaderCI.UseCombinedTextureSamplers = true;
            //// Create shadow vertex shader
            //RefCntAutoPtr<IShader> pShadowVS;
            //{
            //    ShaderCI.Desc.ShaderType = SHADER_TYPE_VERTEX;
            //    ShaderCI.EntryPoint      = "main";
            //    ShaderCI.Desc.Name       = "Cube Shadow VS";
            //    ShaderCI.FilePath        = "cube_shadow.vsh";
            //    m_pDevice->CreateShader(ShaderCI, &pShadowVS);
            //}
            //PSOCreateInfo.pVS = pShadowVS;

            //// We don't use pixel shader as we are only interested in populating the depth buffer
            //PSOCreateInfo.pPS = nullptr;

            //PSOCreateInfo.GraphicsPipeline.InputLayout.LayoutElements = LayoutElems;
            //PSOCreateInfo.GraphicsPipeline.InputLayout.NumElements    = _countof(LayoutElems);

            //PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE_STATIC;

            //if (m_pDevice->GetDeviceCaps().Features.DepthClamp)
            //{
            //    // Disable depth clipping to render objects that are closer than near
            //    // clipping plane. This is not required for this tutorial, but real applications
            //    // will most likely want to do this.
            //    PSOCreateInfo.GraphicsPipeline.RasterizerDesc.DepthClipEnable = False;
            //}

            //m_pDevice->CreateGraphicsPipelineState(PSOCreateInfo, &m_pCubeShadowPSO);
            //m_pCubeShadowPSO->GetStaticVariableByName(SHADER_TYPE_VERTEX, "Constants")->Set(m_VSConstants);
            //m_pCubeShadowPSO->CreateShaderResourceBinding(&m_CubeShadowSRB, true);
        }

        public void Dispose()
        {
            m_pCubePSO.Dispose();
            m_VSConstants.Dispose();
        }

        public void exceededMaxDelta()
        {

        }

        public void loopStarting()
        {

        }

        public unsafe void sendUpdate(Clock clock)
        {
            var pRTV = m_pSwapChain.GetCurrentBackBufferRTV();
            var pDSV = m_pSwapChain.GetDepthBufferDSV();
            var preTransform = m_pSwapChain.GetDesc_PreTransform;
            m_pImmediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            // Clear the back buffer
            var ClearColor = new Engine.Color(0.350f, 0.350f, 0.350f, 1.0f);

            // Clear the back buffer
            // Let the engine perform required state transitions
            m_pImmediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            m_pImmediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            

            this.m_pSwapChain.Present(1);
        }

        const String VSSource =
    @"cbuffer Constants
{
    float4x4 g_WorldViewProj;
};

// Vertex shader takes two inputs: vertex position and uv coordinates.
// By convention, Diligent Engine expects vertex shader inputs to be 
// labeled 'ATTRIBn', where n is the attribute number.
struct VSInput
{
    float3 Pos : ATTRIB0;
    float2 UV  : ATTRIB1;
};

struct PSInput 
{ 
    float4 Pos : SV_POSITION; 
    float2 UV  : TEX_COORD; 
};

// Note that if separate shader objects are not supported (this is only the case for old GLES3.0 devices), vertex
// shader output variable name must match exactly the name of the pixel shader input variable.
// If the variable has structure type (like in this example), the structure declarations must also be indentical.
void main(in  VSInput VSIn,
          out PSInput PSIn) 
{
    PSIn.Pos = mul( float4(VSIn.Pos,1.0), g_WorldViewProj);
    PSIn.UV  = VSIn.UV;
}
";

        const String PSSource =
    @"Texture2D    g_Texture;
SamplerState g_Texture_sampler; // By convention, texture samplers must use the '_sampler' suffix

struct PSInput 
{ 
    float4 Pos : SV_POSITION; 
    float2 UV : TEX_COORD; 
};

struct PSOutput
{
    float4 Color : SV_TARGET;
};

void main(in  PSInput  PSIn,
          out PSOutput PSOut)
{
    PSOut.Color = g_Texture.Sample(g_Texture_sampler, PSIn.UV); 
}
";
    }
}
