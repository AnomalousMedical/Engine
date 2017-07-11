﻿using Anomalous.GameApp.Config;
using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Cameras;
using Anomalous.libRocketWidget;
using Anomalous.OSPlatform;
using Autofac;
using BulletPlugin;
using Engine;
using Engine.Platform;
using Engine.Renderer;
using libRocketPlugin;
using Logging;
using MyGUIPlugin;
using OgrePlugin;
using SoundPlugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Anomalous.GameApp
{
    public class GameApp : App
    {
        private CoreConfig coreConfig;
        private LogFileListener logListener;
        private PluginManager pluginManager;

        private ContainerBuilder builder = new ContainerBuilder();
        private UpdateTimer mainTimer;
        private IStartup startup;

        public event Action<GameApp> Initialized;

        public GameApp(IStartup startup)
        {
            this.startup = startup;

            builder.RegisterInstance(this)
                .ExternallyOwned()
                .As<App>()
                .As<GameApp>();
        }

        public override void Dispose()
        {
            CoreConfig.save();
            PerformanceMonitor.destroyEnabledState();

            pluginManager.Dispose();
            base.Dispose();
            logListener.Dispose();
        }

        public override bool OnInit()
        {
            coreConfig = new CoreConfig(startup.Name);

            //Create the log.
            logListener = new LogFileListener();
            logListener.openLogFile(CoreConfig.LogFile);
            Log.Default.addLogListener(logListener);
            Log.ImportantInfo("Running from directory {0}", FolderFinder.ExecutableFolder);
            startup.ConfigureServices(builder);

            builder.RegisterType<SceneController>()
                .IfNotRegistered(typeof(SceneController))
                .SingleInstance()
                .As<SceneController>()
                .OnActivated(a =>
                {
                    a.Instance.setDynamicMode(true);
                });

            builder.RegisterType<VirtualTextureSceneViewLink>()
                .IfNotRegistered(typeof(VirtualTextureSceneViewLink))
                .SingleInstance()
                .As<VirtualTextureSceneViewLink>();

            BuildPluginManager();

            //Create containers
            var scope = this.pluginManager.GlobalScope;

            //Build engine
            var pluginManager = scope.Resolve<PluginManager>();
            mainTimer = scope.Resolve<UpdateTimer>();
            var frameClearManager = scope.Resolve<FrameClearManager>();

            PerformanceMonitor.setupEnabledState(scope.Resolve<SystemTimer>());

            MyGUIInterface.Instance.CommonResourceGroup.addResource(GetType().AssemblyQualifiedName, "EmbeddedScalableResource", true);
            MyGUIInterface.Instance.CommonResourceGroup.addResource(startup.GetType().AssemblyQualifiedName, "EmbeddedScalableResource", true);

            startup.Initialized(this, pluginManager);

            if (Initialized != null)
            {
                Initialized.Invoke(this);
            }

            return true;
        }

        private void BuildPluginManager()
        {
            var mainWindow = new NativeOSWindow(startup.Title,
                new IntVector2(-1, -1),
                new IntSize2(CoreConfig.EngineConfig.HorizontalRes, CoreConfig.EngineConfig.VerticalRes));

            builder.RegisterInstance(mainWindow)
                .As<OSWindow>()
                .As<NativeOSWindow>();

            mainWindow.Closed += w =>
            {
                if (PlatformConfig.CloseMainWindowOnShutdown)
                {
                    mainWindow.close();
                }
                this.exit();
            };

            //Setup DPI
            float pixelScale = mainWindow.WindowScaling;
            switch (CoreConfig.ExtraScaling)
            {
                case UIExtraScale.Smaller:
                    pixelScale -= .15f;
                    break;
                case UIExtraScale.Larger:
                    pixelScale += .25f;
                    break;
            }

            ScaleHelper._setScaleFactor(pixelScale);

            pluginManager = new PluginManager(CoreConfig.ConfigFile, builder);

            builder.RegisterType<NativeSystemTimer>()
                .IfNotRegistered(typeof(SystemTimer))
                .SingleInstance()
                .As<SystemTimer>();

            builder.RegisterType<NativeUpdateTimer>()
                .IfNotRegistered(typeof(UpdateTimer))
                .SingleInstance()
                .As<UpdateTimer>()
                .As<NativeUpdateTimer>();

            builder.Register(c => new NativeInputHandler(c.Resolve<NativeOSWindow>(), CoreConfig.EnableMultitouch))
                .IfNotRegistered(typeof(InputHandler))
                .SingleInstance()
                .As<InputHandler>();

            builder.RegisterType<EventManager>()
                .WithParameter(new TypedParameter(typeof(IEnumerable), Enum.GetValues(typeof(EventLayers))))
                .SingleInstance()
                .As<EventManager>();
                
            MyGUIInterface.EventLayerKey = EventLayers.Gui;
            MyGUIInterface.CreateGuiGestures = CoreConfig.EnableMultitouch && PlatformConfig.TouchType == TouchType.Screen;

            OgreInterface.CompressedTextureSupport = CompressedTextureSupport.None;

            //Configure plugins
            pluginManager.OnConfigureDefaultWindow = delegate (out WindowInfo defaultWindow)
            {
                //Setup main window
                defaultWindow = new WindowInfo(mainWindow, "Primary");
                defaultWindow.Fullscreen = CoreConfig.EngineConfig.Fullscreen;
                defaultWindow.MonitorIndex = 0;

                if (CoreConfig.EngineConfig.Fullscreen)
                {
                    mainWindow.setSize(CoreConfig.EngineConfig.HorizontalRes, CoreConfig.EngineConfig.VerticalRes);
                    mainWindow.ExclusiveFullscreen = true;
                    defaultWindow.Width = CoreConfig.EngineConfig.HorizontalRes;
                    defaultWindow.Height = CoreConfig.EngineConfig.VerticalRes;
                }
                else
                {
                    mainWindow.Maximized = true;
                }
                mainWindow.show();
            };

            //GuiFrameworkCamerasInterface.CameraTransitionTime = MedicalConfig.CameraTransitionTime;
            //GuiFrameworkCamerasInterface.DefaultCameraButton = MedicalConfig.CameraMouseButton;
            GuiFrameworkCamerasInterface.MoveCameraEventLayer = EventLayers.Cameras;
            GuiFrameworkCamerasInterface.SelectWindowEventLayer = EventLayers.AfterGui;
            GuiFrameworkCamerasInterface.ShortcutEventLayer = EventLayers.AfterGui;
            GuiFrameworkCamerasInterface.TouchType = PlatformConfig.TouchType;
            GuiFrameworkCamerasInterface.PanKey = PlatformConfig.PanKey;

            pluginManager.addPluginAssembly(typeof(OgreInterface).Assembly());
            pluginManager.addPluginAssembly(typeof(BulletInterface).Assembly());
            pluginManager.addPluginAssembly(typeof(NativePlatformPlugin).Assembly());
            pluginManager.addPluginAssembly(typeof(MyGUIInterface).Assembly());
            pluginManager.addPluginAssembly(typeof(RocketInterface).Assembly());
            pluginManager.addPluginAssembly(typeof(SoundPluginInterface).Assembly());
            pluginManager.addPluginAssembly(typeof(GuiFrameworkInterface).Assembly());
            pluginManager.addPluginAssembly(typeof(RocketWidgetInterface).Assembly());
            pluginManager.addPluginAssembly(typeof(GuiFrameworkCamerasInterface).Assembly());
            foreach (var assembly in startup.AdditionalPluginAssemblies)
            {
                pluginManager.addPluginAssembly(assembly);
            }
            pluginManager.initializePlugins();

            var scope = pluginManager.GlobalScope;

            var systemTimer = scope.Resolve<SystemTimer>();
            var mainTimer = scope.Resolve<UpdateTimer>();
            var inputHandler = this.InputHandler = scope.Resolve<InputHandler>();
            var eventManager = scope.Resolve<EventManager>();

            //Intialize the platform
            BulletInterface.Instance.ShapeMargin = 0.005f;

            if (OgreConfig.VSync && CoreConfig.EngineConfig.FPSCap < 300)
            {
                //Use a really high framerate cap if vsync is on since it will cap our 
                //framerate for us. If the user has requested a higher rate use it anyway.
                mainTimer.FramerateCap = 300;
            }
            else
            {
                mainTimer.FramerateCap = CoreConfig.EngineConfig.FPSCap;
            }

            mainTimer.addUpdateListener(new EventUpdateListener(eventManager));

            pluginManager.setPlatformInfo(mainTimer, eventManager);

            GuiFrameworkInterface.Instance.handleCursors(mainWindow);

            SoundConfig.initialize(CoreConfig.ConfigFile);

            GuiFrameworkInterface.Instance.handleCursors(mainWindow);
            SoundPluginInterface.Instance.setResourceWindow(mainWindow);

            var touchMouseGuiForwarder = new TouchMouseGuiForwarder(eventManager, inputHandler, systemTimer, mainWindow, EventLayers.Last);
            touchMouseGuiForwarder.ForwardTouchesAsMouse = PlatformConfig.ForwardTouchAsMouse;
            var myGuiKeyboard = new MyGUIOnscreenKeyboardManager(touchMouseGuiForwarder);
            var rocketKeyboard = new RocketWidgetOnscreenKeyboardManager(touchMouseGuiForwarder);
        }

        public override int OnExit()
        {
            return 0;
        }

        public override void OnIdle()
        {
            mainTimer.OnIdle();
        }

        public void saveCrashLog()
        {
            if (logListener != null)
            {
                DateTime now = DateTime.Now;
                String crashFile = String.Format(CultureInfo.InvariantCulture, "{0}/log {1}-{2}-{3} {4}.{5}.{6}.log", CoreConfig.CrashLogDirectory, now.Month, now.Day, now.Year, now.Hour, now.Minute, now.Second);
                logListener.saveCrashLog(crashFile);
            }
        }

        public InputHandler InputHandler { get; private set; }

        public String PrimaryArchivePath { get; set; }
    }
}