using Anomalous.OSPlatform;
using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOsPlatform(this IServiceCollection services, PluginManager pluginManager)
        {
            pluginManager.addPlugin(new NativePlatformPlugin());

            return services;
        }
    }
}
