using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        public static NativeOSWindow CreateAndAddNativeOSWindow(this IServiceCollection services, App app, Action<WindowOptions> configureWindowOptions = null)
        {
            var options = new WindowOptions();

            configureWindowOptions?.Invoke(options);

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

        public static IServiceCollection AddOsPlatform(this IServiceCollection services, PluginManager pluginManager)
        {
            services.TryAddSingleton<SystemTimer, NativeSystemTimer>();
            services.TryAddSingleton<UpdateTimer, NativeUpdateTimer>();

            pluginManager.addPlugin(new NativePlatformPlugin());

            return services;
        }
    }
}
