using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OSPlatformServiceCollectionExtensions
    {
        public static IServiceCollection AddOSPlatform(this IServiceCollection services, PluginManager pluginManager, Action<OSPlatformOptions> configure = null)
        {
            var options = new OSPlatformOptions();
            configure?.Invoke(options);

            services.TryAddSingleton<InputHandler>(s =>
            {
                return new NativeInputHandler(s.GetRequiredService<NativeOSWindow>(), options.EnableMultitiouch, s.GetRequiredService<ILogger<NativeInputHandler>>());
            });

            services.TryAddSingleton<EventManager>(s =>
            {
                return new EventManager(s.GetRequiredService<InputHandler>(), Enum.GetValues(options.EventLayersType), s.GetRequiredService<ILogger<EventManager>>());
            });

            services.TryAddSingleton<SystemTimer, NativeSystemTimer>();

            pluginManager.AddPlugin(new NativePlatformPlugin());

            return services;
        }
    }
}
