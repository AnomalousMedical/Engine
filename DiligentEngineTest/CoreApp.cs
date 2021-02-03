using Anomalous.OSPlatform;
using DiligentEnginePlugin;
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

            base.BeforeMainWindowDispose(); //replaces pluginManager.Dispose();, needs to be better
            OSPlatformServiceCollectionExtensions.DestroyNativeOSWindow(mainWindow);
            base.Dispose();
        }

        public override bool OnInit(IServiceCollection services, PluginManager pluginManager)
        {
            mainWindow = services.CreateAndAddNativeOSWindow(this, o =>
            {

            });

            services.AddLogging(o =>
            {
                o.AddConsole();
            });

            services.TryAddSingleton<InputHandler>(s =>
            {
                bool makeConfig_EnableMultitouch = false;
                return new NativeInputHandler(s.GetRequiredService<NativeOSWindow>(), makeConfig_EnableMultitouch, s.GetRequiredService<ILogger<NativeInputHandler>>());
            });

            services.TryAddSingleton<EventManager>(s =>
            {
                return new EventManager(s.GetRequiredService<InputHandler>(), Enum.GetValues(typeof(EventLayers)), s.GetRequiredService<ILogger<EventManager>>());
            });

            services.AddDiligentEngine(pluginManager);
            services.AddOsPlatform(pluginManager);
            services.AddSoundPlugin(pluginManager);

            //Add this app's services
            services.TryAddSingleton<SimpleUpdateListener>();

            return true;
        }

        public override bool OnLink(IServiceScope globalScope)
        {
            var log = globalScope.ServiceProvider.GetRequiredService<ILogger<CoreApp>>();
            log.LogInformation("Running from directory {0}", FolderFinder.ExecutableFolder);

            mainTimer = globalScope.ServiceProvider.GetRequiredService<UpdateTimer>();
            var eventManager = globalScope.ServiceProvider.GetRequiredService<EventManager>();

            ////Setup framerate cap
            //if (PlatformConfig.FpsCap.HasValue)
            //{
            //    //Use platform cap if it is set always
            //    mainTimer.FramerateCap = PlatformConfig.FpsCap.Value;
            //}
            //else if (OgreConfig.VSync)
            //{
            //    //Use a unlimited framerate cap if vsync is on since it will cap our 
            //    //framerate for us. If the user has requested a higher rate use it anyway.
            //    mainTimer.FramerateCap = 0;
            //}
            //else
            //{
            //    //Otherwise config cap
            //    mainTimer.FramerateCap = CoreConfig.EngineConfig.FPSCap;
            //}

            mainTimer.addUpdateListener(new EventUpdateListener(eventManager));

            var updateListener = globalScope.ServiceProvider.GetRequiredService<SimpleUpdateListener>();
            mainTimer.addUpdateListener(updateListener);

            SoundConfig.MasterVolume = 1.0f;

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
