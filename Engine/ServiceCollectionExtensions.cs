using Engine;
using Engine.CameraMovement;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AnomalousEngineServiceCollectionExtensions
    {
        public static IServiceCollection AddFirstPersonFlyCamera(this IServiceCollection services, Action<FirstPersonFlyCamera.Description> configure = null)
        {
            var desc = new FirstPersonFlyCamera.Description();
            configure?.Invoke(desc);

            services.AddSingleton<FirstPersonFlyCamera>();
            services.AddSingleton<FirstPersonFlyCamera.Description>(desc);

            return services;
        }
    }
}
