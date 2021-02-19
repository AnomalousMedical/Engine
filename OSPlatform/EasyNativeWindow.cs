using Engine;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anomalous.OSPlatform
{
    public static class EasyNativeWindow
    {
        /// <summary>
        /// Create a native os window
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static NativeOSWindow Create(this IServiceCollection services, App app, Action<WindowOptions> configure = null)
        {
            var options = new WindowOptions();

            configure?.Invoke(options);

            var mainWindow = new NativeOSWindow(options.Title, options.Position, options.Size);
            services.TryAddSingleton<OSWindow>(mainWindow); //This is externally owned
            services.TryAddSingleton<NativeOSWindow>(mainWindow); //This is externally owned
            services.TryAddSingleton<IScaleHelper, ScaleHelper>(); //Can add this now that the window is known

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

            return mainWindow;
        }

        public static void Destroy(NativeOSWindow window)
        {
            window.Dispose();
        }
    }
}
