using Anomalous.OSPlatform;
using DiligentEngine;
using Engine;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;

namespace Tutorial13_ShadowMap
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
                o.Title = "Diligent - Tutorial 13 - Shadow Map";
            });

            services.AddLogging(o =>
            {
                o.AddConsole();
            });

            services.AddDiligentEngine(pluginManager);
            services.AddOSPlatform(pluginManager);
            services.AddFirstPersonFlyCamera();

            //Add this app's services
            services.TryAddSingleton<ShadowMapUpdateListener>();

            return true;
        }

        public override bool OnLink(IServiceProvider serviceProvider)
        {
            var log = serviceProvider.GetRequiredService<ILogger<CoreApp>>();
            log.LogInformation("Running from directory {0}", FolderFinder.ExecutableFolder);

            var vfs = serviceProvider.GetRequiredService<VirtualFileSystem>();
            vfs.addArchive("../../../../Assets");

            mainTimer = serviceProvider.GetRequiredService<UpdateTimer>();

            var updateListener = serviceProvider.GetRequiredService<ShadowMapUpdateListener>();
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
