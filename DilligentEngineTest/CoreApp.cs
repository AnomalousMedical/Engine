﻿using Anomalous.OSPlatform;
using DilligentEnginePlugin;
using Engine;
using Engine.Platform;
using Engine.Renderer;
using Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SoundPlugin;
using System;
using System.Globalization;
using System.IO;

namespace DilligentEngineTest
{
    public class CoreApp : App
    {
        private LogFileListener logListener;
        private PluginManager pluginManager;
        private NativeOSWindow mainWindow;

        private IServiceCollection services = new ServiceCollection();
        private UpdateTimer mainTimer;

        public event Action<CoreApp> Initialized;

        public CoreApp()
        {
            services.TryAddSingleton<App>(this); //This is externally owned
            services.TryAddSingleton<CoreApp>(this); //This is externally owned
        }

        public override void Dispose()
        {
            PerformanceMonitor.destroyEnabledState();

            pluginManager.Dispose();
            mainWindow.Dispose();
            base.Dispose();
            logListener.Dispose();
        }

        public override bool OnInit()
        {
            //Create the log.
            logListener = new LogFileListener();
            logListener.openLogFile("test.log");
            Log.Default.addLogListener(logListener);
            Log.ImportantInfo("Running from directory {0}", FolderFinder.ExecutableFolder);
            //startup.ConfigureServices(services);
            BuildPluginManager();

            //Create containers
            var scope = this.pluginManager.GlobalScope;

            //Build engine
            var pluginManager = scope.ServiceProvider.GetRequiredService<PluginManager>();
            mainTimer = scope.ServiceProvider.GetRequiredService<UpdateTimer>();
            //var frameClearManager = scope.ServiceProvider.GetRequiredService<FrameClearManager>();

            PerformanceMonitor.setupEnabledState(scope.ServiceProvider.GetRequiredService<SystemTimer>());

            //startup.Initialized(this, pluginManager);

            if (Initialized != null)
            {
                Initialized.Invoke(this);
            }

            return true;
        }

        private void BuildPluginManager()
        {
            mainWindow = new NativeOSWindow("TEST APP TITLE",
                new IntVector2(-1, -1), 
                new IntSize2(1920, 1080));

            services.TryAddSingleton<OSWindow>(mainWindow); //This is externally owned
            services.TryAddSingleton<NativeOSWindow>(mainWindow); //This is externally owned
            
            mainWindow.Closed += w =>
            {
                //if (PlatformConfig.CloseMainWindowOnShutdown)
                {
                    mainWindow.close();
                }
                this.exit();
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

            ConfigFile configFile = new ConfigFile("config.ini");

            pluginManager = new PluginManager(configFile, services);

            services.TryAddSingleton<SystemTimer, NativeSystemTimer>();

            services.TryAddSingleton<UpdateTimer, NativeUpdateTimer>();

            services.TryAddSingleton<InputHandler>(s =>
            {
                bool makeConfig_EnableMultitouch = false;
                return new NativeInputHandler(s.GetRequiredService<NativeOSWindow>(), makeConfig_EnableMultitouch);
            });

            services.TryAddSingleton<EventManager>(s =>
            {
                return new EventManager(s.GetRequiredService<InputHandler>(), Enum.GetValues(typeof(EventLayers)));
            });

            services.TryAddSingleton<SimpleUpdateListener>();

            //Configure plugins
            pluginManager.OnConfigureDefaultWindow = (out WindowInfo defaultWindow) =>
            {
                //Setup main window
                defaultWindow = new WindowInfo(mainWindow, "Primary");
                defaultWindow.Fullscreen = false;// CoreConfig.EngineConfig.Fullscreen;
                defaultWindow.MonitorIndex = 0;

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
            };

            //pluginManager.addPluginAssembly(typeof(BulletInterface).Assembly);
            pluginManager.addPluginAssembly(typeof(DilligentEnginePluginInterface).Assembly);
            pluginManager.addPluginAssembly(typeof(NativePlatformPlugin).Assembly);
            pluginManager.addPluginAssembly(typeof(SoundPluginInterface).Assembly);
            //foreach(var assembly in startup.AdditionalPluginAssemblies)
            //{
            //    pluginManager.addPluginAssembly(assembly);
            //}
            pluginManager.initializePlugins();

            var scope = pluginManager.GlobalScope;

            var systemTimer = scope.ServiceProvider.GetRequiredService<SystemTimer>();
            var mainTimer = scope.ServiceProvider.GetRequiredService<UpdateTimer>();
            var inputHandler = this.InputHandler = scope.ServiceProvider.GetRequiredService<InputHandler>();
            var eventManager = scope.ServiceProvider.GetRequiredService<EventManager>();

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

            pluginManager.setPlatformInfo(mainTimer, eventManager);
            mainTimer.addUpdateListener(new EventUpdateListener(eventManager));

            var updateListener = scope.ServiceProvider.GetRequiredService<SimpleUpdateListener>();
            mainTimer.addUpdateListener(updateListener);

            SoundConfig.initialize(configFile);

            SoundPluginInterface.Instance.setResourceWindow(mainWindow);
        }

        public override int OnExit()
        {
            return 0;
        }

        public override void OnIdle()
        {
            mainTimer.OnIdle();
        }

        internal void saveCrashLog()
        {
            if (logListener != null)
            {
                DateTime now = DateTime.Now;
                String crashFile = String.Format(CultureInfo.InvariantCulture, "{0}/log {1}-{2}-{3} {4}.{5}.{6}.log", Directory.GetCurrentDirectory(), now.Month, now.Day, now.Year, now.Hour, now.Minute, now.Second);
                logListener.saveCrashLog(crashFile);
            }
        }

        public InputHandler InputHandler { get; private set; }
    }
}