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

namespace RTSandbox
{
    class RTSandboxUpdateListener : UpdateListener, IDisposable
    {
        //Camera Settings
        float YFov = MathFloat.PI / 4.0f;
        float ZNear = 0.1f;
        float ZFar = 100f;

        private readonly GraphicsEngine graphicsEngine;
        private readonly NativeOSWindow window;
        private readonly FirstPersonFlyCamera cameraControls;
        private readonly VirtualFileSystem virtualFileSystem;
        private readonly RTImageBlitter imageBlitter;
        private readonly CubeBLAS cubeBLAS;
        private readonly ProceduralBLAS proceduralBLAS;
        private readonly RTCameraAndLight cameraAndLight;
        private readonly RTInstances rtInstances;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext immediateContext;

        UInt32 m_MaxRecursionDepth = 8;

        const int NumTextures = 4;
        const int NumCubes = 4;

        private AutoPtr<IBuffer> m_ConstantsCB;
        private Constants m_Constants;
        private AutoPtr<IPipelineState> m_pRayTracingPSO;
        private AutoPtr<IShaderResourceBinding> m_pRayTracingSRB;

        AutoPtr<ITopLevelAS> m_pTLAS;
        AutoPtr<IBuffer> m_ScratchBuffer;
        AutoPtr<IBuffer> m_InstanceBuffer;

        AutoPtr<IShaderBindingTable> m_pSBT;

        public unsafe RTSandboxUpdateListener
        (
            GraphicsEngine graphicsEngine, 
            ShaderLoader<RTShaders> shaderLoader, 
            TextureLoader textureLoader, 
            NativeOSWindow window,
            FirstPersonFlyCamera cameraControls,
            VirtualFileSystem virtualFileSystem,
            RTImageBlitter imageBlitter,
            CubeBLAS cubeBLAS,
            ProceduralBLAS proceduralBLAS,
            RTCameraAndLight cameraAndLight,
            RTInstances blasInstances,
            IRTSandboxScene scene
        )
        {
            this.graphicsEngine = graphicsEngine;
            this.window = window;
            this.cameraControls = cameraControls;
            this.virtualFileSystem = virtualFileSystem;
            this.imageBlitter = imageBlitter;
            this.cubeBLAS = cubeBLAS;
            this.proceduralBLAS = proceduralBLAS;
            this.cameraAndLight = cameraAndLight;
            this.rtInstances = blasInstances;
            this.swapChain = graphicsEngine.SwapChain;
            this.immediateContext = graphicsEngine.ImmediateContext;

            var m_pDevice = graphicsEngine.RenderDevice;

            // Create a buffer with shared constants.
            BufferDesc BuffDesc = new BufferDesc();
            BuffDesc.Name = "Constant buffer";
            BuffDesc.uiSizeInBytes = (uint)sizeof(Constants);
            BuffDesc.Usage = USAGE.USAGE_DEFAULT;
            BuffDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;

            m_ConstantsCB = m_pDevice.CreateBuffer(BuffDesc);
            //VERIFY_EXPR(m_ConstantsCB != nullptr);

            CreateRayTracingPSO(shaderLoader);
            LoadTextures(textureLoader);
            m_pRayTracingSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, "g_CubeAttribsCB").Set(cubeBLAS.Attribs);
            m_pRayTracingSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_INTERSECTION, "g_BoxAttribs").Set(proceduralBLAS.Attribs.GetDefaultView(BUFFER_VIEW_TYPE.BUFFER_VIEW_SHADER_RESOURCE));
            UpdateTLAS();
            CreateSBT();

            // Initialize constants, uses stuff setup above so order is important.
            m_Constants = Constants.CreateDefault(m_MaxRecursionDepth);
        }

        unsafe void CreateRayTracingPSO(ShaderLoader shaderLoader)
        {
            var m_pDevice = graphicsEngine.RenderDevice;

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

            // Create ray generation shader.
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_RAY_GEN;
            ShaderCI.Desc.Name = "Ray tracing RG";
            ShaderCI.Source = shaderLoader.LoadShader("assets/RayTrace.rgen");
            ShaderCI.EntryPoint = "main";
            using var pRayGen = m_pDevice.CreateShader(ShaderCI, Macros);
            //VERIFY_EXPR(pRayGen != nullptr);


            // Create miss shaders.
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_RAY_MISS;
            ShaderCI.Desc.Name = "Primary ray miss shader";
            ShaderCI.Source = shaderLoader.LoadShader("assets/PrimaryMiss.rmiss");
            ShaderCI.EntryPoint = "main";
            using var pPrimaryMiss = m_pDevice.CreateShader(ShaderCI, Macros);
            //VERIFY_EXPR(pPrimaryMiss != nullptr);

            ShaderCI.Desc.Name = "Shadow ray miss shader";
            ShaderCI.Source = shaderLoader.LoadShader("assets/ShadowMiss.rmiss");
            ShaderCI.EntryPoint = "main";
            using var pShadowMiss = m_pDevice.CreateShader(ShaderCI, Macros);
            //VERIFY_EXPR(pShadowMiss != nullptr);

            // Create closest hit shaders.
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT;
            ShaderCI.Desc.Name = "Cube primary ray closest hit shader";
            ShaderCI.Source = shaderLoader.LoadShader("assets/CubePrimaryHit.rchit");
            ShaderCI.EntryPoint = "main";
            using var pCubePrimaryHit = m_pDevice.CreateShader(ShaderCI, Macros);
            //VERIFY_EXPR(pCubePrimaryHit != nullptr);

            ShaderCI.Desc.Name = "Ground primary ray closest hit shader";
            ShaderCI.Source = shaderLoader.LoadShader("assets/Ground.rchit");
            ShaderCI.EntryPoint = "main";
            using var pGroundHit = m_pDevice.CreateShader(ShaderCI, Macros);
            //VERIFY_EXPR(pGroundHit != nullptr);

            ShaderCI.Desc.Name = "Glass primary ray closest hit shader";
            //ShaderCI.FilePath = "GlassPrimaryHit.rchit";
            ShaderCI.Source = shaderLoader.LoadShader("assets/GlassPrimaryHit.rchit");
            ShaderCI.EntryPoint = "main";
            using var pGlassPrimaryHit = m_pDevice.CreateShader(ShaderCI, Macros);
            //VERIFY_EXPR(pGlassPrimaryHit != nullptr);

            ShaderCI.Desc.Name = "Sphere primary ray closest hit shader";
            ShaderCI.Source = shaderLoader.LoadShader("assets/SpherePrimaryHit.rchit");
            ShaderCI.EntryPoint = "main";
            using var pSpherePrimaryHit = m_pDevice.CreateShader(ShaderCI, Macros);
            //VERIFY_EXPR(pSpherePrimaryHit != nullptr);

            //// Create intersection shader for a procedural sphere.
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_RAY_INTERSECTION;
            ShaderCI.Desc.Name = "Sphere intersection shader";
            ShaderCI.Source = shaderLoader.LoadShader("assets/SphereIntersection.rint");
            ShaderCI.EntryPoint = "main";
            using var pSphereIntersection = m_pDevice.CreateShader(ShaderCI, Macros);
            //VERIFY_EXPR(pSphereIntersection != nullptr);

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

            // Specify the maximum ray recursion depth.
            // WARNING: the driver does not track the recursion depth and it is the
            //          application's responsibility to not exceed the specified limit.
            //          The value is used to reserve the necessary stack size and
            //          exceeding it will likely result in driver crash.
            PSOCreateInfo.RayTracingPipeline.MaxRecursionDepth = (byte)m_MaxRecursionDepth;

            // Per-shader data is not used.
            PSOCreateInfo.RayTracingPipeline.ShaderRecordSize = 0;

            // DirectX 12 only: set attribute and payload size. Values should be as small as possible to minimize the memory usage.
            PSOCreateInfo.MaxAttributeSize = (uint)Math.Max(sizeof(/*BuiltInTriangleIntersectionAttributes*/ Vector2), sizeof(HLSL.ProceduralGeomIntersectionAttribs));
            PSOCreateInfo.MaxPayloadSize = (uint)Math.Max(sizeof(HLSL.PrimaryRayPayload), sizeof(HLSL.ShadowRayPayload));


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
            var m_pImmediateContext = graphicsEngine.ImmediateContext;

            // Load textures
            var pTexSRVs = new List<IDeviceObject>(NumTextures);
            var pTexNormalSRVs = new List<IDeviceObject>(NumTextures);
            var pTex = new List<AutoPtr<ITexture>>(NumTextures);
            var Barriers = new List<StateTransitionDesc>(NumTextures);
            try
            {
                var fileNames = new[]
                {
                    "Rock019",
                    "Rock029",
                    "MetalPlates001",
                    "Fabric021"
                };

                for (int tex = 0; tex < NumTextures; ++tex)
                {
                    {
                        var textureFile = $"cc0Textures/{fileNames[tex]}_1K_Color.jpg";

                        using var logoStream = virtualFileSystem.openStream(textureFile, FileMode.Open);
                        var color = textureLoader.LoadTexture(logoStream, $"Color {tex} Texture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D, true);
                        pTex.Add(color);

                        // Get shader resource view from the texture
                        var pTextureSRV = color.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);
                        pTexSRVs.Add(pTextureSRV);
                        Barriers.Add(new StateTransitionDesc { pResource = color.Obj, OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_SHADER_RESOURCE, UpdateResourceState = true });
                    }

                    {
                        var textureFile = $"cc0Textures/{fileNames[tex]}_1K_Normal.jpg";

                        using var logoStream = virtualFileSystem.openStream(textureFile, FileMode.Open);
                        var normal = textureLoader.LoadTexture(logoStream, $"Normal {tex} Texture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D, true);
                        pTex.Add(normal);

                        // Get shader resource view from the texture
                        var pTextureSRV = normal.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);
                        pTexNormalSRVs.Add(pTextureSRV);
                        Barriers.Add(new StateTransitionDesc { pResource = normal.Obj, OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_SHADER_RESOURCE, UpdateResourceState = true });
                    }
                }
                m_pImmediateContext.TransitionResourceStates(Barriers);

                m_pRayTracingSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, "g_CubeTextures").SetArray(pTexSRVs);
                m_pRayTracingSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, "g_CubeNormalTextures").SetArray(pTexNormalSRVs);

                // Load ground texture
                {
                    var ground = "cc0Textures/RoofingTiles006_1K_Color.jpg";

                    using var stream = virtualFileSystem.openStream(ground, FileMode.Open, FileAccess.Read, FileShare.Read);
                    using var pGroundTex = textureLoader.LoadTexture(stream, "Ground Texture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D, true);
                    // Get shader resource view from the texture
                    var m_TextureSRV = pGroundTex.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);

                    m_pRayTracingSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, "g_GroundTexture").Set(m_TextureSRV);
                }
                {
                    var ground = "cc0Textures/RoofingTiles006_1K_Normal.jpg";

                    using var stream = virtualFileSystem.openStream(ground, FileMode.Open, FileAccess.Read, FileShare.Read);
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

        void UpdateTLAS()
        {
            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pImmediateContext = graphicsEngine.ImmediateContext;
            // Create or update top-level acceleration structure

            bool NeedUpdate = true;

            uint numInstances = (uint)rtInstances.Instances.Count;

            // Create TLAS
            if (m_pTLAS == null)
            {
                var TLASDesc = new TopLevelASDesc();
                TLASDesc.Name = "TLAS";
                TLASDesc.MaxInstanceCount = numInstances;
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
                BuffDesc.uiSizeInBytes = ITopLevelAS.TLAS_INSTANCE_DATA_SIZE * numInstances;

                m_InstanceBuffer = m_pDevice.CreateBuffer(BuffDesc, new BufferData());
                //VERIFY_EXPR(m_InstanceBuffer != nullptr);
            }

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
            Attribs.pInstances = rtInstances.Instances;

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
        void CreateSBT()
        {
            var m_pDevice = graphicsEngine.RenderDevice;
            // Create shader binding table.

            var SBTDesc = new ShaderBindingTableDesc();
            SBTDesc.Name = "SBT";
            SBTDesc.pPSO = m_pRayTracingPSO.Obj;

            m_pSBT = m_pDevice.CreateSBT(SBTDesc);
            //VERIFY_EXPR(m_pSBT != nullptr);

            m_pSBT.Obj.BindRayGenShader("Main", IntPtr.Zero);

            m_pSBT.Obj.BindMissShader("PrimaryMiss", RtStructures.PRIMARY_RAY_INDEX, IntPtr.Zero);
            m_pSBT.Obj.BindMissShader("ShadowMiss", RtStructures.SHADOW_RAY_INDEX, IntPtr.Zero);

            // Hit groups for primary ray
            rtInstances.BindShaders(m_pSBT.Obj, m_pTLAS.Obj);

            // Hit groups for shadow ray.
            // null means no shaders are bound and hit shader invocation will be skipped.
            m_pSBT.Obj.BindHitGroupForTLAS(m_pTLAS.Obj, RtStructures.SHADOW_RAY_INDEX, null, IntPtr.Zero);
        }

        public void Dispose()
        {
            m_pSBT.Dispose();
            m_InstanceBuffer?.Dispose();
            m_ScratchBuffer?.Dispose();
            m_pTLAS?.Dispose();
            m_pRayTracingSRB.Dispose();
            m_pRayTracingPSO.Dispose();
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

            //This is the old clear loop, leaving in place in case we want or need the screen clear, but I think with pure rt there is no need
            //since we blit a texture to the full screen over and over.
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
                cameraAndLight.GetCameraPosition(cameraControls.Position, cameraControls.Orientation, preTransformMatrix, cameraProj, out var CameraWorldPos, out var CameraViewProj);

                var Frustum = new ViewFrustum();
                cameraAndLight.ExtractViewFrustumPlanesFromMatrix(CameraViewProj, Frustum, false);

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

                GetPlaneIntersection(ViewFrustum.PLANE_IDX.BOTTOM_PLANE_IDX, ViewFrustum.PLANE_IDX.LEFT_PLANE_IDX, out m_Constants.FrustumRayLB);
                GetPlaneIntersection(ViewFrustum.PLANE_IDX.LEFT_PLANE_IDX, ViewFrustum.PLANE_IDX.TOP_PLANE_IDX, out m_Constants.FrustumRayLT);
                GetPlaneIntersection(ViewFrustum.PLANE_IDX.RIGHT_PLANE_IDX, ViewFrustum.PLANE_IDX.BOTTOM_PLANE_IDX, out m_Constants.FrustumRayRB);
                GetPlaneIntersection(ViewFrustum.PLANE_IDX.TOP_PLANE_IDX, ViewFrustum.PLANE_IDX.RIGHT_PLANE_IDX, out m_Constants.FrustumRayRT);
                m_Constants.CameraPos = new Vector4(CameraWorldPos.x, CameraWorldPos.y, CameraWorldPos.z, 1.0f) * -1.0f;

                fixed (Constants* constantsPtr = &m_Constants)
                {
                    m_pImmediateContext.UpdateBuffer(m_ConstantsCB.Obj, 0, (uint)sizeof(Constants), new IntPtr(constantsPtr), RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
                }
            }

            //Trace rays
            {
                m_pRayTracingSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_GEN, "g_ColorBuffer").Set(imageBlitter.TextureView);

                m_pImmediateContext.SetPipelineState(m_pRayTracingPSO.Obj);
                m_pImmediateContext.CommitShaderResources(m_pRayTracingSRB.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

                var Attribs = new TraceRaysAttribs();
                Attribs.DimensionX = imageBlitter.Width;
                Attribs.DimensionY = imageBlitter.Height;
                Attribs.pSBT = m_pSBT.Obj;

                m_pImmediateContext.TraceRays(Attribs);
            }

            // Blit to swapchain image
            imageBlitter.Blit();
        }
    }
}
