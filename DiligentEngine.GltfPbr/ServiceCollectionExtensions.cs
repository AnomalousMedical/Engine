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
            var options = new PbrOptions();
            configure?.Invoke(options);

            services.AddSingleton<ShaderLoader<PbrRenderer>>(s =>
            {
                return new ShaderLoader<PbrRenderer>(new EmbeddedResourceProvider(Assembly.GetExecutingAssembly(), "DiligentEngine.GltfPbr."));
            });

            services.AddSingleton(typeof(PbrRenderer), s =>
            {
                var shapeLoader = s.GetRequiredService<ShaderLoader<PbrRenderer>>();
                var ge = s.GetRequiredService<GraphicsEngine>();

                var RendererCI = new PbrRendererCreateInfo();
                options.CustomizePbrOptions?.Invoke(RendererCI);
                if (RendererCI.RTVFmt == TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN)
                {
                    RendererCI.RTVFmt = ge.SwapChain.GetDesc_ColorBufferFormat;
                }
                if (RendererCI.DSVFmt == TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN)
                { 
                    RendererCI.DSVFmt = ge.SwapChain.GetDesc_DepthBufferFormat;
                }
                
                return new PbrRenderer(ge.RenderDevice, ge.ImmediateContext, RendererCI, shapeLoader);
            });

            return services;
        }
    }
}
