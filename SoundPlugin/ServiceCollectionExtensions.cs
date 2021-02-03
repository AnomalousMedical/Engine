using SoundPlugin;
using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSoundPlugin(this IServiceCollection services, PluginManager pluginManager)
        {
            pluginManager.addPlugin(new SoundPluginInterface());

            return services;
        }
    }
}
