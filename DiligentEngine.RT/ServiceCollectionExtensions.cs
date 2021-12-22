using DiligentEngine;
using DiligentEngine.RT;
using DiligentEngine.RT.ShaderSets;
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
        public static IServiceCollection AddDiligentEngineRt(this IServiceCollection services, Action<RTOptions> configure = null)
        {
            var options = new RTOptions();
            configure?.Invoke(options);

            services.AddSingleton<IResourceProvider<ShaderLoader<RTShaders>>>(s =>
                new EmbeddedResourceProvider<ShaderLoader<RTShaders>>(Assembly.GetExecutingAssembly(), "DiligentEngine.RT."));

            services.AddSingleton<BLASBuilder>();
            services.AddSingleton<RTImageBlitter>();
            services.AddSingleton<CubeBLAS>();
            services.AddTransient<MeshBLAS>();
            services.AddSingleton<RTCameraAndLight>();
            services.AddSingleton<RayTracingRenderer>();
            services.AddSingleton<GeneralShaders>();

            services.AddTransient<PrimaryHitShader>();
            services.AddTransient<TextureSet>();

            return services;
        }
    }
}
