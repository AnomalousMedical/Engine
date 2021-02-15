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
using Engine.CameraMovement;

namespace Tutorial13_ShadowMap
{
    class ShadowMapUpdateListener : UpdateListener, IDisposable
    {
        private readonly GraphicsEngine graphicsEngine;
        private readonly NativeOSWindow window;
        private readonly TextureLoader textureLoader;
        private readonly ShaderLoader<ShadowMapUpdateListener> shaderLoader;
        private readonly VirtualFileSystem virtualFileSystem;
        private readonly FirstPersonFlyCamera firstPersonFlyCamera;
        private readonly ISwapChain m_pSwapChain;
        private readonly IDeviceContext m_pImmediateContext;
        private readonly IRenderDevice m_pDevice;

        private AutoPtr<IPipelineState> m_pCubePSO;
        private AutoPtr<IPipelineState> m_pCubeShadowPSO;
        private AutoPtr<IPipelineState> m_pPlanePSO;
        private AutoPtr<IPipelineState> m_pShadowMapVisPSO;
        private AutoPtr<IBuffer> m_CubeVertexBuffer;
        private AutoPtr<IBuffer> m_CubeIndexBuffer;
        private AutoPtr<IBuffer> m_VSConstants;
        private AutoPtr<IShaderResourceBinding> m_CubeSRB;
        private AutoPtr<IShaderResourceBinding> m_CubeShadowSRB;
        private AutoPtr<IShaderResourceBinding> m_PlaneSRB;
        private AutoPtr<IShaderResourceBinding> m_ShadowMapVisSRB;
        private AutoPtr<ITextureView> m_ShadowMapDSV;
        private AutoPtr<ITextureView> m_ShadowMapSRV;

        float4x4 m_CubeWorldMatrix = float4x4.Identity;
        float4x4 m_CameraViewProjMatrix;
        float3 m_LightDirection = new float3(-0.49f, -0.60f, 0.64f).normalized();
        float4x4 m_WorldToShadowMapUVDepthMatr;
        Uint32 m_ShadowMapSize = 512;

        TEXTURE_FORMAT m_ShadowMapFormat = TEXTURE_FORMAT.TEX_FORMAT_D16_UNORM;

        static readonly List<LayoutElement> DefaultLayoutElems = new List<LayoutElement>
        {
            // Per-vertex data - first buffer slot
            // Attribute 0 - vertex position
            new LayoutElement{InputIndex = 0, BufferSlot = 0, NumComponents = 3, ValueType = VALUE_TYPE.VT_FLOAT32, IsNormalized = false},
            // Attribute 1 - texture coordinates
            new LayoutElement{InputIndex = 1, BufferSlot = 0, NumComponents = 2, ValueType = VALUE_TYPE.VT_FLOAT32, IsNormalized = false}
        };

        // Layout of this structure matches the one we defined in pipeline state
        [StructLayout(LayoutKind.Sequential)]
        struct Vertex
        {
            public float3 pos;
            public float2 uv;
            public float3 normal;
        };

        [StructLayout(LayoutKind.Sequential)]
        struct CubeConstants //This is meant to be the same as PlaneConstants
        {
            public float4x4 WorldViewProj;
            public float4x4 NormalTranform;
            public float4 LightDirection;
        };

        [StructLayout(LayoutKind.Sequential)]
        struct PlaneConstants //This is meant to be the same as CubeConstants
        {
            public float4x4 CameraViewProj;
            public float4x4 WorldToShadowMapUVDepth;
            public float4 LightDirection;
        };

        public unsafe ShadowMapUpdateListener(
            GraphicsEngine graphicsEngine
            ,NativeOSWindow window
            ,TextureLoader textureLoader
            ,ShaderLoader<ShadowMapUpdateListener> shaderLoader
            ,VirtualFileSystem virtualFileSystem
            ,FirstPersonFlyCamera firstPersonFlyCamera)
        {
            this.graphicsEngine = graphicsEngine;
            this.window = window;
            this.textureLoader = textureLoader;
            this.shaderLoader = shaderLoader;
            this.virtualFileSystem = virtualFileSystem;
            this.firstPersonFlyCamera = firstPersonFlyCamera;
            this.m_pSwapChain = graphicsEngine.SwapChain;
            this.m_pImmediateContext = graphicsEngine.ImmediateContext;
            this.m_pDevice = graphicsEngine.RenderDevice;

            firstPersonFlyCamera.Position = new float3(0, 0, -12);

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
                Barriers.Add(new StateTransitionDesc(){pResource = m_VSConstants.Obj, OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_CONSTANT_BUFFER, UpdateResourceState = true });
            }

            CreateCubePSO();
            CreatePlanePSO();
            CreateShadowMapVisPSO();

            //// Load cube

            // In this tutorial we need vertices with normals
            CreateVertexBuffer();
            // Load index buffer
            m_CubeIndexBuffer = CreateIndexBuffer(m_pDevice);
            // Explicitly transition vertex and index buffers to required states
            Barriers.Add(new StateTransitionDesc() { pResource = m_CubeVertexBuffer.Obj, OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_VERTEX_BUFFER, UpdateResourceState = true });
            Barriers.Add(new StateTransitionDesc() { pResource = m_CubeIndexBuffer.Obj, OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_INDEX_BUFFER, UpdateResourceState = true });
            // Load texture
            using var textureStream = virtualFileSystem.openStream("AnomalousEngine.png", Engine.Resources.FileMode.Open);
            using var CubeTexture = textureLoader.LoadTexture(textureStream, "Cube Texture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D, true);
            //auto CubeTexture = TexturedCube::LoadTexture(m_pDevice, "DGLogo.png");
            m_CubeSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "g_Texture").Set(CubeTexture.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));
            // Transition the texture to shader resource state
            Barriers.Add(new StateTransitionDesc() { pResource = CubeTexture.Obj, OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_SHADER_RESOURCE, UpdateResourceState = true });

            CreateShadowMap();

            m_pImmediateContext.TransitionResourceStates(Barriers);
        }

        private static AutoPtr<IPipelineState> CreateTexCubePipelineState(IRenderDevice pDevice,
                                                  TEXTURE_FORMAT RTVFormat,
                                                  TEXTURE_FORMAT DSVFormat,
                                                  String VSSource,
                                                  String PSSource,
                                                  List<LayoutElement> LayoutElements = null,
                                                  Uint8 SampleCount = 1)
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
            ShaderCI.Source          = PSSource;
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

            // Since we are using mutable variable, we must create a shader resource binding object
            // http://diligentgraphics.com/2016/03/23/resource-binding-model-in-diligent-engine-2-0/
            m_CubeSRB = m_pCubePSO.Obj.CreateShaderResourceBinding(true);


            // Create shadow pass PSO
            var PSOCreateInfo = new GraphicsPipelineStateCreateInfo();

            PSOCreateInfo.PSODesc.Name = "Cube shadow PSO";

            // This is a graphics pipeline
            PSOCreateInfo.PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_GRAPHICS;

            // clang-format off
            // Shadow pass doesn't use any render target outputs
            PSOCreateInfo.GraphicsPipeline.NumRenderTargets = 0;
            PSOCreateInfo.GraphicsPipeline.RTVFormats_0 = TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN;
            // The DSV format is the shadow map format
            PSOCreateInfo.GraphicsPipeline.DSVFormat = m_ShadowMapFormat;
            PSOCreateInfo.GraphicsPipeline.PrimitiveTopology = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
            // Cull back faces
            PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE.CULL_MODE_BACK;
            // Enable depth testing
            PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthEnable = true;
            // clang-format on

            var ShaderCI = new ShaderCreateInfo();
            // Tell the system that the shader source code is in HLSL.
            // For OpenGL, the engine will convert this into GLSL under the hood.
            ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;
            // OpenGL backend requires emulated combined HLSL texture samplers (g_Texture + g_Texture_sampler combination)
            ShaderCI.UseCombinedTextureSamplers = true;
            // Create shadow vertex shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_VERTEX;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Cube Shadow VS";
            ShaderCI.Source = shaderLoader.LoadShader("cube_shadow.vsh");
            using var pShadowVS = m_pDevice.CreateShader(ShaderCI);
            PSOCreateInfo.pVS = pShadowVS.Obj;

            // We don't use pixel shader as we are only interested in populating the depth buffer
            PSOCreateInfo.pPS = null;

            PSOCreateInfo.GraphicsPipeline.InputLayout.LayoutElements = LayoutElems;

            PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC;

            //if (m_pDevice.GetDeviceCaps().Features.DepthClamp) //Should look for this, need to wrap
            {
                // Disable depth clipping to render objects that are closer than near
                // clipping plane. This is not required for this tutorial, but real applications
                // will most likely want to do this.
                PSOCreateInfo.GraphicsPipeline.RasterizerDesc.DepthClipEnable = false;
            }

            m_pCubeShadowPSO = m_pDevice.CreateGraphicsPipelineState(PSOCreateInfo);
            m_pCubeShadowPSO.Obj.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_VERTEX, "Constants").Set(m_VSConstants.Obj);
            m_CubeShadowSRB = m_pCubeShadowPSO.Obj.CreateShaderResourceBinding(true);
        }

        void CreatePlanePSO()
        {
            var PSOCreateInfo = new GraphicsPipelineStateCreateInfo();

            // Pipeline state name is used by the engine to report issues.
            // It is always a good idea to give objects descriptive names.
            PSOCreateInfo.PSODesc.Name = "Plane PSO";

            // This is a graphics pipeline
            PSOCreateInfo.PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_GRAPHICS;

            // This tutorial renders to a single render target
            PSOCreateInfo.GraphicsPipeline.NumRenderTargets             = 1;
            // Set render target format which is the format of the swap chain's color buffer
            PSOCreateInfo.GraphicsPipeline.RTVFormats_0                = m_pSwapChain.GetDesc_ColorBufferFormat;
            // Set depth buffer format which is the format of the swap chain's back buffer
            PSOCreateInfo.GraphicsPipeline.DSVFormat                    = m_pSwapChain.GetDesc_DepthBufferFormat;
            // Primitive topology defines what kind of primitives will be rendered by this pipeline state
            PSOCreateInfo.GraphicsPipeline.PrimitiveTopology            = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_STRIP;
            // No cull
            PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode      = CULL_MODE.CULL_MODE_NONE;
            // Enable depth testing
            PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthEnable = true;

            var ShaderCI = new ShaderCreateInfo();
            // Tell the system that the shader source code is in HLSL.
            // For OpenGL, the engine will convert this into GLSL under the hood.
            ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;

            // OpenGL backend requires emulated combined HLSL texture samplers (g_Texture + g_Texture_sampler combination)
            ShaderCI.UseCombinedTextureSamplers = true;

            // Create plane vertex shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_VERTEX;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Plane VS";
            ShaderCI.Source = shaderLoader.LoadShader("plane.vsh");
            using var pPlaneVS = m_pDevice.CreateShader(ShaderCI);

            // Create plane pixel shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_PIXEL;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Plane PS";
            ShaderCI.Source = shaderLoader.LoadShader("plane.psh");
            using var pPlanePS = m_pDevice.CreateShader(ShaderCI);

            PSOCreateInfo.pVS = pPlaneVS.Obj;
            PSOCreateInfo.pPS = pPlanePS.Obj;

            // Define variable type that will be used by default
            PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC;

            // Shader variables should typically be mutable, which means they are expected
            // to change on a per-instance basis
            var Vars = new List<ShaderResourceVariableDesc>
            {
                new ShaderResourceVariableDesc{ ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, Name = "g_ShadowMap", Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_MUTABLE }
            };
            PSOCreateInfo.PSODesc.ResourceLayout.Variables    = Vars;

            // Define immutable comparison sampler for g_ShadowMap. Immutable samplers should be used whenever possible
            var ComparsionSampler = new SamplerDesc();
            ComparsionSampler.ComparisonFunc = COMPARISON_FUNCTION.COMPARISON_FUNC_LESS;
            ComparsionSampler.MinFilter      = FILTER_TYPE.FILTER_TYPE_COMPARISON_LINEAR;
            ComparsionSampler.MagFilter      = FILTER_TYPE.FILTER_TYPE_COMPARISON_LINEAR;
            ComparsionSampler.MipFilter      = FILTER_TYPE.FILTER_TYPE_COMPARISON_LINEAR;
            var ImtblSamplers = new List<ImmutableSamplerDesc>
            {
                new ImmutableSamplerDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, SamplerOrTextureName = "g_ShadowMap", Desc = ComparsionSampler}
            };
            PSOCreateInfo.PSODesc.ResourceLayout.ImmutableSamplers    = ImtblSamplers;

            m_pPlanePSO = m_pDevice.CreateGraphicsPipelineState(PSOCreateInfo);

            // Since we did not explcitly specify the type for 'Constants' variable, default
            // type (SHADER_RESOURCE_VARIABLE_TYPE_STATIC) will be used. Static variables never
            // change and are bound directly through the pipeline state object.
            m_pPlanePSO.Obj.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_VERTEX, "Constants").Set(m_VSConstants.Obj);
        }

        void CreateShadowMapVisPSO()
        {
            var PSOCreateInfo = new GraphicsPipelineStateCreateInfo();

            PSOCreateInfo.PSODesc.Name = "Shadow Map Vis PSO";

            // This is a graphics pipeline
            PSOCreateInfo.PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_GRAPHICS;

            // This tutorial renders to a single render target
            PSOCreateInfo.GraphicsPipeline.NumRenderTargets             = 1;
            // Set render target format which is the format of the swap chain's color buffer
            PSOCreateInfo.GraphicsPipeline.RTVFormats_0                 = m_pSwapChain.GetDesc_ColorBufferFormat;
            // Set depth buffer format which is the format of the swap chain's back buffer
            PSOCreateInfo.GraphicsPipeline.DSVFormat                    = m_pSwapChain.GetDesc_DepthBufferFormat;
            // Primitive topology defines what kind of primitives will be rendered by this pipeline state
            PSOCreateInfo.GraphicsPipeline.PrimitiveTopology            = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_STRIP;
            // No cull
            PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode      = CULL_MODE.CULL_MODE_NONE;
            // Disable depth testing
            PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthEnable = false;

            var ShaderCI = new ShaderCreateInfo();
            // Tell the system that the shader source code is in HLSL.
            // For OpenGL, the engine will convert this into GLSL under the hood.
            ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;

            // OpenGL backend requires emulated combined HLSL texture samplers (g_Texture + g_Texture_sampler combination)
            ShaderCI.UseCombinedTextureSamplers = true;

            // Create shadow map visualization vertex shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_VERTEX;
            ShaderCI.EntryPoint      = "main";
            ShaderCI.Desc.Name       = "Shadow Map Vis VS";
            ShaderCI.Source          = shaderLoader.LoadShader("shadow_map_vis.vsh");
            using var pShadowMapVisVS = m_pDevice.CreateShader(ShaderCI);

            // Create shadow map visualization pixel shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_PIXEL;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name  = "Shadow Map Vis PS";
            ShaderCI.Source     = shaderLoader.LoadShader("shadow_map_vis.psh");
            using var pShadowMapVisPS = m_pDevice.CreateShader(ShaderCI);

            PSOCreateInfo.pVS = pShadowMapVisVS.Obj;
            PSOCreateInfo.pPS = pShadowMapVisPS.Obj;

            // Define variable type that will be used by default
            PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_MUTABLE;

            var SamLinearClampDesc = new SamplerDesc();
            var ImtblSamplers = new List<ImmutableSamplerDesc>
            {
                new ImmutableSamplerDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, SamplerOrTextureName = "g_ShadowMap", Desc = SamLinearClampDesc}
            };
            PSOCreateInfo.PSODesc.ResourceLayout.ImmutableSamplers = ImtblSamplers;

            m_pShadowMapVisPSO = m_pDevice.CreateGraphicsPipelineState(PSOCreateInfo);
        }

        unsafe void CreateVertexBuffer()
        {
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

            var CubeVerts = new Vertex[]
            {
                new Vertex{pos = new float3(-1,-1,-1), uv = new float2(0,1), normal = new float3(0, 0, -1)},
                new Vertex{pos = new float3(-1,+1,-1), uv = new float2(0,0), normal = new float3(0, 0, -1)},
                new Vertex{pos = new float3(+1,+1,-1), uv = new float2(1,0), normal = new float3(0, 0, -1)},
                new Vertex{pos = new float3(+1,-1,-1), uv = new float2(1,1), normal = new float3(0, 0, -1)},

                new Vertex{pos = new float3(-1,-1,-1), uv = new float2(0,1), normal = new float3(0, -1, 0)},
                new Vertex{pos = new float3(-1,-1,+1), uv = new float2(0,0), normal = new float3(0, -1, 0)},
                new Vertex{pos = new float3(+1,-1,+1), uv = new float2(1,0), normal = new float3(0, -1, 0)},
                new Vertex{pos = new float3(+1,-1,-1), uv = new float2(1,1), normal = new float3(0, -1, 0)},

                new Vertex{pos = new float3(+1,-1,-1), uv = new float2(0,1), normal = new float3(+1, 0, 0)},
                new Vertex{pos = new float3(+1,-1,+1), uv = new float2(1,1), normal = new float3(+1, 0, 0)},
                new Vertex{pos = new float3(+1,+1,+1), uv = new float2(1,0), normal = new float3(+1, 0, 0)},
                new Vertex{pos = new float3(+1,+1,-1), uv = new float2(0,0), normal = new float3(+1, 0, 0)},

                new Vertex{pos = new float3(+1,+1,-1), uv = new float2(0,1), normal = new float3(0, +1, 0)},
                new Vertex{pos = new float3(+1,+1,+1), uv = new float2(0,0), normal = new float3(0, +1, 0)},
                new Vertex{pos = new float3(-1,+1,+1), uv = new float2(1,0), normal = new float3(0, +1, 0)},
                new Vertex{pos = new float3(-1,+1,-1), uv = new float2(1,1), normal = new float3(0, +1, 0)},

                new Vertex{pos = new float3(-1,+1,-1), uv = new float2(1,0), normal = new float3(-1, 0, 0)},
                new Vertex{pos = new float3(-1,+1,+1), uv = new float2(0,0), normal = new float3(-1, 0, 0)},
                new Vertex{pos = new float3(-1,-1,+1), uv = new float2(0,1), normal = new float3(-1, 0, 0)},
                new Vertex{pos = new float3(-1,-1,-1), uv = new float2(1,1), normal = new float3(-1, 0, 0)},

                new Vertex{pos = new float3(-1,-1,+1), uv = new float2(1,1), normal = new float3(0, 0, +1)},
                new Vertex{pos = new float3(+1,-1,+1), uv = new float2(0,1), normal = new float3(0, 0, +1)},
                new Vertex{pos = new float3(+1,+1,+1), uv = new float2(0,0), normal = new float3(0, 0, +1)},
                new Vertex{pos = new float3(-1,+1,+1), uv = new float2(1,0), normal = new float3(0, 0, +1)}
            };

            var VertBuffDesc = new BufferDesc();
            VertBuffDesc.Name          = "Cube vertex buffer";
            VertBuffDesc.Usage         = USAGE.USAGE_IMMUTABLE;
            VertBuffDesc.BindFlags     = BIND_FLAGS.BIND_VERTEX_BUFFER;
            VertBuffDesc.uiSizeInBytes = (uint)(sizeof(Vertex) * CubeVerts.Length);
            fixed (Vertex* vertices = CubeVerts)
            {
                var VBData = new BufferData();
                VBData.pData = new IntPtr(vertices);
                VBData.DataSize = (uint)(sizeof(Vertex) * CubeVerts.Length);
                m_CubeVertexBuffer = m_pDevice.CreateBuffer(VertBuffDesc, VBData);
            }
        }

        unsafe AutoPtr<IBuffer> CreateIndexBuffer(IRenderDevice m_pDevice)
        {
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
                return m_pDevice.CreateBuffer(IndBuffDesc, IBData);
            }
        }

        void CreateShadowMap()
        {
            var SMDesc = new TextureDesc();
            SMDesc.Name = "Shadow map";
            SMDesc.Type = RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D;
            SMDesc.Width = m_ShadowMapSize;
            SMDesc.Height = m_ShadowMapSize;
            SMDesc.Format = m_ShadowMapFormat;
            SMDesc.BindFlags = BIND_FLAGS.BIND_SHADER_RESOURCE | BIND_FLAGS.BIND_DEPTH_STENCIL;
            using var ShadowMap = m_pDevice.CreateTexture(SMDesc, null);
            m_ShadowMapSRV = new AutoPtr<ITextureView>(ShadowMap.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));
            m_ShadowMapDSV = new AutoPtr<ITextureView>(ShadowMap.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_DEPTH_STENCIL));

            // Create SRBs that use shadow map as mutable variable
            m_PlaneSRB = m_pPlanePSO.Obj.CreateShaderResourceBinding(true);
            m_PlaneSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "g_ShadowMap").Set(m_ShadowMapSRV.Obj);

            m_ShadowMapVisSRB = m_pShadowMapVisPSO.Obj.CreateShaderResourceBinding(true);
            m_ShadowMapVisSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "g_ShadowMap").Set(m_ShadowMapSRV.Obj);
        }

        public void Dispose()
        {
            m_ShadowMapVisSRB.Dispose();
            m_PlaneSRB.Dispose();
            m_ShadowMapDSV.Dispose();
            m_ShadowMapSRV.Dispose();
            m_CubeIndexBuffer.Dispose();
            m_CubeVertexBuffer.Dispose();
            m_pShadowMapVisPSO.Dispose();
            m_pPlanePSO.Dispose();
            m_CubeShadowSRB.Dispose();
            m_pCubeShadowPSO.Dispose();
            m_CubeSRB.Dispose();
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
            firstPersonFlyCamera.UpdateInput(clock);

            // Render shadow map
            m_pImmediateContext.SetRenderTargets(null, m_ShadowMapDSV.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            m_pImmediateContext.ClearDepthStencil(m_ShadowMapDSV.Obj, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            RenderShadowMap();

            // Bind main back buffer
            var pRTV = m_pSwapChain.GetCurrentBackBufferRTV();
            var pDSV = m_pSwapChain.GetDepthBufferDSV();
            var preTransform = m_pSwapChain.GetDesc_PreTransform;
            m_pImmediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            var ClearColor = new Engine.Color(0.350f, 0.350f, 0.350f, 1.0f);
            m_pImmediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            m_pImmediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            // Get projection matrix adjusted to the current screen orientation
            var SrfPreTransform = CameraHelpers.GetSurfacePretransformMatrix(new Vector3(0, 0, 1), preTransform);
            var Proj = CameraHelpers.GetAdjustedProjectionMatrix((float)Math.PI / 4.0f, 0.1f, 100.0f, window.WindowWidth, window.WindowHeight, preTransform);

            var trans = Matrix4x4.Translation(-firstPersonFlyCamera.Position);
            var rot = firstPersonFlyCamera.Orientation.toRotationMatrix4x4();

            // Compute camera view-projection matrix
            m_CameraViewProjMatrix = trans * rot * SrfPreTransform * Proj;

            RenderCube(ref m_CameraViewProjMatrix, false);
            RenderPlane();
            RenderShadowMapVis();

            this.m_pSwapChain.Present(1);
        }

        void RenderShadowMap()
        {
            float3 f3LightSpaceX, f3LightSpaceY, f3LightSpaceZ;
            f3LightSpaceZ = m_LightDirection.normalized();

            var min_cmp = Math.Min(Math.Min(Math.Abs(m_LightDirection.x), Math.Abs(m_LightDirection.y)), Math.Abs(m_LightDirection.z));
            if (min_cmp == Math.Abs(m_LightDirection.x))
                f3LightSpaceX = new float3(1, 0, 0);
            else if (min_cmp == Math.Abs(m_LightDirection.y))
                f3LightSpaceX = new float3(0, 1, 0);
            else
                f3LightSpaceX = new float3(0, 0, 1);

            f3LightSpaceY = f3LightSpaceZ.cross(ref f3LightSpaceX);
            f3LightSpaceX = f3LightSpaceY.cross(ref f3LightSpaceZ);
            f3LightSpaceX = f3LightSpaceX.normalized();
            f3LightSpaceY = f3LightSpaceY.normalized();

            float4x4 WorldToLightViewSpaceMatr = float4x4.ViewFromBasis(ref f3LightSpaceX, ref f3LightSpaceY, ref f3LightSpaceZ);

            // For this tutorial we know that the scene center is at (0,0,0).
            // Real applications will want to compute tight bounds

            float3 f3SceneCenter = new float3(0, 0, 0);
            float SceneRadius = (float)Math.Sqrt(3);
            float3 f3MinXYZ = f3SceneCenter - new float3(SceneRadius, SceneRadius, SceneRadius);
            float3 f3MaxXYZ = f3SceneCenter + new float3(SceneRadius, SceneRadius, SceneRadius * 5);
            float3 f3SceneExtent = f3MaxXYZ - f3MinXYZ;

            //var DevCaps = m_pDevice->GetDeviceCaps();
            bool IsGL = false;// DevCaps.IsGLDevice();
            float4 f4LightSpaceScale;
            f4LightSpaceScale.x = 2.0f / f3SceneExtent.x;
            f4LightSpaceScale.y = 2.0f / f3SceneExtent.y;
            f4LightSpaceScale.z = (IsGL ? 2.0f : 1.0f) / f3SceneExtent.z;
            // Apply bias to shift the extent to [-1,1]x[-1,1]x[0,1] for DX or to [-1,1]x[-1,1]x[-1,1] for GL
            // Find bias such that f3MinXYZ -> (-1,-1,0) for DX or (-1,-1,-1) for GL
            float4 f4LightSpaceScaledBias;
            f4LightSpaceScaledBias.x = -f3MinXYZ.x * f4LightSpaceScale.x - 1.0f;
            f4LightSpaceScaledBias.y = -f3MinXYZ.y * f4LightSpaceScale.y - 1.0f;
            f4LightSpaceScaledBias.z = -f3MinXYZ.z * f4LightSpaceScale.z + (IsGL ? -1.0f : 0.0f);

            float4x4 ScaleMatrix = float4x4.Scale(f4LightSpaceScale.x, f4LightSpaceScale.y, f4LightSpaceScale.z);
            float4x4 ScaledBiasMatrix = float4x4.Translation(f4LightSpaceScaledBias.x, f4LightSpaceScaledBias.y, f4LightSpaceScaledBias.z);

            // Note: bias is applied after scaling!
            float4x4 ShadowProjMatr = ScaleMatrix * ScaledBiasMatrix;

            // Adjust the world to light space transformation matrix
            float4x4 WorldToLightProjSpaceMatr = WorldToLightViewSpaceMatr * ShadowProjMatr;

            var NDCAttribs = m_pDevice.GetDeviceCaps_GetNDCAttribs(); //If this does not have to be called every frame avoid it, its very slow
            float4x4 ProjToUVScale = float4x4.Scale(0.5f, NDCAttribs.YtoVScale, NDCAttribs.ZtoDepthScale);
            float4x4 ProjToUVBias = float4x4.Translation(0.5f, 0.5f, NDCAttribs.GetZtoDepthBias());

            m_WorldToShadowMapUVDepthMatr = WorldToLightProjSpaceMatr * ProjToUVScale * ProjToUVBias;

            RenderCube(ref WorldToLightProjSpaceMatr, true);
        }

        void RenderCube(ref float4x4 CameraViewProj, bool IsShadowPass)
        {
            // Update constant buffer
            unsafe
            {
                // Map the buffer and write current world-view-projection matrix
                IntPtr data = m_pImmediateContext.MapBuffer(m_VSConstants.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);
                CubeConstants* CBConstants = (CubeConstants*)data.ToPointer();
                CBConstants->WorldViewProj = (m_CubeWorldMatrix * CameraViewProj).Transpose();
                var NormalMatrix = m_CubeWorldMatrix.RemoveTranslation().inverse();
                // We need to do inverse-transpose, but we also need to transpose the matrix
                // before writing it to the buffer
                CBConstants->NormalTranform = NormalMatrix;
                CBConstants->LightDirection = new float4(m_LightDirection.x, m_LightDirection.y, m_LightDirection.z, 0);
                m_pImmediateContext.UnmapBuffer(m_VSConstants.Obj, MAP_TYPE.MAP_WRITE);
            }

            // Bind vertex buffer
            var pBuffs =  new IBuffer[] { m_CubeVertexBuffer.Obj };
            // Note that since resouces have been explicitly transitioned to required states, we use RESOURCE_STATE_TRANSITION_MODE_VERIFY flag
            m_pImmediateContext.SetVertexBuffers(0, 1, pBuffs, null, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_VERIFY, SET_VERTEX_BUFFERS_FLAGS.SET_VERTEX_BUFFERS_FLAG_RESET);
            m_pImmediateContext.SetIndexBuffer(m_CubeIndexBuffer.Obj, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_VERIFY);

            // Set pipeline state and commit resources
            if (IsShadowPass)
            {
                m_pImmediateContext.SetPipelineState(m_pCubeShadowPSO.Obj);
                m_pImmediateContext.CommitShaderResources(m_CubeShadowSRB.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_VERIFY);
            }
            else
            {
                m_pImmediateContext.SetPipelineState(m_pCubePSO.Obj);
                m_pImmediateContext.CommitShaderResources(m_CubeSRB.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_VERIFY);
            }

            var DrawAttrs = new DrawIndexedAttribs {NumIndices = 36, IndexType = VALUE_TYPE.VT_UINT32, Flags = DRAW_FLAGS.DRAW_FLAG_VERIFY_ALL };
            m_pImmediateContext.DrawIndexed(DrawAttrs);
        }

        void RenderPlane()
        {
            unsafe
            {
                IntPtr data = m_pImmediateContext.MapBuffer(m_VSConstants.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);
                PlaneConstants* CBConstants = (PlaneConstants*)data.ToPointer();
                CBConstants->CameraViewProj = m_CameraViewProjMatrix.Transpose();
                CBConstants->WorldToShadowMapUVDepth = m_WorldToShadowMapUVDepthMatr.Transpose();
                CBConstants->LightDirection = new Vector4(m_LightDirection.x, m_LightDirection.y, m_LightDirection.z, 0);
                m_pImmediateContext.UnmapBuffer(m_VSConstants.Obj, MAP_TYPE.MAP_WRITE);
            }

            m_pImmediateContext.SetPipelineState(m_pPlanePSO.Obj);
            // Commit shader resources. RESOURCE_STATE_TRANSITION_MODE_TRANSITION mode
            // makes sure that resources are transitioned to required states.
            // Note that Vulkan requires shadow map to be transitioned to DEPTH_READ state, not SHADER_RESOURCE
            m_pImmediateContext.CommitShaderResources(m_PlaneSRB.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            var DrawAttrs = new DrawAttribs {NumVertices = 4, Flags = DRAW_FLAGS.DRAW_FLAG_VERIFY_ALL };
            m_pImmediateContext.Draw(DrawAttrs);
        }

        void RenderShadowMapVis()
        {
            m_pImmediateContext.SetPipelineState(m_pShadowMapVisPSO.Obj);
            m_pImmediateContext.CommitShaderResources(m_ShadowMapVisSRB.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_VERIFY);

            var DrawAttrs = new DrawAttribs { NumVertices = 4, Flags = DRAW_FLAGS.DRAW_FLAG_VERIFY_ALL };
            m_pImmediateContext.Draw(DrawAttrs);
        }
    }
}
