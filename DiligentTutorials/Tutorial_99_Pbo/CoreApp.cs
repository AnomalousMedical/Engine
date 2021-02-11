using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;

namespace Tutorial_99_Pbo
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
                o.Title = "Diligent - Tutorial 99 - PBO";
            });

            services.AddLogging(o =>
            {
                o.AddConsole();
            });

            services.AddDiligentEngine(pluginManager)
                    .AddDiligentEnginePbr(o =>
                    {
                        o.CustomizePbrOptions = RendererCI =>
                        {
                            RendererCI.AllowDebugView = true;
                            RendererCI.UseIBL = true;
                        };
                    })
                    .AddDiligentEnginePbrShapes();

            services.AddOSPlatform(pluginManager);

            //Add this app's services
            services.TryAddSingleton<PboUpdateListener>();

            return true;
        }

        public override bool OnLink(IServiceScope globalScope)
        {
            var log = globalScope.ServiceProvider.GetRequiredService<ILogger<CoreApp>>();
            log.LogInformation("Running from directory {0}", FolderFinder.ExecutableFolder);

            //Setup virtual file system
            var vfs = globalScope.ServiceProvider.GetRequiredService<VirtualFileSystem>();
            var assetPath = Path.GetFullPath("../../../../../../Engine-Next-Assets"); //This needs to be less hardcoded.
            vfs.addArchive(assetPath);

            mainTimer = globalScope.ServiceProvider.GetRequiredService<UpdateTimer>();

            var updateListener = globalScope.ServiceProvider.GetRequiredService<PboUpdateListener>();
            mainTimer.addUpdateListener(updateListener);

            PerformanceMonitor.setupEnabledState(globalScope.ServiceProvider.GetRequiredService<SystemTimer>());

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
