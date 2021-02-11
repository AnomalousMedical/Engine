using DiligentEngine;
using DiligentEngine.GltfPbr;
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

            services.AddSingleton<IResourceProvider<ShaderLoader<PbrRenderer>>>(s =>
                new EmbeddedResourceProvider<ShaderLoader<PbrRenderer>>(Assembly.GetExecutingAssembly(), "DiligentEngine.GltfPbr."));
            services.TryAddSingleton<ShaderLoader<PbrRenderer>>();

            services.AddSingleton<IResourceProvider<CC0TextureLoader>>(s =>
                            new VirtualFilesystemResourceProvider<CC0TextureLoader>(s.GetRequiredService<VirtualFileSystem>()));
            services.TryAddSingleton<CC0TextureLoader>();

            services.AddSingleton<IResourceProvider<EnvironmentMapBuilder>>(s =>
                            new VirtualFilesystemResourceProvider<EnvironmentMapBuilder>(s.GetRequiredService<VirtualFileSystem>()));
            services.TryAddSingleton<EnvironmentMapBuilder>();

            services.AddSingleton<IPbrCameraAndLight, PbrCameraAndLight>();

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
