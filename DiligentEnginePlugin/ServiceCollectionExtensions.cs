using DiligentEnginePlugin;
using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDiligentEngine(this IServiceCollection services, PluginManager pluginManager)
        {
            pluginManager.addPlugin(new DiligentEnginePluginInterface());

            return services;
        }
    }
}
