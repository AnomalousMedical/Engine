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
        public static IServiceCollection AddDiligentEnginePbr(this IServiceCollection services)
        {
            services.AddSingleton<ShaderLoader<PbrRenderer>>(s =>
            {
                return new ShaderLoader<PbrRenderer>(new EmbeddedResourceProvider(Assembly.GetExecutingAssembly(), "DiligentEngine.GltfPbr."));
            });

            services.AddSingleton(typeof(PbrRenderer), s =>
            {
                var shapeLoader = s.GetRequiredService<ShaderLoader<PbrRenderer>>();
                var ge = s.GetRequiredService<GraphicsEngine>();
                var m_pSwapChain = ge.SwapChain;

                var BackBufferFmt = m_pSwapChain.GetDesc_ColorBufferFormat;
                var DepthBufferFmt = m_pSwapChain.GetDesc_DepthBufferFormat;

                var RendererCI = new PbrRendererCreateInfo();
                RendererCI.RTVFmt = BackBufferFmt;
                RendererCI.DSVFmt = DepthBufferFmt;
                RendererCI.AllowDebugView = true;
                RendererCI.UseIBL = true;
                
                return new PbrRenderer(ge.RenderDevice, ge.ImmediateContext, RendererCI, shapeLoader);
            });

            return services;
        }
    }
}
