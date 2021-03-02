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
    public class SimpleShadowMapRenderer : IDisposable
    {
        private readonly ShaderLoader<SimpleShadowMapRenderer> shaderLoader;

        private AutoPtr<IPipelineState> m_pShadowPSO;
        private AutoPtr<IShaderResourceBinding> m_ShadowSRB;
        private AutoPtr<IPipelineState> m_pShadowMapVisPSO;
        private AutoPtr<ITextureView> m_ShadowMapDSV;
        private AutoPtr<ITextureView> m_ShadowMapSRV;
        private AutoPtr<IShaderResourceBinding> m_ShadowMapVisSRB;

        TEXTURE_FORMAT m_ShadowMapFormat = TEXTURE_FORMAT.TEX_FORMAT_D16_UNORM;
        float4x4 m_WorldToShadowMapUVDepthMatr;
        float4x4 m_WorldToLightProjSpaceMatr;
        Uint32 m_ShadowMapSize = 512;
        private AutoPtr<IBuffer> m_ShadowTransformsCB;

        public ITextureView ShadowMapSRV => m_ShadowMapSRV.Obj;

        public SimpleShadowMapRenderer(ShaderLoader<SimpleShadowMapRenderer> shaderLoader, GraphicsEngine graphicsEngine, bool enableShadowMapVis)
        {
            this.shaderLoader = shaderLoader;
            IRenderDevice pDevice = graphicsEngine.RenderDevice;
            IDeviceContext pCtx = graphicsEngine.ImmediateContext;

            unsafe
            {
                BufferDesc CBDesc = new BufferDesc();
                CBDesc.Name = "GLTF shadow transform CB";
                CBDesc.uiSizeInBytes = (uint)sizeof(GLTFShadowTransform);
                CBDesc.Usage = USAGE.USAGE_DYNAMIC;
                CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;

                m_ShadowTransformsCB = pDevice.CreateBuffer(CBDesc);
            }

            var Barriers = new List<StateTransitionDesc>
            {
                new StateTransitionDesc{pResource = m_ShadowTransformsCB.Obj,  OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_CONSTANT_BUFFER, UpdateResourceState = true}
            };
            pCtx.TransitionResourceStates(Barriers);

            if (enableShadowMapVis)
            {
                CreateShadowMapVisPSO(graphicsEngine.SwapChain, pDevice);
            }

            CreateShadowPSO(graphicsEngine.RenderDevice);
        }

        public void Dispose()
        {
            m_pShadowMapVisPSO?.Dispose();
            m_pShadowPSO?.Dispose();
            m_ShadowMapVisSRB?.Dispose();
            m_ShadowSRB?.Dispose();
            m_ShadowMapDSV?.Dispose();
            m_ShadowMapSRV?.Dispose();
            m_ShadowTransformsCB?.Dispose();
        }

        private void CreateShadowPSO(IRenderDevice pDevice)
        {
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
            PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode = CULL_MODE.CULL_MODE_NONE; //Might want to make this an option or something, but makes plane shadows render
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
            ShaderCI.Desc.Name = "Shadow VS";
            ShaderCI.Source = shaderLoader.LoadShader("GLTF_PBR/private/shadow.vsh");
            using var pShadowVS = pDevice.CreateShader(ShaderCI);

            PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_MUTABLE;
            var Vars = new List<ShaderResourceVariableDesc>
            {
                new ShaderResourceVariableDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_VERTEX, Name = "cbTransform",      Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC}
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
            m_pShadowPSO.Obj.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_VERTEX, "cbTransform").Set(m_ShadowTransformsCB.Obj);
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

            if (m_pShadowMapVisPSO != null)
            {
                m_ShadowMapVisSRB = m_pShadowMapVisPSO.Obj.CreateShaderResourceBinding(true);
                m_ShadowMapVisSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_PIXEL, "g_ShadowMap").Set(m_ShadowMapSRV.Obj);
            }
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

        public void BeginShadowMap(
            IRenderDevice renderDevice,
            IDeviceContext immediateContext,
            in Vector3 m_LightDirection,
            in float3 f3SceneCenter,
            float SceneRadius)
        {
            immediateContext.SetRenderTargets(null, m_ShadowMapDSV.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(m_ShadowMapDSV.Obj, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            float3 f3LightSpaceX, f3LightSpaceY, f3LightSpaceZ;
            f3LightSpaceZ = m_LightDirection.normalized();

            var min_cmp = Math.Min(Math.Min(Math.Abs(m_LightDirection.x), Math.Abs(m_LightDirection.y)), Math.Abs(m_LightDirection.z));
            if (min_cmp == Math.Abs(m_LightDirection.x))
            {
                f3LightSpaceX = new float3(1, 0, 0);
            }
            else if (min_cmp == Math.Abs(m_LightDirection.y))
            {
                f3LightSpaceX = new float3(0, 1, 0);
            }
            else
            {
                f3LightSpaceX = new float3(0, 0, 1);
            }

            f3LightSpaceY = f3LightSpaceZ.cross(ref f3LightSpaceX);
            f3LightSpaceX = f3LightSpaceY.cross(ref f3LightSpaceZ);
            f3LightSpaceX = f3LightSpaceX.normalized();
            f3LightSpaceY = f3LightSpaceY.normalized();

            float4x4 WorldToLightViewSpaceMatr = float4x4.ViewFromBasis(ref f3LightSpaceX, ref f3LightSpaceY, ref f3LightSpaceZ);

            SceneRadius = (float)Math.Sqrt(SceneRadius);
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

            this.m_WorldToShadowMapUVDepthMatr = WorldToLightProjSpaceMatr * ProjToUVScale * ProjToUVBias;
            this.m_WorldToLightProjSpaceMatr = WorldToLightProjSpaceMatr;
        }

        public float4x4 WorldToShadowMapUVDepthMatr => m_WorldToShadowMapUVDepthMatr;

        public void RenderShadowMap(IDeviceContext pCtx,
            IBuffer vertexBuffer,
            IBuffer skinVertexBuffer,
            IBuffer indexBuffer,
            Uint32 numIndices,
            ref Vector3 position,
            ref Quaternion rotation
            )
        {
            //Have to take inverse of rotations to have renderer render them correctly
            var nodeMatrix = rotation.inverse().toRotationMatrix4x4(position);
            RenderShadowMap(pCtx, vertexBuffer, skinVertexBuffer, indexBuffer, numIndices, ref nodeMatrix);
        }

        public void RenderShadowMap(IDeviceContext pCtx,
            IBuffer vertexBuffer,
            IBuffer skinVertexBuffer,
            IBuffer indexBuffer,
            Uint32 numIndices,
            ref Vector3 position,
            ref Quaternion rotation,
            ref Vector3 scale
            )
        {
            //Have to take inverse of rotations to have renderer render them correctly
            var nodeMatrix = rotation.inverse().toRotationMatrix4x4() * Matrix4x4.Scale(scale) * Matrix4x4.Translation(position);
            RenderShadowMap(pCtx, vertexBuffer, skinVertexBuffer, indexBuffer, numIndices, ref nodeMatrix);
        }

        public unsafe void RenderShadowMap(IDeviceContext pCtx,
            IBuffer vertexBuffer,
            IBuffer skinVertexBuffer,
            IBuffer indexBuffer,
            Uint32 numIndices,
            ref Matrix4x4 nodeMatrix
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
                IntPtr data = pCtx.MapBuffer(m_ShadowTransformsCB.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);
                var transform = (GLTFShadowTransform*)data.ToPointer();

                transform->WorldViewProj = (nodeMatrix * m_WorldToLightProjSpaceMatr).Transpose();

                pCtx.UnmapBuffer(m_ShadowTransformsCB.Obj, MAP_TYPE.MAP_WRITE);
            }

            DrawIndexedAttribs DrawAttrs = new DrawIndexedAttribs();
            DrawAttrs.IndexType = VALUE_TYPE.VT_UINT32;
            DrawAttrs.NumIndices = numIndices;
            DrawAttrs.Flags = DRAW_FLAGS.DRAW_FLAG_VERIFY_ALL;
            pCtx.DrawIndexed(DrawAttrs);
        }

        public void RenderShadowMapVis(IDeviceContext m_pImmediateContext)
        {
            if (m_pShadowMapVisPSO != null)
            {
                m_pImmediateContext.SetPipelineState(m_pShadowMapVisPSO.Obj);
                m_pImmediateContext.CommitShaderResources(m_ShadowMapVisSRB.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

                var DrawAttrs = new DrawAttribs { NumVertices = 4, Flags = DRAW_FLAGS.DRAW_FLAG_VERIFY_ALL };
                m_pImmediateContext.Draw(DrawAttrs);
            }
        }
    }
}
