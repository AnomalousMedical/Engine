using DiligentEngine.RT.ShaderSets;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT
{ 
    //Injection class to type to rt system.
    public class RTShaders
    {
        private readonly ShaderLoader<RTShaders> shaderLoader;
        private readonly GraphicsEngine graphicsEngine;
        internal RayTracingPipelineStateCreateInfo PSOCreateInfo { get; private set; }

        public RTShaders(ShaderLoader<RTShaders> shaderLoader, GraphicsEngine graphicsEngine)
        {
            this.shaderLoader = shaderLoader;
            this.graphicsEngine = graphicsEngine;
            this.PSOCreateInfo = CreatePSOCreateInfo();
        }

        private unsafe static RayTracingPipelineStateCreateInfo CreatePSOCreateInfo()
        {
            // Prepare ray tracing pipeline description.
            RayTracingPipelineStateCreateInfo PSOCreateInfo = new RayTracingPipelineStateCreateInfo();

            PSOCreateInfo.PSODesc.Name = "Ray tracing PSO";
            PSOCreateInfo.PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_RAY_TRACING;

            // Setup shader groups
            PSOCreateInfo.pGeneralShaders = new List<RayTracingGeneralShaderGroup>();

            PSOCreateInfo.pTriangleHitShaders = new List<RayTracingTriangleHitShaderGroup>();

            PSOCreateInfo.pProceduralHitShaders = new List<RayTracingProceduralHitShaderGroup>();

            // Per-shader data is not used.
            PSOCreateInfo.RayTracingPipeline.ShaderRecordSize = 0;

            // DirectX 12 only: set attribute and payload size. Values should be as small as possible to minimize the memory usage.
            PSOCreateInfo.MaxAttributeSize = (uint)sizeof(/*BuiltInTriangleIntersectionAttributes*/ Vector2);
            PSOCreateInfo.MaxPayloadSize = (uint)Math.Max(sizeof(DiligentEngine.RT.HLSL.PrimaryRayPayload), sizeof(DiligentEngine.RT.HLSL.ShadowRayPayload));


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
                new ImmutableSamplerDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, SamplerOrTextureName = "g_SamLinearWrap", Desc = SamLinearWrapDesc},
                new ImmutableSamplerDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_RAY_ANY_HIT, SamplerOrTextureName = "g_SamLinearWrap", Desc = SamLinearWrapDesc}
            };

            var Variables = new List<ShaderResourceVariableDesc> //
            {
                new ShaderResourceVariableDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_RAY_GEN | SHADER_TYPE.SHADER_TYPE_RAY_MISS | SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, Name = "g_ConstantsCB", Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC},
                new ShaderResourceVariableDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_RAY_GEN | SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, Name = "g_TLAS", Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC},
                new ShaderResourceVariableDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_RAY_GEN, Name = "g_ColorBuffer", Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC} //This is the buffer where the rays are written
            };

            PSOCreateInfo.PSODesc.ResourceLayout.Variables = Variables;
            PSOCreateInfo.PSODesc.ResourceLayout.ImmutableSamplers = ImmutableSamplers;
            PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_MUTABLE;

            return PSOCreateInfo;
        }

        /// <summary>
        /// Create a new GeneralShaders group. The caller is responsible for disposing the returned object.
        /// </summary>
        /// <returns></returns>
        public GeneralShaders CreateGeneralShaders()
        {
            return new GeneralShaders(graphicsEngine, shaderLoader, PSOCreateInfo);
        }

        /// <summary>
        /// Create a new PrimaryHitShader group. The caller is responsible for disposing the returned object.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public PrimaryHitShader CreatePrimaryHitShader(PrimaryHitShader.Desc description)
        {
            return new PrimaryHitShader(graphicsEngine, shaderLoader, PSOCreateInfo, description);
        }
    }
}
