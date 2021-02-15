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

namespace DiligentEngine.GltfPbr
{
    public class PbrRenderer<T> : PbrRenderer
    {
        public PbrRenderer(IRenderDevice pDevice, IDeviceContext pCtx, PbrRendererCreateInfo CI, ShaderLoader<PbrRenderer> shaderLoader) : base(pDevice, pCtx, CI, shaderLoader)
        {
        }
    }

    public class PbrRenderer : IDisposable
    {
        public const int DefaultNormal = 0x00FF7F7F;
        public const int DefaultPhysical = 0x0000FF00;

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

        private PbrRendererCreateInfo m_Settings;
        private readonly ShaderLoader<PbrRenderer> shaderLoader;
        private AutoPtr<ITextureView> m_pBRDF_LUT_SRV;

        private PSOCache m_PSOCache = new PSOCache();

        private AutoPtr<ITextureView> m_pWhiteTexSRV;
        private AutoPtr<ITextureView> m_pBlackTexSRV;
        private AutoPtr<ITextureView> m_pDefaultNormalMapSRV;
        private AutoPtr<ITextureView> m_pDefaultPhysDescSRV;

        private AutoPtr<ITextureView> m_pIrradianceCubeSRV;
        private AutoPtr<ITextureView> m_pPrefilteredEnvMapSRV;
        private AutoPtr<IPipelineState> m_pPrecomputeIrradianceCubePSO;
        private AutoPtr<IPipelineState> m_pPrefilterEnvMapPSO;
        private AutoPtr<IShaderResourceBinding> m_pPrecomputeIrradianceCubeSRB;
        private AutoPtr<IShaderResourceBinding> m_pPrefilterEnvMapSRB;

        private AutoPtr<IBuffer> m_TransformsCB;
        private AutoPtr<IBuffer> m_GLTFAttribsCB;
        private AutoPtr<IBuffer> m_PrecomputeEnvMapAttribsCB;
        private AutoPtr<IBuffer> m_JointsBuffer;

        //Shadows
        private AutoPtr<IPipelineState> m_pShadowPSO;
        private AutoPtr<IShaderResourceBinding> m_ShadowSRB;
        private AutoPtr<IPipelineState> m_pShadowMapVisPSO;
        private AutoPtr<ITextureView> m_ShadowMapDSV;
        private AutoPtr<ITextureView> m_ShadowMapSRV;
        private AutoPtr<IShaderResourceBinding> m_ShadowMapVisSRB;

        TEXTURE_FORMAT m_ShadowMapFormat = TEXTURE_FORMAT.TEX_FORMAT_D16_UNORM;
        float4x4 m_WorldToShadowMapUVDepthMatr;
        Uint32 m_ShadowMapSize = 512;

        public PbrRenderer(IRenderDevice pDevice, IDeviceContext pCtx, PbrRendererCreateInfo CI, ShaderLoader<PbrRenderer> shaderLoader)
        {
            this.m_Settings = CI;
            this.shaderLoader = shaderLoader;

            if (m_Settings.UseIBL)
            {
                PrecomputeBRDF(pDevice, pCtx);

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
                DataSpan.Fill(DefaultNormal); //for (auto & c : Data) c = 0x00FF7F7F;
                using var pDefaultNormalMap = pDevice.CreateTexture(TexDesc, InitData);
                m_pDefaultNormalMapSRV = new AutoPtr<ITextureView>(pDefaultNormalMap.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));

                TexDesc.Name = "Default physical description map for GLTF renderer";
                DataSpan.Fill(DefaultPhysical);  //for (auto & c : Data) c = 0x0000FF00;
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

                    CreatePSO(pDevice);
                }
            }
        }

        public void Dispose()
        {
            m_pShadowMapVisPSO?.Dispose();
            m_pShadowPSO?.Dispose();
            m_ShadowMapVisSRB?.Dispose();
            m_ShadowSRB?.Dispose();
            m_ShadowMapDSV?.Dispose();
            m_ShadowMapSRV?.Dispose();

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
            m_pPrefilteredEnvMapSRV?.Dispose();
            m_pIrradianceCubeSRV?.Dispose();
            m_pBRDF_LUT_SRV?.Dispose();
        }
        public void PrecomputeBRDF(IRenderDevice pDevice, IDeviceContext pCtx)
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

        private void CreatePSO(IRenderDevice pDevice)
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

            ShaderMacroHelper Macros = CreateShaderMacros();
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
                var Key = new PSOKey { AlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE, DoubleSided = false };

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
                var Key = new PSOKey { AlphaMode = PbrAlphaMode.ALPHA_MODE_BLEND, DoubleSided = false };

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

        private ShaderMacroHelper CreateShaderMacros()
        {
            var Macros = new ShaderMacroHelper();
            Macros.AddShaderMacro("MAX_JOINT_COUNT", m_Settings.MaxJointCount);
            Macros.AddShaderMacro("ALLOW_DEBUG_VIEW", m_Settings.AllowDebugView);
            Macros.AddShaderMacro("TONE_MAPPING_MODE", "TONE_MAPPING_MODE_UNCHARTED2");
            Macros.AddShaderMacro("GLTF_PBR_USE_IBL", m_Settings.UseIBL);
            Macros.AddShaderMacro("GLTF_PBR_USE_AO", m_Settings.UseAO);
            Macros.AddShaderMacro("GLTF_PBR_USE_EMISSIVE", m_Settings.UseEmissive);
            Macros.AddShaderMacro("USE_TEXTURE_ATLAS", m_Settings.UseTextureAtlas);
            Macros.AddShaderMacro("PBR_WORKFLOW_METALLIC_ROUGHNESS", (Int32)PbrWorkflow.PBR_WORKFLOW_METALL_ROUGH);
            Macros.AddShaderMacro("PBR_WORKFLOW_SPECULAR_GLOSINESS", (Int32)PbrWorkflow.PBR_WORKFLOW_SPEC_GLOSS);
            Macros.AddShaderMacro("GLTF_ALPHA_MODE_OPAQUE", (Int32)PbrAlphaMode.ALPHA_MODE_OPAQUE);
            Macros.AddShaderMacro("GLTF_ALPHA_MODE_MASK", (Int32)PbrAlphaMode.ALPHA_MODE_MASK);
            Macros.AddShaderMacro("GLTF_ALPHA_MODE_BLEND", (Int32)PbrAlphaMode.ALPHA_MODE_BLEND);
            return Macros;
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

            // This is how you would bind the shadow to a srb
            //m_PlaneSRB = m_pPlanePSO.Obj.CreateShaderResourceBinding(true);
            //m_PlaneSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "g_ShadowMap").Set(m_ShadowMapSRV.Obj);

            return pSRB;
        }

        public void PrecomputeCubemaps(IRenderDevice pDevice, IDeviceContext pCtx, ITextureView pEnvironmentMap)
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

        public void CreateShadowPSO(ISwapChain swapChain, IRenderDevice pDevice, IBuffer pCameraAttribs)
        {
            CreateShadowMapVisPSO(swapChain, pDevice);

            // Define vertex shader input layout
            var Inputs = new List<LayoutElement>
            {
                new LayoutElement{InputIndex = 0, BufferSlot = 0, NumComponents = 3, ValueType = VALUE_TYPE.VT_FLOAT32},   //float3 Pos     : ATTRIB0;
                new LayoutElement{InputIndex = 1, BufferSlot = 0, NumComponents = 3, ValueType = VALUE_TYPE.VT_FLOAT32},   //float3 Normal  : ATTRIB1;
                new LayoutElement{InputIndex = 2, BufferSlot = 0, NumComponents = 2, ValueType = VALUE_TYPE.VT_FLOAT32},   //float2 UV0     : ATTRIB2;
                new LayoutElement{InputIndex = 3, BufferSlot = 0, NumComponents = 2, ValueType = VALUE_TYPE.VT_FLOAT32},   //float2 UV1     : ATTRIB3;
                new LayoutElement{InputIndex = 4, BufferSlot = 1, NumComponents = 4, ValueType = VALUE_TYPE.VT_FLOAT32},   //float4 Joint0  : ATTRIB4;
                new LayoutElement{InputIndex = 5, BufferSlot = 1, NumComponents = 4, ValueType = VALUE_TYPE.VT_FLOAT32}    //float4 Weight0 : ATTRIB5;
            };

            // Create shadow pass PSO
            var PSOCreateInfo = new GraphicsPipelineStateCreateInfo();

            PSOCreateInfo.PSODesc.Name = "Shadow PSO";

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

            ShaderMacroHelper Macros = CreateShaderMacros();
            var ShaderCI = new ShaderCreateInfo();
            // Tell the system that the shader source code is in HLSL.
            // For OpenGL, the engine will convert this into GLSL under the hood.
            ShaderCI.SourceLanguage = SHADER_SOURCE_LANGUAGE.SHADER_SOURCE_LANGUAGE_HLSL;
            // OpenGL backend requires emulated combined HLSL texture samplers (g_Texture + g_Texture_sampler combination)
            ShaderCI.UseCombinedTextureSamplers = true;
            // Create shadow vertex shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_VERTEX;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Shadow VS";
            ShaderCI.Source = shaderLoader.LoadShader("GLTF_PBR/private/RenderGLTF_PBR.vsh", "Common/public", "GLTF_PBR/public");
            using var pShadowVS = pDevice.CreateShader(ShaderCI, Macros);

            PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_MUTABLE;
            var Vars = new List<ShaderResourceVariableDesc>
            {
                new ShaderResourceVariableDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_VERTEX, Name = "cbTransforms",      Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC},
                new ShaderResourceVariableDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_VERTEX, Name = "cbCameraAttribs",   Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC},
                new ShaderResourceVariableDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_VERTEX, Name = "cbJointTransforms", Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC}
            };
            PSOCreateInfo.PSODesc.ResourceLayout.Variables = Vars;

            PSOCreateInfo.pVS = pShadowVS.Obj;

            // We don't use pixel shader as we are only interested in populating the depth buffer
            PSOCreateInfo.pPS = null;

            PSOCreateInfo.GraphicsPipeline.InputLayout.LayoutElements = Inputs;

            PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC;

            //if (m_pDevice.GetDeviceCaps().Features.DepthClamp) //Should look for this, need to wrap
            {
                // Disable depth clipping to render objects that are closer than near
                // clipping plane. This is not required for this tutorial, but real applications
                // will most likely want to do this.
                PSOCreateInfo.GraphicsPipeline.RasterizerDesc.DepthClipEnable = false;
            }

            m_pShadowPSO = pDevice.CreateGraphicsPipelineState(PSOCreateInfo);
            m_pShadowPSO.Obj.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_VERTEX, "cbTransforms").Set(m_TransformsCB.Obj);
            m_pShadowPSO.Obj.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_VERTEX, "cbCameraAttribs").Set(pCameraAttribs);
            m_pShadowPSO.Obj.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_VERTEX, "cbJointTransforms").Set(m_JointsBuffer.Obj);
            m_ShadowSRB = m_pShadowPSO.Obj.CreateShaderResourceBinding(true);

            //Create the shadow map now that we have a pso
            var SMDesc = new TextureDesc();
            SMDesc.Name = "Shadow map";
            SMDesc.Type = RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D;
            SMDesc.Width = m_ShadowMapSize;
            SMDesc.Height = m_ShadowMapSize;
            SMDesc.Format = m_ShadowMapFormat;
            SMDesc.BindFlags = BIND_FLAGS.BIND_SHADER_RESOURCE | BIND_FLAGS.BIND_DEPTH_STENCIL;
            using var ShadowMap = pDevice.CreateTexture(SMDesc, null);
            m_ShadowMapSRV = new AutoPtr<ITextureView>(ShadowMap.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));
            m_ShadowMapDSV = new AutoPtr<ITextureView>(ShadowMap.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_DEPTH_STENCIL));

            m_ShadowMapVisSRB = m_pShadowMapVisPSO.Obj.CreateShaderResourceBinding(true);
            m_ShadowMapVisSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "g_ShadowMap").Set(m_ShadowMapSRV.Obj);
        }

        void CreateShadowMapVisPSO(ISwapChain m_pSwapChain, IRenderDevice m_pDevice)
        {
            var PSOCreateInfo = new GraphicsPipelineStateCreateInfo();

            PSOCreateInfo.PSODesc.Name = "Shadow Map Vis PSO";

            // This is a graphics pipeline
            PSOCreateInfo.PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_GRAPHICS;

            // This tutorial renders to a single render target
            PSOCreateInfo.GraphicsPipeline.NumRenderTargets = 1;
            // Set render target format which is the format of the swap chain's color buffer
            PSOCreateInfo.GraphicsPipeline.RTVFormats_0 = m_pSwapChain.GetDesc_ColorBufferFormat;
            // Set depth buffer format which is the format of the swap chain's back buffer
            PSOCreateInfo.GraphicsPipeline.DSVFormat = m_pSwapChain.GetDesc_DepthBufferFormat;
            // Primitive topology defines what kind of primitives will be rendered by this pipeline state
            PSOCreateInfo.GraphicsPipeline.PrimitiveTopology = PRIMITIVE_TOPOLOGY.PRIMITIVE_TOPOLOGY_TRIANGLE_STRIP;
            // No cull
            PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE.CULL_MODE_NONE;
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
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Shadow Map Vis VS";
            ShaderCI.Source = shaderLoader.LoadShader("Shadow/shadow_map_vis.vsh");
            using var pShadowMapVisVS = m_pDevice.CreateShader(ShaderCI);

            // Create shadow map visualization pixel shader
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_PIXEL;
            ShaderCI.EntryPoint = "main";
            ShaderCI.Desc.Name = "Shadow Map Vis PS";
            ShaderCI.Source = shaderLoader.LoadShader("Shadow/shadow_map_vis.psh");
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

        //-------------------------- RENDERING --------------------------------

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

        public void Render(IDeviceContext pCtx,
            IShaderResourceBinding materialSRB,
            IBuffer vertexBuffer,
            IBuffer skinVertexBuffer,
            IBuffer indexBuffer,
            Uint32 numIndices,
            ref Vector3 position,
            ref Quaternion rotation,
            PbrRenderAttribs renderAttribs
            )
        {
            //Have to take inverse of rotations to have renderer render them correctly
            var nodeMatrix = rotation.inverse().toRotationMatrix4x4(position);
            Render(pCtx, materialSRB, vertexBuffer, skinVertexBuffer, indexBuffer, numIndices, ref nodeMatrix, renderAttribs);
        }

        public void Render(IDeviceContext pCtx,
            IShaderResourceBinding materialSRB,
            IBuffer vertexBuffer,
            IBuffer skinVertexBuffer,
            IBuffer indexBuffer,
            Uint32 numIndices,
            ref Vector3 position,
            ref Quaternion rotation,
            ref Vector3 scale,
            PbrRenderAttribs renderAttribs
            )
        {
            //Have to take inverse of rotations to have renderer render them correctly
            var nodeMatrix = rotation.inverse().toRotationMatrix4x4() * Matrix4x4.Scale(scale) * Matrix4x4.Translation(position);
            Render(pCtx, materialSRB, vertexBuffer, skinVertexBuffer, indexBuffer, numIndices, ref nodeMatrix, renderAttribs);
        }

        private unsafe void Render(IDeviceContext pCtx,
            IShaderResourceBinding materialSRB,
            IBuffer vertexBuffer,
            IBuffer skinVertexBuffer,
            IBuffer indexBuffer,
            Uint32 numIndices,
            ref Matrix4x4 nodeMatrix,
            PbrRenderAttribs renderAttribs
            )
        {
            var doubleSided = false;

            IBuffer[] pBuffs = new IBuffer[] { vertexBuffer, skinVertexBuffer };
            pCtx.SetVertexBuffers(0, (uint)pBuffs.Length, pBuffs, null, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION, SET_VERTEX_BUFFERS_FLAGS.SET_VERTEX_BUFFERS_FLAG_RESET);
            pCtx.SetIndexBuffer(indexBuffer, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            var Key = new PSOKey { AlphaMode = renderAttribs.AlphaMode, DoubleSided = doubleSided };
            var pCurrPSO = m_PSOCache.GetPSO(Key);
            pCtx.SetPipelineState(pCurrPSO);

            pCtx.CommitShaderResources(materialSRB, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            unsafe
            {
                IntPtr data = pCtx.MapBuffer(m_TransformsCB.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);
                var transform = (GLTFNodeShaderTransforms*)data.ToPointer();

                transform->NodeMatrix = nodeMatrix;
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
                var pGLTFAttribs = (GLTFAttribs*)data.ToPointer();

                var MaterialInfo = &pGLTFAttribs->MaterialInfo;

                MaterialInfo->BaseColorFactor = renderAttribs.BaseColorFactor;
                MaterialInfo->EmissiveFactor = renderAttribs.EmissiveFactor;
                MaterialInfo->SpecularFactor = renderAttribs.SpecularFactor;

                MaterialInfo->Workflow = (int)renderAttribs.Workflow;
                MaterialInfo->BaseColorTextureUVSelector = renderAttribs.BaseColorTextureUVSelector;
                MaterialInfo->PhysicalDescriptorTextureUVSelector = renderAttribs.PhysicalDescriptorTextureUVSelector;
                MaterialInfo->NormalTextureUVSelector = renderAttribs.NormalTextureUVSelector;

                MaterialInfo->OcclusionTextureUVSelector = renderAttribs.OcclusionTextureUVSelector;
                MaterialInfo->EmissiveTextureUVSelector = renderAttribs.EmissiveTextureUVSelector;
                MaterialInfo->BaseColorSlice = renderAttribs.BaseColorSlice;
                MaterialInfo->PhysicalDescriptorSlice = renderAttribs.PhysicalDescriptorSlice;

                MaterialInfo->NormalSlice = renderAttribs.NormalSlice;
                MaterialInfo->OcclusionSlice = renderAttribs.OcclusionSlice;
                MaterialInfo->EmissiveSlice = renderAttribs.EmissiveSlice;
                MaterialInfo->MetallicFactor = renderAttribs.MetallicFactor;

                MaterialInfo->RoughnessFactor = renderAttribs.RoughnessFactor;
                MaterialInfo->AlphaMode = (int)renderAttribs.AlphaMode;
                MaterialInfo->AlphaMaskCutoff = renderAttribs.AlphaMaskCutoff;
                MaterialInfo->Dummy0 = renderAttribs.Dummy0;

                MaterialInfo->BaseColorUVScaleBias = renderAttribs.BaseColorUVScaleBias;
                MaterialInfo->PhysicalDescriptorUVScaleBias = renderAttribs.PhysicalDescriptorUVScaleBias;
                MaterialInfo->NormalMapUVScaleBias = renderAttribs.NormalMapUVScaleBias;
                MaterialInfo->OcclusionUVScaleBias = renderAttribs.OcclusionUVScaleBias;
                MaterialInfo->EmissiveUVScaleBias = renderAttribs.EmissiveUVScaleBias;

                var ShaderParams = &pGLTFAttribs->RenderParameters;

                ShaderParams->AverageLogLum = renderAttribs.AverageLogLum;
                ShaderParams->MiddleGray = renderAttribs.MiddleGray;
                ShaderParams->WhitePoint = renderAttribs.WhitePoint;
                ShaderParams->IBLScale = renderAttribs.IBLScale;
                ShaderParams->DebugViewType = (int)renderAttribs.DebugViewType;
                ShaderParams->OcclusionStrength = renderAttribs.OcclusionStrength;
                ShaderParams->EmissionScale = renderAttribs.EmissionScale;
                ShaderParams->PrefilteredCubeMipLevels = m_Settings.UseIBL ? m_pPrefilteredEnvMapSRV.Obj.GetTexture().GetDesc_MipLevels : 0f; //This line is valid

                pCtx.UnmapBuffer(m_GLTFAttribsCB.Obj, MAP_TYPE.MAP_WRITE);
            }

            DrawIndexedAttribs DrawAttrs = new DrawIndexedAttribs();
            DrawAttrs.IndexType = VALUE_TYPE.VT_UINT32;
            DrawAttrs.NumIndices = numIndices;
            DrawAttrs.Flags = DRAW_FLAGS.DRAW_FLAG_VERIFY_ALL;
            pCtx.DrawIndexed(DrawAttrs);
        }




        public void BeginShadowMap(
            IRenderDevice renderDevice,
            IDeviceContext immediateContext,
            Vector3 m_LightDirection)
        {
            immediateContext.SetRenderTargets(null, m_ShadowMapDSV.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(m_ShadowMapDSV.Obj, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

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

            var NDCAttribs = renderDevice.GetDeviceCaps_GetNDCAttribs(); //If this does not have to be called every frame avoid it, its very slow
            float4x4 ProjToUVScale = float4x4.Scale(0.5f, NDCAttribs.YtoVScale, NDCAttribs.ZtoDepthScale);
            float4x4 ProjToUVBias = float4x4.Translation(0.5f, 0.5f, NDCAttribs.GetZtoDepthBias());

            m_WorldToShadowMapUVDepthMatr = WorldToLightProjSpaceMatr * ProjToUVScale * ProjToUVBias;
        }

        public float4x4 WorldToShadowMapUVDepthMatr => m_WorldToShadowMapUVDepthMatr;

        public void RenderShadowMap(IDeviceContext pCtx,
            IBuffer vertexBuffer,
            IBuffer skinVertexBuffer,
            IBuffer indexBuffer,
            Uint32 numIndices,
            ref Vector3 position,
            ref Quaternion rotation,
            PbrRenderAttribs renderAttribs
            )
        {
            //Have to take inverse of rotations to have renderer render them correctly
            var nodeMatrix = rotation.inverse().toRotationMatrix4x4(position);
            RenderShadowMap(pCtx, vertexBuffer, skinVertexBuffer, indexBuffer, numIndices, ref nodeMatrix, renderAttribs);
        }

        public void RenderShadowMap(IDeviceContext pCtx,
            IBuffer vertexBuffer,
            IBuffer skinVertexBuffer,
            IBuffer indexBuffer,
            Uint32 numIndices,
            ref Vector3 position,
            ref Quaternion rotation,
            ref Vector3 scale,
            PbrRenderAttribs renderAttribs
            )
        {
            //Have to take inverse of rotations to have renderer render them correctly
            var nodeMatrix = rotation.inverse().toRotationMatrix4x4() * Matrix4x4.Scale(scale) * Matrix4x4.Translation(position);
            RenderShadowMap(pCtx, vertexBuffer, skinVertexBuffer, indexBuffer, numIndices, ref nodeMatrix, renderAttribs);
        }

        public unsafe void RenderShadowMap(IDeviceContext pCtx,
            IBuffer vertexBuffer,
            IBuffer skinVertexBuffer,
            IBuffer indexBuffer,
            Uint32 numIndices,
            ref Matrix4x4 nodeMatrix,
            PbrRenderAttribs renderAttribs
            )
        {
            IBuffer[] pBuffs = new IBuffer[] { vertexBuffer, skinVertexBuffer };
            pCtx.SetVertexBuffers(0, (uint)pBuffs.Length, pBuffs, null, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION, SET_VERTEX_BUFFERS_FLAGS.SET_VERTEX_BUFFERS_FLAG_RESET);
            pCtx.SetIndexBuffer(indexBuffer, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            //Render using the shadow pso and srb.
            pCtx.SetPipelineState(m_pShadowPSO.Obj);
            pCtx.CommitShaderResources(m_ShadowSRB.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            unsafe
            {
                IntPtr data = pCtx.MapBuffer(m_TransformsCB.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);
                var transform = (GLTFNodeShaderTransforms*)data.ToPointer();

                transform->NodeMatrix = nodeMatrix;
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
                var pGLTFAttribs = (GLTFAttribs*)data.ToPointer();

                var MaterialInfo = &pGLTFAttribs->MaterialInfo;

                MaterialInfo->BaseColorFactor = renderAttribs.BaseColorFactor;
                MaterialInfo->EmissiveFactor = renderAttribs.EmissiveFactor;
                MaterialInfo->SpecularFactor = renderAttribs.SpecularFactor;

                MaterialInfo->Workflow = (int)renderAttribs.Workflow;
                MaterialInfo->BaseColorTextureUVSelector = renderAttribs.BaseColorTextureUVSelector;
                MaterialInfo->PhysicalDescriptorTextureUVSelector = renderAttribs.PhysicalDescriptorTextureUVSelector;
                MaterialInfo->NormalTextureUVSelector = renderAttribs.NormalTextureUVSelector;

                MaterialInfo->OcclusionTextureUVSelector = renderAttribs.OcclusionTextureUVSelector;
                MaterialInfo->EmissiveTextureUVSelector = renderAttribs.EmissiveTextureUVSelector;
                MaterialInfo->BaseColorSlice = renderAttribs.BaseColorSlice;
                MaterialInfo->PhysicalDescriptorSlice = renderAttribs.PhysicalDescriptorSlice;

                MaterialInfo->NormalSlice = renderAttribs.NormalSlice;
                MaterialInfo->OcclusionSlice = renderAttribs.OcclusionSlice;
                MaterialInfo->EmissiveSlice = renderAttribs.EmissiveSlice;
                MaterialInfo->MetallicFactor = renderAttribs.MetallicFactor;

                MaterialInfo->RoughnessFactor = renderAttribs.RoughnessFactor;
                MaterialInfo->AlphaMode = (int)renderAttribs.AlphaMode;
                MaterialInfo->AlphaMaskCutoff = renderAttribs.AlphaMaskCutoff;
                MaterialInfo->Dummy0 = renderAttribs.Dummy0;

                MaterialInfo->BaseColorUVScaleBias = renderAttribs.BaseColorUVScaleBias;
                MaterialInfo->PhysicalDescriptorUVScaleBias = renderAttribs.PhysicalDescriptorUVScaleBias;
                MaterialInfo->NormalMapUVScaleBias = renderAttribs.NormalMapUVScaleBias;
                MaterialInfo->OcclusionUVScaleBias = renderAttribs.OcclusionUVScaleBias;
                MaterialInfo->EmissiveUVScaleBias = renderAttribs.EmissiveUVScaleBias;

                var ShaderParams = &pGLTFAttribs->RenderParameters;

                ShaderParams->AverageLogLum = renderAttribs.AverageLogLum;
                ShaderParams->MiddleGray = renderAttribs.MiddleGray;
                ShaderParams->WhitePoint = renderAttribs.WhitePoint;
                ShaderParams->IBLScale = renderAttribs.IBLScale;
                ShaderParams->DebugViewType = (int)renderAttribs.DebugViewType;
                ShaderParams->OcclusionStrength = renderAttribs.OcclusionStrength;
                ShaderParams->EmissionScale = renderAttribs.EmissionScale;
                ShaderParams->PrefilteredCubeMipLevels = m_Settings.UseIBL ? m_pPrefilteredEnvMapSRV.Obj.GetTexture().GetDesc_MipLevels : 0f; //This line is valid

                pCtx.UnmapBuffer(m_GLTFAttribsCB.Obj, MAP_TYPE.MAP_WRITE);
            }

            DrawIndexedAttribs DrawAttrs = new DrawIndexedAttribs();
            DrawAttrs.IndexType = VALUE_TYPE.VT_UINT32;
            DrawAttrs.NumIndices = numIndices;
            DrawAttrs.Flags = DRAW_FLAGS.DRAW_FLAG_VERIFY_ALL;
            pCtx.DrawIndexed(DrawAttrs);
        }

        public void RenderShadowMapVis(IDeviceContext m_pImmediateContext)
        {
            m_pImmediateContext.SetPipelineState(m_pShadowMapVisPSO.Obj);
            m_pImmediateContext.CommitShaderResources(m_ShadowMapVisSRB.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            var DrawAttrs = new DrawAttribs { NumVertices = 4, Flags = DRAW_FLAGS.DRAW_FLAG_VERIFY_ALL };
            m_pImmediateContext.Draw(DrawAttrs);
        }
    }
}
