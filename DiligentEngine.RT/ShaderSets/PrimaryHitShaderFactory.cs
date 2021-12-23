using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT.ShaderSets
{
    public class PrimaryHitShaderFactory
    {
        private readonly GraphicsEngine graphicsEngine;
        private readonly ShaderLoader<RTShaders> shaderLoader;
        private readonly RayTracingRenderer rayTracingRenderer;

        public PrimaryHitShaderFactory(GraphicsEngine graphicsEngine, ShaderLoader<RTShaders> shaderLoader, RayTracingRenderer rayTracingRenderer)
        {
            this.graphicsEngine = graphicsEngine;
            this.shaderLoader = shaderLoader;
            this.rayTracingRenderer = rayTracingRenderer;
        }

        /// <summary>
        /// Create a shader. The caller is responsible for disposing the instance.
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="numTextures"></param>
        /// <returns></returns>
        public async Task<PrimaryHitShader> Create(String baseName, int numTextures)
        {
            var shader = new PrimaryHitShader();
            await shader.SetupShaders(baseName, numTextures, graphicsEngine, shaderLoader, rayTracingRenderer);
            return shader;
        }
    }
}
