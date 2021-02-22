using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SoundPlugin;
using System;
using System.Globalization;
using System.IO;

namespace DiligentEngineTest
{
    public class CoreApp : App
    {
        private NativeOSWindow mainWindow;
        private UpdateTimer mainTimer;

        public CoreApp()
        {

        }

        public override void Dispose()
        {
            PerformanceMonitor.destroyEnabledState();

            base.DisposeGlobalScope();
            EasyNativeWindow.Destroy(mainWindow);
            base.FinalDispose();
        }

        public override bool OnInit(IServiceCollection services, PluginManager pluginManager)
        {
            mainWindow = EasyNativeWindow.Create(services, this, o =>
            {
                o.Title = "Tiny Engine App with Sound and Diligent";
            });

            services.AddLogging(o =>
            {
                o.AddConsole();
            });

            services.AddDiligentEngine(pluginManager);
            services.AddOSPlatform(pluginManager);
            services.AddSoundPlugin(pluginManager);

            //Add this app's services
            services.TryAddSingleton<SimpleUpdateListener>();

            return true;
        }

        public override bool OnLink(IServiceProvider serviceProvider)
        {
            var log = serviceProvider.GetRequiredService<ILogger<CoreApp>>();
            log.LogInformation("Running from directory {0}", FolderFinder.ExecutableFolder);

            mainTimer = serviceProvider.GetRequiredService<UpdateTimer>();

            var updateListener = serviceProvider.GetRequiredService<SimpleUpdateListener>();
            mainTimer.addUpdateListener(updateListener);

            PerformanceMonitor.setupEnabledState(serviceProvider.GetRequiredService<SystemTimer>());

            return true;
        }

        public override int OnExit()
        {
            return 0;
        }

        public override void OnIdle()
        {
            mainTimer?.OnIdle();
        }
    }
}
