using BepuPlugin;
using Engine;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBepuPlugin(this IServiceCollection services)
        {
            services.TryAddSingleton<IBepuScene, BepuScene>();

            return services;
        }
    }
}
