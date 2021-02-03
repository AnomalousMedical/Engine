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
            mainWindow.Dispose();
            base.Dispose();
        }

        public override bool OnInit(IServiceCollection services, PluginManager pluginManager)
        {
            mainWindow = new NativeOSWindow("TEST APP TITLE",
                new IntVector2(-1, -1),
                new IntSize2(1920, 1080));

            //if (CoreConfig.EngineConfig.Fullscreen)
            //{
            //    mainWindow.setSize(CoreConfig.EngineConfig.HorizontalRes, CoreConfig.EngineConfig.VerticalRes);
            //    mainWindow.ExclusiveFullscreen = true;
            //    defaultWindow.Width = CoreConfig.EngineConfig.HorizontalRes;
            //    defaultWindow.Height = CoreConfig.EngineConfig.VerticalRes;
            //}
            //else
            //{
            //    mainWindow.Maximized = true;
            //}
            mainWindow.show();

            services.TryAddSingleton<OSWindow>(mainWindow); //This is externally owned
            services.TryAddSingleton<NativeOSWindow>(mainWindow); //This is externally owned

            mainWindow.Closed += w =>
            {
                mainWindow.close();
                this.Exit();
            };

            //Setup DPI
            float pixelScale = mainWindow.WindowScaling;
            //switch (CoreConfig.ExtraScaling)
            //{
            //    case UIExtraScale.Smaller:
            //        pixelScale -= .15f;
            //        break;
            //    case UIExtraScale.Larger:
            //        pixelScale += .25f;
            //        break;
            //}

            ScaleHelper._setScaleFactor(pixelScale);

            services.AddLogging(o =>
            {
                o.AddConsole();
            });

            services.TryAddSingleton<SystemTimer, NativeSystemTimer>();

            services.TryAddSingleton<UpdateTimer, NativeUpdateTimer>();

            services.TryAddSingleton<InputHandler>(s =>
            {
                bool makeConfig_EnableMultitouch = false;
                return new NativeInputHandler(s.GetRequiredService<NativeOSWindow>(), makeConfig_EnableMultitouch, s.GetRequiredService<ILogger<NativeInputHandler>>());
            });

            services.TryAddSingleton<EventManager>(s =>
            {
                return new EventManager(s.GetRequiredService<InputHandler>(), Enum.GetValues(typeof(EventLayers)), s.GetRequiredService<ILogger<EventManager>>());
            });

            services.TryAddSingleton<SimpleUpdateListener>();

            //addPluginAssembly(typeof(BulletInterface).Assembly);

            services.AddDiligentEngine(pluginManager);

            pluginManager.addPluginAssembly(typeof(NativePlatformPlugin).Assembly);
            pluginManager.addPluginAssembly(typeof(SoundPluginInterface).Assembly);

            return true;
        }

        public override bool OnLink(IServiceScope globalScope)
        {
            var log = globalScope.ServiceProvider.GetRequiredService<ILogger<CoreApp>>();
            log.LogInformation("Running from directory {0}", FolderFinder.ExecutableFolder);

            var systemTimer = globalScope.ServiceProvider.GetRequiredService<SystemTimer>();
            mainTimer = globalScope.ServiceProvider.GetRequiredService<UpdateTimer>();
            var inputHandler = this.InputHandler = globalScope.ServiceProvider.GetRequiredService<InputHandler>();
            var eventManager = globalScope.ServiceProvider.GetRequiredService<EventManager>();

            //Intialize the platform
            //BulletInterface.Instance.ShapeMargin = 0.005f;

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

            SoundPluginInterface.Instance.setResourceWindow(mainWindow);

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

        public InputHandler InputHandler { get; private set; }
    }
}
