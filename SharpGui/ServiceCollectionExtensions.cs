using Engine;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OSPlatformServiceCollectionExtensions
    {
        public static IServiceCollection AddSharpGui(this IServiceCollection services, Action<SharpGuiOptions> configure = null)
        {
            var options = new SharpGuiOptions();
            configure?.Invoke(options);

            services.AddSingleton<SharpGuiOptions>(options);
            services.TryAddSingleton<ISharpGui, SharpGuiImpl>();
            services.TryAddSingleton<SharpGuiBuffer>();
            services.TryAddSingleton<SharpGuiRenderer>();

            return services;
        }
    }
}
