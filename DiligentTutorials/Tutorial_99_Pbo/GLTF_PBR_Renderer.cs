﻿using DiligentEngine;
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
    struct GLTFMaterialShaderInfo
    {
        float4 BaseColorFactor;
        float4 EmissiveFactor;
        float4 SpecularFactor;

        int Workflow;
        float BaseColorTextureUVSelector;
        float PhysicalDescriptorTextureUVSelector;
        float NormalTextureUVSelector;

        float OcclusionTextureUVSelector;
        float EmissiveTextureUVSelector;
        float BaseColorSlice;
        float PhysicalDescriptorSlice;

        float NormalSlice;
        float OcclusionSlice;
        float EmissiveSlice;
        float MetallicFactor;

        float RoughnessFactor;
        int AlphaMode;
        float AlphaMaskCutoff;
        float Dummy0;

        // When texture atlas is used, UV scale and bias applied to
        // each texture coordinate set
        float4 BaseColorUVScaleBias;
        float4 PhysicalDescriptorUVScaleBias;
        float4 NormalMapUVScaleBias;
        float4 OcclusionUVScaleBias;
        float4 EmissiveUVScaleBias;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct GLTFRendererShaderParameters
    {
        float AverageLogLum;
        float MiddleGray;
        float WhitePoint;
        float PrefilteredCubeMipLevels;

        float IBLScale;
        int DebugViewType;
        float OcclusionStrength;
        float EmissionScale;
    };

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
                if(m_PSOCache.Count <= Idx)
                {
                    var start = m_PSOCache.Count;
                    var end = (int)Idx + 1;
                    m_PSOCache.Capacity = end;
                    for(var i = start; i < end; ++i)
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

        AutoPtr<IBuffer> m_TransformsCB;
        AutoPtr<IBuffer> m_GLTFAttribsCB;
        AutoPtr<IBuffer> m_JointsBuffer;

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
                    BufferDesc CBDesc = new BufferDesc();
                    CBDesc.Name = "GLTF node transforms CB";
                    CBDesc.uiSizeInBytes = (uint)sizeof(GLTFNodeShaderTransforms);
                    CBDesc.Usage = USAGE.USAGE_DYNAMIC;
                    CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                    CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;

                    m_TransformsCB = pDevice.CreateBuffer(CBDesc);

                    CBDesc.Name = "GLTF attribs CB";
                    CBDesc.uiSizeInBytes = (uint)(sizeof(GLTFMaterialShaderInfo) + sizeof(GLTFRendererShaderParameters));
                    CBDesc.Usage = USAGE.USAGE_DYNAMIC;
                    CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                    CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;

                    m_GLTFAttribsCB = pDevice.CreateBuffer(CBDesc);

                    CBDesc.Name = "GLTF joint tranforms";
                    CBDesc.uiSizeInBytes = (uint)sizeof(float4x4) * m_Settings.MaxJointCount;
                    CBDesc.Usage = USAGE.USAGE_DYNAMIC;
                    CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                    CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;

                    m_JointsBuffer = pDevice.CreateBuffer(CBDesc);

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
            Macros.AddShaderMacro("USE_TEXTURE_ATLAS", m_Settings.UseTextureAtals);
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
                var Key = new PSOKey{ AlphaMode = ALPHA_MODE.ALPHA_MODE_BLEND, DoubleSided = false };

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



        struct PrecomputeEnvMapAttribs
        {
            float4x4 Rotation;

            float Roughness;
            float EnvMapDim;
            uint NumSamples;
            float Dummy;
        };

        void PrecomputeCubemaps(IRenderDevice  pDevice,
                                           IDeviceContext pCtx,
                                           ITextureView   pEnvironmentMap)
        {
            if (!m_Settings.UseIBL)
            {
                //LOG_WARNING_MESSAGE("IBL is disabled, so precomputing cube maps will have no effect");
                return;
            }

            //if (!m_PrecomputeEnvMapAttribsCB)
            //{
            //    CreateUniformBuffer(pDevice, sizeof(PrecomputeEnvMapAttribs), "Precompute env map attribs CB", &m_PrecomputeEnvMapAttribsCB);
            //}

        //    if (!m_pPrecomputeIrradianceCubePSO)
        //    {
        //        ShaderCreateInfo ShaderCI;
        //        ShaderCI.SourceLanguage             = SHADER_SOURCE_LANGUAGE_HLSL;
        //        ShaderCI.UseCombinedTextureSamplers = true;
        //        ShaderCI.pShaderSourceStreamFactory = &DiligentFXShaderSourceStreamFactory::GetInstance();

        //        ShaderMacroHelper Macros;
        //        Macros.AddShaderMacro("NUM_PHI_SAMPLES", 64);
        //        Macros.AddShaderMacro("NUM_THETA_SAMPLES", 32);
        //        ShaderCI.Macros = Macros;
        //        RefCntAutoPtr<IShader> pVS;
        //        {
        //            ShaderCI.Desc.ShaderType = SHADER_TYPE_VERTEX;
        //            ShaderCI.EntryPoint      = "main";
        //            ShaderCI.Desc.Name       = "Cubemap face VS";
        //            ShaderCI.FilePath        = "CubemapFace.vsh";
        //            pDevice->CreateShader(ShaderCI, &pVS);
        //        }

        //        // Create pixel shader
        //        RefCntAutoPtr<IShader> pPS;
        //        {
        //            ShaderCI.Desc.ShaderType = SHADER_TYPE_PIXEL;
        //            ShaderCI.EntryPoint      = "main";
        //            ShaderCI.Desc.Name       = "Precompute irradiance cube map PS";
        //            ShaderCI.FilePath        = "ComputeIrradianceMap.psh";
        //            pDevice->CreateShader(ShaderCI, &pPS);
        //        }

        //        GraphicsPipelineStateCreateInfo PSOCreateInfo;
        //        PipelineStateDesc&              PSODesc          = PSOCreateInfo.PSODesc;
        //        GraphicsPipelineDesc&           GraphicsPipeline = PSOCreateInfo.GraphicsPipeline;

        //        PSODesc.Name         = "Precompute irradiance cube PSO";
        //        PSODesc.PipelineType = PIPELINE_TYPE_GRAPHICS;

        //        GraphicsPipeline.NumRenderTargets             = 1;
        //        GraphicsPipeline.RTVFormats[0]                = IrradianceCubeFmt;
        //        GraphicsPipeline.PrimitiveTopology            = PRIMITIVE_TOPOLOGY_TRIANGLE_STRIP;
        //        GraphicsPipeline.RasterizerDesc.CullMode      = CULL_MODE_NONE;
        //        GraphicsPipeline.DepthStencilDesc.DepthEnable = False;

        //        PSOCreateInfo.pVS = pVS;
        //        PSOCreateInfo.pPS = pPS;

        //        PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE_STATIC;
        //        // clang-format off
        //        ShaderResourceVariableDesc Vars[] = 
        //        {
        //            {SHADER_TYPE_PIXEL, "g_EnvironmentMap", SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC}
        //        };
        //        // clang-format on
        //        PSODesc.ResourceLayout.NumVariables = _countof(Vars);
        //        PSODesc.ResourceLayout.Variables    = Vars;

        //        // clang-format off
        //        ImmutableSamplerDesc ImtblSamplers[] =
        //        {
        //            {SHADER_TYPE_PIXEL, "g_EnvironmentMap", Sam_LinearClamp}
        //        };
        //        // clang-format on
        //        PSODesc.ResourceLayout.NumImmutableSamplers = _countof(ImtblSamplers);
        //        PSODesc.ResourceLayout.ImmutableSamplers    = ImtblSamplers;

        //        pDevice->CreateGraphicsPipelineState(PSOCreateInfo, &m_pPrecomputeIrradianceCubePSO);
        //        m_pPrecomputeIrradianceCubePSO->GetStaticVariableByName(SHADER_TYPE_VERTEX, "cbTransform")->Set(m_PrecomputeEnvMapAttribsCB);
        //        m_pPrecomputeIrradianceCubePSO->CreateShaderResourceBinding(&m_pPrecomputeIrradianceCubeSRB, true);
        //    }

        //    if (!m_pPrefilterEnvMapPSO)
        //    {
        //        ShaderCreateInfo ShaderCI;
        //        ShaderCI.SourceLanguage             = SHADER_SOURCE_LANGUAGE_HLSL;
        //        ShaderCI.UseCombinedTextureSamplers = true;
        //        ShaderCI.pShaderSourceStreamFactory = &DiligentFXShaderSourceStreamFactory::GetInstance();

        //        ShaderMacroHelper Macros;
        //        Macros.AddShaderMacro("OPTIMIZE_SAMPLES", 1);
        //        ShaderCI.Macros = Macros;

        //        RefCntAutoPtr<IShader> pVS;
        //        {
        //            ShaderCI.Desc.ShaderType = SHADER_TYPE_VERTEX;
        //            ShaderCI.EntryPoint      = "main";
        //            ShaderCI.Desc.Name       = "Cubemap face VS";
        //            ShaderCI.FilePath        = "CubemapFace.vsh";
        //            pDevice->CreateShader(ShaderCI, &pVS);
        //        }

        //        // Create pixel shader
        //        RefCntAutoPtr<IShader> pPS;
        //        {
        //            ShaderCI.Desc.ShaderType = SHADER_TYPE_PIXEL;
        //            ShaderCI.EntryPoint      = "main";
        //            ShaderCI.Desc.Name       = "Prefilter environment map PS";
        //            ShaderCI.FilePath        = "PrefilterEnvMap.psh";
        //            pDevice->CreateShader(ShaderCI, &pPS);
        //        }

        //        GraphicsPipelineStateCreateInfo PSOCreateInfo;
        //        PipelineStateDesc&              PSODesc          = PSOCreateInfo.PSODesc;
        //        GraphicsPipelineDesc&           GraphicsPipeline = PSOCreateInfo.GraphicsPipeline;

        //        PSODesc.Name         = "Prefilter environment map PSO";
        //        PSODesc.PipelineType = PIPELINE_TYPE_GRAPHICS;

        //        GraphicsPipeline.NumRenderTargets             = 1;
        //        GraphicsPipeline.RTVFormats[0]                = PrefilteredEnvMapFmt;
        //        GraphicsPipeline.PrimitiveTopology            = PRIMITIVE_TOPOLOGY_TRIANGLE_STRIP;
        //        GraphicsPipeline.RasterizerDesc.CullMode      = CULL_MODE_NONE;
        //        GraphicsPipeline.DepthStencilDesc.DepthEnable = False;

        //        PSOCreateInfo.pVS = pVS;
        //        PSOCreateInfo.pPS = pPS;

        //        PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE_STATIC;
        //        // clang-format off
        //        ShaderResourceVariableDesc Vars[] = 
        //        {
        //            {SHADER_TYPE_PIXEL, "g_EnvironmentMap", SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC}
        //        };
        //        // clang-format on
        //        PSODesc.ResourceLayout.NumVariables = _countof(Vars);
        //        PSODesc.ResourceLayout.Variables    = Vars;

        //        // clang-format off
        //        ImmutableSamplerDesc ImtblSamplers[] =
        //        {
        //            {SHADER_TYPE_PIXEL, "g_EnvironmentMap", Sam_LinearClamp}
        //        };
        //        // clang-format on
        //        PSODesc.ResourceLayout.NumImmutableSamplers = _countof(ImtblSamplers);
        //        PSODesc.ResourceLayout.ImmutableSamplers    = ImtblSamplers;

        //        pDevice->CreateGraphicsPipelineState(PSOCreateInfo, &m_pPrefilterEnvMapPSO);
        //        m_pPrefilterEnvMapPSO->GetStaticVariableByName(SHADER_TYPE_VERTEX, "cbTransform")->Set(m_PrecomputeEnvMapAttribsCB);
        //        m_pPrefilterEnvMapPSO->GetStaticVariableByName(SHADER_TYPE_PIXEL, "FilterAttribs")->Set(m_PrecomputeEnvMapAttribsCB);
        //        m_pPrefilterEnvMapPSO->CreateShaderResourceBinding(&m_pPrefilterEnvMapSRB, true);
        //    }


        //    // clang-format off
	       // const std::array<float4x4, 6> Matrices =
        //    {
        ///* +X */ float4x4::RotationY(+PI_F / 2.f),
        ///* -X */ float4x4::RotationY(-PI_F / 2.f),
        ///* +Y */ float4x4::RotationX(-PI_F / 2.f),
        ///* -Y */ float4x4::RotationX(+PI_F / 2.f),
        ///* +Z */ float4x4::Identity(),
        ///* -Z */ float4x4::RotationY(PI_F)
	       // };
        //    // clang-format on

        //    pCtx->SetPipelineState(m_pPrecomputeIrradianceCubePSO);
        //    m_pPrecomputeIrradianceCubeSRB->GetVariableByName(SHADER_TYPE_PIXEL, "g_EnvironmentMap")->Set(pEnvironmentMap);
        //    pCtx->CommitShaderResources(m_pPrecomputeIrradianceCubeSRB, RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
        //    auto*       pIrradianceCube    = m_pIrradianceCubeSRV->GetTexture();
        //    const auto& IrradianceCubeDesc = pIrradianceCube->GetDesc();
        //    for (Uint32 mip = 0; mip < IrradianceCubeDesc.MipLevels; ++mip)
        //    {
        //        for (Uint32 face = 0; face < 6; ++face)
        //        {
        //            TextureViewDesc RTVDesc(TEXTURE_VIEW_RENDER_TARGET, RESOURCE_DIM_TEX_2D_ARRAY);
        //            RTVDesc.Name            = "RTV for irradiance cube texture";
        //            RTVDesc.MostDetailedMip = mip;
        //            RTVDesc.FirstArraySlice = face;
        //            RTVDesc.NumArraySlices  = 1;
        //            RefCntAutoPtr<ITextureView> pRTV;
        //            pIrradianceCube->CreateView(RTVDesc, &pRTV);
        //            ITextureView* ppRTVs[] = {pRTV};
        //            pCtx->SetRenderTargets(_countof(ppRTVs), ppRTVs, nullptr, RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
        //            {
        //                MapHelper<PrecomputeEnvMapAttribs> Attribs(pCtx, m_PrecomputeEnvMapAttribsCB, MAP_WRITE, MAP_FLAG_DISCARD);
        //                Attribs->Rotation = Matrices[face];
        //            }
        //            DrawAttribs drawAttrs(4, DRAW_FLAG_VERIFY_ALL);
        //            pCtx->Draw(drawAttrs);
        //        }
        //    }

        //    pCtx->SetPipelineState(m_pPrefilterEnvMapPSO);
        //    m_pPrefilterEnvMapSRB->GetVariableByName(SHADER_TYPE_PIXEL, "g_EnvironmentMap")->Set(pEnvironmentMap);
        //    pCtx->CommitShaderResources(m_pPrefilterEnvMapSRB, RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
        //    auto*       pPrefilteredEnvMap    = m_pPrefilteredEnvMapSRV->GetTexture();
        //    const auto& PrefilteredEnvMapDesc = pPrefilteredEnvMap->GetDesc();
        //    for (Uint32 mip = 0; mip < PrefilteredEnvMapDesc.MipLevels; ++mip)
        //    {
        //        for (Uint32 face = 0; face < 6; ++face)
        //        {
        //            TextureViewDesc RTVDesc(TEXTURE_VIEW_RENDER_TARGET, RESOURCE_DIM_TEX_2D_ARRAY);
        //            RTVDesc.Name            = "RTV for prefiltered env map cube texture";
        //            RTVDesc.MostDetailedMip = mip;
        //            RTVDesc.FirstArraySlice = face;
        //            RTVDesc.NumArraySlices  = 1;
        //            RefCntAutoPtr<ITextureView> pRTV;
        //            pPrefilteredEnvMap->CreateView(RTVDesc, &pRTV);
        //            ITextureView* ppRTVs[] = {pRTV};
        //            pCtx->SetRenderTargets(_countof(ppRTVs), ppRTVs, nullptr, RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

        //            {
        //                MapHelper<PrecomputeEnvMapAttribs> Attribs(pCtx, m_PrecomputeEnvMapAttribsCB, MAP_WRITE, MAP_FLAG_DISCARD);
        //                Attribs->Rotation   = Matrices[face];
        //                Attribs->Roughness  = static_cast<float>(mip) / static_cast<float>(PrefilteredEnvMapDesc.MipLevels);
        //                Attribs->EnvMapDim  = static_cast<float>(PrefilteredEnvMapDesc.Width);
        //                Attribs->NumSamples = 256;
        //            }

        //            DrawAttribs drawAttrs(4, DRAW_FLAG_VERIFY_ALL);
        //            pCtx->Draw(drawAttrs);
        //        }
        //    }

        //    // clang-format off
        //    StateTransitionDesc Barriers[] = 
        //    {
        //        {m_pPrefilteredEnvMapSRV->GetTexture(), RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_SHADER_RESOURCE, true},
        //        {m_pIrradianceCubeSRV->GetTexture(),    RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_SHADER_RESOURCE, true}
        //    };
        //    // clang-format on
        //    pCtx->TransitionResourceStates(_countof(Barriers), Barriers);
        }
    }
}