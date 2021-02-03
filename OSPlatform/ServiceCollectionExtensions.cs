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
        /// <summary>
        /// Create a native os window
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static NativeOSWindow CreateAndAddNativeOSWindow(this IServiceCollection services, App app, Action<WindowOptions> configure = null)
        {
            var options = new WindowOptions();

            configure?.Invoke(options);

            var mainWindow = new NativeOSWindow(options.Title, options.Position, options.Size);
            services.TryAddSingleton<OSWindow>(mainWindow); //This is externally owned
            services.TryAddSingleton<NativeOSWindow>(mainWindow); //This is externally owned

            if (options.Fullscreen)
            {
                mainWindow.setSize(options.FullScreenSize.Width, options.FullScreenSize.Height);
                mainWindow.ExclusiveFullscreen = true;
            }
            else
            {
                mainWindow.Maximized = options.Maximized;
            }
            mainWindow.show();

            mainWindow.Closed += w =>
            {
                mainWindow.close();
                app.Exit();
            };

            //Setup DPI
            float pixelScale = mainWindow.WindowScaling;
            ScaleHelper._setScaleFactor(pixelScale);

            return mainWindow;
        }

        public static void DestroyNativeOSWindow(NativeOSWindow window)
        {
            window.Dispose();
        }

        public static IServiceCollection AddOSPlatform(this IServiceCollection services, PluginManager pluginManager, Action<OSPlatformOptions> configure = null)
        {
            var options = new OSPlatformOptions();
            configure?.Invoke(options);

            services.TryAddSingleton<InputHandler>(s =>
            {
                bool makeConfig_EnableMultitouch = false;
                return new NativeInputHandler(s.GetRequiredService<NativeOSWindow>(), makeConfig_EnableMultitouch, s.GetRequiredService<ILogger<NativeInputHandler>>());
            });

            services.TryAddSingleton<EventManager>(s =>
            {
                return new EventManager(s.GetRequiredService<InputHandler>(), Enum.GetValues(options.EventLayersType), s.GetRequiredService<ILogger<EventManager>>());
            });

            services.TryAddSingleton<SystemTimer, NativeSystemTimer>();
            services.TryAddSingleton<UpdateTimer, NativeUpdateTimer>();

            pluginManager.addPlugin(new NativePlatformPlugin());

            return services;
        }
    }
}
