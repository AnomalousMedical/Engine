using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Cameras;
using Anomalous.OSPlatform;
using Autofac;
using Engine;
using Engine.ObjectManagement;
using Engine.Platform;
using Engine.Renderer;
using Logging;
using MyGUIPlugin;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.Minimus.OgreOnly
{
    public class OgreOnlyApp : App
    {
        private CoreConfig coreConfig;
        private LogFileListener logListener;
        private Color clearColor = Color.Black;
        private FrameClearManager frameClearManager;
        private NativeUpdateTimer mainTimer;

        ContainerBuilder builder = new ContainerBuilder();

        IContainer container;
        ILifetimeScope sceneScope;

        public OgreOnlyApp()
        {

        }

        public override bool OnInit()
        {
            coreConfig = new CoreConfig("Anomalous Minimus");

            //Create the log.
            logListener = new LogFileListener();
            logListener.openLogFile(CoreConfig.LogFile);
            Log.Default.addLogListener(logListener);
            Log.ImportantInfo("Running from directory {0}", FolderFinder.ExecutableFolder);

            builder.Register<NativeOSWindow>(c => new NativeOSWindow("Anomalous Minimus", new IntVector2(-1, -1), new IntSize2(CoreConfig.EngineConfig.HorizontalRes, CoreConfig.EngineConfig.VerticalRes)))
                .SingleInstance()
                .OnActivated(a =>
                {
                    //Main Window
                    var mainWindow = a.Instance;
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
                });

            builder.Register(c => new PluginManager(CoreConfig.ConfigFile))
                .SingleInstance()
                .OnActivated(a =>
                {
                    var pluginManager = a.Instance;
                    var mainWindow = a.Context.Resolve<NativeOSWindow>();

                    //Configure plugins
                    pluginManager.OnConfigureDefaultWindow = (out WindowInfo defaultWindow) =>
                    {
                        //Setup main window
                        defaultWindow = new WindowInfo(mainWindow, "Primary");
                        defaultWindow.Fullscreen = CoreConfig.EngineConfig.Fullscreen;
                        defaultWindow.MonitorIndex = 0;

                        if (CoreConfig.EngineConfig.Fullscreen)
                        {
                            mainWindow.setSize(CoreConfig.EngineConfig.HorizontalRes, CoreConfig.EngineConfig.VerticalRes);
                            mainWindow.ExclusiveFullscreen = true;
                        }
                        else
                        {
                            mainWindow.Maximized = true;
                        }
                        mainWindow.show();
                    };

                    pluginManager.addPluginAssembly(typeof(OgreInterface).Assembly);
                    pluginManager.addPluginAssembly(typeof(NativePlatformPlugin).Assembly);
                    pluginManager.initializePlugins();

                    var mainTimer = a.Context.Resolve<UpdateTimer>();
                    pluginManager.setPlatformInfo(mainTimer, a.Context.Resolve<EventManager>());

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
                });

            builder.RegisterType<NativeSystemTimer>()
                .SingleInstance()
                .As<SystemTimer>();

            builder.RegisterType<NativeUpdateTimer>()
                .SingleInstance()
                .As<UpdateTimer>()
                .As<NativeUpdateTimer>();

            builder.Register(c => new NativeInputHandler(c.Resolve<NativeOSWindow>(), CoreConfig.EnableMultitouch))
                .SingleInstance()
                .As<InputHandler>();

            builder.Register(c => new EventManager(c.Resolve<InputHandler>(), Enum.GetValues(typeof(EventLayers))))
                .SingleInstance()
                .OnActivated(a =>
                {
                    a.Context.Resolve<UpdateTimer>().addUpdateListener(new EventUpdateListener(a.Instance));
                });

            builder.RegisterType<SceneController>()
                .SingleInstance();

            builder.RegisterType<OnUpdateListener>()
                .SingleInstance()
                .OnActivated(a =>
                {
                    a.Context.Resolve<UpdateTimer>().addUpdateListener(a.Instance);
                });

            builder.Register(c => new OgreSceneManagerDefinition("Ogre"))
                .InstancePerScene();

            builder.Register(c =>
            {
                //Create scene
                //Create a simple scene to use to show the models
                SimSceneDefinition sceneDefiniton = new SimSceneDefinition();
                var ogreScene = c.Resolve<OgreSceneManagerDefinition>();
                SimSubSceneDefinition mainSubScene = new SimSubSceneDefinition("Main");
                sceneDefiniton.addSimElementManagerDefinition(ogreScene);
                sceneDefiniton.addSimSubSceneDefinition(mainSubScene);
                mainSubScene.addBinding(ogreScene);
                sceneDefiniton.DefaultSubScene = "Main";

                return sceneDefiniton;
            }).InstancePerScene();

            builder.Register(c => c.Resolve<SimSceneDefinition>().createScene()).InstancePerScene();

            builder.Register<FrameClearManager>(c => new FrameClearManager(OgreInterface.Instance.OgrePrimaryWindow.OgreRenderTarget, Color.Blue))
                .SingleInstance();

            container = builder.Build();
            sceneScope = container.BeginLifetimeScope(LifetimeScopes.Scene);
            var sceneController = sceneScope.Resolve<SceneController>(); //Have to resolve the scene controller to get the engine setup

            //Grab what we need for our hacky scene
            mainTimer = sceneScope.Resolve<NativeUpdateTimer>();
            this.frameClearManager = sceneScope.Resolve<FrameClearManager>();

            sceneScope.Resolve<SimScene>();
            sceneScope.Resolve<OnUpdateListener>().OnUpdate += OnUpdate;

            PerformanceMonitor.setupEnabledState(sceneScope.Resolve<SystemTimer>());

            return true;
        }

        private void OnUpdate(Clock time)
        {
            clearColor.b += (0.5f * time.DeltaSeconds);
            clearColor.b %= 1.0f;
            frameClearManager.ClearColor = clearColor;
        }

        public override int OnExit()
        {
            CoreConfig.save();
            PerformanceMonitor.destroyEnabledState();
            sceneScope.Dispose();
            container.Dispose();

            base.Dispose();

            logListener.Dispose();

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
                String crashFile = String.Format(CultureInfo.InvariantCulture, "{0}/log {1}-{2}-{3} {4}.{5}.{6}.log", CoreConfig.CrashLogDirectory, now.Month, now.Day, now.Year, now.Hour, now.Minute, now.Second);
                logListener.saveCrashLog(crashFile);
            }
        }
    }
}
