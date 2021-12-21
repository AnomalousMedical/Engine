using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT.ShaderSets
{
    public class PrimaryHitShader : IDisposable
    {
        public class Desc
        {
            public int NumTextures { get; set; } = 1;
        }

        private readonly RayTracingPipelineStateCreateInfo PSOCreateInfo;
        private readonly AutoPtr<IShader> pCubePrimaryHit;
        private readonly RayTracingTriangleHitShaderGroup primaryHitShaderGroup;

        private readonly ShaderResourceVariableDesc verticesDesc;
        private readonly ShaderResourceVariableDesc indicesDesc;


        public PrimaryHitShader(GraphicsEngine graphicsEngine, ShaderLoader<RTShaders> shaderLoader, RayTracingPipelineStateCreateInfo PSOCreateInfo, Desc desc)
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

            var shaderVars = CreateShaderVars(desc);

            // Create closest hit shaders.
            ShaderCI.Desc.ShaderType = SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT;
            ShaderCI.Desc.Name = "Cube primary ray closest hit shader";
            ShaderCI.Source = shaderLoader.LoadShader(shaderVars, "assets/CubePrimaryHit.rchit");
            ShaderCI.EntryPoint = "main";
            pCubePrimaryHit = m_pDevice.CreateShader(ShaderCI, Macros);
            //VERIFY_EXPR(pCubePrimaryHit != nullptr);

            // Primary ray hit group for the textured cube.
            primaryHitShaderGroup = new RayTracingTriangleHitShaderGroup { Name = "CubePrimaryHit", pClosestHitShader = pCubePrimaryHit.Obj };

            verticesDesc = new ShaderResourceVariableDesc { ShaderStages = SHADER_TYPE.SHADER_TYPE_RAY_GEN | SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, Name = "g_Vertices", Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC };
            indicesDesc = new ShaderResourceVariableDesc { ShaderStages = SHADER_TYPE.SHADER_TYPE_RAY_GEN | SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, Name = "g_Indices", Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC };

            PSOCreateInfo.pTriangleHitShaders.Add(primaryHitShaderGroup);

            PSOCreateInfo.PSODesc.ResourceLayout.Variables.Add(verticesDesc);
            PSOCreateInfo.PSODesc.ResourceLayout.Variables.Add(indicesDesc);
        }

        public void Dispose()
        {
            PSOCreateInfo.PSODesc.ResourceLayout.Variables.Remove(indicesDesc);
            PSOCreateInfo.PSODesc.ResourceLayout.Variables.Remove(verticesDesc);

            PSOCreateInfo.pTriangleHitShaders.Remove(primaryHitShaderGroup);

            pCubePrimaryHit.Dispose();
        }

        public void BindBlas(BLASInstance bLASInstance, IShaderResourceBinding rayTracingSRB)
        {
            rayTracingSRB.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, "g_Vertices").Set(bLASInstance.AttrVertexBuffer.Obj.GetDefaultView(BUFFER_VIEW_TYPE.BUFFER_VIEW_SHADER_RESOURCE));
            rayTracingSRB.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, "g_Indices").Set(bLASInstance.IndexBuffer.Obj.GetDefaultView(BUFFER_VIEW_TYPE.BUFFER_VIEW_SHADER_RESOURCE));
        }

        public void BindTextures(IShaderResourceBinding m_pRayTracingSRB, TextureManager textureManager)
        {
            m_pRayTracingSRB.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, "g_CubeTextures")?.SetArray(textureManager.TexSRVs);
            m_pRayTracingSRB.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, "g_CubeNormalTextures")?.SetArray(textureManager.TexNormalSRVs);
        }

        private Dictionary<String, String> CreateShaderVars(Desc description)
        {
            var shaderVars = new Dictionary<string, string>()
            {
                { "NUM_TEXTURES", description.NumTextures.ToString() },
                { "VERTICES", "g_Vertices" },
                { "INDICES", "g_Indices" },
                { "COLOR_TEXTURES", "g_CubeTextures" },
                { "NORMAL_TEXTURES", "g_CubeNormalTextures" }
            };

            return shaderVars;
        }
    }
}
