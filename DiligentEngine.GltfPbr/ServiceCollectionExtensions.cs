using DiligentEngine;
using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
using Engine;
using Engine.Resources;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDiligentEnginePbrShapes(this IServiceCollection services)
        {
            services.TryAddSingleton<Cube>();
            services.TryAddSingleton<DoubleSizeCube>();
            services.TryAddSingleton<Plane>();

            return services;
        }

        public static IServiceCollection AddDiligentEnginePbr(this IServiceCollection services, Action<PbrOptions> configure = null)
        {
            PbrOptions options = CommonRegister(services, configure);

            services.AddSingleton<PbrRenderer>(s =>
            {
                var shapeLoader = s.GetRequiredService<ShaderLoader<PbrRenderer>>();
                var ge = s.GetRequiredService<GraphicsEngine>();

                var RendererCI = new PbrRendererCreateInfo();
                options.CustomizePbrOptions?.Invoke(RendererCI);
                CheckTextureFormats(ge, RendererCI);

                return new PbrRenderer(ge.RenderDevice, ge.ImmediateContext, RendererCI, shapeLoader);
            });

            return services;
        }

        public static IServiceCollection AddDiligentEnginePbr<T>(this IServiceCollection services, Action<PbrOptions> configure = null)
        {
            PbrOptions options = CommonRegister(services, configure);

            services.AddSingleton<PbrRenderer<T>>(s =>
            {
                var shapeLoader = s.GetRequiredService<ShaderLoader<PbrRenderer>>();
                var ge = s.GetRequiredService<GraphicsEngine>();
            
                var RendererCI = new PbrRendererCreateInfo();
                options.CustomizePbrOptions?.Invoke(RendererCI);
                CheckTextureFormats(ge, RendererCI);

                return new PbrRenderer<T>(ge.RenderDevice, ge.ImmediateContext, RendererCI, shapeLoader);
            });

            return services;
        }

        private static PbrOptions CommonRegister(IServiceCollection services, Action<PbrOptions> configure)
        {
            var options = new PbrOptions();
            configure?.Invoke(options);

            //The Pbr Renderer Shaders are embedded, so override the resource provider to get them from
            //the embedded file resources
            services.AddSingleton<IResourceProvider<ShaderLoader<PbrRenderer>>>(s =>
                new EmbeddedResourceProvider<ShaderLoader<PbrRenderer>>(Assembly.GetExecutingAssembly(), "DiligentEngine.GltfPbr."));

            services.TryAddSingleton<CC0TextureLoader>();
            services.TryAddSingleton<ICC0MaterialTextureBuilder, CC0MaterialTextureBuilder>();
            services.TryAddSingleton<EnvironmentMapBuilder>();

            services.TryAddSingleton<IPbrCameraAndLight, PbrCameraAndLight>();

            return options;
        }

        private static void CheckTextureFormats(GraphicsEngine ge, PbrRendererCreateInfo RendererCI)
        {
            if (RendererCI.RTVFmt == TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN)
            {
                RendererCI.RTVFmt = ge.SwapChain.GetDesc_ColorBufferFormat;
            }
            if (RendererCI.DSVFmt == TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN)
            {
                RendererCI.DSVFmt = ge.SwapChain.GetDesc_DepthBufferFormat;
            }
        }
    }
}
