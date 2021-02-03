using Engine;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEnginePlugin
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
