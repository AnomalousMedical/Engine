using DiligentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Tutorial_99_Pbo
{
    class GLTF_PBR_Renderer : IDisposable
    {
        static Uint32     BRDF_LUT_Dim = 512;

        public class CreateInfo
        {
            /// Render target format.
            public TEXTURE_FORMAT RTVFmt = TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN;

            /// Depth-buffer format.

            /// \note   If both RTV and DSV formats are TEX_FORMAT_UNKNOWN,
            ///         the renderer will not initialize PSO, uniform buffers and other
            ///         resources. It is expected that an application will use custom
            ///         render callback function.
            public TEXTURE_FORMAT DSVFmt = TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN;

            /// Indicates if front face is CCW.
            public bool FrontCCW = false;

            /// Indicates if the renderer should allow debug views.
            /// Rendering with debug views disabled is more efficient.
            public bool AllowDebugView = false;

            /// Indicates whether to use IBL.
            public bool UseIBL = false;

            /// Whether to use ambient occlusion texture.
            public bool UseAO = true;

            /// Whether to use emissive texture.
            public bool UseEmissive = true;

            /// When set to true, pipeline state will be compiled with immutable samplers.
            /// When set to false, samplers from the texture views will be used.
            public bool UseImmutableSamplers = true;

            /// Whether to use texture atlas (e.g. apply UV transforms when sampling textures).
            public bool UseTextureAtals = false;

            /// Immutable sampler for color map texture.
            public SamplerDesc ColorMapImmutableSampler = new SamplerDesc();

            /// Immutable sampler for physical description map texture.
            public SamplerDesc PhysDescMapImmutableSampler = new SamplerDesc();

            /// Immutable sampler for normal map texture.
            public SamplerDesc NormalMapImmutableSampler = new SamplerDesc();

            /// Immutable sampler for AO texture.
            public SamplerDesc AOMapImmutableSampler = new SamplerDesc();

            /// Immutable sampler for emissive map texture.
            public SamplerDesc EmissiveMapImmutableSampler = new SamplerDesc();

            /// Maximum number of joints
            public Uint32 MaxJointCount = 64;
        };

        private CreateInfo m_Settings;
        AutoPtr<ITextureView> m_pBRDF_LUT_SRV;

        public GLTF_PBR_Renderer(IRenderDevice pDevice, IDeviceContext pCtx, CreateInfo CI, ShaderLoader shaderLoader)
        {
            this.m_Settings = CI;

            if (m_Settings.UseIBL)
            {
                PrecomputeBRDF(pDevice, pCtx, shaderLoader);

                //TextureDesc TexDesc;
                //TexDesc.Name = "Irradiance cube map for GLTF renderer";
                //TexDesc.Type = RESOURCE_DIM_TEX_CUBE;
                //TexDesc.Usage = USAGE_DEFAULT;
                //TexDesc.BindFlags = BIND_SHADER_RESOURCE | BIND_RENDER_TARGET;
                //TexDesc.Width = IrradianceCubeDim;
                //TexDesc.Height = IrradianceCubeDim;
                //TexDesc.Format = IrradianceCubeFmt;
                //TexDesc.ArraySize = 6;
                //TexDesc.MipLevels = 0;

                //RefCntAutoPtr<ITexture> IrradainceCubeTex;
                //pDevice->CreateTexture(TexDesc, nullptr, &IrradainceCubeTex);
                //m_pIrradianceCubeSRV = IrradainceCubeTex->GetDefaultView(TEXTURE_VIEW_SHADER_RESOURCE);

                //TexDesc.Name = "Prefiltered environment map for GLTF renderer";
                //TexDesc.Width = PrefilteredEnvMapDim;
                //TexDesc.Height = PrefilteredEnvMapDim;
                //TexDesc.Format = PrefilteredEnvMapFmt;
                //RefCntAutoPtr<ITexture> PrefilteredEnvMapTex;
                //pDevice->CreateTexture(TexDesc, nullptr, &PrefilteredEnvMapTex);
                //m_pPrefilteredEnvMapSRV = PrefilteredEnvMapTex->GetDefaultView(TEXTURE_VIEW_SHADER_RESOURCE);
            }

        //    {
        //        static constexpr Uint32 TexDim = 8;

        //        TextureDesc TexDesc;
        //        TexDesc.Name = "White texture for GLTF renderer";
        //        TexDesc.Type = RESOURCE_DIM_TEX_2D_ARRAY;
        //        TexDesc.Usage = USAGE_IMMUTABLE;
        //        TexDesc.BindFlags = BIND_SHADER_RESOURCE;
        //        TexDesc.Width = TexDim;
        //        TexDesc.Height = TexDim;
        //        TexDesc.Format = TEX_FORMAT_RGBA8_UNORM;
        //        TexDesc.MipLevels = 1;
        //        std::vector<Uint32> Data(TexDim* TexDim, 0xFFFFFFFF);
        //        TextureSubResData Level0Data{ Data.data(), TexDim * 4};
        //        TextureData InitData{ &Level0Data, 1};
        //        RefCntAutoPtr<ITexture> pWhiteTex;
        //        pDevice->CreateTexture(TexDesc, &InitData, &pWhiteTex);
        //        m_pWhiteTexSRV = pWhiteTex->GetDefaultView(TEXTURE_VIEW_SHADER_RESOURCE);

        //        TexDesc.Name = "Black texture for GLTF renderer";
        //        for (auto & c : Data) c = 0;
        //        RefCntAutoPtr<ITexture> pBlackTex;
        //        pDevice->CreateTexture(TexDesc, &InitData, &pBlackTex);
        //        m_pBlackTexSRV = pBlackTex->GetDefaultView(TEXTURE_VIEW_SHADER_RESOURCE);

        //        TexDesc.Name = "Default normal map for GLTF renderer";
        //        for (auto & c : Data) c = 0x00FF7F7F;
        //        RefCntAutoPtr<ITexture> pDefaultNormalMap;
        //        pDevice->CreateTexture(TexDesc, &InitData, &pDefaultNormalMap);
        //        m_pDefaultNormalMapSRV = pDefaultNormalMap->GetDefaultView(TEXTURE_VIEW_SHADER_RESOURCE);

        //        TexDesc.Name = "Default physical description map for GLTF renderer";
        //        for (auto & c : Data) c = 0x0000FF00;
        //        RefCntAutoPtr<ITexture> pDefaultPhysDesc;
        //        pDevice->CreateTexture(TexDesc, &InitData, &pDefaultPhysDesc);
        //        m_pDefaultPhysDescSRV = pDefaultPhysDesc->GetDefaultView(TEXTURE_VIEW_SHADER_RESOURCE);

        //        // clang-format off
        //        StateTransitionDesc Barriers[] =
        //        {
        //    {pWhiteTex,         RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_SHADER_RESOURCE, true},
        //    {pBlackTex,         RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_SHADER_RESOURCE, true},
        //    {pDefaultNormalMap, RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_SHADER_RESOURCE, true},
        //    {pDefaultPhysDesc,  RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_SHADER_RESOURCE, true}
        //};
        //        // clang-format on
        //        pCtx->TransitionResourceStates(_countof(Barriers), Barriers);

        //        RefCntAutoPtr<ISampler> pDefaultSampler;
        //        pDevice->CreateSampler(Sam_LinearClamp, &pDefaultSampler);
        //        m_pWhiteTexSRV->SetSampler(pDefaultSampler);
        //        m_pBlackTexSRV->SetSampler(pDefaultSampler);
        //        m_pDefaultNormalMapSRV->SetSampler(pDefaultSampler);
        //    }

        //    if (CI.RTVFmt != TEX_FORMAT_UNKNOWN || CI.DSVFmt != TEX_FORMAT_UNKNOWN)
        //    {
        //        CreateUniformBuffer(pDevice, sizeof(GLTFNodeShaderTransforms), "GLTF node transforms CB", &m_TransformsCB);
        //        CreateUniformBuffer(pDevice, sizeof(GLTFMaterialShaderInfo) + sizeof(GLTFRendererShaderParameters), "GLTF attribs CB", &m_GLTFAttribsCB);
        //        CreateUniformBuffer(pDevice, static_cast<Uint32>(sizeof(float4x4) * m_Settings.MaxJointCount), "GLTF joint tranforms", &m_JointsBuffer);

        //        // clang-format off
        //        StateTransitionDesc Barriers[] =
        //        {
        //    {m_TransformsCB,  RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_CONSTANT_BUFFER, true},
        //    {m_GLTFAttribsCB, RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_CONSTANT_BUFFER, true},
        //    {m_JointsBuffer,  RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_CONSTANT_BUFFER, true}
        //};
        //        // clang-format on
        //        pCtx->TransitionResourceStates(_countof(Barriers), Barriers);

        //        CreatePSO(pDevice);
        //    }
        }

        public void Dispose()
        {
            m_pBRDF_LUT_SRV.Dispose();
        }
        public void PrecomputeBRDF(IRenderDevice pDevice, IDeviceContext pCtx, ShaderLoader shaderLoader)
        {
            TextureDesc TexDesc = new TextureDesc();
            TexDesc.Name = "GLTF BRDF Look-up texture";
            TexDesc.Type = RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D;
            TexDesc.Usage = USAGE.USAGE_DEFAULT;
            TexDesc.BindFlags = BIND_FLAGS.BIND_SHADER_RESOURCE | BIND_FLAGS.BIND_RENDER_TARGET;
            TexDesc.Width = BRDF_LUT_Dim;
            TexDesc.Height = BRDF_LUT_Dim;
            TexDesc.Format = TEXTURE_FORMAT.TEX_FORMAT_RG16_FLOAT;
            TexDesc.MipLevels = 1;
            using var pBRDF_LUT = pDevice.CreateTexture(TexDesc, null);
            m_pBRDF_LUT_SRV = new AutoPtr<ITextureView>(pBRDF_LUT.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));

            GraphicsPipelineStateCreateInfo PSOCreateInfo = new GraphicsPipelineStateCreateInfo();
            PipelineStateDesc PSODesc = PSOCreateInfo.PSODesc;
            GraphicsPipelineDesc GraphicsPipeline = PSOCreateInfo.GraphicsPipeline;

            PSODesc.Name = "Precompute GLTF BRDF LUT PSO";
            PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_GRAPHICS;

            GraphicsPipeline.NumRenderTargets = 1;
            GraphicsPipeline.RTVFormats_0 = TexDesc.Format;
            GraphicsPipeline.PrimitiveTopology = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
            GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE.CULL_MODE_NONE;
            GraphicsPipeline.DepthStencilDesc.DepthEnable = false;

            ShaderCreateInfo ShaderCI = new ShaderCreateInfo();
            ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;
            ShaderCI.UseCombinedTextureSamplers = true;
            //ShaderCI.pShaderSourceStreamFactory = &DiligentFXShaderSourceStreamFactory::GetInstance();
            //Need to figure out shader sources

            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_VERTEX;
            ShaderCI.EntryPoint = "FullScreenTriangleVS";
            ShaderCI.Desc.Name = "Full screen triangle VS";
            ShaderCI.Source = shaderLoader.LoadShader("Common/private/FullScreenTriangleVS.fx");
            using var pVS = pDevice.CreateShader(ShaderCI);

            // Create pixel shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_PIXEL;
            ShaderCI.EntryPoint = "PrecomputeBRDF_PS";
            ShaderCI.Desc.Name = "Precompute GLTF BRDF PS";
            ShaderCI.Source = shaderLoader.LoadShader("GLTF_PBR/private/PrecomputeGLTF_BRDF.psh", "Common/private/", "Common/public/");
            using var pPS = pDevice.CreateShader(ShaderCI);

            // Finally, create the pipeline state
            PSOCreateInfo.pVS = pVS.Obj;
            PSOCreateInfo.pPS = pPS.Obj;
            using var PrecomputeBRDF_PSO = pDevice.CreateGraphicsPipelineState(PSOCreateInfo);
            pCtx.SetPipelineState(PrecomputeBRDF_PSO.Obj);

            var pRTVs = pBRDF_LUT.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_RENDER_TARGET); //Only one of these
            pCtx.SetRenderTarget(pRTVs, null, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            var attrs = new DrawAttribs { NumVertices = 3, Flags = DRAW_FLAGS.DRAW_FLAG_VERIFY_ALL };
            pCtx.Draw(attrs);

            var Barriers = new List<StateTransitionDesc>()
            {
                new StateTransitionDesc{pResource = pBRDF_LUT.Obj, OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_SHADER_RESOURCE, UpdateResourceState = true}
            };
            pCtx.TransitionResourceStates(Barriers);
        }
    }
}
