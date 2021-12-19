using DiligentEngine;
using DiligentEngine.RT;
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
            services.AddSingleton<IResourceProvider<ShaderLoader<RTShaders>>>(s =>
                new EmbeddedResourceProvider<ShaderLoader<RTShaders>>(Assembly.GetExecutingAssembly(), "DiligentEngine.RT."));

            services.AddSingleton<RTImageBlitter>();
            services.AddSingleton<CubeBLAS>();
            services.AddSingleton<ProceduralBLAS>();
            services.AddSingleton<RTCameraAndLight>();
            services.AddSingleton<RTInstances>();

            return services;
        }
    }
}
