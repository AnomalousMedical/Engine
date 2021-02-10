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

            return services;
        }
    }
}
