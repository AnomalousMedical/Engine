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
using System.IO;

namespace DiligentEngineCube
{
    class TextureUpdateListener : UpdateListener, IDisposable
    {
        private readonly GraphicsEngine graphicsEngine;
        private readonly NativeOSWindow window;
        private readonly TextureLoader textureLoader;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext m_pImmediateContext;

        private AutoPtr<IPipelineState> m_pPSO;
        private AutoPtr<IBuffer> m_VSConstants;
        private AutoPtr<IBuffer> m_CubeVertexBuffer;
        private AutoPtr<IBuffer> m_CubeIndexBuffer;
        private AutoPtr<IShaderResourceBinding> m_SRB;

        public unsafe TextureUpdateListener(GraphicsEngine graphicsEngine, NativeOSWindow window, TextureLoader textureLoader)
        {
            this.graphicsEngine = graphicsEngine;
            this.window = window;
            this.textureLoader = textureLoader;
            this.swapChain = graphicsEngine.SwapChain;
            this.m_pImmediateContext = graphicsEngine.ImmediateContext;

            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pSwapChain = graphicsEngine.SwapChain;

            // Pipeline state object encompasses configuration of all GPU stages

            GraphicsPipelineStateCreateInfo PSOCreateInfo = new GraphicsPipelineStateCreateInfo();

            // Pipeline state name is used by the engine to report issues.
            // It is always a good idea to give objects descriptive names.
            PSOCreateInfo.PSODesc.Name = "Cube PSO";

            // This is a graphics pipeline
            PSOCreateInfo.PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_GRAPHICS;

            // clang-format off
            // This tutorial will render to a single render target
            PSOCreateInfo.GraphicsPipeline.NumRenderTargets = 1;
            // Set render target format which is the format of the swap chain's color buffer
            PSOCreateInfo.GraphicsPipeline.RTVFormats_0 = m_pSwapChain.GetDesc_ColorBufferFormat;
            // Set depth buffer format which is the format of the swap chain's back buffer
            PSOCreateInfo.GraphicsPipeline.DSVFormat = m_pSwapChain.GetDesc_DepthBufferFormat;
            // Primitive topology defines what kind of primitives will be rendered by this pipeline state
            PSOCreateInfo.GraphicsPipeline.PrimitiveTopology = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
            // Cull back faces
            PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE.CULL_MODE_BACK;
            // Enable depth testing
            PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthEnable = true;
            // clang-format on

            ShaderCreateInfo ShaderCI = new ShaderCreateInfo();
            // Tell the system that the shader source code is in HLSL.
            // For OpenGL, the engine will convert this into GLSL under the hood.
            ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;

            // OpenGL backend requires emulated combined HLSL texture samplers (g_Texture + g_Texture_sampler combination)
            ShaderCI.UseCombinedTextureSamplers = true;

            // Create a vertex shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_VERTEX;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Cube VS";
            ShaderCI.Source = VSSource;
            using var pVS = m_pDevice.CreateShader(ShaderCI);
            // Create dynamic uniform buffer that will store our transformation matrix
            // Dynamic buffers can be frequently updated by the CPU

            BufferDesc CBDesc = new BufferDesc();
            CBDesc.Name = "VS constants CB";
            CBDesc.uiSizeInBytes = (uint)sizeof(Matrix4x4);
            CBDesc.Usage = USAGE.USAGE_DYNAMIC;
            CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
            CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;

            m_VSConstants = m_pDevice.CreateBuffer(CBDesc);

            // Create a pixel shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_PIXEL;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Cube PS";
            ShaderCI.Source = PSSource;
            //ShaderCI.FilePath = "cube.psh";
            using var pPS = m_pDevice.CreateShader(ShaderCI);

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
                    NumComponents = 2,
                    ValueType = VALUE_TYPE.VT_FLOAT32,
                    IsNormalized = false
                },
            };

            PSOCreateInfo.pVS = pVS.Obj;
            PSOCreateInfo.pPS = pPS.Obj;

            PSOCreateInfo.GraphicsPipeline.InputLayout.LayoutElements = LayoutElems;

            // Define variable type that will be used by default
            PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC;

            // clang-format off
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
            // clang-format on
            PSOCreateInfo.PSODesc.ResourceLayout.ImmutableSamplers = ImtblSamplers;

            m_pPSO = m_pDevice.CreateGraphicsPipelineState(PSOCreateInfo);

            // Since we did not explcitly specify the type for 'Constants' variable, default
            // type (SHADER_RESOURCE_VARIABLE_TYPE_STATIC) will be used. Static variables
            // never change and are bound directly through the pipeline state object.
            m_pPSO.Obj.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_VERTEX, "Constants").Set(m_VSConstants.Obj);

            // Since we are using mutable variable, we must create a shader resource binding object
            // http://diligentgraphics.com/2016/03/23/resource-binding-model-in-diligent-engine-2-0/
            m_SRB = m_pPSO.Obj.CreateShaderResourceBinding(true);

            CreateVertexBuffer();
            CreateIndexBuffer();
            LoadTexture();
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vertex
        {
            public Vector3 pos;
            public Vector2 uv;
        };

        unsafe void CreateVertexBuffer()
        {
            var m_pDevice = graphicsEngine.RenderDevice;

            // Cube vertices

            //      (-1,+1,+1)________________(+1,+1,+1)
            //               /|              /|
            //              / |             / |
            //             /  |            /  |
            //            /   |           /   |
            //(-1,-1,+1) /____|__________/(+1,-1,+1)
            //           |    |__________|____|
            //           |   /(-1,+1,-1) |    /(+1,+1,-1)
            //           |  /            |   /
            //           | /             |  /
            //           |/              | /
            //           /_______________|/
            //        (-1,-1,-1)       (+1,-1,-1)
            //

            // clang-format off
            // This time we have to duplicate verices because texture coordinates cannot
            // be shared
            var CubeVerts = new Vertex[]
            {
                new Vertex{pos = new Vector3(-1,-1,-1), uv = new Vector2(0,1)},
                new Vertex{pos = new Vector3(-1,+1,-1), uv = new Vector2(0,0)},
                new Vertex{pos = new Vector3(+1,+1,-1), uv = new Vector2(1,0)},
                new Vertex{pos = new Vector3(+1,-1,-1), uv = new Vector2(1,1)},

                new Vertex{pos = new Vector3(-1,-1,-1), uv = new Vector2(0,1)},
                new Vertex{pos = new Vector3(-1,-1,+1), uv = new Vector2(0,0)},
                new Vertex{pos = new Vector3(+1,-1,+1), uv = new Vector2(1,0)},
                new Vertex{pos = new Vector3(+1,-1,-1), uv = new Vector2(1,1)},

                new Vertex{pos = new Vector3(+1,-1,-1), uv = new Vector2(0,1)},
                new Vertex{pos = new Vector3(+1,-1,+1), uv = new Vector2(1,1)},
                new Vertex{pos = new Vector3(+1,+1,+1), uv = new Vector2(1,0)},
                new Vertex{pos = new Vector3(+1,+1,-1), uv = new Vector2(0,0)},

                new Vertex{pos = new Vector3(+1,+1,-1), uv = new Vector2(0,1)},
                new Vertex{pos = new Vector3(+1,+1,+1), uv = new Vector2(0,0)},
                new Vertex{pos = new Vector3(-1,+1,+1), uv = new Vector2(1,0)},
                new Vertex{pos = new Vector3(-1,+1,-1), uv = new Vector2(1,1)},

                new Vertex{pos = new Vector3(-1,+1,-1), uv = new Vector2(1,0)},
                new Vertex{pos = new Vector3(-1,+1,+1), uv = new Vector2(0,0)},
                new Vertex{pos = new Vector3(-1,-1,+1), uv = new Vector2(0,1)},
                new Vertex{pos = new Vector3(-1,-1,-1), uv = new Vector2(1,1)},

                new Vertex{pos = new Vector3(-1,-1,+1), uv = new Vector2(1,1)},
                new Vertex{pos = new Vector3(+1,-1,+1), uv = new Vector2(0,1)},
                new Vertex{pos = new Vector3(+1,+1,+1), uv = new Vector2(0,0)},
                new Vertex{pos = new Vector3(-1,+1,+1), uv = new Vector2(1,0)}
            };


            // Create a vertex buffer that stores cube vertices
            BufferDesc VertBuffDesc = new BufferDesc();
            VertBuffDesc.Name = "Cube vertex buffer";
            VertBuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
            VertBuffDesc.BindFlags = BIND_FLAGS.BIND_VERTEX_BUFFER;
            VertBuffDesc.uiSizeInBytes = (uint)(sizeof(Vertex) * CubeVerts.Length);
            BufferData VBData = new BufferData();
            fixed (Vertex* vertices = CubeVerts)
            {
                VBData.pData = new IntPtr(vertices);
                VBData.DataSize = (uint)(sizeof(Vertex) * CubeVerts.Length);
                m_CubeVertexBuffer = m_pDevice.CreateBuffer(VertBuffDesc, VBData);
            }
        }

        unsafe void CreateIndexBuffer()
        {
            var m_pDevice = graphicsEngine.RenderDevice;

            // clang-format off
            var Indices = new UInt32[]
            {
                2,0,1,    2,3,0,
                4,6,5,    4,7,6,
                8,10,9,   8,11,10,
                12,14,13, 12,15,14,
                16,18,17, 16,19,18,
                20,21,22, 20,22,23
            };
            // clang-format on

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
                m_CubeIndexBuffer = m_pDevice.CreateBuffer(IndBuffDesc, IBData);
            }
        }

        void LoadTexture()
        {
            var logo = Path.GetFullPath("assets/AnomalousEngine.png");
            Console.WriteLine(logo);

            using var stream = File.Open(logo, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var Tex = textureLoader.LoadTexture(stream, "Cube Texture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D, true);
            // Get shader resource view from the texture
            var m_TextureSRV = Tex.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);

            // Set texture SRV in the SRB
            m_SRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "g_Texture").Set(m_TextureSRV);
        }

        public void Dispose()
        {
            m_CubeIndexBuffer.Dispose();
            m_CubeVertexBuffer.Dispose();
            m_SRB.Dispose();
            m_pPSO.Dispose();
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
            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            var preTransform = swapChain.GetDesc_PreTransform;
            m_pImmediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            // Clear the back buffer
            var ClearColor = new Engine.Color(0.350f, 0.350f, 0.350f, 1.0f);

            // Clear the back buffer
            // Let the engine perform required state transitions
            m_pImmediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            m_pImmediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            {
                var trans = new Vector3(0, 0, 0);
                var rot = new Quaternion(clock.CurrentTimeMicro * Clock.MicroToSeconds % (2 * (float)Math.PI), 0f, 0f);

                var CubeModelTransform = rot.toRotationMatrix4x4(trans);

                var View = Matrix4x4.Translation(0f, 0f, 5f);

                // Get pretransform matrix that rotates the scene according the surface orientation
                var SrfPreTransform = CameraHelpers.GetSurfacePretransformMatrix(new Vector3(0, 0, 1), preTransform);

                // Get projection matrix adjusted to the current screen orientation
                var Proj = CameraHelpers.GetAdjustedProjectionMatrix((float)Math.PI / 4.0f, 0.1f, 100f, window.WindowWidth, window.WindowHeight, preTransform);

                // Compute world-view-projection matrix
                var m_WorldViewProjMatrix = CubeModelTransform * View * SrfPreTransform * Proj;

                // Map the buffer and write current world-view-projection matrix
                IntPtr data = m_pImmediateContext.MapBuffer(m_VSConstants.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);
                Matrix4x4* viewProjMat = (Matrix4x4*)data.ToPointer();
                //Mat is d3d, ogre style row major, need to transpose to send to diligent
                viewProjMat[0] = m_WorldViewProjMatrix.Transpose();
                m_pImmediateContext.UnmapBuffer(m_VSConstants.Obj, MAP_TYPE.MAP_WRITE);
            }

            // Bind vertex and index buffers
            UInt32[] offset = new UInt32[] { 0 };
            IBuffer[] pBuffs = new IBuffer[] { m_CubeVertexBuffer.Obj };
            m_pImmediateContext.SetVertexBuffers(0, 1, pBuffs, offset, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION, SET_VERTEX_BUFFERS_FLAGS.SET_VERTEX_BUFFERS_FLAG_RESET);
            m_pImmediateContext.SetIndexBuffer(m_CubeIndexBuffer.Obj, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            // Set the pipeline state
            m_pImmediateContext.SetPipelineState(m_pPSO.Obj);
            // Commit shader resources. RESOURCE_STATE_TRANSITION_MODE_TRANSITION mode
            // makes sure that resources are transitioned to required states.
            m_pImmediateContext.CommitShaderResources(m_SRB.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            DrawIndexedAttribs DrawAttrs = new DrawIndexedAttribs();     // This is an indexed draw call
            DrawAttrs.IndexType = VALUE_TYPE.VT_UINT32; // Index type
            DrawAttrs.NumIndices = 36;
            // Verify the state of vertex and index buffers
            DrawAttrs.Flags = DRAW_FLAGS.DRAW_FLAG_VERIFY_ALL;
            m_pImmediateContext.DrawIndexed(DrawAttrs);

            this.swapChain.Present(1);
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
