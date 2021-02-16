using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SharpImGuiTest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OSPlatformServiceCollectionExtensions
    {
        public static IServiceCollection AddSharpGui(this IServiceCollection services)
        {
            services.TryAddSingleton<SharpGuiState>();
            services.TryAddSingleton<SharpGuiBuffer>();
            services.TryAddSingleton<SharpGuiRenderer>();

            return services;
        }
    }
}
