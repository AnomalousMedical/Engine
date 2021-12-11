using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngineRayTracing
{
    class RayTracingUpdateListener : UpdateListener, IDisposable
    {
        private readonly GraphicsEngine graphicsEngine;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext immediateContext;

        UInt32 m_MaxRecursionDepth = 8;

        const int NumTextures = 4;
        const int NumCubes = 4;

        private AutoPtr<IBuffer> m_ConstantsCB;
        private Constants m_Constants;

        public unsafe RayTracingUpdateListener(GraphicsEngine graphicsEngine, ShaderLoader<RayTracingUpdateListener> shaderLoader)
        {
            this.graphicsEngine = graphicsEngine;
            this.swapChain = graphicsEngine.SwapChain;
            this.immediateContext = graphicsEngine.ImmediateContext;

            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pSwapChain = graphicsEngine.SwapChain;

            // Create a buffer with shared constants.
            BufferDesc BuffDesc = new BufferDesc();
            BuffDesc.Name = "Constant buffer";
            BuffDesc.uiSizeInBytes = (uint)sizeof(Constants);
            BuffDesc.Usage = USAGE.USAGE_DEFAULT;
            BuffDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;

            m_ConstantsCB = m_pDevice.CreateBuffer(BuffDesc);
            //VERIFY_EXPR(m_ConstantsCB != nullptr);

            //CreateGraphicsPSO(); //Skip for now to create rt stuff instead
            CreateRayTracingPSO(shaderLoader);
            //LoadTextures();
            //CreateCubeBLAS();
            //CreateProceduralBLAS();
            //UpdateTLAS();
            //CreateSBT();

            // Setup camera.
            //m_Camera.SetPos(float3(-7.f, -0.5f, 16.5f));
            //m_Camera.SetRotation(-2.67f, -0.145f);
            //m_Camera.SetRotationSpeed(0.005f);
            //m_Camera.SetMoveSpeed(5.f);
            //m_Camera.SetSpeedUpScales(5.f, 10.f);

            // Initialize constants.
            m_Constants = new Constants
            {
                ClipPlanes = new Vector2(0.1f, 100.0f),
                ShadowPCF = 1,
                MaxRecursion = Math.Min(6, m_MaxRecursionDepth),

                // Sphere constants.
                SphereReflectionColorMask = new Vector3(0.81f, 1.0f, 0.45f),
                SphereReflectionBlur = 1,

                // Glass cube constants.
                GlassReflectionColorMask = new Vector3(0.22f, 0.83f, 0.93f),
                GlassAbsorption = 0.5f,
                GlassMaterialColor = new Vector4(0.33f, 0.93f, 0.29f, 0f),
                GlassIndexOfRefraction = new Vector2(1.5f, 1.02f),
                GlassEnableDispersion = 0,

                // Wavelength to RGB and index of refraction interpolation factor.
                DispersionSamples_0 = new Vector4(0.140000f, 0.000000f, 0.266667f, 0.53f),
                DispersionSamples_1 = new Vector4(0.130031f, 0.037556f, 0.612267f, 0.25f),
                DispersionSamples_2 = new Vector4(0.100123f, 0.213556f, 0.785067f, 0.16f),
                DispersionSamples_3 = new Vector4(0.050277f, 0.533556f, 0.785067f, 0.00f),
                DispersionSamples_4 = new Vector4(0.000000f, 0.843297f, 0.619682f, 0.13f),
                DispersionSamples_5 = new Vector4(0.000000f, 0.927410f, 0.431834f, 0.38f),
                DispersionSamples_6 = new Vector4(0.000000f, 0.972325f, 0.270893f, 0.27f),
                DispersionSamples_7 = new Vector4(0.000000f, 0.978042f, 0.136858f, 0.19f),
                DispersionSamples_8 = new Vector4(0.324000f, 0.944560f, 0.029730f, 0.47f),
                DispersionSamples_9 = new Vector4(0.777600f, 0.871879f, 0.000000f, 0.64f),
                DispersionSamples_10 = new Vector4(0.972000f, 0.762222f, 0.000000f, 0.77f),
                DispersionSamples_11 = new Vector4(0.971835f, 0.482222f, 0.000000f, 0.62f),
                DispersionSamples_12 = new Vector4(0.886744f, 0.202222f, 0.000000f, 0.73f),
                DispersionSamples_13 = new Vector4(0.715967f, 0.000000f, 0.000000f, 0.68f),
                DispersionSamples_14 = new Vector4(0.459920f, 0.000000f, 0.000000f, 0.91f),
                DispersionSamples_15 = new Vector4(0.218000f, 0.000000f, 0.000000f, 0.99f),
                DispersionSampleCount = 4,

                AmbientColor = new Vector4(1f, 1f, 1f, 0f) * 0.015f,
                LightPos_0 = new Vector4(8.00f, -8.0f, +0.00f, 0f),
                LightColor_0 = new Vector4(1.00f, +0.8f, +0.80f, 0f),
                LightPos_1 = new Vector4(0.00f, -4.0f, -5.00f, 0f),
                LightColor_1 = new Vector4(0.85f, +1.0f, +0.85f, 0f),

                // Random points on disc.
                DiscPoints_0 = new Vector4(+0.0f, +0.0f, +0.9f, -0.9f),
                DiscPoints_1 = new Vector4(-0.8f, +1.0f, -1.1f, -0.8f),
                DiscPoints_2 = new Vector4(+1.5f, +1.2f, -2.1f, +0.7f),
                DiscPoints_3 = new Vector4(+0.1f, -2.2f, -0.2f, +2.4f),
                DiscPoints_4 = new Vector4(+2.4f, -0.3f, -3.0f, +2.8f),
                DiscPoints_5 = new Vector4(+2.0f, -2.6f, +0.7f, +3.5f),
                DiscPoints_6 = new Vector4(-3.2f, -1.6f, +3.4f, +2.2f),
                DiscPoints_7 = new Vector4(-1.8f, -3.2f, -1.1f, +3.6f),
            };
        }

        //void CreateGraphicsPSO()
        //{
        //    var m_pDevice = graphicsEngine.RenderDevice;
        //    var m_pSwapChain = graphicsEngine.SwapChain;

        //    // Create graphics pipeline to blit render target into swapchain image.

        //    GraphicsPipelineStateCreateInfo PSOCreateInfo = new GraphicsPipelineStateCreateInfo();

        //    PSOCreateInfo.PSODesc.Name         = "Image blit PSO";
        //    PSOCreateInfo.PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_GRAPHICS;

        //    PSOCreateInfo.GraphicsPipeline.NumRenderTargets             = 1;
        //    PSOCreateInfo.GraphicsPipeline.RTVFormats_0                 = m_pSwapChain.GetDesc_ColorBufferFormat;
        //    PSOCreateInfo.GraphicsPipeline.PrimitiveTopology            = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_STRIP;
        //    PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode      = CULL_MODE.CULL_MODE_NONE;
        //    PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthEnable = false;

        //    ShaderCreateInfo ShaderCI = new ShaderCreateInfo();
        //    ShaderCI.UseCombinedTextureSamplers = true;
        //    ShaderCI.SourceLanguage             = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;
        //    ShaderCI.ShaderCompiler             = SHADER_COMPILER.SHADER_COMPILER_DXC;

        //    // Create a shader source stream factory to load shaders from files.
        //    //RefCntAutoPtr<IShaderSourceInputStreamFactory> pShaderSourceFactory;
        //    //m_pEngineFactory->CreateDefaultShaderSourceStreamFactory(nullptr, &pShaderSourceFactory);
        //    //ShaderCI.pShaderSourceStreamFactory = pShaderSourceFactory;

        //    RefCntAutoPtr<IShader> pVS;
        //    {
        //        ShaderCI.Desc.ShaderType = SHADER_TYPE_VERTEX;
        //        ShaderCI.EntryPoint      = "main";
        //        ShaderCI.Desc.Name       = "Image blit VS";
        //        ShaderCI.FilePath        = "ImageBlit.vsh";
        //        m_pDevice->CreateShader(ShaderCI, &pVS);
        //        VERIFY_EXPR(pVS != nullptr);
        //    }

        //    RefCntAutoPtr<IShader> pPS;
        //    {
        //        ShaderCI.Desc.ShaderType = SHADER_TYPE_PIXEL;
        //        ShaderCI.EntryPoint      = "main";
        //        ShaderCI.Desc.Name       = "Image blit PS";
        //        ShaderCI.FilePath        = "ImageBlit.psh";
        //        m_pDevice->CreateShader(ShaderCI, &pPS);
        //        VERIFY_EXPR(pPS != nullptr);
        //    }

        //    PSOCreateInfo.pVS = pVS;
        //    PSOCreateInfo.pPS = pPS;

        //    SamplerDesc SamLinearClampDesc{
        //        FILTER_TYPE_LINEAR, FILTER_TYPE_LINEAR, FILTER_TYPE_LINEAR,
        //        TEXTURE_ADDRESS_CLAMP, TEXTURE_ADDRESS_CLAMP, TEXTURE_ADDRESS_CLAMP};

        //    ImmutableSamplerDesc ImmutableSamplers[] = {{SHADER_TYPE_PIXEL, "g_Texture", SamLinearClampDesc}};

        //    PSOCreateInfo.PSODesc.ResourceLayout.ImmutableSamplers    = ImmutableSamplers;
        //    PSOCreateInfo.PSODesc.ResourceLayout.NumImmutableSamplers = _countof(ImmutableSamplers);
        //    PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType  = SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC;

        //    m_pDevice->CreateGraphicsPipelineState(PSOCreateInfo, &m_pImageBlitPSO);
        //    VERIFY_EXPR(m_pImageBlitPSO != nullptr);

        //    m_pImageBlitPSO->CreateShaderResourceBinding(&m_pImageBlitSRB, true);
        //    VERIFY_EXPR(m_pImageBlitSRB != nullptr);
        //}

        void CreateRayTracingPSO(ShaderLoader shaderLoader)
        {
            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pSwapChain = graphicsEngine.SwapChain;

            m_MaxRecursionDepth = Math.Min(m_MaxRecursionDepth, m_pDevice.DeviceProperties_MaxRayTracingRecursionDepth);

            // Prepare ray tracing pipeline description.
            RayTracingPipelineStateCreateInfo PSOCreateInfo = new RayTracingPipelineStateCreateInfo();

            PSOCreateInfo.PSODesc.Name = "Ray tracing PSO";
            PSOCreateInfo.PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_RAY_TRACING;

            // Define shader macros
            ShaderMacroHelper Macros = new ShaderMacroHelper();
            Macros.AddShaderMacro("NUM_TEXTURES", NumTextures);

            ShaderCreateInfo ShaderCI = new ShaderCreateInfo();
            // We will not be using combined texture samplers as they
            // are only required for compatibility with OpenGL, and ray
            // tracing is not supported in OpenGL backend.
            ShaderCI.UseCombinedTextureSamplers = false;

            // Only new DXC compiler can compile HLSL ray tracing shaders.
            ShaderCI.ShaderCompiler = SHADER_COMPILER.SHADER_COMPILER_DXC;

            // Shader model 6.3 is required for DXR 1.0, shader model 6.5 is required for DXR 1.1 and enables additional features.
            // Use 6.3 for compatibility with DXR 1.0 and VK_NV_ray_tracing.
            ShaderCI.HLSLVersion = new ShaderVersion{ Major = 6, Minor = 3 };
            ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;

            // Create a shader source stream factory to load shaders from files.
            //RefCntAutoPtr<IShaderSourceInputStreamFactory> pShaderSourceFactory;
            //m_pEngineFactory->CreateDefaultShaderSourceStreamFactory(nullptr, &pShaderSourceFactory);
            //ShaderCI.pShaderSourceStreamFactory = pShaderSourceFactory;

            // Create ray generation shader.
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_RAY_GEN;
            ShaderCI.Desc.Name = "Ray tracing RG";
            //ShaderCI.FilePath = "RayTrace.rgen";
            ShaderCI.Source = shaderLoader.LoadShader("assets/RayTrace.rgen");
            ShaderCI.EntryPoint = "main";
            using var pRayGen = m_pDevice.CreateShader(ShaderCI, Macros);
            //VERIFY_EXPR(pRayGen != nullptr);
            

            //// Create miss shaders.
            //ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_RAY_MISS;
            //ShaderCI.Desc.Name = "Primary ray miss shader";
            //ShaderCI.FilePath = "PrimaryMiss.rmiss";
            //ShaderCI.EntryPoint = "main";
            //using var pPrimaryMiss = m_pDevice.CreateShader(ShaderCI, Macros);
            ////VERIFY_EXPR(pPrimaryMiss != nullptr);

            //ShaderCI.Desc.Name = "Shadow ray miss shader";
            //ShaderCI.FilePath = "ShadowMiss.rmiss";
            //ShaderCI.EntryPoint = "main";
            //using var pShadowMiss = m_pDevice.CreateShader(ShaderCI, Macros);
            ////VERIFY_EXPR(pShadowMiss != nullptr);

            //// Create closest hit shaders.
            //RefCntAutoPtr<IShader> pCubePrimaryHit, pGroundHit, pGlassPrimaryHit, pSpherePrimaryHit;
            //{
            //    ShaderCI.Desc.ShaderType = SHADER_TYPE_RAY_CLOSEST_HIT;
            //    ShaderCI.Desc.Name = "Cube primary ray closest hit shader";
            //    ShaderCI.FilePath = "CubePrimaryHit.rchit";
            //    ShaderCI.EntryPoint = "main";
            //    m_pDevice->CreateShader(ShaderCI, &pCubePrimaryHit);
            //    VERIFY_EXPR(pCubePrimaryHit != nullptr);

            //    ShaderCI.Desc.Name = "Ground primary ray closest hit shader";
            //    ShaderCI.FilePath = "Ground.rchit";
            //    ShaderCI.EntryPoint = "main";
            //    m_pDevice->CreateShader(ShaderCI, &pGroundHit);
            //    VERIFY_EXPR(pGroundHit != nullptr);

            //    ShaderCI.Desc.Name = "Glass primary ray closest hit shader";
            //    ShaderCI.FilePath = "GlassPrimaryHit.rchit";
            //    ShaderCI.EntryPoint = "main";
            //    m_pDevice->CreateShader(ShaderCI, &pGlassPrimaryHit);
            //    VERIFY_EXPR(pGlassPrimaryHit != nullptr);

            //    ShaderCI.Desc.Name = "Sphere primary ray closest hit shader";
            //    ShaderCI.FilePath = "SpherePrimaryHit.rchit";
            //    ShaderCI.EntryPoint = "main";
            //    m_pDevice->CreateShader(ShaderCI, &pSpherePrimaryHit);
            //    VERIFY_EXPR(pSpherePrimaryHit != nullptr);
            //}

            //// Create intersection shader for a procedural sphere.
            //RefCntAutoPtr<IShader> pSphereIntersection;
            //{
            //    ShaderCI.Desc.ShaderType = SHADER_TYPE_RAY_INTERSECTION;
            //    ShaderCI.Desc.Name = "Sphere intersection shader";
            //    ShaderCI.FilePath = "SphereIntersection.rint";
            //    ShaderCI.EntryPoint = "main";
            //    m_pDevice->CreateShader(ShaderCI, &pSphereIntersection);
            //    VERIFY_EXPR(pSphereIntersection != nullptr);
            //}

            //// Setup shader groups
            //const RayTracingGeneralShaderGroup GeneralShaders[] = //
            //    {
            //        // Ray generation shader is an entry point for a ray tracing pipeline.
            //        {"Main", pRayGen},
            //        // Primary ray miss shader.
            //        {"PrimaryMiss", pPrimaryMiss},
            //        // Shadow ray miss shader.
            //        {"ShadowMiss", pShadowMiss} //
            //    };
            //const RayTracingTriangleHitShaderGroup TriangleHitShaders[] = //
            //    {
            //        // Primary ray hit group for the textured cube.
            //        {"CubePrimaryHit", pCubePrimaryHit},
            //        // Primary ray hit group for the ground.
            //        {"GroundHit", pGroundHit},
            //        // Primary ray hit group for the glass cube.
            //        {"GlassPrimaryHit", pGlassPrimaryHit} //
            //    };
            //const RayTracingProceduralHitShaderGroup ProceduralHitShaders[] = //
            //    {
            //        // Intersection and closest hit shaders for the procedural sphere.
            //        {"SpherePrimaryHit", pSphereIntersection, pSpherePrimaryHit},
            //        // Only intersection shader is needed for shadows.
            //        {"SphereShadowHit", pSphereIntersection} //
            //    };

            //PSOCreateInfo.pGeneralShaders = GeneralShaders;
            //PSOCreateInfo.GeneralShaderCount = _countof(GeneralShaders);
            //PSOCreateInfo.pTriangleHitShaders = TriangleHitShaders;
            //PSOCreateInfo.TriangleHitShaderCount = _countof(TriangleHitShaders);
            //PSOCreateInfo.pProceduralHitShaders = ProceduralHitShaders;
            //PSOCreateInfo.ProceduralHitShaderCount = _countof(ProceduralHitShaders);

            //// Specify the maximum ray recursion depth.
            //// WARNING: the driver does not track the recursion depth and it is the
            ////          application's responsibility to not exceed the specified limit.
            ////          The value is used to reserve the necessary stack size and
            ////          exceeding it will likely result in driver crash.
            //PSOCreateInfo.RayTracingPipeline.MaxRecursionDepth = static_cast<Uint8>(m_MaxRecursionDepth);

            //// Per-shader data is not used.
            //PSOCreateInfo.RayTracingPipeline.ShaderRecordSize = 0;

            //// DirectX 12 only: set attribute and payload size. Values should be as small as possible to minimize the memory usage.
            //PSOCreateInfo.MaxAttributeSize = std::max<Uint32>(sizeof(/*BuiltInTriangleIntersectionAttributes*/ float2), sizeof(HLSL::ProceduralGeomIntersectionAttribs));
            //PSOCreateInfo.MaxPayloadSize = std::max<Uint32>(sizeof(HLSL::PrimaryRayPayload), sizeof(HLSL::ShadowRayPayload));

            //// Define immutable sampler for g_Texture and g_GroundTexture. Immutable samplers should be used whenever possible
            //SamplerDesc SamLinearWrapDesc{
            //    FILTER_TYPE_LINEAR, FILTER_TYPE_LINEAR, FILTER_TYPE_LINEAR,
            //    TEXTURE_ADDRESS_WRAP, TEXTURE_ADDRESS_WRAP, TEXTURE_ADDRESS_WRAP //
            //};
            //ImmutableSamplerDesc ImmutableSamplers[] = //
            //    {
            //        {SHADER_TYPE_RAY_CLOSEST_HIT, "g_SamLinearWrap", SamLinearWrapDesc} //
            //    };

            //ShaderResourceVariableDesc Variables[] = //
            //    {
            //        {SHADER_TYPE_RAY_GEN | SHADER_TYPE_RAY_MISS | SHADER_TYPE_RAY_CLOSEST_HIT, "g_ConstantsCB", SHADER_RESOURCE_VARIABLE_TYPE_STATIC},
            //        {SHADER_TYPE_RAY_GEN, "g_ColorBuffer", SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC}
            //        //
            //    };

            //PSOCreateInfo.PSODesc.ResourceLayout.Variables = Variables;
            //PSOCreateInfo.PSODesc.ResourceLayout.NumVariables = _countof(Variables);
            //PSOCreateInfo.PSODesc.ResourceLayout.ImmutableSamplers = ImmutableSamplers;
            //PSOCreateInfo.PSODesc.ResourceLayout.NumImmutableSamplers = _countof(ImmutableSamplers);
            //PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE_MUTABLE;

            //m_pDevice->CreateRayTracingPipelineState(PSOCreateInfo, &m_pRayTracingPSO);
            //VERIFY_EXPR(m_pRayTracingPSO != nullptr);

            //m_pRayTracingPSO->GetStaticVariableByName(SHADER_TYPE_RAY_GEN, "g_ConstantsCB")->Set(m_ConstantsCB);
            //m_pRayTracingPSO->GetStaticVariableByName(SHADER_TYPE_RAY_MISS, "g_ConstantsCB")->Set(m_ConstantsCB);
            //m_pRayTracingPSO->GetStaticVariableByName(SHADER_TYPE_RAY_CLOSEST_HIT, "g_ConstantsCB")->Set(m_ConstantsCB);

            //m_pRayTracingPSO->CreateShaderResourceBinding(&m_pRayTracingSRB, true);
            //VERIFY_EXPR(m_pRayTracingSRB != nullptr);
        }

        public void Dispose()
        {
            m_ConstantsCB.Dispose();
        }

        public void exceededMaxDelta()
        {

        }

        public void loopStarting()
        {

        }

        public void sendUpdate(Clock clock)
        {
            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            immediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            var ClearColor = new Color(0.350f, 0.350f, 0.350f, 1.0f);

            // Clear the back buffer
            // Let the engine perform required state transitions
            immediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            this.swapChain.Present(1);
        }
    }
}
