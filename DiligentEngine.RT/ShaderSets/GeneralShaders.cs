using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT.ShaderSets
{
    public class GeneralShaders : IDisposable
    {
        private RayTracingPipelineStateCreateInfo PSOCreateInfo;
        private AutoPtr<IShader> pRayGen;
        private AutoPtr<IShader> pPrimaryMiss;
        private AutoPtr<IShader> pShadowMiss;

        private RayTracingGeneralShaderGroup rayGenShaderGroup;
        private RayTracingGeneralShaderGroup primaryMissShaderGroup;
        private RayTracingGeneralShaderGroup shadowMissShaderGroup;
        private readonly GraphicsEngine graphicsEngine;
        private readonly ShaderLoader shaderLoader;

        public GeneralShaders(GraphicsEngine graphicsEngine, ShaderLoader<RTShaders> shaderLoader)
        {
            this.graphicsEngine = graphicsEngine;
            this.shaderLoader = shaderLoader;
        }

        public void Setup(RayTracingPipelineStateCreateInfo PSOCreateInfo)
        { 
            this.PSOCreateInfo = PSOCreateInfo;
            var m_pDevice = graphicsEngine.RenderDevice;

            // Define shader macros
            ShaderMacroHelper Macros = new ShaderMacroHelper();

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
            pRayGen = m_pDevice.CreateShader(ShaderCI, Macros);
            //VERIFY_EXPR(pRayGen != nullptr);

            // Create miss shaders.
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_RAY_MISS;
            ShaderCI.Desc.Name = "Primary ray miss shader";
            ShaderCI.Source = shaderLoader.LoadShader("assets/PrimaryMiss.rmiss");
            ShaderCI.EntryPoint = "main";
            pPrimaryMiss = m_pDevice.CreateShader(ShaderCI, Macros);
            //VERIFY_EXPR(pPrimaryMiss != nullptr);

            ShaderCI.Desc.Name = "Shadow ray miss shader";
            ShaderCI.Source = shaderLoader.LoadShader("assets/ShadowMiss.rmiss");
            ShaderCI.EntryPoint = "main";
            pShadowMiss = m_pDevice.CreateShader(ShaderCI, Macros);
            //VERIFY_EXPR(pShadowMiss != nullptr);

            // Ray generation shader is an entry point for a ray tracing pipeline.
            rayGenShaderGroup = new RayTracingGeneralShaderGroup { Name = "Main", pShader = pRayGen.Obj };
            // Primary ray miss shader.
            primaryMissShaderGroup = new RayTracingGeneralShaderGroup { Name = "PrimaryMiss", pShader = pPrimaryMiss.Obj };
            // Shadow ray miss shader.
            shadowMissShaderGroup = new RayTracingGeneralShaderGroup { Name = "ShadowMiss", pShader = pShadowMiss.Obj };

            PSOCreateInfo.pGeneralShaders.Add(rayGenShaderGroup);
            PSOCreateInfo.pGeneralShaders.Add(primaryMissShaderGroup);
            PSOCreateInfo.pGeneralShaders.Add(shadowMissShaderGroup);
        }

        public void Dispose()
        {
            PSOCreateInfo.pGeneralShaders.Remove(shadowMissShaderGroup);
            PSOCreateInfo.pGeneralShaders.Remove(primaryMissShaderGroup);
            PSOCreateInfo.pGeneralShaders.Remove(rayGenShaderGroup);

            pRayGen.Dispose();
            pPrimaryMiss.Dispose();
            pShadowMiss.Dispose();
        }
    }
}
