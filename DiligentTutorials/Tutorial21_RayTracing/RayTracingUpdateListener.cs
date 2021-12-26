using Anomalous.OSPlatform;
using DiligentEngine;
using Engine;
using Engine.CameraMovement;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngineRayTracing
{
    class RayTracingUpdateListener : UpdateListener, IDisposable
    {
        //Camera Settings
        float YFov = MathFloat.PI / 4.0f;
        float ZNear = 0.1f;
        float ZFar = 100f;

        private readonly GraphicsEngine graphicsEngine;
        private readonly NativeOSWindow window;
        private readonly FirstPersonFlyCamera cameraControls;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext immediateContext;

        UInt32 m_MaxRecursionDepth = 8;

        const int NumTextures = 4;
        const int NumCubes = 4;

        private AutoPtr<IBuffer> m_ConstantsCB;
        private Constants m_Constants;
        private AutoPtr<IPipelineState> m_pRayTracingPSO;
        private AutoPtr<IShaderResourceBinding> m_pRayTracingSRB;

        AutoPtr<IPipelineState> m_pImageBlitPSO;
        AutoPtr<IShaderResourceBinding> m_pImageBlitSRB;

        AutoPtr<IBuffer> m_CubeAttribsCB;
        AutoPtr<IBuffer> m_BoxAttribsCB;
        AutoPtr<IBuffer> pCubeVertexBuffer;
        AutoPtr<IBuffer> pCubeIndexBuffer;

        AutoPtr<IBottomLevelAS> m_pCubeBLAS;

        AutoPtr<IBottomLevelAS> m_pProceduralBLAS;

        AutoPtr<ITopLevelAS> m_pTLAS;
        AutoPtr<IBuffer> m_ScratchBuffer;
        AutoPtr<IBuffer> m_InstanceBuffer;

        float m_AnimationTime = 0.0f;
        bool[] m_EnableCubes = new bool[] { true, true, true, true };

        AutoPtr<IShaderBindingTable> m_pSBT;

        TEXTURE_FORMAT m_ColorBufferFormat = TEXTURE_FORMAT.TEX_FORMAT_RGBA8_UNORM;
        AutoPtr<ITexture> m_pColorRT;

        public unsafe RayTracingUpdateListener
        (
            GraphicsEngine graphicsEngine, 
            ShaderLoader<RayTracingUpdateListener> shaderLoader, 
            TextureLoader textureLoader, 
            NativeOSWindow window,
            FirstPersonFlyCamera cameraControls
        )
        {
            this.graphicsEngine = graphicsEngine;
            this.window = window;
            this.cameraControls = cameraControls;
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

            CreateGraphicsPSO(shaderLoader);
            CreateRayTracingPSO(shaderLoader);
            LoadTextures(textureLoader);
            CreateCubeBLAS();
            CreateProceduralBLAS();
            UpdateTLAS();
            CreateSBT();

            WindowResize((uint)window.WindowWidth, (uint)window.WindowHeight);
            window.Resized += _ => WindowResize((uint)window.WindowWidth, (uint)window.WindowHeight);

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

        void CreateGraphicsPSO(ShaderLoader shaderLoader)
        {
            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pSwapChain = graphicsEngine.SwapChain;

            // Create graphics pipeline to blit render target into swapchain image.

            GraphicsPipelineStateCreateInfo PSOCreateInfo = new GraphicsPipelineStateCreateInfo();

            PSOCreateInfo.PSODesc.Name = "Image blit PSO";
            PSOCreateInfo.PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_GRAPHICS;

            PSOCreateInfo.GraphicsPipeline.NumRenderTargets = 1;
            PSOCreateInfo.GraphicsPipeline.RTVFormats_0 = m_pSwapChain.GetDesc_ColorBufferFormat;
            PSOCreateInfo.GraphicsPipeline.PrimitiveTopology = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_STRIP;
            PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE.CULL_MODE_NONE;
            PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthEnable = false;

            ShaderCreateInfo ShaderCI = new ShaderCreateInfo();
            ShaderCI.UseCombinedTextureSamplers = true;
            ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;
            ShaderCI.ShaderCompiler = SHADER_COMPILER.SHADER_COMPILER_DXC;

            // Create a shader source stream factory to load shaders from files.
            //RefCntAutoPtr<IShaderSourceInputStreamFactory> pShaderSourceFactory;
            //m_pEngineFactory->CreateDefaultShaderSourceStreamFactory(nullptr, &pShaderSourceFactory);
            //ShaderCI.pShaderSourceStreamFactory = pShaderSourceFactory;

            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_VERTEX;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Image blit VS";
            //ShaderCI.FilePath = "ImageBlit.vsh";
            ShaderCI.Source = shaderLoader.LoadShader("assets/ImageBlit.vsh");
            using var pVS = m_pDevice.CreateShader(ShaderCI);
            //VERIFY_EXPR(pVS != nullptr);

            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_PIXEL;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Image blit PS";
            //ShaderCI.FilePath = "ImageBlit.psh";
            ShaderCI.Source = shaderLoader.LoadShader("assets/ImageBlit.psh");
            using var pPS = m_pDevice.CreateShader(ShaderCI);
            //VERIFY_EXPR(pPS != nullptr);

            PSOCreateInfo.pVS = pVS.Obj;
            PSOCreateInfo.pPS = pPS.Obj;

            var SamLinearClampDesc = new SamplerDesc
            {
                MinFilter = FILTER_TYPE.FILTER_TYPE_LINEAR,
                MagFilter = FILTER_TYPE.FILTER_TYPE_LINEAR,
                MipFilter = FILTER_TYPE.FILTER_TYPE_LINEAR,
                AddressU = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_CLAMP,
                AddressV = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_CLAMP,
                AddressW = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_CLAMP
            };

            var ImmutableSamplers = new List<ImmutableSamplerDesc>
            {
                new ImmutableSamplerDesc{ ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, SamplerOrTextureName = "g_Texture", Desc = SamLinearClampDesc }
            };

            PSOCreateInfo.PSODesc.ResourceLayout.ImmutableSamplers = ImmutableSamplers;
            PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC;

            m_pImageBlitPSO = m_pDevice.CreateGraphicsPipelineState(PSOCreateInfo);
            //VERIFY_EXPR(m_pImageBlitPSO != nullptr);

            m_pImageBlitSRB = m_pImageBlitPSO.Obj.CreateShaderResourceBinding(true);
            //VERIFY_EXPR(m_pImageBlitSRB != nullptr);
        }

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
            ShaderCI.HLSLVersion = new ShaderVersion { Major = 6, Minor = 3 };
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


            // Create miss shaders.
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_RAY_MISS;
            ShaderCI.Desc.Name = "Primary ray miss shader";
            //ShaderCI.FilePath = "PrimaryMiss.rmiss";
            ShaderCI.Source = shaderLoader.LoadShader("assets/PrimaryMiss.rmiss");
            ShaderCI.EntryPoint = "main";
            using var pPrimaryMiss = m_pDevice.CreateShader(ShaderCI, Macros);
            //VERIFY_EXPR(pPrimaryMiss != nullptr);

            ShaderCI.Desc.Name = "Shadow ray miss shader";
            //ShaderCI.FilePath = "ShadowMiss.rmiss";
            ShaderCI.Source = shaderLoader.LoadShader("assets/ShadowMiss.rmiss");
            ShaderCI.EntryPoint = "main";
            using var pShadowMiss = m_pDevice.CreateShader(ShaderCI, Macros);
            //VERIFY_EXPR(pShadowMiss != nullptr);

            //// Create closest hit shaders.
            //RefCntAutoPtr<IShader> pCubePrimaryHit, pGroundHit, pGlassPrimaryHit, pSpherePrimaryHit;
            //{
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT;
            ShaderCI.Desc.Name = "Cube primary ray closest hit shader";
            //ShaderCI.FilePath = "CubePrimaryHit.rchit";
            ShaderCI.Source = shaderLoader.LoadShader("assets/CubePrimaryHit.rchit");
            ShaderCI.EntryPoint = "main";
            using var pCubePrimaryHit = m_pDevice.CreateShader(ShaderCI, Macros);
            //VERIFY_EXPR(pCubePrimaryHit != nullptr);

            ShaderCI.Desc.Name = "Ground primary ray closest hit shader";
            //ShaderCI.FilePath = "Ground.rchit";
            ShaderCI.Source = shaderLoader.LoadShader("assets/Ground.rchit");
            ShaderCI.EntryPoint = "main";
            using var pGroundHit = m_pDevice.CreateShader(ShaderCI, Macros);
            //    VERIFY_EXPR(pGroundHit != nullptr);

            ShaderCI.Desc.Name = "Glass primary ray closest hit shader";
            //ShaderCI.FilePath = "GlassPrimaryHit.rchit";
            ShaderCI.Source = shaderLoader.LoadShader("assets/GlassPrimaryHit.rchit");
            ShaderCI.EntryPoint = "main";
            using var pGlassPrimaryHit = m_pDevice.CreateShader(ShaderCI, Macros);
            //    VERIFY_EXPR(pGlassPrimaryHit != nullptr);

            ShaderCI.Desc.Name = "Sphere primary ray closest hit shader";
            //ShaderCI.FilePath = "SpherePrimaryHit.rchit";
            ShaderCI.Source = shaderLoader.LoadShader("assets/SpherePrimaryHit.rchit");
            ShaderCI.EntryPoint = "main";
            using var pSpherePrimaryHit = m_pDevice.CreateShader(ShaderCI, Macros);
            //    VERIFY_EXPR(pSpherePrimaryHit != nullptr);
            //}

            //// Create intersection shader for a procedural sphere.
            //RefCntAutoPtr<IShader> pSphereIntersection;
            //{
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_RAY_INTERSECTION;
            ShaderCI.Desc.Name = "Sphere intersection shader";
            //ShaderCI.FilePath = "SphereIntersection.rint";
            ShaderCI.Source = shaderLoader.LoadShader("assets/SphereIntersection.rint");
            ShaderCI.EntryPoint = "main";
            using var pSphereIntersection = m_pDevice.CreateShader(ShaderCI, Macros);
            //    VERIFY_EXPR(pSphereIntersection != nullptr);
            //}

            // Setup shader groups
            PSOCreateInfo.pGeneralShaders = new List<RayTracingGeneralShaderGroup>
            {
                // Ray generation shader is an entry point for a ray tracing pipeline.
                new RayTracingGeneralShaderGroup { Name = "Main", pShader = pRayGen.Obj },
                // Primary ray miss shader.
                new RayTracingGeneralShaderGroup { Name = "PrimaryMiss", pShader = pPrimaryMiss.Obj },
                // Shadow ray miss shader.
                new RayTracingGeneralShaderGroup { Name = "ShadowMiss", pShader = pShadowMiss.Obj }
            };

            PSOCreateInfo.pTriangleHitShaders = new List<RayTracingTriangleHitShaderGroup>
            {
                // Primary ray hit group for the textured cube.
                new RayTracingTriangleHitShaderGroup { Name = "CubePrimaryHit", pClosestHitShader = pCubePrimaryHit.Obj },
                // Primary ray hit group for the ground.
                new RayTracingTriangleHitShaderGroup { Name = "GroundHit", pClosestHitShader = pGroundHit.Obj },
                // Primary ray hit group for the glass cube.
                new RayTracingTriangleHitShaderGroup { Name = "GlassPrimaryHit", pClosestHitShader = pGlassPrimaryHit.Obj }
            };

            PSOCreateInfo.pProceduralHitShaders = new List<RayTracingProceduralHitShaderGroup>
            {
                // Intersection and closest hit shaders for the procedural sphere.
                new RayTracingProceduralHitShaderGroup { Name = "SpherePrimaryHit", pIntersectionShader = pSphereIntersection.Obj, pClosestHitShader = pSpherePrimaryHit.Obj },
                // Only intersection shader is needed for shadows.
                new RayTracingProceduralHitShaderGroup { Name = "SphereShadowHit", pIntersectionShader = pSphereIntersection.Obj }
            };

            // GeneralShaders;
            ////PSOCreateInfo.GeneralShaderCount = _countof(GeneralShaders);
            // = TriangleHitShaders;
            ////PSOCreateInfo.TriangleHitShaderCount = _countof(TriangleHitShaders);
            // = ProceduralHitShaders;
            ////PSOCreateInfo.ProceduralHitShaderCount = _countof(ProceduralHitShaders);

            // Specify the maximum ray recursion depth.
            // WARNING: the driver does not track the recursion depth and it is the
            //          application's responsibility to not exceed the specified limit.
            //          The value is used to reserve the necessary stack size and
            //          exceeding it will likely result in driver crash.
            PSOCreateInfo.RayTracingPipeline.MaxRecursionDepth = (byte)m_MaxRecursionDepth;

            // Per-shader data is not used.
            PSOCreateInfo.RayTracingPipeline.ShaderRecordSize = 0;

            //--- START POTENTIAL BLOCK ---
            //  Might want this part
            //// DirectX 12 only: set attribute and payload size. Values should be as small as possible to minimize the memory usage.
            //PSOCreateInfo.MaxAttributeSize = std::max<Uint32>(sizeof(/*BuiltInTriangleIntersectionAttributes*/ float2), sizeof(HLSL::ProceduralGeomIntersectionAttribs));
            //PSOCreateInfo.MaxPayloadSize = std::max<Uint32>(sizeof(HLSL::PrimaryRayPayload), sizeof(HLSL::ShadowRayPayload));
            //--- END POTENTIAL BLOCK ---

            // Define immutable sampler for g_Texture and g_GroundTexture. Immutable samplers should be used whenever possible
            var SamLinearWrapDesc = new SamplerDesc
            {
                MinFilter = FILTER_TYPE.FILTER_TYPE_LINEAR,
                MagFilter = FILTER_TYPE.FILTER_TYPE_LINEAR,
                MipFilter = FILTER_TYPE.FILTER_TYPE_LINEAR,
                AddressU = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_WRAP,
                AddressV = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_WRAP,
                AddressW = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_WRAP
            };
            var ImmutableSamplers = new List<ImmutableSamplerDesc>
            {
                new ImmutableSamplerDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, SamplerOrTextureName = "g_SamLinearWrap", Desc = SamLinearWrapDesc} //
            };

            var Variables = new List<ShaderResourceVariableDesc> //
            {
                new ShaderResourceVariableDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_RAY_GEN | SHADER_TYPE.SHADER_TYPE_RAY_MISS | SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, Name = "g_ConstantsCB", Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC},
                new ShaderResourceVariableDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_RAY_GEN, Name = "g_ColorBuffer", Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC}
            };

            PSOCreateInfo.PSODesc.ResourceLayout.Variables = Variables;
            PSOCreateInfo.PSODesc.ResourceLayout.ImmutableSamplers = ImmutableSamplers;
            PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_MUTABLE;

            this.m_pRayTracingPSO = m_pDevice.CreateRayTracingPipelineState(PSOCreateInfo);
            //VERIFY_EXPR(m_pRayTracingPSO != nullptr);

            m_pRayTracingPSO.Obj.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_GEN, "g_ConstantsCB").Set(m_ConstantsCB.Obj);
            m_pRayTracingPSO.Obj.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_MISS, "g_ConstantsCB").Set(m_ConstantsCB.Obj);
            m_pRayTracingPSO.Obj.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, "g_ConstantsCB").Set(m_ConstantsCB.Obj);

            m_pRayTracingSRB = m_pRayTracingPSO.Obj.CreateShaderResourceBinding(true);
            //VERIFY_EXPR(m_pRayTracingSRB != nullptr);
        }

        void LoadTextures(TextureLoader textureLoader)
        {
            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pSwapChain = graphicsEngine.SwapChain;
            var m_pImmediateContext = graphicsEngine.ImmediateContext;

            // Load textures
            var pTexSRVs = new List<IDeviceObject>(NumTextures);
            var pTex = new List<AutoPtr<ITexture>>(NumTextures);
            var Barriers = new List<StateTransitionDesc>(NumTextures);
            try
            {
                for (int tex = 0; tex < NumTextures; ++tex)
                {
                    //// Load current texture
                    //TextureLoadInfo loadInfo;
                    //loadInfo.IsSRGB = true;

                    //std::stringstream FileNameSS;
                    //FileNameSS << "DGLogo" << tex << ".png";
                    //auto FileName = FileNameSS.str();
                    //CreateTextureFromFile(FileName.c_str(), loadInfo, m_pDevice, &pTex[tex]);


                    var logo = Path.GetFullPath($"assets/AnomalousEngine{tex}.png");
                    Console.WriteLine(logo);

                    using var logoStream = File.Open(logo, FileMode.Open, FileAccess.Read, FileShare.Read);
                    pTex.Add(textureLoader.LoadTexture(logoStream, $"Logo {tex} Texture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D, true));

                    // Get shader resource view from the texture
                    var pTextureSRV = pTex[tex].Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);
                    pTexSRVs.Add(pTextureSRV);
                    Barriers.Add(new StateTransitionDesc { pResource = pTex[tex].Obj, OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_SHADER_RESOURCE, UpdateResourceState = true });
                }
                m_pImmediateContext.TransitionResourceStates(Barriers);

                m_pRayTracingSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, "g_CubeTextures").SetArray(pTexSRVs);

                // Load ground texture
                {
                    var ground = Path.GetFullPath("assets/RoofingTiles006_1K_Color.jpg");

                    using var stream = File.Open(ground, FileMode.Open, FileAccess.Read, FileShare.Read);
                    using var pGroundTex = textureLoader.LoadTexture(stream, "Ground Texture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D, true);
                    // Get shader resource view from the texture
                    var m_TextureSRV = pGroundTex.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);

                    m_pRayTracingSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, "g_GroundTexture").Set(m_TextureSRV);
                }
                {
                    var ground = Path.GetFullPath("assets/RoofingTiles006_1K_Normal.jpg");

                    using var stream = File.Open(ground, FileMode.Open, FileAccess.Read, FileShare.Read);
                    using var pGroundTex = textureLoader.LoadTexture(stream, "Ground Texture Normal", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D, true);
                    // Get shader resource view from the texture
                    var m_TextureSRV = pGroundTex.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);

                    m_pRayTracingSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, "g_GroundNormalTexture").Set(m_TextureSRV);
                }
            }
            finally
            {
                foreach (var i in pTex)
                {
                    i.Dispose();
                }
            }
        }

        unsafe void CreateCubeBLAS()
        {
            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pImmediateContext = graphicsEngine.ImmediateContext;

            // clang-format off
            var CubePos = new Vector3[]
            {
                new Vector3(-1,-1,-1), new Vector3(-1,+1,-1), new Vector3(+1,+1,-1), new Vector3(+1,-1,-1),
                new Vector3(-1,-1,-1), new Vector3(-1,-1,+1), new Vector3(+1,-1,+1), new Vector3(+1,-1,-1),
                new Vector3(+1,-1,-1), new Vector3(+1,-1,+1), new Vector3(+1,+1,+1), new Vector3(+1,+1,-1),
                new Vector3(+1,+1,-1), new Vector3(+1,+1,+1), new Vector3(-1,+1,+1), new Vector3(-1,+1,-1),
                new Vector3(-1,+1,-1), new Vector3(-1,+1,+1), new Vector3(-1,-1,+1), new Vector3(-1,-1,-1),
                new Vector3(-1,-1,+1), new Vector3(+1,-1,+1), new Vector3(+1,+1,+1), new Vector3(-1,+1,+1)
            };

            var CubeUV = new Vector4[]
            {
                new Vector4(0,0,0,0), new Vector4(0,1,0,0), new Vector4(1,1,0,0), new Vector4(1,0,0,0),
                new Vector4(0,0,0,0), new Vector4(0,1,0,0), new Vector4(1,1,0,0), new Vector4(1,0,0,0),
                new Vector4(0,0,0,0), new Vector4(1,0,0,0), new Vector4(1,1,0,0), new Vector4(0,1,0,0),
                new Vector4(0,0,0,0), new Vector4(0,1,0,0), new Vector4(1,1,0,0), new Vector4(1,0,0,0),
                new Vector4(1,1,0,0), new Vector4(0,1,0,0), new Vector4(0,0,0,0), new Vector4(1,0,0,0),
                new Vector4(1,0,0,0), new Vector4(0,0,0,0), new Vector4(0,1,0,0), new Vector4(1,1,0,0)
            };

            var CubeNormals = new Vector4[]
            {
                new Vector4(0, 0, -1, 0), new Vector4(0, 0, -1, 0), new Vector4(0, 0, -1, 0), new Vector4(0, 0, -1, 0),
                new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0),
                new Vector4(+1, 0, 0, 0), new Vector4(+1, 0, 0, 0), new Vector4(+1, 0, 0, 0), new Vector4(+1, 0, 0, 0),
                new Vector4(0, +1, 0, 0), new Vector4(0, +1, 0, 0), new Vector4(0, +1, 0, 0), new Vector4(0, +1, 0, 0),
                new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0),
                new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0)
            };

            var Indices = new uint[]
            {
                2,0,1,    2,3,0,
                4,6,5,    4,7,6,
                8,10,9,   8,11,10,
                12,14,13, 12,15,14,
                16,18,17, 16,19,18,
                20,21,22, 20,22,23
            };
            // clang-format on

            // Create a buffer with cube attributes.
            // These attributes will be used in the hit shader to calculate UVs and normal for intersection point.
            {
                var Attribs = new CubeAttribs();

                //static_assert(sizeof(Attribs.UVs) == sizeof(CubeUV), "size mismatch");
                //std::memcpy(Attribs.UVs, CubeUV, sizeof(CubeUV));
                Attribs.SetUvs(CubeUV);

                //static_assert(sizeof(Attribs.Normals) == sizeof(CubeNormals), "size mismatch");
                //std::memcpy(Attribs.Normals, CubeNormals, sizeof(CubeNormals));
                Attribs.SetNormals(CubeNormals);

                for (int i = 0; i < Indices.Length; i += 3)
                {
                    Attribs.SetPrimitive(i / 3, Indices[i], Indices[i + 1], Indices[i + 2], 0);
                }

                var BufData = new BufferData()
                {
                    pData = new IntPtr(&Attribs), //This is stack allocated, so just get the pointer, everything else complains
                    DataSize = (uint)sizeof(CubeAttribs)
                };
                var BuffDesc = new BufferDesc();
                BuffDesc.Name = "Cube Attribs";
                BuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
                BuffDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                BuffDesc.uiSizeInBytes = BufData.DataSize;

                m_CubeAttribsCB = m_pDevice.CreateBuffer(BuffDesc, BufData);
                //VERIFY_EXPR(m_CubeAttribsCB != nullptr);

                m_pRayTracingSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, "g_CubeAttribsCB").Set(m_CubeAttribsCB.Obj);
            }

            // Create vertex buffer
            {
                var BuffDesc = new BufferDesc();
                BuffDesc.Name = "Cube vertices";
                BuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
                BuffDesc.BindFlags = BIND_FLAGS.BIND_RAY_TRACING;

                BufferData BufData = new BufferData();
                fixed (Vector3* vertices = CubePos)
                {
                    BufData.pData = new IntPtr(vertices);
                    BufData.DataSize = BuffDesc.uiSizeInBytes = (uint)(sizeof(Vector3) * CubePos.Length);
                    pCubeVertexBuffer = m_pDevice.CreateBuffer(BuffDesc, BufData);
                }

                //VERIFY_EXPR(pCubeVertexBuffer != nullptr);
            }

            // Create index buffer
            {
                var BuffDesc = new BufferDesc();
                BuffDesc.Name = "Cube indices";
                BuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
                BuffDesc.BindFlags = BIND_FLAGS.BIND_RAY_TRACING;

                BufferData BufData = new BufferData();
                fixed (uint* vertices = Indices)
                {
                    BufData.pData = new IntPtr(vertices);
                    BufData.DataSize = BuffDesc.uiSizeInBytes = (uint)(sizeof(uint) * Indices.Length);
                    pCubeIndexBuffer = m_pDevice.CreateBuffer(BuffDesc, BufData);
                }

                //VERIFY_EXPR(pCubeIndexBuffer != nullptr);
            }

            // Create & build bottom level acceleration structure
            {
                // Create BLAS
                var Triangles = new BLASTriangleDesc();
                {
                    Triangles.GeometryName = "Cube";
                    Triangles.MaxVertexCount = (uint)CubePos.Length;
                    Triangles.VertexValueType = VALUE_TYPE.VT_FLOAT32;
                    Triangles.VertexComponentCount = 3;
                    Triangles.MaxPrimitiveCount = (uint)(Indices.Length / 3);
                    Triangles.IndexType = VALUE_TYPE.VT_UINT32;

                    var ASDesc = new BottomLevelASDesc();
                    ASDesc.Name = "Cube BLAS";
                    ASDesc.Flags = RAYTRACING_BUILD_AS_FLAGS.RAYTRACING_BUILD_AS_PREFER_FAST_TRACE;
                    ASDesc.pTriangles = new List<BLASTriangleDesc> { Triangles };

                    m_pCubeBLAS = m_pDevice.CreateBLAS(ASDesc);
                    //VERIFY_EXPR(m_pCubeBLAS != nullptr);
                }

                // Create scratch buffer
                using var pScratchBuffer = m_pDevice.CreateBuffer(new BufferDesc()
                {
                    Name = "BLAS Scratch Buffer",
                    Usage = USAGE.USAGE_DEFAULT,
                    BindFlags = BIND_FLAGS.BIND_RAY_TRACING,
                    uiSizeInBytes = m_pCubeBLAS.Obj.ScratchBufferSizes_Build,
                }, new BufferData());

                // Build BLAS
                var TriangleData = new BLASBuildTriangleData();
                TriangleData.GeometryName = Triangles.GeometryName;
                TriangleData.pVertexBuffer = pCubeVertexBuffer.Obj;
                TriangleData.VertexStride = (uint)sizeof(Vector3);
                TriangleData.VertexCount = Triangles.MaxVertexCount;
                TriangleData.VertexValueType = Triangles.VertexValueType;
                TriangleData.VertexComponentCount = Triangles.VertexComponentCount;
                TriangleData.pIndexBuffer = pCubeIndexBuffer.Obj;
                TriangleData.PrimitiveCount = Triangles.MaxPrimitiveCount;
                TriangleData.IndexType = Triangles.IndexType;
                TriangleData.Flags = RAYTRACING_GEOMETRY_FLAGS.RAYTRACING_GEOMETRY_FLAG_OPAQUE;

                var Attribs = new BuildBLASAttribs();
                Attribs.pBLAS = m_pCubeBLAS.Obj;
                Attribs.pTriangleData = new List<BLASBuildTriangleData> { TriangleData };

                // Scratch buffer will be used to store temporary data during BLAS build.
                // Previous content in the scratch buffer will be discarded.
                Attribs.pScratchBuffer = pScratchBuffer.Obj;

                // Allow engine to change resource states.
                Attribs.BLASTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;
                Attribs.GeometryTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;
                Attribs.ScratchBufferTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;

                m_pImmediateContext.BuildBLAS(Attribs);
            }
        }

        unsafe void CreateProceduralBLAS()
        {
            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pImmediateContext = graphicsEngine.ImmediateContext;

            //static_assert(sizeof(HLSL::BoxAttribs) % 16 == 0, "BoxAttribs must be aligned by 16 bytes");

            var Boxes = new BoxAttribs() { minX = -2.5f, minY = -2.5f, minZ = -2.5f, maxX = 2.5f, maxY = 2.5f, maxZ = 2.5f };

            // Create box buffer
            {
                var BufData = new BufferData { pData = new IntPtr(&Boxes), DataSize = (uint)sizeof(BoxAttribs) };
                var BuffDesc = new BufferDesc();
                BuffDesc.Name = "AABB Buffer";
                BuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
                BuffDesc.BindFlags = BIND_FLAGS.BIND_RAY_TRACING | BIND_FLAGS.BIND_SHADER_RESOURCE;
                BuffDesc.uiSizeInBytes = BufData.DataSize;
                BuffDesc.ElementByteStride = (uint)sizeof(BoxAttribs);
                BuffDesc.Mode = BUFFER_MODE.BUFFER_MODE_STRUCTURED;

                m_BoxAttribsCB = m_pDevice.CreateBuffer(BuffDesc, BufData);
                //VERIFY_EXPR(m_BoxAttribsCB != nullptr);

                m_pRayTracingSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_INTERSECTION, "g_BoxAttribs").Set(m_BoxAttribsCB.Obj.GetDefaultView(BUFFER_VIEW_TYPE.BUFFER_VIEW_SHADER_RESOURCE));
            }

            // Create & build bottom level acceleration structure
            {
                // Create BLAS
                var BoxInfo = new BLASBoundingBoxDesc();
                {
                    BoxInfo.GeometryName = "Box";
                    BoxInfo.MaxBoxCount = 1;

                    var ASDesc = new BottomLevelASDesc();
                    ASDesc.Name = "Procedural BLAS";
                    ASDesc.Flags = RAYTRACING_BUILD_AS_FLAGS.RAYTRACING_BUILD_AS_PREFER_FAST_TRACE;
                    ASDesc.pBoxes = new List<BLASBoundingBoxDesc>() { BoxInfo };

                    m_pProceduralBLAS = m_pDevice.CreateBLAS(ASDesc);
                    //VERIFY_EXPR(m_pProceduralBLAS != nullptr);
                }

                // Create scratch buffer
                using var pScratchBuffer = m_pDevice.CreateBuffer(new BufferDesc()
                {
                    Name = "BLAS Scratch Buffer",
                    Usage = USAGE.USAGE_DEFAULT,
                    BindFlags = BIND_FLAGS.BIND_RAY_TRACING,
                    uiSizeInBytes = m_pProceduralBLAS.Obj.ScratchBufferSizes_Build,
                }, new BufferData());

                // Build BLAS
                var BoxData = new BLASBuildBoundingBoxData();
                BoxData.GeometryName = BoxInfo.GeometryName;
                BoxData.BoxCount = 1;
                BoxData.BoxStride = (uint)sizeof(BoxAttribs);
                BoxData.pBoxBuffer = m_BoxAttribsCB.Obj;

                var Attribs = new BuildBLASAttribs();
                Attribs.pBLAS = m_pProceduralBLAS.Obj;
                Attribs.pBoxData = new List<BLASBuildBoundingBoxData> { BoxData };

                // Scratch buffer will be used to store temporary data during BLAS build.
                // Previous content in the scratch buffer will be discarded.
                Attribs.pScratchBuffer = pScratchBuffer.Obj;

                // Allow engine to change resource states.
                Attribs.BLASTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;
                Attribs.GeometryTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;
                Attribs.ScratchBufferTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;

                m_pImmediateContext.BuildBLAS(Attribs);
            }
        }

        void UpdateTLAS()
        {
            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pImmediateContext = graphicsEngine.ImmediateContext;
            // Create or update top-level acceleration structure

            const int NumInstances = NumCubes + 3;

            bool NeedUpdate = true;

            // Create TLAS
            if (m_pTLAS == null)
            {
                var TLASDesc = new TopLevelASDesc();
                TLASDesc.Name = "TLAS";
                TLASDesc.MaxInstanceCount = NumInstances;
                TLASDesc.Flags = RAYTRACING_BUILD_AS_FLAGS.RAYTRACING_BUILD_AS_ALLOW_UPDATE | RAYTRACING_BUILD_AS_FLAGS.RAYTRACING_BUILD_AS_PREFER_FAST_TRACE;

                m_pTLAS = m_pDevice.CreateTLAS(TLASDesc);
                //VERIFY_EXPR(m_pTLAS != nullptr);

                NeedUpdate = false; // build on first run

                m_pRayTracingSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_GEN, "g_TLAS").Set(m_pTLAS.Obj);
                m_pRayTracingSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, "g_TLAS").Set(m_pTLAS.Obj);
            }

            // Create scratch buffer
            if (m_ScratchBuffer == null)
            {
                m_ScratchBuffer = m_pDevice.CreateBuffer(new BufferDesc()
                {
                    Name = "TLAS Scratch Buffer",
                    Usage = USAGE.USAGE_DEFAULT,
                    BindFlags = BIND_FLAGS.BIND_RAY_TRACING,
                    uiSizeInBytes = Math.Max(m_pTLAS.Obj.ScratchBufferSizes_Build, m_pTLAS.Obj.ScratchBufferSizes_Update)
                }, new BufferData());
            }

            // Create instance buffer
            if (m_InstanceBuffer == null)
            {
                var BuffDesc = new BufferDesc();
                BuffDesc.Name = "TLAS Instance Buffer";
                BuffDesc.Usage = USAGE.USAGE_DEFAULT;
                BuffDesc.BindFlags = BIND_FLAGS.BIND_RAY_TRACING;
                BuffDesc.uiSizeInBytes = ITopLevelAS.TLAS_INSTANCE_DATA_SIZE * NumInstances;

                m_InstanceBuffer = m_pDevice.CreateBuffer(BuffDesc, new BufferData());
                //VERIFY_EXPR(m_InstanceBuffer != nullptr);
            }

            // Setup instances
            var Instances = new List<TLASBuildInstanceData>(NumInstances);
            for (var i = 0; i < NumInstances; i++)
            {
                Instances.Add(new TLASBuildInstanceData());
            }

            var CubeInstData = new CubeInstanceData[] // clang-format off
            {
                new CubeInstanceData{ BasePos = new Vector3( 1, 1,  1), TimeOffset = 0.00f},
                new CubeInstanceData{ BasePos = new Vector3( 2, 0, -1), TimeOffset = 0.53f},
                new CubeInstanceData{ BasePos = new Vector3( -1, 1,  2), TimeOffset = 1.27f},
                new CubeInstanceData{ BasePos = new Vector3( -2, 0, -1), TimeOffset = 4.16f}
            };
            //// clang-format on
            //static_assert(_countof(CubeInstData) == NumCubes, "Cube instance data array size mismatch");

            void AnimateOpaqueCube(TLASBuildInstanceData Dst) //
            {
                float t = MathF.Sin(m_AnimationTime * MathF.PI * 0.5f) + CubeInstData[Dst.CustomId].TimeOffset;
                Vector3 Pos = CubeInstData[Dst.CustomId].BasePos * 2.0f + new Vector3(MathF.Sin(t * 1.13f), MathF.Sin(t * 0.77f), MathF.Sin(t * 2.15f)) * 0.5f;
                float angle = 0.1f * MathF.PI * (m_AnimationTime + CubeInstData[Dst.CustomId].TimeOffset * 2.0f);

                if (!m_EnableCubes[Dst.CustomId])
                    Dst.Mask = 0;

                var mat = Matrix4x4.RotationY(angle);
                mat.SetTranslation(Pos.x, Pos.y, Pos.z);

                Dst.Transform = CreateInstanceMatrix(mat);
            };

            Instances[0].InstanceName = "Cube Instance 1";
            Instances[0].CustomId = 0; // texture index
            Instances[0].pBLAS = m_pCubeBLAS.Obj;
            Instances[0].Mask = RtStructures.OPAQUE_GEOM_MASK;
            AnimateOpaqueCube(Instances[0]);

            Instances[1].InstanceName = "Cube Instance 2";
            Instances[1].CustomId = 1; // texture index
            Instances[1].pBLAS = m_pCubeBLAS.Obj;
            Instances[1].Mask = RtStructures.OPAQUE_GEOM_MASK;
            AnimateOpaqueCube(Instances[1]);

            Instances[2].InstanceName = "Cube Instance 3";
            Instances[2].CustomId = 2; // texture index
            Instances[2].pBLAS = m_pCubeBLAS.Obj;
            Instances[2].Mask = RtStructures.OPAQUE_GEOM_MASK;
            AnimateOpaqueCube(Instances[2]);

            Instances[3].InstanceName = "Cube Instance 4";
            Instances[3].CustomId = 3; // texture index
            Instances[3].pBLAS = m_pCubeBLAS.Obj;
            Instances[3].Mask = RtStructures.OPAQUE_GEOM_MASK;
            AnimateOpaqueCube(Instances[3]);

            Instances[4].InstanceName = "Ground Instance";
            Instances[4].pBLAS = m_pCubeBLAS.Obj;
            Instances[4].Mask = RtStructures.OPAQUE_GEOM_MASK;
            Instances[4].Transform = CreateInstanceMatrix(0.0f, 6.0f, 0.0f, Matrix3x3.Scale(100.0f, 0.1f, 100.0f));

            Instances[5].InstanceName = "Sphere Instance";
            Instances[5].CustomId = 0; // box index
            Instances[5].pBLAS = m_pProceduralBLAS.Obj;
            Instances[5].Mask = RtStructures.OPAQUE_GEOM_MASK;
            Instances[5].Transform = CreateInstanceMatrix(-3.0f, 3.0f, -5f, Matrix3x3.RotationY(0));

            Instances[6].InstanceName = "Glass Instance";
            Instances[6].pBLAS = m_pCubeBLAS.Obj;
            Instances[6].Mask = RtStructures.TRANSPARENT_GEOM_MASK;
            Instances[6].Transform = CreateInstanceMatrix(3.0f, 4.0f, -5.0f, Matrix3x3.Scale(1.5f, 1.5f, 1.5f)); // * Matrix3x3.RotationY(m_AnimationTime * MathF.PI * 0.25f)

            // Build or update TLAS
            var Attribs = new BuildTLASAttribs();
            Attribs.pTLAS = m_pTLAS.Obj;
            Attribs.Update = NeedUpdate;

            // Scratch buffer will be used to store temporary data during TLAS build or update.
            // Previous content in the scratch buffer will be discarded.
            Attribs.pScratchBuffer = m_ScratchBuffer.Obj;

            // Instance buffer will store instance data during TLAS build or update.
            // Previous content in the instance buffer will be discarded.
            Attribs.pInstanceBuffer = m_InstanceBuffer.Obj;

            // Instances will be converted to the format that is required by the graphics driver and copied to the instance buffer.
            Attribs.pInstances = Instances;

            // Bind hit shaders per instance, it allows you to change the number of geometries in BLAS without invalidating the shader binding table.
            Attribs.BindingMode = HIT_GROUP_BINDING_MODE.HIT_GROUP_BINDING_MODE_PER_INSTANCE;
            Attribs.HitGroupStride = RtStructures.HIT_GROUP_STRIDE;

            // Allow engine to change resource states.
            Attribs.TLASTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;
            Attribs.BLASTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;
            Attribs.InstanceBufferTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;
            Attribs.ScratchBufferTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;

            m_pImmediateContext.BuildTLAS(Attribs);
        }

        public static InstanceMatrix CreateInstanceMatrix(in Matrix4x4 input)
        {
            //All of the normal instance matrix constructors do conversion, this is good for general use, but this
            //project's purpose is to track the c++ code more closely. Therefore we want to keep the math in its original
            //form as much as possible.
            return new InstanceMatrix()
            {
                m00 = input.m00,
                m01 = input.m01,
                m02 = input.m02,
                m10 = input.m10,
                m11 = input.m11,
                m12 = input.m12,
                m20 = input.m20,
                m21 = input.m21,
                m22 = input.m22,

                m03 = input.m30,
                m13 = input.m31,
                m23 = input.m32
            };
        }

        public InstanceMatrix CreateInstanceMatrix(float x, float y, float z, in Matrix3x3 rot)
        {
            //All of the normal instance matrix constructors do conversion, this is good for general use, but this
            //project's purpose is to track the c++ code more closely. Therefore we want to keep the math in its original
            //form as much as possible.
            return new InstanceMatrix()
            {
                m00 = rot.m00,
                m01 = rot.m01,
                m02 = rot.m02,
                m10 = rot.m10,
                m11 = rot.m11,
                m12 = rot.m12,
                m20 = rot.m20,
                m21 = rot.m21,
                m22 = rot.m22,
                m03 = x,
                m13 = y,
                m23 = z
            };
        }

        void CreateSBT()
        {
            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pImmediateContext = graphicsEngine.ImmediateContext;
            // Create shader binding table.

            var SBTDesc = new ShaderBindingTableDesc();
            SBTDesc.Name = "SBT";
            SBTDesc.pPSO = m_pRayTracingPSO.Obj;

            m_pSBT = m_pDevice.CreateSBT(SBTDesc);
            //VERIFY_EXPR(m_pSBT != nullptr);

            m_pSBT.Obj.BindRayGenShader("Main", IntPtr.Zero, 0);

            m_pSBT.Obj.BindMissShader("PrimaryMiss", RtStructures.PRIMARY_RAY_INDEX, IntPtr.Zero, 0);
            m_pSBT.Obj.BindMissShader("ShadowMiss", RtStructures.SHADOW_RAY_INDEX, IntPtr.Zero, 0);

            // Hit groups for primary ray
            // clang-format off
            m_pSBT.Obj.BindHitGroupForInstance(m_pTLAS.Obj, "Cube Instance 1", RtStructures.PRIMARY_RAY_INDEX, "CubePrimaryHit", IntPtr.Zero, 0);
            m_pSBT.Obj.BindHitGroupForInstance(m_pTLAS.Obj, "Cube Instance 2", RtStructures.PRIMARY_RAY_INDEX, "CubePrimaryHit", IntPtr.Zero, 0);
            m_pSBT.Obj.BindHitGroupForInstance(m_pTLAS.Obj, "Cube Instance 3", RtStructures.PRIMARY_RAY_INDEX, "CubePrimaryHit", IntPtr.Zero, 0);
            m_pSBT.Obj.BindHitGroupForInstance(m_pTLAS.Obj, "Cube Instance 4", RtStructures.PRIMARY_RAY_INDEX, "CubePrimaryHit", IntPtr.Zero, 0);
            m_pSBT.Obj.BindHitGroupForInstance(m_pTLAS.Obj, "Ground Instance", RtStructures.PRIMARY_RAY_INDEX, "GroundHit", IntPtr.Zero, 0);
            m_pSBT.Obj.BindHitGroupForInstance(m_pTLAS.Obj, "Glass Instance", RtStructures.PRIMARY_RAY_INDEX, "GlassPrimaryHit", IntPtr.Zero, 0);
            m_pSBT.Obj.BindHitGroupForInstance(m_pTLAS.Obj, "Sphere Instance", RtStructures.PRIMARY_RAY_INDEX, "SpherePrimaryHit", IntPtr.Zero, 0);
            // clang-format on

            // Hit groups for shadow ray.
            // null means no shaders are bound and hit shader invocation will be skipped.
            m_pSBT.Obj.BindHitGroupForTLAS(m_pTLAS.Obj, RtStructures.SHADOW_RAY_INDEX, null, IntPtr.Zero, 0);

            // We must specify the intersection shader for procedural geometry.
            m_pSBT.Obj.BindHitGroupForInstance(m_pTLAS.Obj, "Sphere Instance", RtStructures.SHADOW_RAY_INDEX, "SphereShadowHit", IntPtr.Zero, 0);
        }

        public void Dispose()
        {
            m_pColorRT?.Dispose();
            m_pSBT.Dispose();
            m_InstanceBuffer?.Dispose();
            m_ScratchBuffer?.Dispose();
            m_pTLAS?.Dispose();
            m_pProceduralBLAS.Dispose();
            m_BoxAttribsCB.Dispose();
            m_pCubeBLAS.Dispose();
            pCubeIndexBuffer.Dispose();
            pCubeVertexBuffer.Dispose();
            m_CubeAttribsCB.Dispose();
            m_pRayTracingSRB.Dispose();
            m_pRayTracingPSO.Dispose();
            m_pImageBlitPSO.Dispose();
            m_pImageBlitSRB.Dispose();
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
            cameraControls.UpdateInput(clock);

            //var pRTV = swapChain.GetCurrentBackBufferRTV();
            //var pDSV = swapChain.GetDepthBufferDSV();
            //immediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            //var ClearColor = new Color(0.350f, 0.350f, 0.350f, 1.0f);

            //// Clear the back buffer
            //// Let the engine perform required state transitions
            //immediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            //immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);


            /// ------ RT
            /// 
            Render();

            this.swapChain.Present(1);
        }

        public void GetCameraPosition(Vector3 position, Quaternion rotation, in Matrix4x4 preTransformMatrix, in Matrix4x4 CameraProj, out Vector3 CameraWorldPos, out Matrix4x4 CameraViewProj)
        {
            //TODO: See how messed up math is
            //For some reason camera defined backward, so take -position
            var CameraView = Matrix4x4.Translation(-position) * rotation.toRotationMatrix4x4();

            // Apply pretransform matrix that rotates the scene according the surface orientation
            CameraView *= preTransformMatrix;

            var CameraWorld = CameraView.inverse();

            // Get projection matrix adjusted to the current screen orientation
            CameraViewProj = CameraView * CameraProj;
            CameraWorldPos = CameraWorld.GetTranslation();
        }

        private unsafe void Render()
        {
            var m_pSwapChain = graphicsEngine.SwapChain;
            var m_pImmediateContext = graphicsEngine.ImmediateContext;

            UpdateTLAS();

            // Update constants
            {

                var pDSV = swapChain.GetDepthBufferDSV();
                var preTransform = swapChain.GetDesc_PreTransform;

                 //= new Vector3(0f, 0f, -15f);
                var preTransformMatrix = CameraHelpers.GetSurfacePretransformMatrix(new Vector3(0, 0, 1), preTransform);
                var cameraProj = CameraHelpers.GetAdjustedProjectionMatrix(YFov, ZNear, ZFar, window.WindowWidth, window.WindowHeight, preTransform);
                GetCameraPosition(cameraControls.Position, cameraControls.Orientation, preTransformMatrix, cameraProj, out var CameraWorldPos, out var CameraViewProj);

                var Frustum = new ViewFrustum();
                ExtractViewFrustumPlanesFromMatrix(CameraViewProj, Frustum, false);

                // Normalize frustum planes.
                for (ViewFrustum.PLANE_IDX i = 0; i < ViewFrustum.PLANE_IDX.NUM_PLANES; ++i)
                {
                    Plane3D plane = Frustum.GetPlane(i);
                    float invlen = 1.0f / plane.Normal.length();
                    plane.Normal *= invlen;
                    plane.Distance *= invlen;
                }

                // Calculate ray formed by the intersection two planes.
                void GetPlaneIntersection(ViewFrustum.PLANE_IDX lhs, ViewFrustum.PLANE_IDX rhs, out Vector4 result)
                {
                    var lp = Frustum.GetPlane(lhs);
                    var rp = Frustum.GetPlane(rhs);

                    Vector3 dir = lp.Normal.cross(rp.Normal);
                    float len = dir.length2();

                    //VERIFY_EXPR(len > 1.0e-5);

                    var v3result = dir * (1.0f / MathF.Sqrt(len));
                    result = new Vector4(v3result.x, v3result.y, v3result.z, 0);
                };

                // clang-format off
                GetPlaneIntersection(ViewFrustum.PLANE_IDX.BOTTOM_PLANE_IDX, ViewFrustum.PLANE_IDX.LEFT_PLANE_IDX, out m_Constants.FrustumRayLB);
                GetPlaneIntersection(ViewFrustum.PLANE_IDX.LEFT_PLANE_IDX, ViewFrustum.PLANE_IDX.TOP_PLANE_IDX, out m_Constants.FrustumRayLT);
                GetPlaneIntersection(ViewFrustum.PLANE_IDX.RIGHT_PLANE_IDX, ViewFrustum.PLANE_IDX.BOTTOM_PLANE_IDX, out m_Constants.FrustumRayRB);
                GetPlaneIntersection(ViewFrustum.PLANE_IDX.TOP_PLANE_IDX, ViewFrustum.PLANE_IDX.RIGHT_PLANE_IDX, out m_Constants.FrustumRayRT);
                // clang-format on
                m_Constants.CameraPos = new Vector4(CameraWorldPos.x, CameraWorldPos.y, CameraWorldPos.z, 1.0f) * -1.0f;

                fixed (Constants* constantsPtr = &m_Constants)
                {
                    m_pImmediateContext.UpdateBuffer(m_ConstantsCB.Obj, 0, (uint)sizeof(Constants), new IntPtr(constantsPtr), RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
                }
            }

            //Trace rays
            {
                m_pRayTracingSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_GEN, "g_ColorBuffer").Set(m_pColorRT.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_UNORDERED_ACCESS));

                m_pImmediateContext.SetPipelineState(m_pRayTracingPSO.Obj);
                m_pImmediateContext.CommitShaderResources(m_pRayTracingSRB.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

                var Attribs = new TraceRaysAttribs();
                Attribs.DimensionX = m_pColorRT.Obj.GetDesc_Width;
                Attribs.DimensionY = m_pColorRT.Obj.GetDesc_Height;
                Attribs.pSBT = m_pSBT.Obj;

                m_pImmediateContext.TraceRays(Attribs);
            }

            // Blit to swapchain image
            {
                m_pImageBlitSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "g_Texture").Set(m_pColorRT.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));

                var pRTV = m_pSwapChain.GetCurrentBackBufferRTV();
                m_pImmediateContext.SetRenderTarget(pRTV, null, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

                m_pImmediateContext.SetPipelineState(m_pImageBlitPSO.Obj);
                m_pImmediateContext.CommitShaderResources(m_pImageBlitSRB.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

                var Attribs = new DrawAttribs();
                Attribs.NumVertices = 4;
                m_pImmediateContext.Draw(Attribs);
            }
        }

        private void ExtractViewFrustumPlanesFromMatrix(in Matrix4x4 Matrix, ViewFrustum Frustum, bool bIsOpenGL)
        {
            // For more details, see Gribb G., Hartmann K., "Fast Extraction of Viewing Frustum Planes from the
            // World-View-Projection Matrix" (the paper is available at
            // http://gamedevs.org/uploads/fast-extraction-viewing-frustum-planes-from-world-view-projection-matrix.pdf)

            // Left clipping plane
            Frustum.LeftPlane.Normal.x = Matrix.m03 + Matrix.m00;
            Frustum.LeftPlane.Normal.y = Matrix.m13 + Matrix.m10;
            Frustum.LeftPlane.Normal.z = Matrix.m23 + Matrix.m20;
            Frustum.LeftPlane.Distance = Matrix.m33 + Matrix.m30;

            // Right clipping plane
            Frustum.RightPlane.Normal.x = Matrix.m03 - Matrix.m00;
            Frustum.RightPlane.Normal.y = Matrix.m13 - Matrix.m10;
            Frustum.RightPlane.Normal.z = Matrix.m23 - Matrix.m20;
            Frustum.RightPlane.Distance = Matrix.m33 - Matrix.m30;

            // Top clipping plane
            Frustum.TopPlane.Normal.x = Matrix.m03 - Matrix.m01;
            Frustum.TopPlane.Normal.y = Matrix.m13 - Matrix.m11;
            Frustum.TopPlane.Normal.z = Matrix.m23 - Matrix.m21;
            Frustum.TopPlane.Distance = Matrix.m33 - Matrix.m31;

            // Bottom clipping plane
            Frustum.BottomPlane.Normal.x = Matrix.m03 + Matrix.m01;
            Frustum.BottomPlane.Normal.y = Matrix.m13 + Matrix.m11;
            Frustum.BottomPlane.Normal.z = Matrix.m23 + Matrix.m21;
            Frustum.BottomPlane.Distance = Matrix.m33 + Matrix.m31;

            // Near clipping plane
            if (bIsOpenGL)
            {
                // -w <= z <= w
                Frustum.NearPlane.Normal.x = Matrix.m03 + Matrix.m02;
                Frustum.NearPlane.Normal.y = Matrix.m13 + Matrix.m12;
                Frustum.NearPlane.Normal.z = Matrix.m23 + Matrix.m22;
                Frustum.NearPlane.Distance = Matrix.m33 + Matrix.m32;
            }
            else
            {
                // 0 <= z <= w
                Frustum.NearPlane.Normal.x = Matrix.m02;
                Frustum.NearPlane.Normal.y = Matrix.m12;
                Frustum.NearPlane.Normal.z = Matrix.m22;
                Frustum.NearPlane.Distance = Matrix.m32;
            }

            // Far clipping plane
            Frustum.FarPlane.Normal.x = Matrix.m03 - Matrix.m02;
            Frustum.FarPlane.Normal.y = Matrix.m13 - Matrix.m12;
            Frustum.FarPlane.Normal.z = Matrix.m23 - Matrix.m22;
            Frustum.FarPlane.Distance = Matrix.m33 - Matrix.m32;
        }

        void WindowResize(UInt32 Width, UInt32 Height)
        {
            var m_pDevice = graphicsEngine.RenderDevice;
            // Update projection matrix.
            float AspectRatio = (float)Width / (float)Height;
            //m_Camera.SetProjAttribs(m_Constants.ClipPlanes.x, m_Constants.ClipPlanes.y, AspectRatio, PI_F / 4.f,
            //                        m_pSwapChain->GetDesc().PreTransform, m_pDevice->GetDeviceCaps().IsGLDevice());

            // Check if the image needs to be recreated.
            if (m_pColorRT != null &&
                m_pColorRT.Obj.GetDesc_Width == Width &&
                m_pColorRT.Obj.GetDesc_Height == Height)
            {
                return;
            }

            if (Width == 0 || Height == 0)
                return;

            m_pColorRT?.Dispose();
            m_pColorRT = null;

            // Create window-size color image.
            var RTDesc = new TextureDesc();
            RTDesc.Name = "Color buffer";
            RTDesc.Type = RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D;
            RTDesc.Width = Width;
            RTDesc.Height = Height;
            RTDesc.BindFlags = BIND_FLAGS.BIND_UNORDERED_ACCESS | BIND_FLAGS.BIND_SHADER_RESOURCE;
            RTDesc.ClearValue.Format = m_ColorBufferFormat;
            RTDesc.Format = m_ColorBufferFormat;

            m_pColorRT = m_pDevice.CreateTexture(RTDesc, null);
        }
    }
}
