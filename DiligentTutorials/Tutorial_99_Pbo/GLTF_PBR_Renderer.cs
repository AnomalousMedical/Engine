using DiligentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

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
using System.Collections;
using Engine;

namespace Tutorial_99_Pbo
{
    enum PBR_WORKFLOW
    {
        PBR_WORKFLOW_METALL_ROUGH = 0,
        PBR_WORKFLOW_SPEC_GLOSS
    };

    enum ALPHA_MODE
    {
        ALPHA_MODE_OPAQUE = 0,
        ALPHA_MODE_MASK,
        ALPHA_MODE_BLEND,
        ALPHA_MODE_NUM_MODES
    };

    [StructLayout(LayoutKind.Sequential)]
    struct GLTFNodeShaderTransforms
    {
        public float4x4 NodeMatrix;

        public int JointCount;
        public float Dummy0;
        public float Dummy1;
        public float Dummy2;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct GLTFShaderAttribs
    {
        public static GLTFShaderAttribs CreateDefault()
        {
            return new GLTFShaderAttribs()
            {
                BaseColorFactor = new float4(1, 1, 1, 1),
                EmissiveFactor = new float4(1, 1, 1, 1),
                SpecularFactor = new float4(1, 1, 1, 1),

                Workflow = (int)PBR_WORKFLOW.PBR_WORKFLOW_METALL_ROUGH,
                BaseColorTextureUVSelector = -1,
                PhysicalDescriptorTextureUVSelector = -1,
                NormalTextureUVSelector = -1,

                OcclusionTextureUVSelector = -1,
                EmissiveTextureUVSelector = -1,
                BaseColorSlice = 0,
                PhysicalDescriptorSlice = 0,

                NormalSlice = 0,
                OcclusionSlice = 0,
                EmissiveSlice = 0,
                MetallicFactor = 1,

                RoughnessFactor = 1,
                AlphaMode = (int)ALPHA_MODE.ALPHA_MODE_OPAQUE,
                AlphaMaskCutoff = 0.5f,
                //Dummy0,

                // When texture atlas is used, UV scale and bias applied to
                // each texture coordinate set
                BaseColorUVScaleBias = new float4(1, 1, 0, 0),
                PhysicalDescriptorUVScaleBias = new float4(1, 1, 0, 0),
                NormalMapUVScaleBias = new float4(1, 1, 0, 0),
                OcclusionUVScaleBias = new float4(1, 1, 0, 0),
                EmissiveUVScaleBias = new float4(1, 1, 0, 0),
            };
        }

        public float4 BaseColorFactor;
        public float4 EmissiveFactor;
        public float4 SpecularFactor;

        public int Workflow;
        public float BaseColorTextureUVSelector;
        public float PhysicalDescriptorTextureUVSelector;
        public float NormalTextureUVSelector;

        public float OcclusionTextureUVSelector;
        public float EmissiveTextureUVSelector;
        public float BaseColorSlice;
        public float PhysicalDescriptorSlice;

        public float NormalSlice;
        public float OcclusionSlice;
        public float EmissiveSlice;
        public float MetallicFactor;

        public float RoughnessFactor;
        public int AlphaMode;
        public float AlphaMaskCutoff;
        public float Dummy0;

        // When texture atlas is used, UV scale and bias applied to
        // each texture coordinate set
        public float4 BaseColorUVScaleBias;
        public float4 PhysicalDescriptorUVScaleBias;
        public float4 NormalMapUVScaleBias;
        public float4 OcclusionUVScaleBias;
        public float4 EmissiveUVScaleBias;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct GLTFRendererShaderParameters
    {
        public float AverageLogLum;
        public float MiddleGray;
        public float WhitePoint;
        public float PrefilteredCubeMipLevels;

        public float IBLScale;
        public int DebugViewType;
        public float OcclusionStrength;
        public float EmissionScale;
    };

    [StructLayout(LayoutKind.Sequential)]
    struct GLTFAttribs
    {
        public GLTFRendererShaderParameters RenderParameters;
        public GLTFShaderAttribs MaterialInfo;
    }

    enum DebugViewType : int
    {
        None = 0,
        BaseColor = 1,
        Transparency = 2,
        NormalMap = 3,
        Occlusion = 4,
        Emissive = 5,
        Metallic = 6,
        Roughness = 7,
        DiffuseColor = 8,
        SpecularColor = 9,
        Reflectance90 = 10,
        MeshNormal = 11,
        PerturbedNormal = 12,
        NdotV = 13,
        DiffuseIBL = 14,
        SpecularIBL = 15,
        NumDebugViews
    }

    class GLTF_PBR_Renderer : IDisposable
    {

        static readonly SamplerDesc Sam_LinearClamp = new SamplerDesc
        {
            MinFilter = FILTER_TYPE.FILTER_TYPE_LINEAR,
            MagFilter = FILTER_TYPE.FILTER_TYPE_LINEAR,
            MipFilter = FILTER_TYPE.FILTER_TYPE_LINEAR,
            AddressU = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_CLAMP,
            AddressV = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_CLAMP,
            AddressW = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_CLAMP
        };

        const Uint32 TexDim = 8;

        const Uint32 BRDF_LUT_Dim = 512;
        const TEXTURE_FORMAT IrradianceCubeFmt = TEXTURE_FORMAT.TEX_FORMAT_RGBA32_FLOAT;
        const TEXTURE_FORMAT PrefilteredEnvMapFmt = TEXTURE_FORMAT.TEX_FORMAT_RGBA16_FLOAT;
        const Uint32 IrradianceCubeDim = 64;
        const Uint32 PrefilteredEnvMapDim = 256;

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
            public bool UseTextureAtlas = false;

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
        }
        class PSOKey
        {
            public ALPHA_MODE AlphaMode = ALPHA_MODE.ALPHA_MODE_OPAQUE;
            public bool DoubleSided = false;
        }

        class PSOCache : IDisposable
        {
            private List<AutoPtr<IPipelineState>> m_PSOCache = new List<AutoPtr<IPipelineState>>();

            static uint GetPSOIdx(PSOKey Key)
            {
                uint PSOIdx;

                PSOIdx = Key.AlphaMode == ALPHA_MODE.ALPHA_MODE_BLEND ? 1 : 0;
                PSOIdx = (uint)(PSOIdx * 2 + (Key.DoubleSided ? 1 : 0));
                return PSOIdx;
            }

            /// <summary>
            /// Add a pso. Creates its own AutoPtr. Caller must dispose their AutoPtr.
            /// </summary>
            /// <param name="Key"></param>
            /// <param name="pPSO"></param>
            public void AddPSO(PSOKey Key, IPipelineState pPSO)
            {
                var Idx = GetPSOIdx(Key);
                if (m_PSOCache.Count <= Idx)
                {
                    var start = m_PSOCache.Count;
                    var end = (int)Idx + 1;
                    m_PSOCache.Capacity = end;
                    for (var i = start; i < end; ++i)
                    {
                        m_PSOCache.Add(null);
                    }
                }

                m_PSOCache[(int)Idx]?.Dispose();

                m_PSOCache[(int)Idx] = new AutoPtr<IPipelineState>(pPSO);
            }

            public IPipelineState GetPSO(PSOKey Key)
            {
                var Idx = GetPSOIdx(Key);
                return Idx < m_PSOCache.Count ? m_PSOCache[(int)Idx].Obj : null;
            }

            public void Dispose()
            {
                foreach (var pso in m_PSOCache.Where(i => i != null))
                {
                    pso.Dispose();
                }
            }

            public IEnumerable<IPipelineState> Items =>
                m_PSOCache.Where(i => i != null).Select(i => i.Obj);
        }

        private CreateInfo m_Settings;
        AutoPtr<ITextureView> m_pBRDF_LUT_SRV;

        private PSOCache m_PSOCache = new PSOCache();

        AutoPtr<ITextureView> m_pWhiteTexSRV;
        AutoPtr<ITextureView> m_pBlackTexSRV;
        AutoPtr<ITextureView> m_pDefaultNormalMapSRV;
        AutoPtr<ITextureView> m_pDefaultPhysDescSRV;

        AutoPtr<ITextureView> m_pIrradianceCubeSRV;
        AutoPtr<ITextureView> m_pPrefilteredEnvMapSRV;
        AutoPtr<IPipelineState> m_pPrecomputeIrradianceCubePSO;
        AutoPtr<IPipelineState> m_pPrefilterEnvMapPSO;
        AutoPtr<IShaderResourceBinding> m_pPrecomputeIrradianceCubeSRB;
        AutoPtr<IShaderResourceBinding> m_pPrefilterEnvMapSRB;

        AutoPtr<IBuffer> m_TransformsCB;
        AutoPtr<IBuffer> m_GLTFAttribsCB;
        AutoPtr<IBuffer> m_PrecomputeEnvMapAttribsCB;
        AutoPtr<IBuffer> m_JointsBuffer;

        /// <summary>
        /// Change the DebugViewType of the renderer to see various debug stages.
        /// Set to none for normal rendering.
        /// </summary>
        public DebugViewType DebugViewType { get { return (DebugViewType)_DebugViewType; } set { _DebugViewType = (int)value; } }
        private int _DebugViewType = (int)DebugViewType.None;

        public GLTF_PBR_Renderer(IRenderDevice pDevice, IDeviceContext pCtx, CreateInfo CI, ShaderLoader shaderLoader)
        {
            this.m_Settings = CI;

            if (m_Settings.UseIBL)
            {
                PrecomputeBRDF(pDevice, pCtx, shaderLoader);

                TextureDesc TexDesc = new TextureDesc();
                TexDesc.Name = "Irradiance cube map for GLTF renderer";
                TexDesc.Type = RESOURCE_DIMENSION.RESOURCE_DIM_TEX_CUBE;
                TexDesc.Usage = USAGE.USAGE_DEFAULT;
                TexDesc.BindFlags = BIND_FLAGS.BIND_SHADER_RESOURCE | BIND_FLAGS.BIND_RENDER_TARGET;
                TexDesc.Width = IrradianceCubeDim;
                TexDesc.Height = IrradianceCubeDim;
                TexDesc.Format = IrradianceCubeFmt;
                TexDesc.ArraySize = 6;
                TexDesc.MipLevels = 0;

                using var IrradainceCubeTex = pDevice.CreateTexture(TexDesc, null);
                m_pIrradianceCubeSRV = new AutoPtr<ITextureView>(IrradainceCubeTex.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));

                TexDesc.Name = "Prefiltered environment map for GLTF renderer";
                TexDesc.Width = PrefilteredEnvMapDim;
                TexDesc.Height = PrefilteredEnvMapDim;
                TexDesc.Format = PrefilteredEnvMapFmt;
                using var PrefilteredEnvMapTex = pDevice.CreateTexture(TexDesc, null);
                m_pPrefilteredEnvMapSRV = new AutoPtr<ITextureView>(PrefilteredEnvMapTex.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));
            }

            unsafe
            {
                TextureDesc TexDesc = new TextureDesc();
                TexDesc.Name = "White texture for GLTF renderer";
                TexDesc.Type = RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY;
                TexDesc.Usage = USAGE.USAGE_IMMUTABLE;
                TexDesc.BindFlags = BIND_FLAGS.BIND_SHADER_RESOURCE;
                TexDesc.Width = TexDim;
                TexDesc.Height = TexDim;
                TexDesc.Format = TEXTURE_FORMAT.TEX_FORMAT_RGBA8_UNORM;
                TexDesc.MipLevels = 1;
                //std::vector<Uint32> Data(TexDim* TexDim, 0xFFFFFFFF); //Fill an array with 0xFFFFFFFF
                int dataLength = (int)(TexDim * TexDim);
                Uint32* Data = stackalloc Uint32[dataLength];
                Span<Uint32> DataSpan = new Span<Uint32>(Data, dataLength);
                DataSpan.Fill(0xFFFFFFFF);
                var Level0Data = new TextureSubResData { pData = new IntPtr(Data), Stride = TexDim * 4 };
                var InitData = new TextureData { pSubResources = new List<TextureSubResData> { Level0Data } };
                using var pWhiteTex = pDevice.CreateTexture(TexDesc, InitData);
                m_pWhiteTexSRV = new AutoPtr<ITextureView>(pWhiteTex.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));

                TexDesc.Name = "Black texture for GLTF renderer";
                DataSpan.Fill(0); //for (auto & c : Data) c = 0;
                using var pBlackTex = pDevice.CreateTexture(TexDesc, InitData);
                m_pBlackTexSRV = new AutoPtr<ITextureView>(pBlackTex.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));

                TexDesc.Name = "Default normal map for GLTF renderer";
                DataSpan.Fill(0x00FF7F7F); //for (auto & c : Data) c = 0x00FF7F7F;
                using var pDefaultNormalMap = pDevice.CreateTexture(TexDesc, InitData);
                m_pDefaultNormalMapSRV = new AutoPtr<ITextureView>(pDefaultNormalMap.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));

                TexDesc.Name = "Default physical description map for GLTF renderer";
                DataSpan.Fill(0x0000FF00);  //for (auto & c : Data) c = 0x0000FF00;
                using var pDefaultPhysDesc = pDevice.CreateTexture(TexDesc, InitData);
                m_pDefaultPhysDescSRV = new AutoPtr<ITextureView>(pDefaultPhysDesc.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));

                var Barriers = new List<StateTransitionDesc>
                {
                    new StateTransitionDesc{pResource = pWhiteTex.Obj,         OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_SHADER_RESOURCE, UpdateResourceState = true},
                    new StateTransitionDesc{pResource = pBlackTex.Obj,         OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_SHADER_RESOURCE, UpdateResourceState = true},
                    new StateTransitionDesc{pResource = pDefaultNormalMap.Obj, OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_SHADER_RESOURCE, UpdateResourceState = true},
                    new StateTransitionDesc{pResource = pDefaultPhysDesc.Obj,  OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_SHADER_RESOURCE, UpdateResourceState = true}
                };

                pCtx.TransitionResourceStates(Barriers);

                using var pDefaultSampler = pDevice.CreateSampler(Sam_LinearClamp);
                m_pWhiteTexSRV.Obj.SetSampler(pDefaultSampler.Obj);
                m_pBlackTexSRV.Obj.SetSampler(pDefaultSampler.Obj);
                m_pDefaultNormalMapSRV.Obj.SetSampler(pDefaultSampler.Obj);
            }

            if (CI.RTVFmt != TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN || CI.DSVFmt != TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN)
            {
                unsafe
                {
                    {
                        BufferDesc CBDesc = new BufferDesc();
                        CBDesc.Name = "GLTF node transforms CB";
                        CBDesc.uiSizeInBytes = (uint)sizeof(GLTFNodeShaderTransforms);
                        CBDesc.Usage = USAGE.USAGE_DYNAMIC;
                        CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                        CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;

                        m_TransformsCB = pDevice.CreateBuffer(CBDesc);
                    }

                    {
                        BufferDesc CBDesc = new BufferDesc();
                        CBDesc.Name = "GLTF attribs CB";
                        CBDesc.uiSizeInBytes = (uint)(sizeof(GLTFAttribs));
                        CBDesc.Usage = USAGE.USAGE_DYNAMIC;
                        CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                        CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;

                        m_GLTFAttribsCB = pDevice.CreateBuffer(CBDesc);
                    }

                    {
                        BufferDesc CBDesc = new BufferDesc();
                        CBDesc.Name = "GLTF joint tranforms";
                        CBDesc.uiSizeInBytes = (uint)sizeof(float4x4) * m_Settings.MaxJointCount;
                        CBDesc.Usage = USAGE.USAGE_DYNAMIC;
                        CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                        CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;

                        m_JointsBuffer = pDevice.CreateBuffer(CBDesc);
                    }

                    // clang-format off
                    var Barriers = new List<StateTransitionDesc>
                    {
                        new StateTransitionDesc{pResource = m_TransformsCB.Obj,  OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_CONSTANT_BUFFER, UpdateResourceState = true},
                        new StateTransitionDesc{pResource = m_GLTFAttribsCB.Obj, OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_CONSTANT_BUFFER, UpdateResourceState = true},
                        new StateTransitionDesc{pResource = m_JointsBuffer.Obj,  OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_CONSTANT_BUFFER, UpdateResourceState = true}
                    };
                    // clang-format on
                    pCtx.TransitionResourceStates(Barriers);

                    CreatePSO(pDevice, shaderLoader);
                }
            }
        }

        public void Dispose()
        {
            m_pPrefilterEnvMapSRB?.Dispose();
            m_pPrefilterEnvMapPSO?.Dispose();
            m_pPrecomputeIrradianceCubeSRB?.Dispose();
            m_pPrecomputeIrradianceCubePSO?.Dispose();
            m_PrecomputeEnvMapAttribsCB?.Dispose();
            m_PSOCache.Dispose();
            m_TransformsCB.Dispose();
            m_GLTFAttribsCB.Dispose();
            m_JointsBuffer.Dispose();
            m_pDefaultPhysDescSRV.Dispose();
            m_pDefaultNormalMapSRV.Dispose();
            m_pBlackTexSRV.Dispose();
            m_pWhiteTexSRV.Dispose();
            m_pPrefilteredEnvMapSRV.Dispose();
            m_pIrradianceCubeSRV.Dispose();
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

        private void CreatePSO(IRenderDevice pDevice, ShaderLoader shaderLoader)
        {
            GraphicsPipelineStateCreateInfo PSOCreateInfo = new GraphicsPipelineStateCreateInfo();
            PipelineStateDesc PSODesc = PSOCreateInfo.PSODesc;
            GraphicsPipelineDesc GraphicsPipeline = PSOCreateInfo.GraphicsPipeline;

            PSODesc.Name = "Render GLTF PBR PSO";
            PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_GRAPHICS;

            GraphicsPipeline.NumRenderTargets = 1;
            GraphicsPipeline.RTVFormats_0 = m_Settings.RTVFmt;
            GraphicsPipeline.DSVFormat = m_Settings.DSVFmt;
            GraphicsPipeline.PrimitiveTopology = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
            GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE.CULL_MODE_BACK;
            GraphicsPipeline.RasterizerDesc.FrontCounterClockwise = m_Settings.FrontCCW;

            ShaderCreateInfo ShaderCI = new ShaderCreateInfo();
            ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;
            ShaderCI.UseCombinedTextureSamplers = true;

            var Macros = new ShaderMacroHelper();
            Macros.AddShaderMacro("MAX_JOINT_COUNT", m_Settings.MaxJointCount);
            Macros.AddShaderMacro("ALLOW_DEBUG_VIEW", m_Settings.AllowDebugView);
            Macros.AddShaderMacro("TONE_MAPPING_MODE", "TONE_MAPPING_MODE_UNCHARTED2");
            Macros.AddShaderMacro("GLTF_PBR_USE_IBL", m_Settings.UseIBL);
            Macros.AddShaderMacro("GLTF_PBR_USE_AO", m_Settings.UseAO);
            Macros.AddShaderMacro("GLTF_PBR_USE_EMISSIVE", m_Settings.UseEmissive);
            Macros.AddShaderMacro("USE_TEXTURE_ATLAS", m_Settings.UseTextureAtlas);
            Macros.AddShaderMacro("PBR_WORKFLOW_METALLIC_ROUGHNESS", (Int32)PBR_WORKFLOW.PBR_WORKFLOW_METALL_ROUGH);
            Macros.AddShaderMacro("PBR_WORKFLOW_SPECULAR_GLOSINESS", (Int32)PBR_WORKFLOW.PBR_WORKFLOW_SPEC_GLOSS);
            Macros.AddShaderMacro("GLTF_ALPHA_MODE_OPAQUE", (Int32)ALPHA_MODE.ALPHA_MODE_OPAQUE);
            Macros.AddShaderMacro("GLTF_ALPHA_MODE_MASK", (Int32)ALPHA_MODE.ALPHA_MODE_MASK);
            Macros.AddShaderMacro("GLTF_ALPHA_MODE_BLEND", (Int32)ALPHA_MODE.ALPHA_MODE_BLEND);
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_VERTEX;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "GLTF PBR VS";
            ShaderCI.Source = shaderLoader.LoadShader("GLTF_PBR/private/RenderGLTF_PBR.vsh", "Common/public", "GLTF_PBR/public");
            using var pVS = pDevice.CreateShader(ShaderCI, Macros);

            // Create pixel shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_PIXEL;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "GLTF PBR PS";
            ShaderCI.Source = shaderLoader.LoadShader("GLTF_PBR/private/RenderGLTF_PBR.psh", "Common/public", "GLTF_PBR/public", "PostProcess/ToneMapping/public");
            using var pPS = pDevice.CreateShader(ShaderCI, Macros);

            var Inputs = new List<LayoutElement>
            {
                new LayoutElement{InputIndex = 0, BufferSlot = 0, NumComponents = 3, ValueType = VALUE_TYPE.VT_FLOAT32},   //float3 Pos     : ATTRIB0;
                new LayoutElement{InputIndex = 1, BufferSlot = 0, NumComponents = 3, ValueType = VALUE_TYPE.VT_FLOAT32},   //float3 Normal  : ATTRIB1;
                new LayoutElement{InputIndex = 2, BufferSlot = 0, NumComponents = 2, ValueType = VALUE_TYPE.VT_FLOAT32},   //float2 UV0     : ATTRIB2;
                new LayoutElement{InputIndex = 3, BufferSlot = 0, NumComponents = 2, ValueType = VALUE_TYPE.VT_FLOAT32},   //float2 UV1     : ATTRIB3;
                new LayoutElement{InputIndex = 4, BufferSlot = 1, NumComponents = 4, ValueType = VALUE_TYPE.VT_FLOAT32},   //float4 Joint0  : ATTRIB4;
                new LayoutElement{InputIndex = 5, BufferSlot = 1, NumComponents = 4, ValueType = VALUE_TYPE.VT_FLOAT32}    //float4 Weight0 : ATTRIB5;
            };
            PSOCreateInfo.GraphicsPipeline.InputLayout.LayoutElements = Inputs;

            PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_MUTABLE;
            var Vars = new List<ShaderResourceVariableDesc>
            {
                new ShaderResourceVariableDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_VERTEX, Name = "cbTransforms",      Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC},
                new ShaderResourceVariableDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL,  Name = "cbGLTFAttribs",     Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC},
                new ShaderResourceVariableDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_VERTEX, Name = "cbJointTransforms", Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC}
            };

            var ImtblSamplers = new List<ImmutableSamplerDesc>();
            if (m_Settings.UseImmutableSamplers)
            {
                ImtblSamplers.Add(new ImmutableSamplerDesc { ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, SamplerOrTextureName = "g_ColorMap", Desc = m_Settings.ColorMapImmutableSampler });
                ImtblSamplers.Add(new ImmutableSamplerDesc { ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, SamplerOrTextureName = "g_PhysicalDescriptorMap", Desc = m_Settings.PhysDescMapImmutableSampler });
                ImtblSamplers.Add(new ImmutableSamplerDesc { ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, SamplerOrTextureName = "g_NormalMap", Desc = m_Settings.NormalMapImmutableSampler });
            }

            if (m_Settings.UseAO)
            {
                ImtblSamplers.Add(new ImmutableSamplerDesc { ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, SamplerOrTextureName = "g_AOMap", Desc = m_Settings.AOMapImmutableSampler });
            }

            if (m_Settings.UseEmissive)
            {
                ImtblSamplers.Add(new ImmutableSamplerDesc { ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, SamplerOrTextureName = "g_EmissiveMap", Desc = m_Settings.EmissiveMapImmutableSampler });
            }

            if (m_Settings.UseIBL)
            {
                Vars.Add(new ShaderResourceVariableDesc { ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, Name = "g_BRDF_LUT", Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC });

                ImtblSamplers.Add(new ImmutableSamplerDesc { ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, SamplerOrTextureName = "g_BRDF_LUT", Desc = Sam_LinearClamp });
                ImtblSamplers.Add(new ImmutableSamplerDesc { ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, SamplerOrTextureName = "g_IrradianceMap", Desc = Sam_LinearClamp });
                ImtblSamplers.Add(new ImmutableSamplerDesc { ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, SamplerOrTextureName = "g_PrefilteredEnvMap", Desc = Sam_LinearClamp });
            }

            PSODesc.ResourceLayout.Variables = Vars;
            PSODesc.ResourceLayout.ImmutableSamplers = ImtblSamplers;

            PSOCreateInfo.pVS = pVS.Obj;
            PSOCreateInfo.pPS = pPS.Obj;

            {
                var Key = new PSOKey { AlphaMode = ALPHA_MODE.ALPHA_MODE_OPAQUE, DoubleSided = false };

                using var pSingleSidedOpaquePSO = pDevice.CreateGraphicsPipelineState(PSOCreateInfo);
                m_PSOCache.AddPSO(Key, pSingleSidedOpaquePSO.Obj);

                PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE.CULL_MODE_NONE;

                Key.DoubleSided = true;

                using var pDobleSidedOpaquePSO = pDevice.CreateGraphicsPipelineState(PSOCreateInfo);
                m_PSOCache.AddPSO(Key, pDobleSidedOpaquePSO.Obj);
            }

            PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE.CULL_MODE_BACK;

            var RT0 = PSOCreateInfo.GraphicsPipeline.BlendDesc.RenderTargets_0;
            RT0.BlendEnable = true;
            RT0.SrcBlend = BLEND_FACTOR.BLEND_FACTOR_SRC_ALPHA;
            RT0.DestBlend = BLEND_FACTOR.BLEND_FACTOR_INV_SRC_ALPHA;
            RT0.BlendOp = BLEND_OPERATION.BLEND_OPERATION_ADD;
            RT0.SrcBlendAlpha = BLEND_FACTOR.BLEND_FACTOR_INV_SRC_ALPHA;
            RT0.DestBlendAlpha = BLEND_FACTOR.BLEND_FACTOR_ZERO;
            RT0.BlendOpAlpha = BLEND_OPERATION.BLEND_OPERATION_ADD;

            {
                var Key = new PSOKey { AlphaMode = ALPHA_MODE.ALPHA_MODE_BLEND, DoubleSided = false };

                using var pSingleSidedBlendPSO = pDevice.CreateGraphicsPipelineState(PSOCreateInfo);
                m_PSOCache.AddPSO(Key, pSingleSidedBlendPSO.Obj);

                PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE.CULL_MODE_NONE;

                Key.DoubleSided = true;

                using var pDoubleSidedBlendPSO = pDevice.CreateGraphicsPipelineState(PSOCreateInfo);
                m_PSOCache.AddPSO(Key, pDoubleSidedBlendPSO.Obj);
            }

            foreach (var PSO in m_PSOCache.Items)
            {
                if (m_Settings.UseIBL)
                {
                    PSO.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "g_BRDF_LUT").Set(m_pBRDF_LUT_SRV.Obj);
                }
                PSO.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_VERTEX, "cbTransforms").Set(m_TransformsCB.Obj);
                PSO.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "cbGLTFAttribs").Set(m_GLTFAttribsCB.Obj);
                PSO.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_VERTEX, "cbJointTransforms").Set(m_JointsBuffer.Obj);
            }
        }

        void InitCommonSRBVars(IShaderResourceBinding pSRB,
                                          IBuffer pCameraAttribs,
                                          IBuffer pLightAttribs)
        {
            //VERIFY_EXPR(pSRB != nullptr);

            if (pCameraAttribs != null)
            {
                pSRB.GetVariableByName(SHADER_TYPE.SHADER_TYPE_VERTEX, "cbCameraAttribs")?.Set(pCameraAttribs);
                pSRB.GetVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "cbCameraAttribs")?.Set(pCameraAttribs);
            }

            if (pLightAttribs != null)
            {
                pSRB.GetVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "cbLightAttribs")?.Set(pLightAttribs);
            }

            if (m_Settings.UseIBL)
            {
                pSRB.GetVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "g_IrradianceMap")?.Set(m_pIrradianceCubeSRV.Obj);
                pSRB.GetVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "g_PrefilteredEnvMap")?.Set(m_pPrefilteredEnvMapSRV.Obj);
            }
        }

        private static void SetTexture(ITexture pTexture, ITextureView pDefaultTexSRV, String VarName, IShaderResourceBinding pSRB) //
        {
            AutoPtr<ITextureView> textureViewPtr = null;
            try
            {
                ITextureView pTexSRV = null;

                if (pTexture != null)
                {
                    if (pTexture.GetDesc_Type == RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY) //This is the only one
                    {
                        pTexSRV = pTexture.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);
                    }
                    else
                    {
                        TextureViewDesc SRVDesc = new TextureViewDesc();
                        SRVDesc.ViewType = TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE;
                        SRVDesc.TextureDim = RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY;
                        textureViewPtr = pTexture.CreateView(SRVDesc);
                        pTexSRV = textureViewPtr.Obj;
                    }
                }

                if (pTexSRV == null)
                {
                    pTexSRV = pDefaultTexSRV;
                }

                pSRB.GetVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, VarName)?.Set(pTexSRV);
            }
            finally
            {
                textureViewPtr?.Dispose();
            }
        }

        public AutoPtr<IShaderResourceBinding> CreateMaterialSRB(
                                          //GLTF::Model&             Model,
                                          //GLTF::Material&          Material,
                                          IBuffer pCameraAttribs = null,
                                          IBuffer pLightAttribs = null,
                                          IPipelineState pPSO = null,
                                          ITexture baseColorMap = null,
                                          ITexture normalMap = null,
                                          ITexture physicalDescriptorMap = null,
                                          ITexture aoMap = null,
                                          ITexture emissiveMap = null)
        {
            if (pPSO == null)
            {
                pPSO = m_PSOCache.GetPSO(new PSOKey());
            }

            //Replaces ppMaterialSRB, this is returned
            var pSRB = pPSO.CreateShaderResourceBinding(true);
            if (pSRB == null)
            {
                throw new Exception("Failed to create material SRB");
            }

            InitCommonSRBVars(pSRB.Obj, pCameraAttribs, pLightAttribs);

            SetTexture(baseColorMap, m_pWhiteTexSRV.Obj, "g_ColorMap", pSRB.Obj);
            SetTexture(physicalDescriptorMap, m_pDefaultPhysDescSRV.Obj, "g_PhysicalDescriptorMap", pSRB.Obj);
            SetTexture(normalMap, m_pDefaultNormalMapSRV.Obj, "g_NormalMap", pSRB.Obj);
            if (m_Settings.UseAO)
            {
                SetTexture(aoMap, m_pWhiteTexSRV.Obj, "g_AOMap", pSRB.Obj);
            }
            if (m_Settings.UseEmissive)
            {
                SetTexture(emissiveMap, m_pBlackTexSRV.Obj, "g_EmissiveMap", pSRB.Obj);
            }

            return pSRB;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct PrecomputeEnvMapAttribs
        {
            public float4x4 Rotation;

            public float Roughness;
            public float EnvMapDim;
            public uint NumSamples;
            public float Dummy;
        };

        public void PrecomputeCubemaps(IRenderDevice pDevice, IDeviceContext pCtx, ITextureView pEnvironmentMap, ShaderLoader shaderLoader)
        {
            if (!m_Settings.UseIBL)
            {
                //LOG_WARNING_MESSAGE("IBL is disabled, so precomputing cube maps will have no effect");
                return;
            }

            if (m_PrecomputeEnvMapAttribsCB == null)
            {
                unsafe
                {
                    BufferDesc CBDesc = new BufferDesc();
                    CBDesc.Name = "Precompute env map attribs CB";
                    CBDesc.uiSizeInBytes = (uint)sizeof(PrecomputeEnvMapAttribs);
                    CBDesc.Usage = USAGE.USAGE_DYNAMIC;
                    CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                    CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;

                    m_PrecomputeEnvMapAttribsCB = pDevice.CreateBuffer(CBDesc);
                }
            }

            if (m_pPrecomputeIrradianceCubePSO == null)
            {
                ShaderCreateInfo ShaderCI = new ShaderCreateInfo();
                ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;
                ShaderCI.UseCombinedTextureSamplers = true;

                ShaderMacroHelper Macros = new ShaderMacroHelper();
                Macros.AddShaderMacro("NUM_PHI_SAMPLES", 64);
                Macros.AddShaderMacro("NUM_THETA_SAMPLES", 32);
                ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_VERTEX;
                ShaderCI.EntryPoint = "main";
                ShaderCI.Desc.Name = "Cubemap face VS";
                ShaderCI.Source = shaderLoader.LoadShader("GLTF_PBR/private/CubemapFace.vsh");
                using var pVS = pDevice.CreateShader(ShaderCI, Macros);

                // Create pixel shader
                ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_PIXEL;
                ShaderCI.EntryPoint = "main";
                ShaderCI.Desc.Name = "Precompute irradiance cube map PS";
                ShaderCI.Source = shaderLoader.LoadShader("GLTF_PBR/private/ComputeIrradianceMap.psh");
                using var pPS = pDevice.CreateShader(ShaderCI, Macros);

                GraphicsPipelineStateCreateInfo PSOCreateInfo = new GraphicsPipelineStateCreateInfo();
                PipelineStateDesc PSODesc = PSOCreateInfo.PSODesc;
                GraphicsPipelineDesc GraphicsPipeline = PSOCreateInfo.GraphicsPipeline;

                PSODesc.Name = "Precompute irradiance cube PSO";
                PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_GRAPHICS;

                GraphicsPipeline.NumRenderTargets = 1;
                GraphicsPipeline.RTVFormats_0 = IrradianceCubeFmt;
                GraphicsPipeline.PrimitiveTopology = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_STRIP;
                GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE.CULL_MODE_NONE;
                GraphicsPipeline.DepthStencilDesc.DepthEnable = false;

                PSOCreateInfo.pVS = pVS.Obj;
                PSOCreateInfo.pPS = pPS.Obj;

                PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC;
                var Vars = new List<ShaderResourceVariableDesc>
                {
                    new ShaderResourceVariableDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, Name = "g_EnvironmentMap", Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC}
                };
                PSODesc.ResourceLayout.Variables = Vars;

                var ImtblSamplers = new List<ImmutableSamplerDesc>
                {
                    new ImmutableSamplerDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_PIXEL, SamplerOrTextureName = "g_EnvironmentMap", Desc = Sam_LinearClamp}
                };
                PSODesc.ResourceLayout.ImmutableSamplers = ImtblSamplers;

                m_pPrecomputeIrradianceCubePSO = pDevice.CreateGraphicsPipelineState(PSOCreateInfo);
                m_pPrecomputeIrradianceCubePSO.Obj.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_VERTEX, "cbTransform").Set(m_PrecomputeEnvMapAttribsCB.Obj);
                m_pPrecomputeIrradianceCubeSRB = m_pPrecomputeIrradianceCubePSO.Obj.CreateShaderResourceBinding(true);
            }

            if (m_pPrefilterEnvMapPSO == null)
            {
                ShaderCreateInfo ShaderCI = new ShaderCreateInfo();
                ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;
                ShaderCI.UseCombinedTextureSamplers = true;

                ShaderMacroHelper Macros = new ShaderMacroHelper();
                Macros.AddShaderMacro("OPTIMIZE_SAMPLES", 1);

                // Create vertex shader
                ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_VERTEX;
                ShaderCI.EntryPoint = "main";
                ShaderCI.Desc.Name = "Cubemap face VS";
                ShaderCI.Source = shaderLoader.LoadShader("GLTF_PBR/private/CubemapFace.vsh", "Common/public");
                using var pVS = pDevice.CreateShader(ShaderCI, Macros);

                // Create pixel shader
                ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_PIXEL;
                ShaderCI.EntryPoint = "main";
                ShaderCI.Desc.Name = "Prefilter environment map PS";
                ShaderCI.Source = shaderLoader.LoadShader("GLTF_PBR/private/PrefilterEnvMap.psh", "Common/public");
                using var pPS = pDevice.CreateShader(ShaderCI, Macros);

                GraphicsPipelineStateCreateInfo PSOCreateInfo = new GraphicsPipelineStateCreateInfo();
                PipelineStateDesc PSODesc = PSOCreateInfo.PSODesc;
                GraphicsPipelineDesc GraphicsPipeline = PSOCreateInfo.GraphicsPipeline;

                PSODesc.Name = "Prefilter environment map PSO";
                PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_GRAPHICS;

                GraphicsPipeline.NumRenderTargets = 1;
                GraphicsPipeline.RTVFormats_0 = PrefilteredEnvMapFmt;
                GraphicsPipeline.PrimitiveTopology = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_STRIP;
                GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE.CULL_MODE_NONE;
                GraphicsPipeline.DepthStencilDesc.DepthEnable = false;

                PSOCreateInfo.pVS = pVS.Obj;
                PSOCreateInfo.pPS = pPS.Obj;

                PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC;
                // clang-format off
                var Vars = new List<ShaderResourceVariableDesc>
                {
                    new ShaderResourceVariableDesc(SHADER_TYPE.SHADER_TYPE_PIXEL, "g_EnvironmentMap", SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC)
                };
                PSODesc.ResourceLayout.Variables = Vars;

                var ImtblSamplers = new List<ImmutableSamplerDesc>
                {
                    new ImmutableSamplerDesc(SHADER_TYPE.SHADER_TYPE_PIXEL, "g_EnvironmentMap", Sam_LinearClamp)
                };
                PSODesc.ResourceLayout.ImmutableSamplers = ImtblSamplers;

                m_pPrefilterEnvMapPSO = pDevice.CreateGraphicsPipelineState(PSOCreateInfo);
                m_pPrefilterEnvMapPSO.Obj.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_VERTEX, "cbTransform").Set(m_PrecomputeEnvMapAttribsCB.Obj);
                m_pPrefilterEnvMapPSO.Obj.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "FilterAttribs").Set(m_PrecomputeEnvMapAttribsCB.Obj);
                m_pPrefilterEnvMapSRB = m_pPrefilterEnvMapPSO.Obj.CreateShaderResourceBinding(true);
            }


            var Matrices = new float4x4[6]
            {
                /* +X */ float4x4.RotationY(+(float)Math.PI / 2f),
                /* -X */ float4x4.RotationY(-(float)Math.PI / 2f),
                /* +Y */ float4x4.RotationX(-(float)Math.PI / 2f),
                /* -Y */ float4x4.RotationX(+(float)Math.PI / 2f),
                /* +Z */ float4x4.Identity,
                /* -Z */ float4x4.RotationY((float)Math.PI)
            };

            pCtx.SetPipelineState(m_pPrecomputeIrradianceCubePSO.Obj);
            m_pPrecomputeIrradianceCubeSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "g_EnvironmentMap").Set(pEnvironmentMap);
            pCtx.CommitShaderResources(m_pPrecomputeIrradianceCubeSRB.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            var pIrradianceCube = m_pIrradianceCubeSRV.Obj.GetTexture();
            var IrradianceCubeDesc_MipLevels = pIrradianceCube.GetDesc_MipLevels;
            for (Uint32 mip = 0; mip < IrradianceCubeDesc_MipLevels; ++mip)
            {
                for (Uint32 face = 0; face < 6; ++face)
                {
                    var RTVDesc = new TextureViewDesc { ViewType = TEXTURE_VIEW_TYPE.TEXTURE_VIEW_RENDER_TARGET, TextureDim = RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY };
                    RTVDesc.Name = "RTV for irradiance cube texture";
                    RTVDesc.MostDetailedMip = mip;
                    RTVDesc.FirstArraySlice = face;
                    RTVDesc.NumArraySlices = 1;
                    using var pRTV = pIrradianceCube.CreateView(RTVDesc);
                    pCtx.SetRenderTarget(pRTV.Obj, null, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

                    unsafe
                    {
                        IntPtr data = pCtx.MapBuffer(m_PrecomputeEnvMapAttribsCB.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);

                        PrecomputeEnvMapAttribs* Attribs = (PrecomputeEnvMapAttribs*)data.ToPointer(); //(pCtx, m_PrecomputeEnvMapAttribsCB, MAP_WRITE, MAP_FLAG_DISCARD);
                        Attribs->Rotation = Matrices[face];

                        pCtx.UnmapBuffer(m_PrecomputeEnvMapAttribsCB.Obj, MAP_TYPE.MAP_WRITE);
                    }

                    var drawAttrs = new DrawAttribs { NumVertices = 4, Flags = DRAW_FLAGS.DRAW_FLAG_VERIFY_ALL };
                    pCtx.Draw(drawAttrs);
                }
            }

            pCtx.SetPipelineState(m_pPrefilterEnvMapPSO.Obj);
            m_pPrefilterEnvMapSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "g_EnvironmentMap").Set(pEnvironmentMap);
            pCtx.CommitShaderResources(m_pPrefilterEnvMapSRB.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            var pPrefilteredEnvMap = m_pPrefilteredEnvMapSRV.Obj.GetTexture();
            var PrefilteredEnvMapDesc_MipLevels = pPrefilteredEnvMap.GetDesc_MipLevels;
            var PrefilteredEnvMapDesc_Width = pPrefilteredEnvMap.GetDesc_Width;
            for (Uint32 mip = 0; mip < PrefilteredEnvMapDesc_MipLevels; ++mip)
            {
                for (Uint32 face = 0; face < 6; ++face)
                {
                    var RTVDesc = new TextureViewDesc { ViewType = TEXTURE_VIEW_TYPE.TEXTURE_VIEW_RENDER_TARGET, TextureDim = RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY };
                    RTVDesc.Name = "RTV for prefiltered env map cube texture";
                    RTVDesc.MostDetailedMip = mip;
                    RTVDesc.FirstArraySlice = face;
                    RTVDesc.NumArraySlices = 1;
                    using var pRTV = pPrefilteredEnvMap.CreateView(RTVDesc);
                    pCtx.SetRenderTarget(pRTV.Obj, null, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

                    unsafe
                    {
                        IntPtr data = pCtx.MapBuffer(m_PrecomputeEnvMapAttribsCB.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);

                        PrecomputeEnvMapAttribs* Attribs = (PrecomputeEnvMapAttribs*)data;// (pCtx, m_PrecomputeEnvMapAttribsCB, MAP_WRITE, MAP_FLAG_DISCARD);
                        Attribs->Rotation = Matrices[face];
                        Attribs->Roughness = (float)(mip) / (float)(PrefilteredEnvMapDesc_MipLevels);
                        Attribs->EnvMapDim = (float)(PrefilteredEnvMapDesc_Width);
                        Attribs->NumSamples = 256;

                        pCtx.UnmapBuffer(m_PrecomputeEnvMapAttribsCB.Obj, MAP_TYPE.MAP_WRITE);
                    }

                    var drawAttrs = new DrawAttribs { NumVertices = 4, Flags = DRAW_FLAGS.DRAW_FLAG_VERIFY_ALL };
                    pCtx.Draw(drawAttrs);
                }
            }

            var Barriers = new List<StateTransitionDesc>
            {
                new StateTransitionDesc{pResource = m_pPrefilteredEnvMapSRV.Obj.GetTexture(), OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_SHADER_RESOURCE, UpdateResourceState = true },
                new StateTransitionDesc{pResource = m_pIrradianceCubeSRV.Obj.GetTexture(),    OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_SHADER_RESOURCE, UpdateResourceState = true}
            };
            pCtx.TransitionResourceStates(Barriers);
        }

        public unsafe void Begin(IDeviceContext pCtx)
        {
            if (m_JointsBuffer != null)
            {
                IntPtr data = pCtx.MapBuffer(m_JointsBuffer.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);
                // In next-gen backends, dynamic buffers must be mapped before the first use in every frame
                var pJoints = (float4x4*)data.ToPointer();

                pCtx.UnmapBuffer(m_JointsBuffer.Obj, MAP_TYPE.MAP_WRITE);
            }
        }

        public unsafe void Render(IDeviceContext pCtx,
            IShaderResourceBinding materialSRB,
            IBuffer vertexBuffer,
            IBuffer skinVertexBuffer,
            IBuffer indexBuffer,
            Uint32 numIndices,
            ALPHA_MODE AlphaMode,
            ref Matrix4x4 position
            )
        {
            var doubleSided = false;

            IBuffer[] pBuffs = new IBuffer[] { vertexBuffer, skinVertexBuffer };//This should be 2 to fix the warning, see gltf_pbr_renderer.cpp line 870
            pCtx.SetVertexBuffers(0, (uint)pBuffs.Length, pBuffs, null, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION, SET_VERTEX_BUFFERS_FLAGS.SET_VERTEX_BUFFERS_FLAG_RESET);
            pCtx.SetIndexBuffer(indexBuffer, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            var Key = new PSOKey { AlphaMode = AlphaMode, DoubleSided = doubleSided };
            var pCurrPSO = m_PSOCache.GetPSO(Key);
            pCtx.SetPipelineState(pCurrPSO);

            pCtx.CommitShaderResources(materialSRB, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            unsafe
            {
                IntPtr data = pCtx.MapBuffer(m_TransformsCB.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);
                var transform = (GLTFNodeShaderTransforms*)data.ToPointer();

                transform->NodeMatrix = position;
                transform->JointCount = 0;

                pCtx.UnmapBuffer(m_TransformsCB.Obj, MAP_TYPE.MAP_WRITE);
            }

            unsafe
            {
                IntPtr data = pCtx.MapBuffer(m_JointsBuffer.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);
                var joints = (float4x4*)data.ToPointer();

                pCtx.UnmapBuffer(m_JointsBuffer.Obj, MAP_TYPE.MAP_WRITE);
            }

            unsafe
            {
                IntPtr data = pCtx.MapBuffer(m_GLTFAttribsCB.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);
                var pGLTFAttribs = (GLTFAttribs*)data.ToPointer();// { pCtx, m_GLTFAttribsCB, MAP_WRITE, MAP_FLAG_DISCARD};

                //Just using hardcoded values for now
                //pGLTFAttribs->MaterialInfo = GLTFShaderAttribs.CreateDefault();
                pGLTFAttribs->MaterialInfo = new GLTFShaderAttribs()
                {
                    BaseColorFactor = new float4(1, 1, 1, 1),
                    EmissiveFactor = new float4(1, 1, 1, 1),
                    SpecularFactor = new float4(1, 1, 1, 1),

                    Workflow = (int)PBR_WORKFLOW.PBR_WORKFLOW_METALL_ROUGH,
                    BaseColorTextureUVSelector = 0,
                    PhysicalDescriptorTextureUVSelector = 0,
                    NormalTextureUVSelector = 0,

                    OcclusionTextureUVSelector = 0,
                    EmissiveTextureUVSelector = 0,
                    BaseColorSlice = 0,
                    PhysicalDescriptorSlice = 0,

                    NormalSlice = 0,
                    OcclusionSlice = 0,
                    EmissiveSlice = 0,
                    MetallicFactor = 1,

                    RoughnessFactor = 1,
                    AlphaMode = (int)ALPHA_MODE.ALPHA_MODE_OPAQUE,
                    AlphaMaskCutoff = 0.5f,
                    Dummy0 = -107374176f, //changed might just be from garbage, who knows

                    // When texture atlas is used, UV scale and bias applied to
                    // each texture coordinate set
                    BaseColorUVScaleBias = new float4(1, 1, 0, 0),
                    PhysicalDescriptorUVScaleBias = new float4(1, 1, 0, 0),
                    NormalMapUVScaleBias = new float4(1, 1, 0, 0),
                    OcclusionUVScaleBias = new float4(1, 1, 0, 0),
                    EmissiveUVScaleBias = new float4(1, 1, 0, 0),
                };

                var ShaderParams = &pGLTFAttribs->RenderParameters;

                //ShaderParams.DebugViewType = static_cast<int>(m_RenderParams.DebugView);
                //ShaderParams.OcclusionStrength = m_RenderParams.OcclusionStrength;
                //ShaderParams.EmissionScale = m_RenderParams.EmissionScale;
                //ShaderParams.AverageLogLum = m_RenderParams.AverageLogLum;
                //ShaderParams.MiddleGray = m_RenderParams.MiddleGray;
                //ShaderParams.WhitePoint = m_RenderParams.WhitePoint;
                //ShaderParams.IBLScale = m_RenderParams.IBLScale;
                //ShaderParams.PrefilteredCubeMipLevels = m_Settings.UseIBL ? static_cast<float>(m_pPrefilteredEnvMapSRV->GetTexture()->GetDesc().MipLevels) : 0f;

                //Take from c++ app, don't just use
                ShaderParams->AverageLogLum = 0.300000012f;
                ShaderParams->MiddleGray = 0.180000007f;
                ShaderParams->WhitePoint = 3.00000000f;
                ShaderParams->PrefilteredCubeMipLevels = 0.00000000f;
                ShaderParams->IBLScale = 1.00000000f;
                ShaderParams->DebugViewType = _DebugViewType;
                ShaderParams->OcclusionStrength = 1.00000000f;
                ShaderParams->EmissionScale = 1.00000000f;
                ShaderParams->PrefilteredCubeMipLevels = m_Settings.UseIBL ? m_pPrefilteredEnvMapSRV.Obj.GetTexture().GetDesc_MipLevels : 0f; //This line is valid

                pCtx.UnmapBuffer(m_GLTFAttribsCB.Obj, MAP_TYPE.MAP_WRITE);
            }

            DrawIndexedAttribs DrawAttrs = new DrawIndexedAttribs();     // This is an indexed draw call for the cube 
            DrawAttrs.IndexType = VALUE_TYPE.VT_UINT32; // Index type
            DrawAttrs.NumIndices = numIndices;
            // Verify the state of vertex and index buffers
            DrawAttrs.Flags = DRAW_FLAGS.DRAW_FLAG_VERIFY_ALL;
            pCtx.DrawIndexed(DrawAttrs);
        }
    }
}
