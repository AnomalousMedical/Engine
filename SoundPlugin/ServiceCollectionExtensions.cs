using SoundPlugin;
using Engine;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSoundPlugin(this IServiceCollection services, PluginManager pluginManager, Action<SoundPluginOptions> configure = null)
        {
            var options = new SoundPluginOptions();
            configure?.Invoke(options);

            pluginManager.AddPlugin(new SoundPluginInterface());

            services.AddSingleton<SoundPluginOptions>(options);
            services.TryAddSingleton<SoundState>();

            return services;
        }
    }
}
